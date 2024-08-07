using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public interface ICalculationService
    {
        decimal CalculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan);
        decimal CalculateWeeklyCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan);
    }
}