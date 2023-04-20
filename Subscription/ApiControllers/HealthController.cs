using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skad.Subscription.Data.Model;

namespace Skad.Subscription.ApiControllers;

[ApiController]
[Route("api/health")]
public class HealthController : Controller
{
    [HttpGet]
    [Route("liveness")]
    public ActionResult<string> GetLiveness()
    {
        var fail = Environment.GetEnvironmentVariable("LIVENESS_FAIL");

        if ((fail ?? "false") == "true")
        {
            return StatusCode(500, new
            {
                Status="service is unhealthy"
            });
        }

        var timeout = Environment.GetEnvironmentVariable("LIVENESS_TIMEOUT");

        if ((timeout ?? "false") == "true")
        {
            Thread.Sleep(60 * 1000); // Sleep for 60 seconds.
        }

        return Ok(new
        {
            Status = "ok"
        });
    }
}
