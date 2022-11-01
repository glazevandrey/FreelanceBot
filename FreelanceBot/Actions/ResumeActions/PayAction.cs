using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.ResumeActions
{
    public class PayAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 13);

            using (var db = new UserContext())
            {
                var resume = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (resume == null)
                {
                    return;
                }
                if (update.Message.Text != "Back")
                {
                    resume.Level = update.Message.Text;
                    db.SaveChanges();
                }
            }

            var btn1 = new KeyboardButton("Forward");
            var btn2 = new KeyboardButton("Back");


            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Select the salary level that you preffer, or leave the field empty:\n\nSingle digit input is available (e.g. 3000$) or range (e.g. 2100-5300$)", replyMarkup: rkm);
        }
    }
}
