using System;
using System.Threading.Tasks;
using Application2.DataLayer;
using Microsoft.Extensions.Configuration;

namespace Application2.Secrets.Conjur
{

    public class ConjurSecretManagedConnectionStringFactory : IConnectionStringFactory
    {
        private readonly IConfiguration _config;
        private readonly IConjurSecretProvider _secrets;

        public ConjurSecretManagedConnectionStringFactory(IConfiguration config, IConjurSecretProvider secrets)
        {
            _config = config;
            _secrets = secrets;
        }

        public async Task<string> BuildConnectionStringAsync(String connectionStringName)
        {
            // The database credentials will be fetched at each call
            // Consider doing some caching in order to avoid overloading conjur

            var connectionString = _config["ConnectionStrings:" + connectionStringName] ?? throw new InvalidOperationException("Connection string not found : " + connectionStringName);
            var usernamePath = _config["Secrets:Conjur:Provider:ConnectionStrings:" + connectionStringName + ":UserNamePath"] ?? throw new InvalidOperationException("No user path found for : " + connectionStringName);
            var passwordPath = _config["Secrets:Conjur:Provider:ConnectionStrings:" + connectionStringName + ":PasswordPath"] ?? throw new InvalidOperationException("No password path found for : " + connectionStringName);

            var username = await _secrets.GetVariableAsync(usernamePath);
            var password = await _secrets.GetVariableAsync(passwordPath);

            return $"{connectionString};Username={username};Password={password};";
        }
    }
}