
namespace Application.Interfaces
{
    public interface IMealGeneratorService
    {
        Task GenerateMonthlyMealsAsync(int year, int month);
    }
}
