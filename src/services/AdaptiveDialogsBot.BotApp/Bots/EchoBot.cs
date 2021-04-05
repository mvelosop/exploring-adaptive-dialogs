using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveDialogsBot.BotApp.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string replyText;

            if (turnContext.Activity.Text.Equals("Hello", StringComparison.OrdinalIgnoreCase))
            {
                var now = DateTime.Now.TimeOfDay;

                var time = now < new TimeSpan(12, 0, 0)
                    ? "morning"
                    : now > new TimeSpan(19, 0, 0)
                        ? "evening"
                        : "afternoon";

                replyText = "Good " + time;
            }
            else
            {
                replyText = $"Echo: {turnContext.Activity.Text}";
            }

            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
