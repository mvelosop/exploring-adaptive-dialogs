using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdaptiveDialogsBot.BotApp.Bots
{
    public class DialogBot<T> : ActivityHandler
        where T : Dialog
    {
        private readonly DialogManager DialogManager;
        protected readonly ILogger Logger;

        public DialogBot(T rootDialog, ILogger<DialogBot<T>> logger)
        {
            Logger = logger;

            DialogManager = new DialogManager(rootDialog);
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Running dialog with Activity.");
            await DialogManager.OnTurnAsync(turnContext, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
