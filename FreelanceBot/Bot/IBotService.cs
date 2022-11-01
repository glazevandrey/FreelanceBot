using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FreelanceBot.Bot
{
    public interface IBotService
    {
        public Task Start(Update update);
        public Task InlineStart(Update update);

    }
}
