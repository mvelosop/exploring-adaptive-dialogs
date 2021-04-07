using AdaptiveDialogsBot.BotApp;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Extensions.Logging;

namespace AdaptiveDialogBot.BotApp.IntegrationTests.Helpers
{
    public class BotApplicationTestAdapter : TestAdapter
    {
        public BotApplicationTestAdapter(
            ILogger<BotApplicationTestAdapter> logger,
            IStorage storage,
            UserState userState,
            ConversationState conversationState)
        {
            Use(new LoggingMiddleware());

            this.UseStorage(storage);
            this.UseBotState(userState);
            this.UseBotState(conversationState);

            OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

                // Send a message to the user
                await turnContext.SendActivityAsync("The bot encountered an error or bug.");
                await turnContext.SendActivityAsync("To continue to run this bot, please fix the bot source code.");

                // Send a trace activity, which will be displayed in the Bot Framework Emulator
                await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message, "https://www.botframework.com/schemas/error", "TurnError");
            };

        }
    }
}
