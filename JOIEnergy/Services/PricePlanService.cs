using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class PricePlanService : IPricePlanService
    {
        public interface Debug { void Log(string s); };

        private readonly List<PricePlan> _pricePlans;
        private IMeterReadingService _meterReadingService;

        private ICalculationService _calculationService;

        public PricePlanService(List<PricePlan> pricePlan, IMeterReadingService meterReadingService, ICalculationService calculationService)
        {
            _pricePlans = pricePlan;
            _meterReadingService = meterReadingService;
            _calculationService = calculationService;
        }

        
        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);

            if (!electricityReadings.Any())
            {
                return new Dictionary<string, decimal>();
            }
            return _pricePlans.ToDictionary(plan => plan.PlanName, plan => _calculationService.CalculateCost(electricityReadings, plan));
        }
    }
}
