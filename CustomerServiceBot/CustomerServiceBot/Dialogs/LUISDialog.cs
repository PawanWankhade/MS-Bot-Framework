using CustomerServiceBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;

namespace CustomerServiceBot.Dialogs
{
    [LuisModel("48ee4353-f0f4-4e0c-85b2-e116d4d043f7", "667fc331781f4d4eb08a13355a648d3e", domain: "westus.api.cognitive.microsoft.com", Staging = true)]
    [Serializable]
    public class LUISDialog : LuisDialog<BugReport>
    {
        private readonly BuildFormDelegate<BugReport> NewBugReport;

        public LUISDialog(BuildFormDelegate<BugReport> newBugReport)
        {
            this.NewBugReport = newBugReport;
        }




        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("GreetingIntent")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }

        [LuisIntent("NewBugReportIntent")]
        public async Task BugReport(IDialogContext context, LuisResult result)
        {
            var enrollmentForm = new FormDialog<BugReport>(new Model.BugReport(), this.NewBugReport, FormOptions.PromptInStart);
            context.Call<BugReport>(enrollmentForm, Callback);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        [LuisIntent("QueryBugType")]
        public async Task QueryBugTypes(IDialogContext context, LuisResult result)
        {
            foreach (var entity in result.Entities.Where(Entity => Entity.Type == "BugType"))
            {
                var value = entity.Entity.ToLower();
                if (Enum.GetNames(typeof(BugTypes)).Where(a => a.ToLower().Equals(value)).Count() > 0)
                {
                    await context.PostAsync("Yes that is a bug type!");
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    await context.PostAsync("I'm sorry that is not a bug type.");
                    context.Wait(MessageReceived);
                    return;
                }
                await context.PostAsync("I'm sorry that is not a bug type.");
                context.Wait(MessageReceived);
                return;

            }

        }



    }
}