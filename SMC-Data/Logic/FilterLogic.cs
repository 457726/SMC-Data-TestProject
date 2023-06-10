using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SMC_Data.Interfaces;
using SMC_Data.Models;
using System.Text.Json;

namespace SMC_Data.Logic
{
    public class FilterLogic : IFilterLogic
    {
        
        public List<string> FilterOutMultiplesZ(IFormFile json)
        {
            string fileContent = ReadFileContent(json);
            List<SplitData> splitData = ParseSplitData(fileContent);
            List<int> numbers = GetNonNullZValues(splitData);
            numbers.Sort();
            Dictionary<int, int> occurrences = CountOccurrences(numbers);
            List<string> results = GenerateResults(occurrences);
            return results;
        }

        private string ReadFileContent(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                return reader.ReadToEnd();
            }
        }

        private List<SplitData> ParseSplitData(string content)
        {
            JObject measurements = JObject.Parse(content);
            IList<JToken> result = measurements["measurements"].Children().ToList();
            return result.Select(o => o.ToObject<SplitData>()).ToList();
        }

        private List<int> GetNonNullXValues(List<SplitData> splitData)
        {
            return splitData.Where(data => data.x != null).Select(data => Convert.ToInt32(data.x)).ToList();
        }

        private List<int> GetNonNullYValues(List<SplitData> splitData)
        {
            return splitData.Where(data => data.y != null).Select(data => Convert.ToInt32(data.y)).ToList();
        }

        private List<int> GetNonNullZValues(List<SplitData> splitData)
        {
            return splitData.Where(data => data.z != null).Select(data => Convert.ToInt32(data.z)).ToList();
        }

        private Dictionary<int, int> CountOccurrences(List<int> numbers)
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();

            foreach (int number in numbers)
            {
                if (number != 0)
                {
                    if (occurrences.ContainsKey(number))
                    {
                        occurrences[number]++;
                    }
                    else
                    {
                        occurrences[number] = 1;
                    }
                }
            }

            return occurrences;
        }

        private List<string> GenerateResults(Dictionary<int, int> occurrences)
        {
            List<string> results = new List<string>();

            foreach (var entry in occurrences)
            {
                string result = $"{entry.Key} occurred {entry.Value} times";
                results.Add(result);
            }

            return results;
        }

        public List<string> FilterOutMultiplesX(IFormFile json)
        {
            string fileContent = ReadFileContent(json);
            List<SplitData> splitData = ParseSplitData(fileContent);
            List<int> numbers = GetNonNullXValues(splitData);
            numbers.Sort();
            Dictionary<int, int> occurrences = CountOccurrences(numbers);
            List<string> results = GenerateResults(occurrences);
            return results;
        }

        public List<string> FilterOutMultiplesY(IFormFile json)
        {
            string fileContent = ReadFileContent(json);
            List<SplitData> splitData = ParseSplitData(fileContent);
            List<int> numbers = GetNonNullYValues(splitData);
            numbers.Sort();
            Dictionary<int, int> occurrences = CountOccurrences(numbers);
            List<string> results = GenerateResults(occurrences);
            return results;
        }
        public IList<SplitData> FilterOutsideBounds(IFormFile json)
        {
            string fileContent = ReadFileContent(json);
            IList<SplitData> measurements = ParseSplitData(fileContent);
            IList<SplitData> filteredData = FilterDataOutsideBounds(measurements);
            return filteredData;
        }

        private IList<SplitData> FilterDataOutsideBounds(IList<SplitData> measurements)
        {
            IList<SplitData> filteredData = new List<SplitData>();
            foreach (SplitData measurement in measurements)
            {
                if (IsMeasurementOutsideBounds(measurement))
                {
                    filteredData.Add(measurement);
                }
            }
            return filteredData;
        }

        private bool IsMeasurementOutsideBounds(SplitData measurement)
        {
            return (measurement.x != null && (measurement.x > 7650 || measurement.x < 300));
        }

        public JObject MedianFilter(IFormFile file)
        {
            try
            {
                var json = new StreamReader(file.OpenReadStream()).ReadToEnd();

                var jObject = JObject.Parse(json);
                var measurements = jObject["measurements"].ToArray();

                for (int i = 0; i < measurements.Length; i += 3)
                {
                    if (i + 2 >= measurements.Length)
                    {
                        // If we've reached the end of the list, there aren't enough
                        // measurements left to calculate a median for this group of 3.
                        break;
                    }

                    var valuesX = new[] { (double?)measurements[i]["x"], (double?)measurements[i + 1]["x"], (double?)measurements[i + 2]["x"] };
                    var valuesY = new[] { (double?)measurements[i]["y"], (double?)measurements[i + 1]["y"], (double?)measurements[i + 2]["y"] };
                    var valuesZ = new[] { (double?)measurements[i]["z"], (double?)measurements[i + 1]["z"], (double?)measurements[i + 2]["z"] };

                    var sortedX = valuesX.OrderBy(v => v).ToArray();
                    var sortedY = valuesY.OrderBy(v => v).ToArray();
                    var sortedZ = valuesZ.OrderBy(v => v).ToArray();

                    var medianX = sortedX[1];
                    var medianY = sortedY[1];
                    var medianZ = sortedZ[1];

                    measurements[i]["x"] = valuesX.Max() == medianX ? medianX : (double?)null;
                    measurements[i]["y"] = valuesY.Max() == medianY ? medianY : (double?)null;
                    measurements[i]["z"] = valuesZ.Max() == medianZ ? medianZ : (double?)null;

                    measurements[i + 1]["x"] = valuesX.Max() == medianX ? medianX : (double?)null;
                    measurements[i + 1]["y"] = valuesY.Max() == medianY ? medianY : (double?)null;
                    measurements[i + 1]["z"] = valuesZ.Max() == medianZ ? medianZ : (double?)null;

                    measurements[i + 2]["x"] = valuesX.Max() == medianX ? medianX : (double?)null;
                    measurements[i + 2]["y"] = valuesY.Max() == medianY ? medianY : (double?)null;
                    measurements[i + 2]["z"] = valuesZ.Max() == medianZ ? medianZ : (double?)null;
                }

                jObject["measurements"] = new JArray(measurements);
                return jObject;
            }
            catch (Exception ex)
            {
                return new JObject(ex.Message);
            }
        }
    }
}
