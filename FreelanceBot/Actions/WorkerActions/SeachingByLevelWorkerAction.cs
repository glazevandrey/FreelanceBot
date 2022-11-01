using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.WorkerActions
{
    public class SeachingByLevelWorkerAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 29);

            var btn1 = ("Intern");
            var btn2 = ("Junior");
            var btn3 = ("Middle");
            var btn4 = ("Senior");
            var btn5 = ("Team Lead");
            var btn6 = ("Back");

            var rk = new List<string>() { btn1, btn2, btn3, btn4, btn5, btn6 };

            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            var list = new List<string>();
            foreach (var item in rk)
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
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>Select necessary level:</b>", replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }
    }
}
