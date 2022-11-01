using FreelanceBot.Database;
using FreelanceBot.Helpers;
using FreelanceBot.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;

namespace FreelanceBot.Actions.WorkerActions
{
    public class ResultSeaLevelWorkerAction : IBaseAction
    {
        public async Task Start(Update update)
        {

            long id = 0;
            if (update.Message != null)
            {
                id = update.Message.From.Id;
            }
            else
            {
                id = update.CallbackQuery.From.Id;
            }

            Program.stageService.SetStage(id, 30);


            string level = "";
            int offset = 0;

            if (update.Message != null)
            {
                level = update.Message.Text.Trim();
            }
            if (update.CallbackQuery != null)
            {
                var parsed = Search.Parse(update.CallbackQuery.Data);
                level = parsed[0];
                offset = Convert.ToInt32(parsed[1]);

            }

            if (offset == 0 && update.Message != null && update.Message.Text != null)
            {
                Search.SendDef(id);
            }

            var list = new List<Job>();
            using (var db = new UserContext())
            {
                list = db.Jobs.Where(m => m.IsDone == true && m.Level == level).ToList();

            }

            Dictionary<int, string> dic = Search.FillKayValue(list);


            var rkm = new InlineKeyboardMarkup(Search.FillButtons(list, offset, level, "level"));

            var neededText = dic[offset];

            await Program.botClient.SendTextMessageAsync(id, neededText, replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }

    }
}
