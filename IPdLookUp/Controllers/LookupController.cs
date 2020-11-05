using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPdLookUp.Entities;
using IPdLookUp.Models;
using IPdLookUp.Types;
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
            if (request.Services == null)
                request.Services = new List<LookUpService>
                {
                    LookUpService.GeoIP,
                    LookUpService.RDAP
                };

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(new AppErrorResult
                {
                    ErrorMessage = "Error processing request.",
                    Errors = ModelState.FirstOrDefault().Value.Errors,
                    FailServices = request.Services
                });

            var res = new AppResult
            {
                Address = request.Address,
                Results = new List<LookUpResult>()
            };

            foreach (var service in request.Services)
            {
                // TODO process request for service
                res.Results.Add(await ServiceProcessor.Process(request.Address, service));
            }


            return new OkObjectResult(res);
        }
    }
}