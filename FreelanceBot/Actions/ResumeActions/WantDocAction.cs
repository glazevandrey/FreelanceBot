using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.ResumeActions
{
    public class WantDocAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 15);
            using (var db = new UserContext())
            {
                var resume = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (resume == null)
                {
                    return;
                }
                if (update.Message.Text != "Back")
                {
                    resume.Place = update.Message.Text;
                    db.SaveChanges();
                }
            }
            var btn1 = new KeyboardButton("Yes");
            var btn2 = new KeyboardButton("No");


            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Would you like to attach a video or image or other document?", replyMarkup: rkm);
        }
    }
}
