using BadmintonBookingSystem.BusinessObject.DTOs.S3;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface IAWSS3Service
    {
        Task<string> UploadFileAsync(S3Object s3Object);
        Task<IList<string>> UpLoadManyFilesAsync(List<S3Object> s3Objects);
    }

}
