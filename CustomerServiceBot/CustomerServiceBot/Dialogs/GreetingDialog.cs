using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace CustomerServiceBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        //#region oldCode
        //        public Task StartAsync(IDialogContext context)
        //        {
        //            context.PostAsync("Hi I'm Panda Bot");
        //            context.Wait(MessageReceivedAsync);
        //            return Task.CompletedTask;
        //        }

        //        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        //        {
        //            dynamic message = await result;
        //            var getName = false;
        //            var userName = string.Empty;

        //            context.UserData.TryGetValue<string>("Name", out userName);
        //            context.UserData.TryGetValue<bool>("GetName", out getName);

        //            if (getName)
        //            {
        //                userName = message.Text;
        //                context.UserData.SetValue<string>("Name", userName);
        //                context.UserData.SetValue<bool>("GetName", false);
        //            }


        //            if (string.IsNullOrEmpty(userName))
        //            {
        //                await context.PostAsync("What is your name ?");
        //                context.UserData.SetValue<bool>("GetName", true);
        //            }
        //            else
        //            {
        //                await context.PostAsync(string.Format("Hi {0}, How can I help you today?", userName));
        //            }
        //            context.Wait(MessageReceivedAsync);

        //        }
        //#endregion

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi I'm Panda Bot");
            await Respond(context);

            context.Wait(MessageReceivedAsync);
        }

        private static async Task Respond(IDialogContext context)
        {
            var userName = string.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync(string.Format("Hi {0}. How are you today?", userName));
            }

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            dynamic message = await result;
            var userName = string.Empty;
            var getName = false;

            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);

               await Respond(context);
                context.Wait(MessageReceivedAsync);

            }
            else
            {
                context.Done(message);
            }
        }



    }
}

