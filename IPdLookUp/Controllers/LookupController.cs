using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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


        [HttpGet("{address}")]
        [ProducesResponseType(typeof(LookUpResults), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LookUpErrors), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RunTasks([FromRoute] string address, LookUpService[] services = null)
        {
            if (services == null)
                services = new[]
                {
                    LookUpService.GeoIP,
                    LookUpService.RDAP
                };

            return new OkObjectResult(new LookUpResults
            {
                Address = address,
                Results = new List<object>()
            });
        }
    }

    public enum LookUpService
    {
        GeoIP,
        RDAP,
        ReverseDNS,
    }

    public struct LookUpResults
    {
        public List<object> Results;

        public string Address;
    }

    public struct LookUpResult
    {
        public LookUpService Service;

        public object Data;
    }

    public struct LookUpErrors
    {
        public string Address;

        public List<LookUpService> Services;

        public string ErrorMessage;
    }
}