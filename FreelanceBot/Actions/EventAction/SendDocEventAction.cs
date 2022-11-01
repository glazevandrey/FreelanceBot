using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.EventAction
{
    public class SendDocEventAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 22);

            var btn1 = new KeyboardButton("Back");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Send image or video to this chat.\nImage must be .jpg format and less then 2 mb.\nVideo must be .mp4 format and be less then 10mb", replyMarkup: rkm);

        }
    }
}
