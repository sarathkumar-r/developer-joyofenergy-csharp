using System;
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
            if (pricePlan == null)
            {
                throw new InvalidOperationException("Price plan not found.");
            }
            
            var electricityReadings = _meterReadingService.GetReadings(smartMeterId);//.Where(reading => reading.Time > System.DateTime.Now.AddDays(-7)).ToList();
 
            if (!electricityReadings.Any())
            {
                electricityReadings = _meterReadingService.GetReadings(smartMeterId);
            }
            return _calculationService.CalculateWeeklyCost(electricityReadings, pricePlan);
        }
    }
}