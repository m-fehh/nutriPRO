using System.ComponentModel.DataAnnotations;

namespace NutriPro.Data.Enums
{
    public enum ETenantType
    {
        [Display(Name = "PJ")]
        PJ = 0,

        [Display(Name = "PF")]
        PF = 1,
    }

    public static class ETenantTypeExtensions
    {
        public static string GetDisplayName(this ETenantType tenantType)
        {
            var displayAttribute = tenantType.GetType()
                .GetMember(tenantType.ToString())
                .FirstOrDefault()?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute?.GetName() ?? tenantType.ToString();
        }
    }
}
