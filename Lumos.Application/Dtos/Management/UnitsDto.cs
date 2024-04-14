using NutriPro.Application.Dtos.Management.Tenants;
using NutriPro.Data.Enums;
using NutriPro.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NutriPro.Application.Dtos.Management
{
    public class UnitsDto : NutriProBaseModel
    {
        [Required(ErrorMessage = "O nome da unidade é obrigatório.")]
        [Display(Name = "Nome da Unidade")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O nível é obrigatório.")]
        [Display(Name = "Nível")]
        public ELevelOrganization Level { get; set; }

        public string LevelName => Level.GetDisplayName();

        [Required(ErrorMessage = "O CPF/CNPJ é obrigatório.")]
        [Display(Name = "CPF/CNPJ")]
        public string CpfCnpj { get; set; }

        public long TenantId { get; set; }

        public TenantsDto Tenant { get; set; }

        public List<UsersDto> Users { get; set; }
    }
}
