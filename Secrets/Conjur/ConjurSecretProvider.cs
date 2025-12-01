using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Conjur;
using Microsoft.Extensions.Configuration;
using System.Net;
using System;

namespace Application2.Secrets.Conjur
{
    public interface IConjurSecretProvider
    {
        Task<string> GetVariableAsync(string secretPath);
    }

    public class ConjurSecretProvider : IConjurSecretProvider
    {

        private readonly ILogger<ConjurSecretProvider> _logger;
        private readonly IConfiguration _config;
        private readonly Client _client;

        public ConjurSecretProvider(IConfiguration config, ILogger<ConjurSecretProvider> logger)
        {
            this._logger = logger;
            this._config = config;
            var applianceUrl = _config["Secrets:Conjur:Config:ApplianceUrl"];
            var account = _config["Secrets:Conjur:Config:Account"];
            var login = config["Secrets:Conjur:Config:Login"] ?? Environment.GetEnvironmentVariable("CONJUR_AUTHN_LOGIN");
            var apiKey = config["Secrets:Conjur:Config:ApiKey"] ?? Environment.GetEnvironmentVariable("CONJUR_AUTHN_API_KEY");
            this._client = new Client(applianceUrl, account)
            {
                Credential = new NetworkCredential(login, apiKey)
            };
            
        }

        public async Task<string> GetVariableAsync(string secretPath)
        {
            _logger.LogInformation("Retreiving secret at path {secretPath}", secretPath);
            var variable = _client.Variable(secretPath);
            return await variable.GetValueAsync();
        }
    }
}