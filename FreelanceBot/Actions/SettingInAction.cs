using FreelanceBot.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions
{
    public class SettingInAction : IBaseAction
    {
        public  async Task Start(Update update)
        {
            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton> { new KeyboardButton("Change role"), new KeyboardButton("Back") });
            rkm.ResizeKeyboard = true;

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>Select necessary option</b>", replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html) ;

        }
    }
}
