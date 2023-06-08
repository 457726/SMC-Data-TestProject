using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SMC_Data.Models;
using System.Text.Json;

namespace SMC_Data.Logic
{
    public class FilterLogic
    {
        public FilterLogic()
        {

        }
        public IList<SplitData> TestData(IFormFile json)
        {
            string fileContent = "";
            using (var reader = new StreamReader(json.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }
            JObject banaan = JObject.Parse(fileContent);
            IList<JToken> result = banaan["measurements"].Children().ToList();
            IList<SplitData> splitData = new List<SplitData>();
            foreach (JToken o in result)
            {
                SplitData searchResult = o.ToObject<SplitData>();
                splitData.Add(searchResult);
            }
            return splitData;
        }

        public List<string> FilterOutMultiplesZ(IFormFile json)
        {
            string fileContent = "";
            List<string> results = new List<string>();
            int[] numbers = new int[100000];
            using (var reader = new StreamReader(json.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }
            JObject measurements = JObject.Parse(fileContent);
            IList<JToken> result = measurements["measurements"].Children().ToList();
            IList<SplitData> splitData = new List<SplitData>();
            foreach (JToken o in result)
            {
                SplitData searchResult = o.ToObject<SplitData>();
                splitData.Add(searchResult);
            }
            var d = new Dictionary<int, int>();
            int index = 0;
            foreach (var item in splitData)
            {
                if (item.z != null)
                {
                    numbers[index] = Convert.ToInt32(item.z);
                    index++;
                }
            }
            Array.Sort(numbers);
            foreach (var item in numbers)
            {
                if (d.ContainsKey(item) && item != 0)
                {
                    d[item]++;
                }
                else
                {
                    d[item] = 1;
                }
            }
            foreach (var val in d)
            {
                string res = val.Key + " occured " + val.Value + " times";
                results.Add(res);
            }
            results.RemoveAt(0);
            return results;
        }
        public List<string> FilterOutMultiplesX(IFormFile json)
        {
            string fileContent = "";
            List<string> results = new List<string>();
            int[] numbers = new int[100000];
            using (var reader = new StreamReader(json.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }
            JObject measurements = JObject.Parse(fileContent);
            IList<JToken> result = measurements["measurements"].Children().ToList();
            IList<SplitData> splitData = new List<SplitData>();
            foreach (JToken o in result)
            {
                SplitData searchResult = o.ToObject<SplitData>();
                splitData.Add(searchResult);
            }
            var d = new Dictionary<int, int>();
            int index = 0;
            foreach (var item in splitData)
            {
                if (item.x != null)
                {
                    numbers[index] = Convert.ToInt32(item.x);
                    index++;
                }
            }
            Array.Sort(numbers);
            foreach (var item in numbers)
            {
                if (d.ContainsKey(item) && item != 0)
                {
                    d[item]++;
                }
                else
                {
                    d[item] = 1;
                }
            }
            foreach (var val in d)
            {
                string res = val.Key + " occured " + val.Value + " times";
                results.Add(res);
            }
            results.RemoveAt(0);
            return results;
        }
        public List<string> FilterOutMultiplesY(IFormFile json)
        {
            string fileContent = "";
            List<string> results = new List<string>();
            int[] numbers = new int[100000];
            using (var reader = new StreamReader(json.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }
            JObject measurements = JObject.Parse(fileContent);
            IList<JToken> result = measurements["measurements"].Children().ToList();
            IList<SplitData> splitData = new List<SplitData>();
            foreach (JToken o in result)
            {
                SplitData searchResult = o.ToObject<SplitData>();
                splitData.Add(searchResult);
            }
            var d = new Dictionary<int, int>();
            int index = 0;
            foreach (var item in splitData)
            {
                if (item.y != null)
                {
                    numbers[index] = Convert.ToInt32(item.y);
                    index++;
                }
            }
            Array.Sort(numbers);
            foreach (var item in numbers)
            {
                if (d.ContainsKey(item) && item != 0)
                {
                    d[item]++;
                }
                else
                {
                    d[item] = 1;
                }
            }
            foreach (var val in d)
            {
                string res = val.Key + " occured " + val.Value + " times";
                results.Add(res);
            }
            results.RemoveAt(0);
            return results;
        }
        public IList<SplitData> FilterOutsideBounds(IFormFile json)
        {
            string fileContent = "";

            using (var reader = new StreamReader(json.OpenReadStream()))

            {
                fileContent = reader.ReadToEnd();
            }

            JObject measurements = JObject.Parse(fileContent);

            IList<JToken> result = measurements["measurements"].Children().ToList();

            IList<SplitData> splitData = new List<SplitData>();

            foreach (JToken o in result)
            {
                SplitData measurement = o.ToObject<SplitData>();
                if (measurement.x != null && measurement.x > 7650 || measurement.x < 300)
                {
                    splitData.Add(measurement);
                }
            }
            return splitData;
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
