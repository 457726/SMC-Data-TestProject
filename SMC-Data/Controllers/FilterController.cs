using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SMC_Data.Logic;
using SMC_Data.Models;
using System.Text.Json;

namespace SMC_Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : Controller
    {
        FilterLogic filterLogic = new();

        [HttpPut("~/TestReturnedData")]
        public JsonResult TestData(IFormFile json)
        {
            var data = filterLogic.TestData(json);
            return Json(data);
        }

        [HttpPut("~/FilterMultiplesZ")]
        public JsonResult FilterOutMultiplesZ(IFormFile json)
        {
            var results = filterLogic.FilterOutMultiplesZ(json);
            return Json(results);
        }
        [HttpPut("~/FilterMultiplesX")]
        public JsonResult FilterOutMultiplesX(IFormFile json)
        {
            var results = filterLogic.FilterOutMultiplesX(json);
            return Json(results);
        }
        [HttpPut("~/FilterMultiplesY")]
        public JsonResult FilterOutMultiplesY(IFormFile json)
        {
            var results = filterLogic.FilterOutMultiplesY(json);
            return Json(results);
        }
        [HttpPut("~/FilterBounds")]
        public JsonResult FilterOutsideBounds(IFormFile json)
        {
            var outsidebounds = filterLogic.FilterOutsideBounds(json);
            return Json(outsidebounds);
        }

        [HttpPatch("~/MedianFilter")]
        public JsonResult MedianFilter(IFormFile file)
        {
            var result = filterLogic.MedianFilter(file);
            return Json(result);
        }
    }
}
