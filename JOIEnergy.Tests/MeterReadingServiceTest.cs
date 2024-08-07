using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using JOIEnergy.Domain;
using Xunit;
using JOIEnergy.Controllers;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {

        private MeterReadingService meterReadingService;
        private CalculationService calculationService;
        private UsageCostService usageCostService;

        private AccountService accountService;

        private static string PRICE_PLAN_1_ID = "test-supplier";
        private static string PRICE_PLAN_2_ID = "best-supplier";
        private static string PRICE_PLAN_3_ID = "second-best-supplier";
        private static string SMART_METER_ID = "smart-meter-id";



        public MeterReadingServiceTest()
        {
            meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>());
            var pricePlans = new List<PricePlan>() { 
                new PricePlan() { PlanName = PRICE_PLAN_1_ID, UnitRate = 0.29m, PeakTimeMultiplier = NoMultipliers() }, 
                new PricePlan() { PlanName = PRICE_PLAN_2_ID, UnitRate = 1, PeakTimeMultiplier = NoMultipliers() },
                new PricePlan() { PlanName = PRICE_PLAN_3_ID, UnitRate = 2, PeakTimeMultiplier = NoMultipliers() } 
            };
            var smartMeterToPricePlanAccounts = new Dictionary<string, string>();
            smartMeterToPricePlanAccounts.Add(SMART_METER_ID, PRICE_PLAN_1_ID);

            accountService = new AccountService(smartMeterToPricePlanAccounts);
            calculationService = new CalculationService();
            usageCostService = new  UsageCostService(accountService,meterReadingService,pricePlans,calculationService);
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull() {
            Assert.Empty(meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            meterReadingService.StoreReadings(SMART_METER_ID, new List<ElectricityReading>() {
                new ElectricityReading() { Time = DateTime.Now, Reading = 25m }
            });

            var electricityReadings = meterReadingService.GetReadings(SMART_METER_ID);

            Assert.Single(electricityReadings);
        }

        [Fact]
        public void ShouldStoreAndRetrieveMeterReadingsCorrectly()
        {
            var readings = new List<ElectricityReading>
            {
                new ElectricityReading { Time = new DateTime(2024, 10, 19, 09, 25, 00, DateTimeKind.Utc), Reading = 1.101m },
                new ElectricityReading { Time = new DateTime(2024, 10, 20, 10, 00, 00, DateTimeKind.Utc), Reading = 0.994m },
                new ElectricityReading { Time = new DateTime(2024, 10, 21, 16, 58, 00, DateTimeKind.Utc), Reading = 0.503m },
                new ElectricityReading { Time = new DateTime(2024, 10, 22, 13, 20, 00, DateTimeKind.Utc), Reading = 1.065m },
                new ElectricityReading { Time = new DateTime(2024, 10, 23, 10, 40, 00, DateTimeKind.Utc), Reading = 0.213m },
                new ElectricityReading { Time = new DateTime(2024, 10, 24, 11, 00, 00, DateTimeKind.Utc), Reading = 0.24m },
                new ElectricityReading { Time = new DateTime(2024, 10, 25, 15, 28, 00, DateTimeKind.Utc), Reading = 0.598m },
                new ElectricityReading { Time = new DateTime(2024, 10, 26, 03, 45, 00, DateTimeKind.Utc), Reading = 0.001m },
                new ElectricityReading { Time = new DateTime(2024, 10, 26, 09, 26, 00, DateTimeKind.Utc), Reading = 0.506m },
                new ElectricityReading { Time = new DateTime(2024, 10, 27, 12, 46, 00, DateTimeKind.Utc), Reading = 1.011m },
                new ElectricityReading { Time = new DateTime(2024, 10, 28, 15, 14, 00, DateTimeKind.Utc), Reading = 1.201m },
                new ElectricityReading { Time = new DateTime(2024, 10, 29, 07, 10, 00, DateTimeKind.Utc), Reading = 0.009m },
                new ElectricityReading { Time = new DateTime(2024, 10, 30, 09, 54, 00, DateTimeKind.Utc), Reading = 0.202m }
            };

            meterReadingService.StoreReadings(SMART_METER_ID, readings);

            var storedReadings = meterReadingService.GetReadings(SMART_METER_ID);
            //test to check the CalculateWeeklyCost method in UsageCostService  
            var cost = usageCostService.CalculateWeeklyCost(SMART_METER_ID);
           //check if cost is euqal to 45.10    
           Assert.Equal(45.10m, cost);

        }

        private static List<PeakTimeMultiplier> NoMultipliers()
        {
            return new List<PeakTimeMultiplier>();
        }

    }
}
