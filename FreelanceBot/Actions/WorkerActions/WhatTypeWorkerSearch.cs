using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.WorkerActions
{
    public class WhatTypeWorkerSearch : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 24);


            var btn1 = "Searching by specialization";
            var btn3 = "Searching by level";
            var btn4 = "Searching by selary";
            var btn5 = "Searching by county/city";
            var btn6 = "Searching by text";
            var btn7 = "Back";

            var mist = new List<string>() { btn1, btn3, btn4, btn5, btn6, btn7 };

            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            var list = new List<string>();
            foreach (var item in mist)
            {
                list.Add(item);
            }

            var rkm = new ReplyKeyboardMarkup("");
            for (var Index = 0; Index < list.Count; Index++)
            {
                cols.Add(new KeyboardButton(list[Index]));
                //if (Index % 2!= 0) continue;
                rows.Add(cols.ToArray());
                cols = new List<KeyboardButton>();
            }

            rkm.Keyboard = rows.ToArray();
            rkm.ResizeKeyboard = true;

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>Select the search type</b>", replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }

    }
}
