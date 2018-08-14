using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;
using CustomerServiceBot.Model;

namespace CustomerServiceBot.Dialogs
{
    [Serializable]
    public class MainDialog
    {
        public static readonly IDialog<string> dialog = Chain.PostToChain()
            .Select(msg => msg.Text)
            .Switch(
                new RegexCase<IDialog<string>>(new System.Text.RegularExpressions.Regex("^hi", RegexOptions.IgnoreCase), (context, txt) =>
                {
                    return Chain.ContinueWith(new GreetingDialog(), AfterGreetingContinuation);
                }),
                new DefaultCase<string, IDialog<string>>((context, txt) =>
                {
                    return Chain.ContinueWith(FormDialog.FromForm(BugReport.BuildForm, FormOptions.PromptInStart), AfterGreetingContinuation);
                }))
            .Unwrap()
            .PostToUser();

        private async static Task<IDialog<string>> AfterGreetingContinuation(IBotContext context, IAwaitable<object> item)
        {
            var token = await item;
            var name = "User";
            context.UserData.TryGetValue<string>("Name", out name);
            return Chain.Return("Thanks for using the Bot "+ name);


        }
    }
}