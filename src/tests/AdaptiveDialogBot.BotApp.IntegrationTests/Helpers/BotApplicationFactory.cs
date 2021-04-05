using AdaptiveDialogBot.BotApp.IntegrationTests.Helpers;
using AdaptiveDialogsBot.BotApp;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdaptiveDialogBot.BotApp.IntegrationTests.EchoBotTests
{
    public class BotApplicationFactory : WebApplicationFactory<Startup>
    {
        private bool _serverStarted;

        protected override IHost CreateHost(IHostBuilder builder)
        {
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

        private void EnsureServerStarted()
        {
            if (_serverStarted) return;

            CreateClient();
            _serverStarted = true;
        }
    }
}
