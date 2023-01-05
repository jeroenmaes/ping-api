using AspNetCore.Authentication.Basic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PingWebApi.Services
{
    public class BasicUserValidationService : IBasicUserValidationService
    {
        private readonly ILogger<BasicUserValidationService> _logger;
        private readonly string _apiUserName;
        private readonly string _apiUserPassword;

        public BasicUserValidationService(ILogger<BasicUserValidationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiUserName = configuration.GetValue<string>("ApiUser");
            _apiUserPassword = configuration.GetValue<string>("ApiUserPassword");
        }

        public async Task<bool> IsValidAsync(string username, string password)
        {
            try
            {
                var isValid = username == _apiUserName && password == _apiUserPassword;
                return isValid;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}