using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOIEnergy.Domain;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("readings")]
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;

        private readonly IUsageCostService _usageCostService;

        public MeterReadingController(IMeterReadingService meterReadingService, IUsageCostService usageCostService)
        {
            _meterReadingService = meterReadingService;
            _usageCostService = usageCostService;
        }

        // POST api/values
        [HttpPost ("store")]
        public ObjectResult Post([FromBody]MeterReadings meterReadings)
        {
            if (!IsMeterReadingsValid(meterReadings)) {
                return new BadRequestObjectResult("Internal Server Error");
            }
            _meterReadingService.StoreReadings(meterReadings.SmartMeterId,meterReadings.ElectricityReadings);
            return new OkObjectResult("{}");
        }

        private bool IsMeterReadingsValid(MeterReadings meterReadings)
        {
            string smartMeterId = meterReadings.SmartMeterId;
            List<ElectricityReading> electricityReadings = meterReadings.ElectricityReadings;
            return smartMeterId != null && smartMeterId.Any()
                    && electricityReadings != null && electricityReadings.Any();
        }

        [HttpGet("read/{smartMeterId}")]
        public ObjectResult GetReading(string smartMeterId) {
            return new OkObjectResult(_meterReadingService.GetReadings(smartMeterId));
        }

        //Get Method yo retrieve Smart Meter Usage Cost for each Price Plan over a week

        [HttpGet("usage-cost/{smartMeterId}")]
        
        public ObjectResult GetWeeklyUsageCost(string smartMeterId) {
            return new OkObjectResult(_usageCostService.CalculateWeeklyCost(smartMeterId));
        }

        //Get Method to Recommend Cheapest
    }
}
