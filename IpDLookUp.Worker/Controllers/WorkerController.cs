using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using IpDLookUp.Services;
using IpDLookUp.Services.Models;
using IpDLookUp.Services.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IpDLookUp.Worker.Controllers
{
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        private ILogger<WorkerController> _logger;

        public WorkerController(ILogger<WorkerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> HandleServiceRequest([FromRoute] string address, ServiceType type)
        {
            _logger.LogInformation($"Processing {type} for {address}");
            // not a fan of the controller having to know the specific
            // types for search service
            return type switch
            {
                ServiceType.GeoIP => new OkObjectResult(await ServiceProcessor.Process<GeoIpModel>(address, type)),
                ServiceType.RDAP => new OkObjectResult(await ServiceProcessor.Process<RdapModel>(address, type)),
                ServiceType.ReverseDNS =>
                    new OkObjectResult(await ServiceProcessor.Process<IPHostEntry>(address, type)),
                ServiceType.SslLabs => new OkObjectResult(await ServiceProcessor.Process<SslLabsModel>(address, type)),
                ServiceType.Ping => new OkObjectResult(await ServiceProcessor.Process<PingModel>(address, type)),
                _ => new BadRequestObjectResult(new ServiceResult<string>
                {
                    Status = ServiceStatus.Bad,
                    Type = type,
                    ErrorMessage = $"Invalid type passed in. Expected a {nameof(ServiceType)} but got {type}"
                })
            };
        }
    }
}