using NutriPro.Application.Configurations;
using NutriPro.Application.Dtos.Management.Tenants;
using NutriPro.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace NutriPro.Application.Dtos.Management
{
    public class UsersDto : NutriProBaseModel
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [Display(Name = "Nome Completo")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        public long TenantId { get; set; }
        public TenantsDto Tenant { get; set; }
        public List<UnitsDto> Units { get; set; }

        public string SerializedUnitsList { get; set; }

        public void DecryptPassword()
        {
            var encryptionService = new AesEncryptionService();
            Password = encryptionService.Decrypt(Password);
        }
    }
}
