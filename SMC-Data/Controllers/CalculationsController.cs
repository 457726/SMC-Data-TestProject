using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMC_Data.Logic;

namespace SMC_Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationsController : Controller
    {
        CalculationsLogic calculationsLogic = new();

        [HttpPut("~/CalculateDistanceCovered")]
        public JsonResult CalculateDistanceCovered(IFormFile json)
        {
            var distance = calculationsLogic.CalculateDistanceCovered(json);
            return Json(distance);
        }

        [HttpPut("~/CalculateAvgSpeed")]
        public JsonResult CalculateAvgSpeed(IFormFile json)
        {
            var avgSpeed = calculationsLogic.CalculateAverageSpeed(json);
            return Json(avgSpeed);
        }
    }
}
