using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SMC_Data.Interfaces;
using SMC_Data.Models;

namespace SMC_Data.Logic
{
    public class CalculationsLogic : ICalculationsLogic
    {
        public List<SplitData> ProcessFile(IFormFile file)
        {
            // Read the file content
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var jsonString = streamReader.ReadToEnd();

                var jsonObject = JObject.Parse(jsonString);

                // Get the "measurements" field from the JSON object
                var measurementsArray = jsonObject["measurements"];

                // Deserialize the "measurements" array into a list of SplitData objects
                var splitDataList = measurementsArray.ToObject<List<SplitData>>();

                // Make sure only valid data points are used
                var filteredList = splitDataList
                    .Where(data => data.x.HasValue && data.y.HasValue)
                    .ToList();

                return filteredList;
            }
        }

        public double CalculateDistanceCovered(IFormFile file)
        {
            var data = ProcessFile(file);

            // Calculate the total distance traveled
            double totalDistance = 0;
            for (int i = 0; i < data.Count - 1; i++)
            {
                var currentPoint = data[i];
                var nextPoint = data[i + 1];

                // Calculate the distance between consecutive points
                var distance = Math.Sqrt(Math.Pow(nextPoint.x.Value - currentPoint.x.Value, 2) + Math.Pow(nextPoint.y.Value - currentPoint.y.Value, 2));

                // Add the distance to the total
                totalDistance += distance;
            }
            return (totalDistance * 0.001);
        }

        public double CalculateAverageSpeed(IFormFile file)
        {
            var data = ProcessFile(file);

            // Calculate the total distance traveled
            var totalDistance = CalculateDistanceCovered(file);

            // Calculate the total time taken (assuming timestamps are in seconds)
            var totalTime = (data[data.Count - 1].t.Value - data[0].t.Value).TotalSeconds;

            // Calculate the average speed and make it Km/h instead of m/s
            var averageSpeed = (totalDistance / totalTime) * 3.6;

            return averageSpeed;
        }
    }
}
