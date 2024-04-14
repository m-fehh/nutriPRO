using NutriPro.Data.Enums;
using NutriPro.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace NutriPro.Application.Dtos.Management.Tenants
{
    public class TenantsDto : NutriProBaseModel
    {
        [Required(ErrorMessage = "O nome do inquilino é obrigatório.")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O email do inquilino é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O tipo de inquilino é obrigatório.")]
        [Display(Name = "Tipo")]
        public ETenantType Type { get; set; }

        [Display(Name = "Tipo")]
        public string TypeName => Type.GetDisplayName();

        [Required(ErrorMessage = "A cidade do inquilino é obrigatória.")]
        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Required(ErrorMessage = "O estado do inquilino é obrigatório.")]
        [Display(Name = "Estado")]
        public string State { get; set; }

        public List<UnitsDto> Units { get; set; }
    }
}
