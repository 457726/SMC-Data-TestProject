using SMC_Data.Models;

namespace SMC_Data.Interfaces
{
    public interface ICalculationsLogic
    {
        double CalculateAverageSpeed(IFormFile file);
        double CalculateDistanceCovered(IFormFile file);
        List<SplitData> ProcessFile(IFormFile file);
    }
}