using FreelanceBot.Executors;
using FreelanceBot.Helpers;
using NLog;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FreelanceBot.Bot
{
    public class BotService : IBotService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MessageExecutor _executor;

        public BotService(MessageExecutor executor)
        {
            _executor = executor;
        }
        public async Task InlineStart(Update update)
        {
            var type = new SetWay();

            var result = type.GetInline(update);
            foreach (var action in result)
            {
                await _executor.Execute(update, action);
            }
        }

        public async Task Start(Update update)
        {
            var type = new SetWay();

            if (!Program.validator.IsValid(update))
            {
                return;
            }

            logger.Info("try get change way");
            var result = type.Get(update);

            if (result != null && result.Count != 0)
            {
                foreach (var action in result)
                {
                    await _executor.Execute(update, action);
                }
            }
        }
    }

}
