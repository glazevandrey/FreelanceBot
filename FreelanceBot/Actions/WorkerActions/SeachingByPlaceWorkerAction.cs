using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.WorkerActions
{
    public class SeachingByPlaceWorkerAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 31);

            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>Enter worker city or country:</b>", replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }
    }
}
