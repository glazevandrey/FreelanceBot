using FreelanceBot.Database;
using FreelanceBot.Helpers;
using FreelanceBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.BossSearchingActions
{
    public class ResultSeaTextAction : IBaseAction
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
            Program.stageService.SetStage(id, 28);



            string text = "";
            int offset = 0;

            if (update.Message != null)
            {
                text = update.Message.Text.Trim();
            }
            if (update.CallbackQuery != null)
            {
                var parsed = Search.Parse(update.CallbackQuery.Data);
                text = parsed[0];
                offset = Convert.ToInt32(parsed[1]);
            }

            if (offset == 0 && update.Message != null && update.Message.Text != null)
            {
                Search.SendDef(id);
            }

            var list = new List<Resume>();
            using (var db = new UserContext())
            {
                list = db.Resumes.Where(m => m.IsDone == true && m.Description.Contains(text)).ToList();

            }




            Dictionary<int, string> dic = Search.FillKayValue(list);



            var btns = Search.FillButtons(list, offset, text, "text");

            var rkm = new InlineKeyboardMarkup(btns);

            var neededText = dic[offset];
            if (list[offset].HaveFile)
            {
                Search.SendFile(id, rkm, neededText, list[offset].FileName);
                return;
            }
            else
            {

                await Program.botClient.SendTextMessageAsync(id, neededText, replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            }
        }

    }
}
