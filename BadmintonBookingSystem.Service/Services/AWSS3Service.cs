using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using BadmintonBookingSystem.BusinessObject.DTOs.S3;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace BadmintonBookingSystem.Service.Services
{
    public class AWSS3Service : IAWSS3Service
    {
        private readonly IConfiguration _configuration;
        private readonly string _awsAccessKey;
        private readonly string _awsSecretKey;

        public AWSS3Service(IConfiguration configuration)
        {
            _configuration = configuration;
            _awsAccessKey = _configuration.GetValue<string>("AwsConfiguration:AWSAccessKey");
            _awsSecretKey = _configuration.GetValue<string>("AwsConfiguration:AWSSecretKey");
        }

        public async Task<string> UploadFileAsync(S3Object s3Object)
        {
            // Validate image type before proceeding
            if (!IsImage(s3Object.InputStream))
            {
                throw new ArgumentException("Only image files are allowed.");
            }

            var credentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3Object.InputStream,
                    Key = s3Object.Name,
                    BucketName = s3Object.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                using var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);
                await transferUtility.UploadAsync(uploadRequest);
                var objectUrl = $"https://{s3Object.BucketName}.s3.{config.RegionEndpoint.SystemName}.amazonaws.com/{s3Object.Name}";
                return objectUrl;
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file to S3", ex);
            }
        }

        public async Task<IList<string>> UpLoadManyFilesAsync(List<S3Object> s3Objects)
        {
            var credentials = new BasicAWSCredentials(_awsAccessKey, _awsSecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };
            var uploadedFileUrls = new List<string>();

            using var client = new AmazonS3Client(credentials, config);
            var transferUtility = new TransferUtility(client);

            foreach (var s3Object in s3Objects)
            {
                // Validate image type before proceeding
                if (!IsImage(s3Object.InputStream))
                {
                    throw new ArgumentException($"File {s3Object.Name} is not an image.");
                }

                try
                {
                    var uploadRequest = new TransferUtilityUploadRequest()
                    {
                        InputStream = s3Object.InputStream,
                        Key = s3Object.Name,
                        BucketName = s3Object.BucketName,
                        CannedACL = S3CannedACL.NoACL
                    };

                    await transferUtility.UploadAsync(uploadRequest);
                    var objectUrl = $"https://{s3Object.BucketName}.s3.{config.RegionEndpoint.SystemName}.amazonaws.com/{s3Object.Name}";
                    uploadedFileUrls.Add(objectUrl);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error uploading file {s3Object.Name} to S3", ex);
                }
            }

            return uploadedFileUrls;
        }

        private bool IsImage(Stream stream)
        {
            try
            {
                // Attempt to load the image
                using (Image image = Image.Load(stream))
                {
                    // Check if the image is valid
                    return true;
                }
            }
            catch (Exception)
            {
                // Failed to load the image or not a valid image format
                return false;
            }
        }
    }
}
