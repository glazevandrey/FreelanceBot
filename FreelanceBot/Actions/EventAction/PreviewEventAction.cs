using FreelanceBot.Database;
using FreelanceBot.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using User = FreelanceBot.Models.User;
using System.Linq;
using File = System.IO.File;
using FreelanceBot.Helpers;

namespace FreelanceBot.Actions.EventAction
{
    public class PreviewEventAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 23);

            var user = new User();
            int count = 0;
            int max = 0;
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id);
                var job = db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (job == null)
                {
                    return;
                }
                count = db.Events.Where(m => m.UserId == update.Message.From.Id && m.IsDone == true).ToList().Count();
                max = user.MaxEvents;
                db.SaveChanges();
            }
            

            if (update.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            {
                try
                {
                    Search.SendFile(update, user, "event", count, max);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("biggest"))
                    {
                        Program.stageService.SetStage(update.Message.From.Id, 22);
                        await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "File is too biggest.");
                        return;
                    }
                    else
                    {
                        Program.stageService.SetStage(update.Message.From.Id, 22);
                        await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Uncorrect file extension.");
                        return;

                    }
                }
                return;
            }

            var resume2 = new Event();
            using (var db = new UserContext())
            {
                resume2 = db.Events.FirstOrDefault(m => m.UserId == update.Message.Chat.Id && m.IsDone == false);
            }
            string text2 = Program.EventView;

            text2 = text2.Replace("[title]", resume2.Title);
            text2 = text2.Replace("[username]", user.Username);
            text2 = text2.Replace("[description]", resume2.Description);
            text2 = text2.Replace("[date]", resume2.StartDate);

            var btn1 = new KeyboardButton("Done");
            var btn2 = new KeyboardButton("Back");
            var btn3 = new KeyboardButton("Cancel");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2 , btn3});
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, $"<b>({count + 1}/{max})</b>\n\n" + text2, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Great! You are one step away from placing an event.\n\nIf everything suits you click Done if not Go Back or Cancel", replyMarkup: rkm);

        }

    }
}
