using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriPro.Data.Models.Management
{
    [Table("tbUsers")]
    public class Users : NutriProBaseModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Cpf { get; set; }
        public long TenantId { get; set; }
        public virtual Tenants Tenant { get; set; }
        public virtual List<Units> Units { get; set; }
    }
}
