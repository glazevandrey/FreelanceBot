using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.JobActions
{
    public class PayCostAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 6);

            using (var db = new UserContext())
            {
                var job = db.Jobs.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (job == null)
                {
                    return;
                }
                if (update.Message.Text != "Back")
                {
                    job.Level = update.Message.Text;
                    db.SaveChanges();
                }
            }

            var btn1 = new KeyboardButton("Forward");
            var btn2 = new KeyboardButton("Back");


            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Enter the salary level, or leave the field empty:\n\nSingle digit input is available (e.g. 3000$) or range (e.g. 2100-5300$)", replyMarkup: rkm);
        }
    }
}
