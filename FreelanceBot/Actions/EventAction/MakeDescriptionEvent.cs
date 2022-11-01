using FreelanceBot.Database;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.EventAction
{
    public class MakeDescriptionEvent : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 19);

            using (var db = new UserContext())
            {
                var ev = db.Events.FirstOrDefault(m=>m.UserId == update.Message.From.Id && m.IsDone == false);
                if (update.Message.Text != "Back")
                {
                    ev.Title = update.Message.Text;
                    db.SaveChanges();
                }
            }

            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Specify the event description (max 4000 chars):", replyMarkup: rkm);
        }
    }
}
