using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Application2.ServiceLayer;
using Microsoft.AspNetCore.Hosting;
using Application2.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Application2.Secrets.Conjur;

namespace Application2
{

    public class Program
    {
        public static void Main(string[] args)
        {

            var initialConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var secretProvider = initialConfig["Secrets:Manager"] ?? "none";

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            switch (secretProvider)
            {
                case "conjur":
                {
                    builder.Services.AddSingleton<IConjurSecretProvider, ConjurSecretProvider>();
                    builder.Services.AddSingleton<IConnectionStringFactory, ConjurSecretManagedConnectionStringFactory>();
                    break;
                }
                default:
                {
                    builder.Services.AddSingleton<IConnectionStringFactory, DefaultConnectionStringFactory>();
                    break;
                }
            }

            builder.Services.AddSingleton<IAppDbContextFactory, ApplicationDbContextFactory>();
            builder.Services.AddScoped<ITodoService, TodoService>();
            builder.WebHost.ConfigureKestrel((context, options) =>
            {
                options.Configure(context.Configuration.GetSection("Kestrel"));
            });
            var app = builder.Build();
            app.MapControllers();
            app.Run();


        }
    }

}