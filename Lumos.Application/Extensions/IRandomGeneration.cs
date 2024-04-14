namespace NutriPro.Application.Extensions
{
    public interface IRandomGeneration
    {
        int Generate(int maxValue = 1000000);
        string GenerateString(int lenght = 10);
        string GenerateHashCode(int lenght);
    }
}
