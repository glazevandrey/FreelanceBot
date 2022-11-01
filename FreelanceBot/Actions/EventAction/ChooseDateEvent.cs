using FreelanceBot.Database;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.EventAction
{
    public class ChooseDateEvent : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 20);

            using (var db = new UserContext())
            {
                var ev = db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (update.Message.Text != "Back")
                {
                    ev.Description = update.Message.Text;
                    db.SaveChanges();
                }
            }

            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Specify the event start date in format:\nday/month/year hour:minutes PM (e.g. 23/08/2022 7:00 PM)", replyMarkup: rkm);
        }
    }
}
