using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FreelanceBot.Actions
{
    public interface IBaseAction
    {
        public Task Start(Update update);
    }
}
