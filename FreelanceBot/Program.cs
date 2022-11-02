using FreelanceBot.Bot;
using FreelanceBot.Database;
using FreelanceBot.Helpers;
using FreelanceBot.Models;
using FreelanceBot.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelanceBot
{
    public class Program
    {
        public static Telegram.Bot.TelegramBotClient botClient;
        public static IBotService _botService;
        public static ValidateRequest validator = new ValidateRequest();
        public static StageService stageService = new StageService();
        public static IBotService botService;
        public static Config config;
        public static string EventView = $"<b>Event - [title]</b>\n\nContact - [username]\n\nDescription - [description]\n\nStart Date - [date]";
        public static string JobView = $"<b>Job - [title]</b>\n\nContact - [username]\n\nDescription - [description]\n\nSpecialist level - [level]\n\nSalary - [pay]\n\nPlace - [place]\n\nType of work - [type]";

        public static string ResumeView = $"<b>Resume - [title]</b>\n\nContact - [username]\n\nDescription - [description]\n\nSpecialist level - [level]\n\nDesired payment - [pay]\n\nPlace - [place]";
        public static void Main(string[] args)
        {
            StartInit();
            CreateHostBuilder(args).Build().Run();
        }

        public static void StartInit()
        {
            string jsonString = System.IO.File.ReadAllText("config.json", Encoding.UTF8);


            // Program.config = System.Text.Json.JsonSerializer.Deserialize<Config>(jsonString);
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
            Program.botClient = new Telegram.Bot.TelegramBotClient("5784734017:AAGIa3V48q-dl4hdTGvXxMZOX-y43KCWQFg");

            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(m => m.ChatId == 1639798390);
                if (user == null)
                {
                    user = db.Users.FirstOrDefault(m => m.Username == "uxuialex");
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
