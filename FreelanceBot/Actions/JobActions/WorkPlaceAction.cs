using FreelanceBot.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.JobActions
{
    public class WorkPlaceAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 7);

            var text = update.Message.Text.Replace("$", string.Empty);
            int pay = 0;
            int min = 0;
            int max = 0;

            if (text.Contains("-"))
            {
                var split = text.Split('-');

                min = Convert.ToInt32(split[0]);
                max = Convert.ToInt32(split[1]);
            }
            else
            {
                if (text != "Forward")
                {
                    try
                    {
                        pay = Convert.ToInt32(text);

                    }
                    catch (Exception)
                    {
                        await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Value must be only number (for example: 2000, or range: 1000-23333):");
                    }
                }
            }

            using (var db = new UserContext())
            {
                var job = db.Jobs.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (job == null)
                {
                    return;
                }
                if (update.Message.Text != "Back")
                {
                    job.Pay = pay;

                    job.PayMin = min;
                    job.PayMax = max;


                    db.SaveChanges();
                }
            }
            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Enter place of job (Country/City):", replyMarkup: rkm);
        }
    }
}
