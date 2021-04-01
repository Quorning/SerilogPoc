using Contract;
using Contract.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using Domain.Serivces;

namespace WebApiWithSeriLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ITestLogningService _testLogningService;

        public LogController(ITestLogningService testLogningService)
        {
            _testLogningService = testLogningService ?? throw new ArgumentNullException(nameof(testLogningService));
        }

        // POST api/<LogrController>
        [HttpPost]
        public LogResponse TestLogning([FromBody] LogRequest requestvalue)
        {
            _testLogningService.TestLog();

            return new LogResponse();
        }
    }
}
