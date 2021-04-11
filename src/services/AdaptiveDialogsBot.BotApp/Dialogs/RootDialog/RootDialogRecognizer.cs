using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;
using System.Collections.Generic;

namespace AdaptiveDialogsBot.BotApp.Dialogs.RootDialog
{
    public class RootDialogRecognizer : RegexRecognizer
    {
        public RootDialogRecognizer()
        {
            Intents = new List<IntentPattern> {
                new IntentPattern {
                    Intent = "Greeting",
                    Pattern = "(Hi|Hello)"
                },
            };
        }
    }
}
