using NutriPro.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriPro.Data.Models.Management
{
    [Table("tbTenants")]
    public class Tenants : NutriProBaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public virtual ETenantType Type { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public virtual EStates State { get; set; }
        public virtual List<Units> Units { get; set; }
    }
}

