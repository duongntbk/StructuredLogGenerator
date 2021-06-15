using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace StructuredLogGenerator
{
    public class LogGenerator : ILogGenerator
    {
        private readonly Random _rs;

        private readonly ILogger<LogGenerator> _logger;

        public LogGenerator(ILogger<LogGenerator> logger)
        {
            _rs = new Random(42);
            _logger = logger;
        }

        public async Task Generate(int logCount)
        {
            for (var i = 0; i < logCount; i++)
            {
                var (resourceName, resourcePercentage, nodeName, delay) =
                    (GetRandomResourceName(), GetRandomResourcePercentage(), GetRandomNodeName(), GetRandomDelay());

                var logLevel = resourcePercentage > 50 ? LogLevel.Warning : LogLevel.Information;

                _logger.Log(logLevel,
                    "{ResourceName} percentage exceeded {ResourcePercentage}% for node {NodeName}",
                    resourceName, resourcePercentage, nodeName);

                await Task.Delay(delay);
            }
        }

        private string GetRandomResourceName() => _rs.Next(0, 2) switch
        {
            0 => "CPU Usage",
            _ => "Memory Usage"
        };

        private int GetRandomResourcePercentage() => _rs.Next(0, 101);

        private string GetRandomNodeName() => _rs.Next(0, 6) switch
        {
            0 => "NodeA",
            1 => "NodeB",
            2 => "NodeC",
            3 => "NodeD",
            4 => "NodeE",
            _ => "NodeF"
        };

        private int GetRandomDelay() => _rs.Next(1, 1001);
    }
}
