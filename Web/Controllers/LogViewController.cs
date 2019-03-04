using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Elasticsearch.Net;
using System;
using Microsoft.Extensions.Options;
using Web.Log;
using Web.Dto;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogViewController : ControllerBase
    {
        private readonly ILogViewRepository logViewRepository;
        public LogViewController(ILogViewRepository logViewRepository)     
        {
            this.logViewRepository = logViewRepository;        
        }

        [HttpGet("Overall")]
        public IActionResult Overall()
        {
            var aggregatedValues = this.logViewRepository.AggregatedValuesForAll();
            var logAggregatedValueDto = new LogAggregatedValueDto()
            {
                TotalCountOfAllRequest = this.logViewRepository.Total().Value,
                TotalCountOfNormal = this.logViewRepository.CountOnLevel("information"),                      
                TotalCountError = this.logViewRepository.CountOnLevel("error"),
                AverageDurationForAllRequest = aggregatedValues.AverageDurationForAllRequest,
                MaxDurationForAllRequest = aggregatedValues.MaxDurationForAllRequest,
                MinDurationForAllRequest = aggregatedValues.MinDurationForAllRequest,
                CountOfEachErrors = this.logViewRepository.GetErrorSet()   
            };

            return Ok(logAggregatedValueDto);                                      
        } 
    }
}