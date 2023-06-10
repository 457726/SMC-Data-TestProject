using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMC_Data.Interfaces;
using SMC_Data.Logic;

namespace SMC_Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationsController : Controller
    {
        private readonly ICalculationsLogic calculationsLogic;

        public CalculationsController(ICalculationsLogic calculations)
        {
            calculationsLogic = calculations;
        }

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

        [HttpPut("~/CalculateHighSpeed")]
        public JsonResult CalculateHighSpeed(IFormFile json)
        {
            var highSpeed = calculationsLogic.CalculateHighestSpeed(json);
            return Json(highSpeed);
        }
    }
}
