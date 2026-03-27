using System;
using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Generator
{
    public class ElectricityReadingGenerator
    {
        public ElectricityReadingGenerator()
        {

        }
        public List<ElectricityReading> Generate(int number)
        {
            var random = new Random();
            
            var readings = Enumerable.Range(0, number)
                .Select(i => new ElectricityReading
                {
                    Reading = (decimal)random.NextDouble(),
                    Time = DateTime.Now.AddSeconds(-i * 10)
                })
                .OrderBy(r => r.Time)
                .ToList();
            
            return readings;
        }
    }
}
