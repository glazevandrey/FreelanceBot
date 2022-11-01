using FreelanceBot.Database;
using FreelanceBot.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.WorkerActions
{
    public class SearchingBySpecWorkerAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 25);

            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            var list = new List<string>();
            foreach (var item in Program.config.Specialization)
            {
                list.Add(item);
            }

            list.Add("Back");

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

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>Choose the necessary specialization</b>", replyMarkup:rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }
    }
}
