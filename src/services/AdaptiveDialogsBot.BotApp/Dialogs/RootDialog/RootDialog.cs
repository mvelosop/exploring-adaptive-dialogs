using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using System.Collections.Generic;

namespace AdaptiveDialogsBot.BotApp.Dialogs.RootDialog
{
    public class RootDialog : AdaptiveDialog
    {
        public RootDialog() : base(nameof(RootDialog))
        {
            Triggers = new List<OnCondition> {
                new OnUnknownIntent {
                    Actions = {
                        new SendActivity("Hi, we're running with Adaptive Dialogs!"),
                    }
                },
            };
        }
    }
}
