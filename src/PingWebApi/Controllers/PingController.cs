using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;

namespace PingWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {        
        private readonly ILogger<PingController> _logger;
        private readonly string _environmentName;

        public PingController(ILogger<PingController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _environmentName = configuration.GetValue<string>("EnvironmentName");
        }

        [HttpGet]
        public PingResult Get()
        {
            var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
            var ip = feature?.LocalIpAddress?.ToString();
                       
            var osNameAndVersion = "";
            var assemblyFileVersion = "";
            try
            {
                assemblyFileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToString();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
                    
            var result = new PingResult
            {
                Host = osNameAndVersion,
                HostName = Environment.MachineName,
                HostIp = ip,
                SystemTime = DateTime.UtcNow.ToString(),
                Version = assemblyFileVersion,
                EnvironmentName = _environmentName
            };

            return result;
        }
    }

    public struct PingResult
    {
        public string HostName { get; internal set; }
        public string HostIp { get; internal set; }
        public string SystemTime { get; internal set; }
        public string Host { get; internal set; }
        public string Version { get; internal set; }
        public string EnvironmentName { get; internal set; }
    }
}
