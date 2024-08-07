namespace JOIEnergy.Services
{
    public interface IUsageCostService
    {
        decimal CalculateWeeklyCost(string smartMeterId);

    }
}