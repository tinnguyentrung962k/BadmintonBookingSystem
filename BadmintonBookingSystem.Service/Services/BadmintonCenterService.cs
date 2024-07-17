using Amazon.S3.Model;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.S3;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services
{
    public class BadmintonCenterService : IBadmintonCenterService
    {
        private readonly IBadmintonCenterRepository _badmintonCenterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private UserManager<UserEntity> _userManager;
        private readonly IAWSS3Service _awsS3Service;
        private readonly ICourtRepository _courtRepository;
        public BadmintonCenterService(IBadmintonCenterRepository badmintonCenterRepository, IUnitOfWork unitOfWork, UserManager<UserEntity> userManager, IAWSS3Service awsS3Service, ICourtRepository courtRepository)
        {
            _badmintonCenterRepository = badmintonCenterRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _awsS3Service = awsS3Service;
            _courtRepository = courtRepository;
        }

        public async Task CreateBadmintonCenter(BadmintonCenterEntity badmintonCenterEntity, List<IFormFile>? picList, IFormFile avatar)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Ensure the manager relationship is valid
                if (!string.IsNullOrEmpty(badmintonCenterEntity.ManagerId))
                {
                    // Check if the manager exists in the database
                    var manager = await _userManager.FindByIdAsync(badmintonCenterEntity.ManagerId);
                    if (manager == null)
                    {
                        throw new Exception("Manager not found"); // Handle appropriately
                    }
                }
                if (avatar != null)
                {
                    var s3Object = new AwsS3Object();
                    var memoryStream = new MemoryStream();
                    await avatar.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset stream position to the beginning

                    s3Object = new AwsS3Object
                    {
                        InputStream = memoryStream,
                        Name = avatar.FileName,
                        BucketName = "badminton-system"
                    };
                    var imageURL = await _awsS3Service.UploadFileAsync(s3Object);
                    badmintonCenterEntity.ImgAvatar = imageURL;
                }

                // Add the badminton center entity to the repository
                var bcEntity = _badmintonCenterRepository.Add(badmintonCenterEntity);

                if (picList is not null)
                {
                    var s3Objects = new List<AwsS3Object>();
                    foreach (var file in picList)
                    {
                        var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        memoryStream.Position = 0; // Reset stream position to the beginning

                        s3Objects.Add(new AwsS3Object
                        {
                            InputStream = memoryStream,
                            Name = file.FileName,
                            BucketName = "badminton-system"
                        });
                    }

                    var imageURLs = await _awsS3Service.UpLoadManyFilesAsync(s3Objects);

                    badmintonCenterEntity.BadmintonCenterImages = imageURLs.Select(url => new BadmintonCenterImage { ImageLink = url }).ToList();
                }
                // Convert IFormFile to S3Object
                

                // Save changes and commit transaction
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Failed to create badminton center", ex);
            }
        }

        public async Task<IEnumerable<BadmintonCenterEntity>> GetAllBadmintonCenterAsync(int pageIndex, int size)
        {
            var badmintonCenter = await _badmintonCenterRepository.QueryHelper()
                .Include(x => x.Manager)
                .Include(x => x.BadmintonCenterImages)
                .GetPagingAsync(pageIndex, size);
            if (!badmintonCenter.Any()) 
            {
                throw new NotFoundException("Empty list!");
            }
            return badmintonCenter;
        }

        public async Task<IEnumerable<BadmintonCenterEntity>> GetAllActiveBadmintonCentersAsync(int pageIndex, int size)
        {
            var badmintonCenter = await _badmintonCenterRepository.QueryHelper()
                .Filter(c=>c.IsActive == true)
                .Include(x => x.Manager)
                .Include(x => x.BadmintonCenterImages)
                .GetPagingAsync(pageIndex, size);
            if (!badmintonCenter.Any())
            {
                throw new NotFoundException("Empty list!");
            }
            return badmintonCenter;
        }

        public async Task<BadmintonCenterEntity> GetBadmintonCenterByIdAsync(string centerId)
        {
            var chosenCenter = await _badmintonCenterRepository
                .QueryHelper()
                .Filter(c => c.Id == centerId)
                .Include(x=>x.Manager)
                .Include(x=>x.BadmintonCenterImages)
                .GetOneAsync();
            if (chosenCenter == null)
            {
                throw new NotFoundException("Not Found!");
            }
            return chosenCenter;
        }

        public async Task<IEnumerable<BadmintonCenterEntity>> SearchBadmintonCentersAsync(BadmintonCenterEntity searchBadmintonCenter)
        {
            var search =  _badmintonCenterRepository.QueryHelper().Filter(c => c.IsActive == true);

            if (!string.IsNullOrEmpty(searchBadmintonCenter.Name))
            {
                search = search.Filter(bc => bc.Name.ToLower().Contains(searchBadmintonCenter.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchBadmintonCenter.Location))
            {
                search = search.Filter(bc => bc.Location.ToLower().Contains(searchBadmintonCenter.Location.ToLower()));
            }
            if (searchBadmintonCenter.OperatingTime != default || searchBadmintonCenter.ClosingTime != default)
            {
                if (searchBadmintonCenter.OperatingTime != default)
                {
                    search = search.Filter(bc => bc.OperatingTime <= searchBadmintonCenter.OperatingTime);
                }
                if (searchBadmintonCenter.ClosingTime != default)
                {
                    search = search.Filter(bc => bc.ClosingTime >= searchBadmintonCenter.ClosingTime);
                }
            }
            if (searchBadmintonCenter.OperatingTime != default && searchBadmintonCenter.ClosingTime != default)
            {
                search = search.Filter(bc => bc.OperatingTime <= searchBadmintonCenter.OperatingTime && bc.ClosingTime >= searchBadmintonCenter.ClosingTime);
            }

            return await search.GetAllAsync();
        }

        public async Task<BadmintonCenterEntity> UpdateBadmintonInfo(BadmintonCenterEntity badmintonCenterEntity, string centerId, List<IFormFile>? newPicList, IFormFile? newAvatar)
        {
            // Retrieve the existing badminton center
            var badmintonCenter = await GetBadmintonCenterByIdAsync(centerId);
            if (!string.IsNullOrEmpty(badmintonCenterEntity.ManagerId))
            {
                // Check if the manager exists in the database
                var manager = await _userManager.FindByIdAsync(badmintonCenterEntity.ManagerId);
                if (manager == null)
                {
                    throw new Exception("Manager not found"); // Handle appropriately
                }
            }

            // Update basic details
            badmintonCenter.Name = badmintonCenterEntity.Name;
            badmintonCenter.Location = badmintonCenterEntity.Location;
            badmintonCenter.ManagerId = badmintonCenterEntity.ManagerId;
            badmintonCenter.OperatingTime = badmintonCenterEntity.OperatingTime;
            badmintonCenter.LastUpdatedTime = DateTimeOffset.UtcNow;
            
            if (newAvatar != null)
            {
                var s3Object = new AwsS3Object();
                var memoryStream = new MemoryStream();
                await newAvatar.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset stream position to the beginning

                s3Object = new AwsS3Object
                {
                    InputStream = memoryStream,
                    Name = newAvatar.FileName,
                    BucketName = "badminton-system"
                };
                var imageURL = await _awsS3Service.UploadFileAsync(s3Object);
                badmintonCenter.ImgAvatar = imageURL;
            }

            // If new images are provided, update the images
            if (newPicList != null && newPicList.Count > 0)
            {
                // Delete existing images from S3
                var existingImages = badmintonCenter.BadmintonCenterImages.Select(img => img.ImageLink).ToList();
                await _awsS3Service.DeleteManyFilesAsync(existingImages);

                // Convert IFormFile to S3Object and upload new images
                var s3Objects = new List<AwsS3Object>();
                foreach (var file in newPicList)
                {
                    var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset stream position to the beginning

                    s3Objects.Add(new AwsS3Object
                    {
                        InputStream = memoryStream,
                        Name = file.FileName,
                        BucketName = "badminton-system"
                    });
                }

                var newImageURLs = await _awsS3Service.UpLoadManyFilesAsync(s3Objects);

                // Map the uploaded image URLs to BadmintonCenterImage entities
                badmintonCenter.BadmintonCenterImages = newImageURLs.Select(url => new BadmintonCenterImage { ImageLink = url }).ToList();
            }

            // Update the badminton center entity in the repository
            _badmintonCenterRepository.Update(badmintonCenter);
            await _unitOfWork.SaveChangesAsync();

            return badmintonCenter;
        }

        public async Task ToggleStatusBadmintonCenter(string centerId)
        {
            var center = await _badmintonCenterRepository.QueryHelper()
                .Filter(c=>c.Id.Equals(centerId))
                .Include(c=>c.Courts)
                .GetOneAsync();
            if (center == null) {
                throw new NotFoundException("Badminton center not found !");
            }
            if (center.IsActive == true)
            {
                center.IsActive = false;
                if (center.Courts.Any())
                {
                    foreach (var court in center.Courts)
                    {
                        court.IsActive = false;
                        _courtRepository.Update(court);
                    }
                }
            }
            if (center.IsActive == false)
            {
                center.IsActive = true;
            }
            center.LastUpdatedTime = DateTime.UtcNow;
            _badmintonCenterRepository.Update(center);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
