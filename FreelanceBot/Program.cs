using FreelanceBot.Bot;
using FreelanceBot.Database;
using FreelanceBot.Helpers;
using FreelanceBot.Models;
using FreelanceBot.Parsers;
using FreelanceBot.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FreelanceBot
{
    public class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static Telegram.Bot.TelegramBotClient botClient;
        public static IBotService _botService;
        public static ValidateRequest validator = new ValidateRequest();
        public static StageService stageService = new StageService();
        public static IBotService botService;
        public static Config config;
        public static string EventView = $"<b>Event - [title]</b>\n\nContact - [username]\n\nDescription - [description]\n\nStart Date - [date]";
        public static string JobView = $"<b>Job - [title]</b>\n\nContact - [username]\n\nDescription - [description]\n\nSpecialist level - [level]\n\nSalary - [pay]\n\nPlace - [place]\n\nType of work - [type]";
        public static string ParsedJobView = $"<b>Job - [title]</b>\n\n[contact]\n\n[description]\n\nSpecialist level - [level]\n\nPlace - [place]\n\nType of work - [type]";

        public static string ResumeView = $"<b>Resume - [title]</b>\n\nContact - [username]\n\nDescription - [description]\n\nSpecialist level - [level]\n\nDesired payment - [pay]\n\nPlace - [place]";
        public static void Main(string[] args)
        {
            logger.Info("START");
            StartInit();
            CreateHostBuilder(args).Build().Run();
        }

        public static void StartInit()
        {
            string jsonString = System.IO.File.ReadAllText("config.json", Encoding.UTF8);

            Program.config = new Config()
            {
                Specialization = new List<string>()
            {

                 "Imposer",
    "Programmer",
    "Software tester",
    "System administrator",
    "Screenwriter of computer games",
    "Neural interface designer",
    "IOS developer",
    "Android developer",
    "Database architect",
    "Database developer",
    "Network administrator",
    "Game developer",
    "System engineer",
    "Information systems specialist",
    "Front-end developer",
    "UX designer",
    "UI designer",
    "UX/UI designer",
    "Graphic designer",
    "Illustrator_designer",
    "QA engineer",
    "Product designer"
            }
            };

            
            //test
            //Program.botClient = new Telegram.Bot.TelegramBotClient("5701845519:AAGTCeu3hMrW6ACu5tc0smIRVojLl9hwIwc");
            
            //prod
            Program.botClient = new Telegram.Bot.TelegramBotClient("5604138902:AAF5nhVvfX9OrMwriHSTdAK1XO5h-hS3l_Y");

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(m => m.ChatId == 1639798390);
                if (user == null)
                {
                    user = db.Users.FirstOrDefault(m => m.Username == "uxuialex");
                    if (user == null)
                    {
                        return;
                    }
                }


                //SendToChannel._adminId = 534325982;
                SendToChannel._adminId = user.ChatId;
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
