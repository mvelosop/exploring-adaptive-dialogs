using Microsoft.Bot.Builder;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveDialogsBot.BotApp
{
    public class LoggingMiddleware : IMiddleware
    {
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
        {
            Log.Information("----- Processing Activity - Message: \"{Message}\" [{Locale}] - Activity: {@Activity}",
                turnContext.Activity.Text, turnContext.Activity.Locale, turnContext.Activity);

            await next.Invoke(cancellationToken);
        }
    }
}
