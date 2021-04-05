using AdaptiveDialogBot.BotApp.IntegrationTests.Helpers;
using AdaptiveDialogsBot.BotApp;
using Destructurama;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace AdaptiveDialogBot.BotApp.IntegrationTests.EchoBotTests
{
    public class BotApplicationFactory : WebApplicationFactory<Startup>
    {
        private bool _serverStarted;
        private bool _disposed;

        protected override IHost CreateHost(IHostBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Destructure.JsonNetTypes()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            builder.UseSerilog();

            builder.ConfigureServices(services =>
            {
                services.AddSingleton<TestAdapter, BotApplicationTestAdapter>();
            });

            return base.CreateHost(builder);
        }

        public IServiceScope CreateScope()
        {
            EnsureServerStarted();
            return Services.CreateScope();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Log.CloseAndFlush();
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        private void EnsureServerStarted()
        {
            if (_serverStarted) return;

            CreateClient();
            _serverStarted = true;
        }
    }
}
