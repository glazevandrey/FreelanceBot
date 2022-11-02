using FreelanceBot.Database;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.EventAction
{
    public class MakeTitleEventAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 18);
            int max = 0;
            using (var db = new UserContext())
            {
                max = db.Users.FirstOrDefault(m=>m.ChatId == update.Message.From.Id).MaxEvents;
                if (db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false) != null)
                {

                }
                else
                {
                    if (db.Events.Where(m => m.UserId == update.Message.From.Id).ToList().Count == max)
                    {
                        var btn1 = new InlineKeyboardButton("Order a new package");

                        if (update.Message.From.Username == null || update.Message.From.Username == "")
                        {
                            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "You need to fill out your telegram username");
                            return;
                        }

                        btn1.CallbackData = $"wantPack:{update.Message.From.Id}:{update.Message.From.Username}:event";

                        var rkm2 = new InlineKeyboardMarkup(btn1);

                        await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Sorry 😔. You have reached the maximum number of events", replyMarkup: rkm2);
                        Program.stageService.SetStage(update.Message.From.Id, 2);

                        return;
                    }

                    db.Events.Add(new Models.Event() { UserId = update.Message.From.Id });
                }
                db.SaveChanges();
            }

            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Specify the event title:", replyMarkup: rkm);
        }
    }
}
