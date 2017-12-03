using StructureMap;
using Serilog;
using Serilog.Filters;
using Serilog.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Request.Validator
{
    public class LoggingRegistry: Registry
    {
        public LoggingRegistry()
        {
            For<ILogger>()
            .Singleton()
            .Use(context => CreateLogger());
        }
        public ILogger CreateLogger()
        {
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config  = configurationBuilder.Build();  
            var logger =  new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()               
                .Enrich.WithProperty("ApplicationName", @"TPP.CLI")
                .Enrich.WithProperty("RequestId", System.Guid.NewGuid().ToString())            
                .CreateLogger();           
            return logger;
        }
    }
}