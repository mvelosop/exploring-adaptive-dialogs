using AdaptiveDialogBot.BotApp.IntegrationTests.EchoBotTests;
using AdaptiveDialogsBot.BotApp.Dialogs.RootDialog;
using FluentAssertions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Testing;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdaptiveDialogBot.BotApp.IntegrationTests.DialogBotTests
{
    public class DialogBotShould : IClassFixture<BotApplicationFactory>, IDisposable
    {
        private bool _disposedValue;
        private readonly IServiceScope _scope;

        public DialogBotShould(BotApplicationFactory applicationFactory)
        {
            _scope = applicationFactory.CreateScope();
        }

        [Fact]
        public async Task Reply_back_whatever_it_receives()
        {
            // Arrange -----------------
            var testAdapter = GetService<TestAdapter>();
            testAdapter.Locale = "en-us";

            var dialogUnderTest = GetService<RootDialog>();
            var testClient = new DialogTestClient(testAdapter, dialogUnderTest);

            var conversationUpdate = Activity.CreateConversationUpdateActivity();
            conversationUpdate.MembersAdded.Add(testAdapter.Conversation.User);

            // Act ---------------------
            var reply = await testClient.SendActivityAsync<IMessageActivity>(conversationUpdate as Activity);
            reply.Text.Should().Be("Hello and welcome!");

            reply = await testClient.SendActivityAsync<IMessageActivity>("Echo TEST!");
            reply.Text.Should().Be("Echo: Echo TEST!");

            // Assert ------------------

        }

        [Fact]
        public async Task Greet_back_on_hello()
        {
            // Arrange -----------------
            var testAdapter = GetService<TestAdapter>();
            testAdapter.Locale = "en-us";

            var dialogUnderTest = GetService<RootDialog>();
            var testClient = new DialogTestClient(testAdapter, dialogUnderTest);

            var conversationUpdate = Activity.CreateConversationUpdateActivity();
            conversationUpdate.MembersAdded.Add(testAdapter.Conversation.User);

            // Act ---------------------
            var reply = await testClient.SendActivityAsync<IMessageActivity>(conversationUpdate as Activity);
            reply.Text.Should().Be("Hello and welcome!");

            reply = await testClient.SendActivityAsync<IMessageActivity>("Hello");
            reply.Text.Should().BeOneOf(new[] {
                "Good morning",
                "Good afternoon",
                "Good evening"
            });

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
