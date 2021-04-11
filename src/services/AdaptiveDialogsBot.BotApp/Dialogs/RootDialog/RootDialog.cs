using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.LanguageGeneration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdaptiveDialogsBot.BotApp.Dialogs.RootDialog
{
    public class RootDialog : AdaptiveDialog
    {
        public RootDialog(
            IHostEnvironment hostEnvironment)
            : base(nameof(RootDialog))
        {
            var dialogRoot = Path.Combine(hostEnvironment.ContentRootPath, "Dialogs");
            var templates = Templates.ParseFile(Path.Combine(dialogRoot, "RootDialog", "RootDialog.lg"));
            Generator = new TemplateEngineLanguageGenerator(templates);

            Triggers = new List<OnCondition> {
                new OnConversationUpdateActivity {
                    Actions = {
                        new Foreach {
                            ItemsProperty = "turn.activity.membersAdded",
                            Actions = {
                                new IfCondition {
                                    Condition = "$foreach.value.id != turn.activity.recipient.id",
                                    Actions = {
                                        new SendActivity("${Welcome()}")
                                    }
                                }
                            }
                        }
                    }
                },

                new OnUnknownIntent {
                    Actions = {
                        new IfCondition {
                            Condition = "toLower(turn.activity.text) == 'hello'",

                            Actions = {
                                new CodeAction(async (dialogContext, options) =>
                                {
                                    var now = DateTime.Now.TimeOfDay;

                                    var time = now < new TimeSpan(12, 0, 0)
                                        ? "morning"
                                        : now > new TimeSpan(19, 0, 0)
                                            ? "evening"
                                            : "afternoon";

                                    dialogContext.State.SetValue("dialog.greetingTime", time);

                                    return await dialogContext.EndDialogAsync(options);
                                }),

                                new SendActivity("${Greeting()}")
                            },

                            ElseActions = {
                                new SendActivity("${Echo()}")
                            }
                        },
                    }
                },
            };
        }
    }
}
