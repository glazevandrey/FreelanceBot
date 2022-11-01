using FreelanceBot.Database;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.ResumeActions
{
    public class DescriptionResumeAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 11);

            using (var db = new UserContext())
            {
                var resume = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (resume == null)
                {
                    return;
                }
                if(update.Message.Text != "Back")
                {
                    resume.Title = update.Message.Text;
                    db.SaveChanges();
                }
            }

            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Enter your description (max 4000 chars):", replyMarkup: rkm);
        }
    }
}
