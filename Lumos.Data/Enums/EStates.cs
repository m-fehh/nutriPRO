using NutriPro.Application.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriPro.Data.Enums
{
    public enum EStates
    {
        [Display(Name ="Acre")]
        AC,
        [Display(Name ="Alagoas")]
        AL,
        [Display(Name ="Amapá")]
        AP,
        [Display(Name ="Amazonas")]
        AM,
        [Display(Name ="Bahia")]
        BA,
        [Display(Name ="Ceará")]
        CE,
        [Display(Name ="Distrito Federal")]
        DF,
        [Display(Name ="Espírito Santo")]
        ES,
        [Display(Name ="Goiás")]
        GO,
        [Display(Name ="Maranhão")]
        MA,
        [Display(Name ="Mato Grosso")]
        MT,
        [Display(Name ="Mato Grosso do Sul")]
        MS,
        [Display(Name ="Minas Gerais")]
        MG,
        [Display(Name ="Pará")]
        PA,
        [Display(Name ="Paraíba")]
        PB,
        [Display(Name ="Paraná")]
        PR,
        [Display(Name ="Pernambuco")]
        PE,
        [Display(Name ="Piauí")]
        PI,
        [Display(Name ="Rio de Janeiro")]
        RJ,
        [Display(Name ="Rio Grande do Norte")]
        RN,
        [Display(Name ="Rio Grande do Sul")]
        RS,
        [Display(Name ="Rondônia")]
        RO,
        [Display(Name ="Roraima")]
        RR,
        [Display(Name ="Santa Catarina")]
        SC,
        [Display(Name ="São Paulo")]
        SP,
        [Display(Name ="Sergipe")]
        SE,
        [Display(Name ="Tocantins")]
        TO
    }

    public static class EStatesExtensions
    {
        public static string GetDisplayName(this EStates states)
        {
            var displayAttribute = states.GetType()
                .GetMember(states.ToString())
                .FirstOrDefault()?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute?.GetName() ?? states.ToString();
        }
    }
}
