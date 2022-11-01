using FreelanceBot.Database;
using FreelanceBot.Helpers;
using FreelanceBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;

namespace FreelanceBot.Actions.WorkerActions
{
    public class ResultSeaByPlaceWorkerAction : IBaseAction
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


            string place = "";
            int offset = 0;

            if (update.Message != null)
            {
                place = update.Message.Text.Trim();
            }
            if (update.CallbackQuery != null)
            {
                var parsed = Search.Parse(update.CallbackQuery.Data);
                place = parsed[0];
                offset = Convert.ToInt32(parsed[1]);

            }

            if (offset == 0 && update.Message != null && update.Message.Text != null)
            {
                Search.SendDef(id);
            }

            var list = new List<Job>();
            using (var db = new UserContext())
            {
                list = db.Jobs.Where(m => m.IsDone == true && m.Place.Contains(place)).ToList();
            }

            Dictionary<int, string> dic = Search.FillKayValue(list);


            var rkm = new InlineKeyboardMarkup(Search.FillButtons(list, offset, place, "place"));

            var neededText = dic[offset];

            await Program.botClient.SendTextMessageAsync(id, neededText, replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);


        }
    }

    }
