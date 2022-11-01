using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.ResumeActions
{
    public class AttachFileAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 16);

            var btn1 = new KeyboardButton("Back");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Send image or video to this chat.\nImage must be .jpg format and less then 2 mb.\nVideo must be .mp4 format and less then 10mb:", replyMarkup: rkm);

        }
    }
}
