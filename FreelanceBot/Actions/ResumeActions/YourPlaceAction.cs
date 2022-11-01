using FreelanceBot.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.ResumeActions
{
    public class YourPlaceAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 14);

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
                if (text != "Forward" && text != "Back")
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
                var resume = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (resume == null)
                {
                    return;
                }
                if (update.Message.Text != "Back")
                {
                    resume.Pay = pay;

                    resume.PayMin = min;
                    resume.PayMax = max;


                    db.SaveChanges();
                }
            }
            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Enter your place:", replyMarkup: rkm);
        }

    }
}
