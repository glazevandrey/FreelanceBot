using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions
{
    public class StartAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id);
                if (user != null)
                {
                    return;
                }

                db.Users.Add(new Models.User() { ChatId = update.Message.From.Id, Username = update.Message.From.Username });
                await db.SaveChangesAsync();
            }

            var btn1 = new KeyboardButton("I am a candidate");
            var btn2 = new KeyboardButton("I am an employer");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2 });
            rkm.ResizeKeyboard = true;

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Good afternoon! This is a freelance bot!\n\nYou can view all current vacancies, resumes and events both in this bot and in our channel: https://t.me/itjobsevents\n\nTo start, choose who you are:", replyMarkup: rkm);
        }
    }
}
