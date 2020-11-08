using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPdLookUp.Entities;
using IpDLookUp.Services;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IPdLookUp.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LookupController : ControllerBase
    {
        private ILogger<LookupController> _logger;

        public LookupController(ILogger<LookupController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        [ProducesResponseType(typeof(AppResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppPartialResult), StatusCodes.Status206PartialContent)]
        [ProducesResponseType(typeof(IAppErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RunTasks([FromBody] LookUpRequest request)
        {

            var badReq = CheckModelState(request);
            if (badReq != null)
                return badReq;

            var res = new AppResult
            {
                Address = request.Address,
                Services = SetDefaultServicesIfNull(request.Services),
            };

            // TODO catch for partial result

            // run in parallel
            var pendingResults = res.Services.Select(service => ServiceProcessor.Process(request.Address, service));

            res.Results = (await Task.WhenAll(pendingResults))
                .ToList();

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