using BadmintonBookingSystem.BusinessObject.DTOs.S3;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public BadmintonCenterService(IBadmintonCenterRepository badmintonCenterRepository, IUnitOfWork unitOfWork, UserManager<UserEntity> userManager, IAWSS3Service awsS3Service)
        {
            _badmintonCenterRepository = badmintonCenterRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _awsS3Service = awsS3Service;
        }

        public async Task CreateBadmintonCenter(BadmintonCenterEntity badmintonCenterEntity, List<IFormFile> picList)
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

                // Add the badminton center entity to the repository
                var bcEntity = _badmintonCenterRepository.Add(badmintonCenterEntity);

                // Convert IFormFile to S3Object
                var s3Objects = new List<S3Object>();
                foreach (var file in picList)
                {
                    var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset stream position to the beginning

                    s3Objects.Add(new S3Object
                    {
                        InputStream = memoryStream,
                        Name = file.FileName,
                        BucketName = "badminton-system"
                    });
                }

                // Upload images to S3
                var imageURLs = await _awsS3Service.UpLoadManyFilesAsync(s3Objects);

                // Map the uploaded image URLs to BadmintonCenterImage entities
                badmintonCenterEntity.BadmintonCenterImages = imageURLs.Select(url => new BadmintonCenterImage { ImageLink = url }).ToList();

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

        public async Task<BadmintonCenterEntity> GetBadmintonCenterByIdAsync(string centerId)
        {
            var chosenCenter = await _badmintonCenterRepository
                .QueryHelper().
                Filter(c => c.Id == centerId)
                .Include(x=>x.Manager)
                .GetOneAsync();
            if (chosenCenter == null)
            {
                throw new NotFoundException("Not Found!");
            }
            return chosenCenter;
        }

        public async Task<BadmintonCenterEntity> UpdateBadmintonInfo(BadmintonCenterEntity badmintonCenterEntity, string centerId)
        {

            var badmintonCenter = await GetBadmintonCenterByIdAsync(centerId) ;
            if (!string.IsNullOrEmpty(badmintonCenterEntity.ManagerId))
            {
                // Check if the manager exists in the database
                var manager = await _userManager.FindByIdAsync(badmintonCenterEntity.ManagerId);
                if (manager == null)
                {
                    throw new Exception("Manager not found"); // Handle appropriately
                }
            }
            badmintonCenter.Name = badmintonCenterEntity.Name;
            badmintonCenter.Location = badmintonCenterEntity.Location;
            badmintonCenter.ManagerId = badmintonCenterEntity.ManagerId;
            badmintonCenter.OperatingTime = badmintonCenterEntity.OperatingTime;
            badmintonCenter.LastUpdatedTime = DateTimeOffset.UtcNow;
             _badmintonCenterRepository.Update(badmintonCenter);
            await _unitOfWork.SaveChangesAsync();
            return badmintonCenter;
        }
    }
}
