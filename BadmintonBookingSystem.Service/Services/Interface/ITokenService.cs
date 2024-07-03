using BadmintonBookingSystem.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface ITokenService
    {
        public string CreateTokenForAccount(UserEntity user);

        public string GenerateRefreshToken();
    }
}
