using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions
{
    public class ChangeClassAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            var btn1 = new KeyboardButton("I am an employer");
            var btn2 = new KeyboardButton("I am a candidate");
            var btn3 = new KeyboardButton("Back");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2, btn3 });
            rkm.ResizeKeyboard = true;

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>Choose who you are</b>", replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}
