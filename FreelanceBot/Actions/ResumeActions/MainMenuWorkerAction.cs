using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.ResumeActions
{
    public class MainMenuWorkerAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 2);
            using (var db = new UserContext())
            {
                db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id).Role = Models.Role.Исполнитель;
                db.SaveChanges();
            }
            var btn1 = new KeyboardButton("Publish a resume");
            var btn3 = new KeyboardButton("Job searching");
            var btn2 = new KeyboardButton("Settings");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn3, btn2 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>This is the main menu of the candidate</b>", replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}
