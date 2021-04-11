using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Builder.LanguageGeneration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

namespace AdaptiveDialogsBot.BotApp.Dialogs
{
    public class GreetingDialog : AdaptiveDialog
    {
        public GreetingDialog(
            IHostEnvironment hostEnvironment)
        {
            var dialogRoot = Path.Combine(hostEnvironment.ContentRootPath, "Dialogs");
            var templates = Templates.ParseFile(Path.Combine(dialogRoot, "GreetingDialog", "GreetingDialog.lg"));
            Generator = new TemplateEngineLanguageGenerator(templates);

            Triggers = new List<OnCondition> {
                new OnBeginDialog {
                    Actions = {

                        new TextInput {
                            Property = "dialog.userName",
                            Prompt = new ActivityTemplate("${RequestUserName()}")
                        },

                        new SendActivity("${ThankUser()}"),

                        new EndDialog("=dialog.userName")
                    }
                }
            };
        }
    }
}
