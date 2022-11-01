using FreelanceBot.Actions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FreelanceBot.Executors
{
    public class MessageExecutor
    {
        public async Task<ActionResult> Execute(Update update, IBaseAction action)
        {
            await action.Start(update);
            return new OkResult();
        }
    }
}
