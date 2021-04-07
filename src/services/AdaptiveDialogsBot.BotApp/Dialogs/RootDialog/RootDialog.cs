using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using System;
using System.Collections.Generic;

namespace AdaptiveDialogsBot.BotApp.Dialogs.RootDialog
{
    public class RootDialog : AdaptiveDialog
    {
        public RootDialog() : base(nameof(RootDialog))
        {
            Triggers = new List<OnCondition> {
                new OnConversationUpdateActivity {
                    Actions = {
                        new Foreach {
                            ItemsProperty = "turn.activity.membersAdded",
                            Actions = {
                                new IfCondition {
                                    Condition = "$foreach.value.id != turn.activity.recipient.id",
                                    Actions = {
                                        new SendActivity("Hello and welcome!")
                                    }
                                }
                            }
                        }
                    }
                },

                new OnUnknownIntent {
                    Actions = {
                        new CodeAction(async (dialogContext, options) =>
                        {
                            string replyText;

                            if (dialogContext.Context.Activity.Text.Equals("Hello", StringComparison.OrdinalIgnoreCase))
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
                                replyText = $"Echo: {dialogContext.Context.Activity.Text}";
                            }

                            await dialogContext.Context.SendActivityAsync(MessageFactory.Text(replyText, replyText));

                            return await dialogContext.EndDialogAsync(options);
                        }),
                    }
                },
            };
        }
    }
}
