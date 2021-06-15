using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Threading.Tasks;

namespace StructuredLogGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider(args);
            var generator = serviceProvider.GetRequiredService<ILogGenerator>();
            await generator.Generate(100);

            Console.WriteLine("Log generated.");
        }

        private static IServiceProvider BuildServiceProvider(string[] args)
        {
            Log.Logger = new LoggerConfiguration().BuildLogger(args);

            var services = new ServiceCollection();

            services
                .AddLogging(logBuilder => logBuilder.AddSerilog(dispose: true))
                .AddSingleton<ILogGenerator, LogGenerator>();

            return services.BuildServiceProvider();
        }
    }

    public static class InjectionExtensions
    {
        public static ILogger BuildLogger(this LoggerConfiguration builder, string[] args)
        {
            LogSink logSink = LogSink.Console;
            if (args.Length == 1)
            {
                Enum.TryParse(args[0], out logSink);
            }

            builder = logSink switch
            {
                LogSink.File => builder.WriteTo.File(new JsonFormatter(), "C:/structured.log"),
                LogSink.Seq => builder.WriteTo.Seq("http://localhost:5341/"),
                _ => builder.WriteTo.Console(new JsonFormatter())
            };

            return builder.CreateLogger();
        }
    }

    public enum LogSink
    {
        Console,
        File,
        Seq
    }
}
