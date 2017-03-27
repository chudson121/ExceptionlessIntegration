using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionlessIntegration.CommandLine;
using ExceptionlessIntegration.Logging.Services;
using NLog;
using ILogger = ExceptionlessIntegration.Logging.Interfaces.ILogger;

namespace ExceptionlessIntegration
{
    public class Program
    {
        private static readonly ILogger Logger = LoggingService.GetLoggingService();
        private static CommandLineProcessor _clp;

        public static void Main(string[] args)
        {
            Logger.Info("Program startup");
            ConfigureCommandLIne(args);
            var exceptionlessConfiguration = new ExceptionlessConfig()
            {
                BaseUrl = System.Configuration.ConfigurationManager.AppSettings["exceptionless:BaseUrl"],
                AccountEmail = System.Configuration.ConfigurationManager.AppSettings["exceptionless:Username"],
                AccountPassword = System.Configuration.ConfigurationManager.AppSettings["exceptionless:Password"],
                OrganizationId = System.Configuration.ConfigurationManager.AppSettings["exceptionless:OrgId"]
            };

            //identify what to do from the command line options

            var eProcessor = new ExceptionlessProcesser(exceptionlessConfiguration);
            //eProcessor.GetProjects();

            eProcessor.CreateProject(new ProjectDto(){name = "test"});

            Console.ReadLine();

        }


        private static void ConfigureCommandLIne(string[] args)
        {
            Logger.Info("Configure Command Line Settings");
            _clp = new CommandLineProcessor(args, Logger);
            Logger.Info("Debug Mode:{0}", _clp.Options.IsDebug);

        }

    }
}
