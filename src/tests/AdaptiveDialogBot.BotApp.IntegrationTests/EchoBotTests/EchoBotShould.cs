using FluentAssertions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AdaptiveDialogBot.BotApp.IntegrationTests.EchoBotTests
{
    public class EchoBotShould : IClassFixture<BotApplicationFactory>, IDisposable
    {
        private bool _disposedValue;
        private readonly IServiceScope _scope;

        public EchoBotShould(BotApplicationFactory applicationFactory)
        {
            _scope = applicationFactory.CreateScope();
        }

        [Fact]
        public async Task Reply_back_whatever_it_receives()
        {
            // Arrange -----------------
            var testAdapter = GetService<TestAdapter>();
            testAdapter.Locale = "en-us";

            var bot = GetService<IBot>();
            var testFlow = new TestFlow(testAdapter, bot);

            // Act ---------------------
            await testFlow
                .SendConversationUpdate()
                .AssertReply("Hello and welcome!")
                .Send("Echo TEST!")
                .AssertReply(activity => activity.AsMessageActivity().Text.Should().Be("Echo: Echo TEST!"))
                .StartTestAsync();

            // Assert ------------------

        }

        [Fact]
        public async Task Greet_back_on_hello()
        {
            // Arrange -----------------
            var testAdapter = GetService<TestAdapter>();
            testAdapter.Locale = "en-us";

            var bot = GetService<IBot>();
            var testFlow = new TestFlow(testAdapter, bot);

            // Act ---------------------
            await testFlow
                .SendConversationUpdate()
                .AssertReply("Hello and welcome!")
                .Send("Hello")
                .AssertReplyOneOf(new[] { 
                    "Good morning",
                    "Good afternoon",
                    "Good evening"
                })
                .StartTestAsync();

            // Assert ------------------

        }

        private T GetService<T>() => _scope.ServiceProvider.GetRequiredService<T>();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _scope.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
