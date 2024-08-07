using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    //class for Weekly Usage Cost of smart meter 
    public class UsageCostService : IUsageCostService
    {
        private readonly IAccountService _accountService;
        private readonly IMeterReadingService _meterReadingService;

        private readonly ICalculationService _calculationService;

        private readonly List<PricePlan> _pricePlans;

        public UsageCostService(IAccountService accountService, IMeterReadingService meterReadingService,List<PricePlan> pricePlans, ICalculationService calculationService)
        {
            _accountService = accountService;
            _meterReadingService = meterReadingService;
            _pricePlans = pricePlans;
            _calculationService = calculationService;
        }

        public decimal CalculateWeeklyCost(string smartMeterId)
        {
            var pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
            var pricePlan = _pricePlans.Find(p => p.PlanName == pricePlanId);
            //get readings for smart meter is and filter only for last 7 days
            var electricityReadings = _meterReadingService.GetReadings(smartMeterId);//.Where(reading => reading.Time > System.DateTime.Now.AddDays(-7)).ToList();
            //if last 7 days readings are not available return original list    
            if (!electricityReadings.Any())
            {
                electricityReadings = _meterReadingService.GetReadings(smartMeterId);
            }
            return _calculationService.CalculateWeeklyCost(electricityReadings, pricePlan);
        }
    }
}