// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NexusDuplette.Core;
using NLog;
using NLog.Extensions.Logging;
using ILogger = NLog.ILogger;


var logger = LogManager.GetCurrentClassLogger();

try
{
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json", true, true)
        .Build();

    if (Environment.CurrentDirectory.Contains("bin"))
    {
        
    }

    var allowedPaths = config.GetRequiredSection("allowedPaths").Get<IEnumerable<string>>().ToList();

    await using var servicesProvider = new ServiceCollection()
        .AddScoped<IDuplettenFinder, DuplettenFinder>()
        .AddSingleton<ILogger, Logger>()
        .AddLogging(loggingBuilder =>
        {
            // configure Logging with NLog
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog(config);
        }).BuildServiceProvider();

    logger.Info("# # # # Start von DuplettenFinder # # # #");
    logger.Debug("Logger initialisiert");

    var service = servicesProvider.GetRequiredService<IDuplettenFinder>();
    logger.Debug("Service {service} initialisiert", service.GetType());
    logger.Info("Starte Duplettensuche");
    

    if (allowedPaths.Count < 1)
    {
        logger.Warn("Alles erledigt, es wurden keine Pfade angegeben");
        
    }
    else
    {
        logger.Debug("Es wurden {anzahl} Pfade zum Analysieren angegeben", allowedPaths.Count);
        service.StartDuplettenMatching(allowedPaths);
    }

    logger.Info("Beende Duplettenfinder");
}
catch (Exception ex)
{
    // NLog: catch any exception and log it.
    logger.Error(ex, "Stopped program because of exception");
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
