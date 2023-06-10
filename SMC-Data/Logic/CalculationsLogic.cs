using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SMC_Data.Interfaces;
using SMC_Data.Models;

namespace SMC_Data.Logic
{
    public class CalculationsLogic : ICalculationsLogic
    {
        private List<SplitData> ProcessFile(IFormFile file)
        {
            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var jsonString = streamReader.ReadToEnd();
                var jsonObject = JObject.Parse(jsonString);
                var measurementsArray = jsonObject["measurements"];
                var splitDataList = measurementsArray.ToObject<List<SplitData>>();
                var filteredList = splitDataList
                    .Where(data => data.x.HasValue && data.y.HasValue)
                    .ToList();

                return filteredList;
            }
        }
        public MetricStats AllCalculations(IFormFile file)
        {
            var results = new MetricStats();
            results.DistanceCovered = CalculateDistanceCovered(file);
            results.AverageSpeed = CalculateAverageSpeed(file);
            results.HighestSpeed = CalculateHighestSpeed(file);
            return results;
        }
        private double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public double CalculateDistanceCovered(IFormFile file)
        {
            var data = ProcessFile(file);

            if (data.Count < 2)
            {
                throw new InvalidOperationException("Insufficient data points to calculate distance.");
            }

            double totalDistance = 0;
            for (int i = 0; i < data.Count - 1; i++)
            {
                var currentPoint = data[i];
                var nextPoint = data[i + 1];

                var distance = CalculateDistance(currentPoint.x.Value, currentPoint.y.Value, nextPoint.x.Value, nextPoint.y.Value);

                totalDistance += distance;
            }

            return (totalDistance * 0.001);
        }

        public double CalculateAverageSpeed(IFormFile file)
        {
            var data = ProcessFile(file);

            if (data.Count < 2)
            {
                throw new InvalidOperationException("Insufficient data points to calculate average speed.");
            }

            var totalDistance = CalculateDistanceCovered(file);
            var totalTime = (data[data.Count - 1].t.Value - data[0].t.Value).TotalSeconds;
            var averageSpeed = (totalDistance / totalTime) * 3.6;

            return averageSpeed;
        }

        public double CalculateHighestSpeed(IFormFile file)
        {
            var data = ProcessFile(file);

            if (data.Count < 2)
            {
                throw new InvalidOperationException("Insufficient data points to calculate highest speed.");
            }

            double highestSpeed = 0;

            for (int i = 0; i < data.Count - 1; i++)
            {
                var currentPoint = data[i];
                var nextPoint = data[i + 1];

                var distance = CalculateDistance(currentPoint.x.Value, currentPoint.y.Value, nextPoint.x.Value, nextPoint.y.Value) * 0.001;
                var time = (nextPoint.t.Value - currentPoint.t.Value).TotalSeconds;
                var speed = distance / time;

                if (speed > highestSpeed)
                {
                    highestSpeed = speed;
                }
            }

            return highestSpeed;
        }
    }

}
