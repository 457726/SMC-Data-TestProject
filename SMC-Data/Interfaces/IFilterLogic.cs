using Newtonsoft.Json.Linq;
using SMC_Data.Models;

namespace SMC_Data.Interfaces
{
    public interface IFilterLogic
    {
        List<string> FilterOutMultiplesX(IFormFile json);
        List<string> FilterOutMultiplesY(IFormFile json);
        List<string> FilterOutMultiplesZ(IFormFile json);
        IList<SplitData> FilterOutsideBounds(IFormFile json);
        JObject MedianFilter(IFormFile file);
        IList<SplitData> TestData(IFormFile json);
    }
}