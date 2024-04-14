using System.ComponentModel.DataAnnotations;

namespace NutriPro.Data.Enums
{
    public enum ELevelOrganization
    {
        [Display(Name = "Matriz")]
        Matriz = 0,          

        [Display(Name = "Filial")]
        Filial = 1,

        [Display(Name = "Pessoa Física")]
        PF = 2,
    }

    public static class ELevelOrganizationExtensions
    {
        public static string GetDisplayName(this ELevelOrganization level)
        {
            var displayAttribute = level.GetType()
                .GetMember(level.ToString())
                .FirstOrDefault()?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute?.GetName() ?? level.ToString();
        }
    }
}
