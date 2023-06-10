using SMC_Data.Models;

namespace SMC_Data.Interfaces
{
    public interface ICalculationsLogic
    {
        double CalculateAverageSpeed(IFormFile file);
        double CalculateDistanceCovered(IFormFile file);
        double CalculateHighestSpeed(IFormFile file);
        public MetricStats AllCalculations(IFormFile file);
    }
}