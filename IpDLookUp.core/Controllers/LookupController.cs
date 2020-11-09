using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IPdLookUp.Core.Entities;
using IPdLookUp.Core.Models;
using IpDLookUp.Services;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IPdLookUp.Core.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LookupController : ControllerBase
    {
        private readonly ILogger<LookupController> _logger;

        private readonly IConfiguration _config;

        public LookupController(ILogger<LookupController> logger, IConfiguration config)
        {
            _logger = logger;

            if (config["WorkerAddress"] != null)
            {
                _config = config;
            }
            else
            {
                _logger.LogError("Missing worker address from configuration");
                throw new ArgumentException(
                    "Expected worker address in config. Make sure there is a configured worker address in appSettings");
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(AppResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppPartialResult), StatusCodes.Status206PartialContent)]
        [ProducesResponseType(typeof(IAppErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RunTasks([FromBody] LookUpRequest request)
        {
            _logger.LogInformation(
                $"New Request with {request.Services?.Count ?? 0} selected services for {request.Address}");
            var badReq = CheckModelState(request);
            if (badReq != null)
                return badReq;

            var services = SetDefaultServicesIfNull(request.Services);

            var res = await WorkerHelper
                .SendToWorkers(_config["WorkerAddress"], request.Address, services);


            return new OkObjectResult(res);
        }

        private IActionResult? CheckModelState(LookUpRequest request)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(new AppErrorResult
                {
                    ErrorMessage = "Error processing request.",
                    Errors = ModelState.FirstOrDefault().Value.Errors,
                    FailServices = SetDefaultServicesIfNull(request.Services)
                });

            return null;
        }

        private List<ServiceType> SetDefaultServicesIfNull(List<ServiceType>? serviceTypes)
        {
            return serviceTypes ?? new List<ServiceType>
            {
                ServiceType.GeoIP,
                ServiceType.Ping,
            };
        }
    }
}