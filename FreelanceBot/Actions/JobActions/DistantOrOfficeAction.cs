using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.JobActions
{
    public class DistantOrOfficeAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 8);

            using (var db = new UserContext())
            {
                var job = db.Jobs.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (job == null)
                {
                    return;
                }
                if (update.Message.Text != "Back")
                {
                    job.Place = update.Message.Text;
                    db.SaveChanges();
                }
            }

            var btn1 = new KeyboardButton("Remote work");
            var btn2 = new KeyboardButton("Office work");
            var btn3 = new KeyboardButton("No matter");
            var btn4 = new KeyboardButton("Back");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2, btn3, btn4 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Choose the type of work:", replyMarkup: rkm);
        }
    }
}
