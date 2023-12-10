using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day5
    {
        public static void Solve()
        {
            using (StreamReader reader = new StreamReader(@".\Day5\input.txt"))
            {
                var seeds = reader.ReadLine().Split(':')[1].Trim().Split(' ').Select(p => Convert.ToInt64(p)).ToArray();
                List<long[][]> maplist = new List<long[][]>();

                var maps = reader.ReadToEnd().Split(':').Select(line => line.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
                foreach (var map in maps)
                {
                    long[][] data = map.Where(m => Char.IsDigit(m[0])).Select(m => m.Split(' ').Select(num => Convert.ToInt64(num)).ToArray()).ToArray();
                    if (data.Length > 0) maplist.Add(data);
                }

                IslandAlmanac almanac = new IslandAlmanac(seeds, maplist[0], maplist[1], maplist[2], maplist[3], maplist[4], maplist[5], maplist[6]);
                Console.WriteLine($"Day 5: Part 1: {Solve_Part1(almanac)}");
                //Console.WriteLine($"Day 5: Part 2: {Solve_Part2(almanac)}");
            }
        }

        private class IslandAlmanac
        {
            public IslandAlmanac(long[] seeds, long[][] seedToSoil, long[][] soilToFertilizer, long[][] fertilizerToWater ,
                long[][] waterToLight, long[][] lightToTemperature, long[][] temperatureToHumidity, long[][] humidityToLocation)
            {
                Seeds = seeds;

                HumidityToLocation = new SeedMap(humidityToLocation.Select(s => new Mapping(s[0], s[1], s[2])).ToArray(), null);
                TemperatureToHumidity = new SeedMap(temperatureToHumidity.Select(s => new Mapping(s[0], s[1], s[2])).ToArray(), HumidityToLocation);
                LightToTemperature = new SeedMap(lightToTemperature.Select(s => new Mapping(s[0], s[1], s[2])).ToArray(), TemperatureToHumidity);
                WaterToLight = new SeedMap(waterToLight.Select(s => new Mapping(s[0], s[1], s[2])).ToArray(), LightToTemperature);
                FertilizerToWater = new SeedMap(fertilizerToWater.Select(s => new Mapping(s[0], s[1], s[2])).ToArray(), WaterToLight);
                SoilToFertilizer = new SeedMap(soilToFertilizer.Select(s => new Mapping(s[0], s[1], s[2])).ToArray(), FertilizerToWater);
                SeedToSoil = new SeedMap(seedToSoil.Select(s => new Mapping(s[0], s[1], s[2])).ToArray(), SoilToFertilizer);
            }

            public long[] Seeds { get; private set; }
            public SeedMap SeedToSoil { get; private set; }
            public SeedMap SoilToFertilizer { get; private set; }
            public SeedMap FertilizerToWater { get; private set; }
            public SeedMap WaterToLight { get; private set; }
            public SeedMap LightToTemperature { get; private set; }
            public SeedMap TemperatureToHumidity { get; private set; }
            public SeedMap HumidityToLocation { get; private set; }
        }

        private class SeedMap: List<Mapping>
        {
            public readonly SeedMap MappedTo;

            public SeedMap(Mapping[] data, SeedMap mapsTo = null) : base(data.Length)
            {
                base.AddRange(data);
                MappedTo = mapsTo;
            }

            public long MapTo(long source)
            {
                var mapping = this.Where(s => s.Contains(source)).SingleOrDefault();
                if(mapping is null && MappedTo != null)
                {
                    return MappedTo.MapTo(source);
                }
                else if(mapping is null)
                {
                    return source;
                }
                long mappedTo = mapping.MapSourceToDest(source);
                if (MappedTo is null)
                {
                    return mappedTo;
                }
                else
                {
                    return MappedTo.MapTo(mappedTo);
                }
            }
        }

        private class Mapping
        {
            public Mapping(long destStartRange, long sourceStartRange, long rangeLength)
            {
                DestinationStartRange = destStartRange;
                SourceStartRange = sourceStartRange;
                RangeLength = rangeLength;
            }

            public bool Contains(long source)
            {
                if (source >= SourceStartRange && source <= SourceStartRange + RangeLength - 1)
                    return true;
                else 
                    return false;
            }

            public long MapSourceToDest(long source)
            {
                long index = source - SourceStartRange;
                long mappedLocation = DestinationStartRange + index;
                return mappedLocation;
            }
            public long DestinationStartRange { get; private set; }
            public long SourceStartRange { get; private set; }
            public long RangeLength { get; private set; }
        }

        private static long Solve_Part1(IslandAlmanac almanac)
        {
            List<long> locations = new List<long>();
            foreach (long seed in almanac.Seeds)
            {
                long location = almanac.SeedToSoil.MapTo(seed);
                locations.Add(location);
            }

            return locations.Min();
        }

        private class Range
        {
            public long Start  { get; set; }
            public long End  { get; set; }
            public bool Contains(long t)
            {
                if (this.Start <= t && this.End >= t) return true;
                return false;
            } 
            public bool IsOverlap(Range r) =>this.Contains(r.Start) || this.Contains(r.End);

            public Range RemakeRange_FromoOverlap(Range r)
            {
                long start = 0, end = 0;
                if (this.Contains(r.Start) && this.Contains(r.End))
                    return null;

                if (this.Contains(r.Start) && !this.Contains(r.End))
                {
                    start = this.End + 1;
                    end = r.End;
                }
                if(!this.Contains(r.Start) && this.Contains(r.End))
                {
                    start = r.Start;
                    end = this.End - 1;
                }

                return new Range() { Start = start, End = end };  
            }

            public Range MapToThisRange { get; set; } = null;
        }

        // trillions of possible results, takes too long on my pc with the 1st solution.
        private static long Solve_Part2(IslandAlmanac almanac)
        {
            return 0;
            throw new NotImplementedException();
        }


    }
}
