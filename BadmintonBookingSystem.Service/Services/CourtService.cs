using BadmintonBookingSystem.BusinessObject.DTOs.S3;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services
{
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _courtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBadmintonCenterRepository _badmintonCenterRepository;
        private readonly IAWSS3Service _awsS3Service;

        public CourtService(IUnitOfWork unitOfWork, ICourtRepository courtRepository,IBadmintonCenterRepository badmintonCenterRepository, IAWSS3Service awsS3Service) 
        {
            _unitOfWork = unitOfWork;
            _courtRepository = courtRepository;
            _badmintonCenterRepository = badmintonCenterRepository;
            _awsS3Service = awsS3Service;
        }
        public async Task CreateNewCourt(CourtEntity courtEntity, List<IFormFile> picList)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var badmintonCenter = await _badmintonCenterRepository.GetOneAsync(courtEntity.CenterId);
                if (badmintonCenter == null)
                {
                    throw new NotFoundException("Chosen Center not found");
                }
                // Add the badminton center entity to the repository
                var cEntity = _courtRepository.Add(courtEntity);

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
                courtEntity.CourtImages = imageURLs.Select(url => new CourtImage { ImageLink = url }).ToList();

                // Save changes and commit transaction
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Failed to create court", ex);
            }
        }

        public async Task<IEnumerable<CourtEntity>> GetAllCourtByCenterId(string centerId, int pageIndex, int size)
        {
            var chosenCenter = await _badmintonCenterRepository.GetOneAsync(centerId);
            if (chosenCenter == null)
            {
                throw new NotFoundException("Chosen Badminton Center is not found");
            }
            var courtList = await _courtRepository.QueryHelper().Include(c => c.BadmintonCenter)
                .Filter(c => c.CenterId.Equals(centerId))
                .GetPagingAsync(pageIndex, size);
            if (!courtList.Any())
            {
                throw new NotFoundException("Empty List !");
            }
            return courtList;
        }

        public async Task<CourtEntity> GetCourtById(string courtId)
        {
            var chosenCourt = await _courtRepository.QueryHelper()
                .Filter(c => c.Id == courtId)
                .Include(x => x.BadmintonCenter)
                .GetOneAsync();
            if (chosenCourt == null)
            {
                throw new NotFoundException("Not Found!");
            }
            return chosenCourt;
        }

        public async Task<CourtEntity> UpdateCourt(CourtEntity entity, string courtId)
        {
            var chosenCourt = await GetCourtById(courtId);
            chosenCourt.CourtName = entity.CourtName;
            chosenCourt.LastUpdatedTime = DateTimeOffset.UtcNow;
            _courtRepository.Update(chosenCourt);
            await _unitOfWork.SaveChangesAsync();
            return chosenCourt;
        }
    }
}
