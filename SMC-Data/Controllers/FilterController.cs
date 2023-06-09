﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SMC_Data.Interfaces;
using SMC_Data.Logic;
using SMC_Data.Models;
using System.Text.Json;

namespace SMC_Data.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : Controller
    {
        private readonly IFilterLogic _filterLogic;
        public FilterController(IFilterLogic filterLogic)
        {
            _filterLogic = filterLogic;
        }

        [HttpPut("~/FilterMultiplesZ")]
        public JsonResult FilterOutMultiplesZ(IFormFile json)
        {
            var results = _filterLogic.FilterOutMultiplesZ(json);
            return Json(results);
        }
        [HttpPut("~/FilterMultiplesX")]
        public JsonResult FilterOutMultiplesX(IFormFile json)
        {
            var results = _filterLogic.FilterOutMultiplesX(json);
            return Json(results);
        }
        [HttpPut("~/FilterMultiplesY")]
        public JsonResult FilterOutMultiplesY(IFormFile json)
        {
            var results = _filterLogic.FilterOutMultiplesY(json);
            return Json(results);
        }
        [HttpPut("~/FilterOutsideBounds")]
        public JsonResult FilterOutsideBounds(IFormFile json)
        {
            var outsidebounds = _filterLogic.FilterOutsideBounds(json);
            return Json(outsidebounds);
        }

        [HttpPut("~/MedianFilter")]
        public ContentResult MedianFilter(IFormFile file)
        {
            var result = _filterLogic.MedianFilter(file);
            return Content(result.ToString(), "application/json");
        }
    }
}
