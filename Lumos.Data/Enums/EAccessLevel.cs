using System.ComponentModel.DataAnnotations;

namespace NutriPro.Application.Enums
{
    public enum EAccessLevel
    {
        [Display(Name = "Admin")]
        Admin = 0,         

        [Display(Name = "Gerente")]
        Manager = 1,       

        [Display(Name = "Analista")]
        Analyst = 2      
    }

    public static class EAccessLevelExtensions
    {
        public static string GetDisplayName(this EAccessLevel accessLevel)
        {
            var displayAttribute = accessLevel.GetType()
                .GetMember(accessLevel.ToString())
                .FirstOrDefault()?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute?.GetName() ?? accessLevel.ToString();
        }
    }
}
