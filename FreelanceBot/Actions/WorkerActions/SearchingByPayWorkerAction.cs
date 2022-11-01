using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.WorkerActions
{
    public class SearchingByPayWorkerAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 33);


            var rkm = new ReplyKeyboardMarkup(new KeyboardButton("Back"));
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "<b>Enter the salary level, or leave the field empty:\n\nSingle digit input is available (e.g. 3000$ or 1500)</b>", replyMarkup: rkm, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }
    }
}
