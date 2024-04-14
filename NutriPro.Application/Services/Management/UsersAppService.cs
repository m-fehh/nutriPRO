using NutriPro.Application.Configurations;
using NutriPro.Application.Consts;
using NutriPro.Application.Interfaces.Management;
using NutriPro.Application.Repositories;
using NutriPro.Data.Models.Management;
using System.Security.Cryptography;
using System.Text;

namespace NutriPro.Application.Services.Management
{
    public class UsersAppService : NutriProAppServiceBase<Users>, IUsersAppService
    {
        public UsersAppService(NutriProSession session, IRepository<Users> repository) : base(session, repository)
        {
        }

        public async Task<Users?> ValidateUserCredentials(string email, string password)
        {
            if (HostConfigurationConst.HostLogin.Equals(email) && HostConfigurationConst.HostPassword.Equals(password)) 
            {
                return new Users
                {
                    FullName = "Admin Master"
                };
            }
            else
            {
                var user = (await _repository.GetAllAsync()).FirstOrDefault(u => u.Email == email);

                var encryptionService = new AesEncryptionService();

                if (user != null && encryptionService.Decrypt(password) == user.Password)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
