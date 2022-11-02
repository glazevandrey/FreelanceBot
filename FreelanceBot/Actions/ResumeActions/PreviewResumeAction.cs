using FreelanceBot.Database;
using FreelanceBot.Helpers;
using FreelanceBot.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;
using File = System.IO.File;
using User = FreelanceBot.Models.User;

namespace FreelanceBot.Actions.ResumeActions
{
    public class PreviewResumeAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 17);
            int count = 0;

            var user = new User();
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(m=>m.ChatId == update.Message.From.Id);
                count = db.Resumes.Where(m => m.UserId == update.Message.From.Id && m.IsDone == true).ToList().Count();

            }

            int max = user.MaxResumes;
            if(update.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            {
                try
                {
                    Search.SendFile(update, user, "resume", count, max);
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

            var resume2 = new Resume();

            using (var db = new UserContext())
            {
                resume2 = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
            }
            string text2 = Program.ResumeView;

            text2 = text2.Replace("[title]", resume2.Title);
            text2 = text2.Replace("[username]", user.Username);
            text2 = text2.Replace("[description]", resume2.Description);
            text2 = text2.Replace("[level]", resume2.Level);
            text2 = text2.Replace("[place]", resume2.Place);

            if (resume2.Pay != 0)
            {
                text2 = text2.Replace("[pay]", resume2.Pay.ToString() + "$");
            }
            else if (resume2.PayMin != 0)
            {
                text2 = text2.Replace("[pay]", resume2.PayMin.ToString()  + " - "+ resume2.PayMax.ToString() + "$");
            }
            else
            {
                text2 = text2.Replace("[pay]", "No expectations");
            }

            var btn1 = new KeyboardButton("Done");
            var btn2 = new KeyboardButton("Back");
            var btn3 = new KeyboardButton("Cancel");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2, btn3 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, $"<b>({count+1}/{max})</b>\n\n" + text2, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Great! You are one step away from placing a resume.\n\nIf everything suits you click Done if not Go Back or Cancel", replyMarkup: rkm);

        }
    }
}
