using FreelanceBot.Database;
using FreelanceBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using static Quartz.Logging.OperationName;
using File = System.IO.File;
using Job = FreelanceBot.Models.Job;
using Update = Telegram.Bot.Types.Update;
using User = FreelanceBot.Models.User;

namespace FreelanceBot.Helpers
{
    public class Search
    {
        public static Dictionary<int, string> FillKayValue(List<Resume> list)
        {
            var dic = new Dictionary<int, string>();
            int x = 0;
            foreach (var resume in list)
            {
                string text = Program.ResumeView;
                using (var db = new UserContext())
                {
                    text = text.Replace("[username]", "@" + db.Users.FirstOrDefault(m => m.ChatId == resume.UserId).Username);
                }

                text = text.Replace("[title]", resume.Title);
                text = text.Replace("[description]", resume.Description);
                text = text.Replace("[level]", resume.Level);
                text = text.Replace("[place]", resume.Place);

                if (resume.Pay != 0)
                {
                    text = text.Replace("[pay]", resume.Pay.ToString() + "$");
                }
                else if (resume.PayMin != 0)
                {
                    text = text.Replace("[pay]", resume.PayMin.ToString() + " - " + resume.PayMax.ToString() + "$");
                }
                else
                {
                    text = text.Replace("[pay]", "No expectations");
                }

                dic.Add(x, text);
                x++;
            }

            return dic;
        }
        public static string FillKayValueParsedIT(Job jobEn)
        {

          
                string text = Program.ParsedJobView;
               
                var desc = Regex.Replace(jobEn.Description, @"<p.*?>", String.Empty, RegexOptions.CultureInvariant).Replace("<p>", "\n").Replace("</p>", string.Empty);
                desc = Regex.Replace(desc, @"<ul.*?>", String.Empty, RegexOptions.CultureInvariant).Replace("<ul>", string.Empty).Replace("</ul>", string.Empty);
                desc = Regex.Replace(desc, @"<li.*?>", String.Empty, RegexOptions.CultureInvariant).Replace("<li>", string.Empty).Replace("</li>", string.Empty);
                desc = desc.Replace("<bold>", "\n<bold>");
                desc = desc.Replace("<b>", "\n<b>");
                desc = desc.Replace("<strong>", "\n<strong>");
                desc = desc.Replace("<br>", "\n");
                desc = desc.Replace("</br>", string.Empty);
                desc = desc.Replace("<br />", "\n");
                desc = desc.Replace("<br/>", "\n");

                text = text.Replace("[title]", jobEn.Title);
                text = text.Replace("[contact]", jobEn.Contact);
                text = text.Replace("[description]", desc);
                text = text.Replace("[level]", jobEn.Level);
                text = text.Replace("[place]", jobEn.Place);
                text = text.Replace("[type]", jobEn.TypeJob);
                
            

            return text;
        }
        public static Dictionary<int, string> FillKayValue(List<Job> list)
        {
            var dic = new Dictionary<int, string>();
            int x = 0;
            foreach (var resume in list)
            {
              
                string text = Program.JobView;
                if (resume.UserId == 0)
                {
                    text = FillKayValueParsedIT(resume);
                    dic.Add(x, text);
                    x++;
                    continue;
                }
                using (var db = new UserContext())
                {
                    text = text.Replace("[username]", "@" + db.Users.FirstOrDefault(m => m.ChatId == resume.UserId).Username);
                }

                text = text.Replace("[title]", resume.Title);
                text = text.Replace("[description]", resume.Description);
                text = text.Replace("[level]", resume.Level);
                text = text.Replace("[place]", resume.Place);
                text = text.Replace("[type]", resume.TypeJob);

                if (resume.Pay != 0)
                {
                    text = text.Replace("[pay]", resume.Pay.ToString() + "$");
                }
                else if (resume.PayMin != 0)
                {
                    text = text.Replace("[pay]", resume.PayMin.ToString() + " - " + resume.PayMax.ToString() + "$");
                }
                else
                {
                    text = text.Replace("[pay]", "No expectations");
                }

                dic.Add(x, text);
                x++;
            }

            return dic;
        }

        public static string[] Parse(string data)
        {
            string[] res = new string[2];
            if (data.Contains("next:"))
            {
                var d = data.Split("-");

                var split1 = d[0].Split(":");
                res[0] = split1[1];

                var split2 = d[1].Split(":");
                res[1] = split2[1].Trim();
            }
            return res;
        }
        public static void SendDef(long id)
        {
            var rkm2 = new ReplyKeyboardMarkup(new KeyboardButton("To main menu"));
            rkm2.ResizeKeyboard = true;
            Program.botClient.SendTextMessageAsync(id, "Select a suitable resume from below or return to the main menu with the To Main Menu button", replyMarkup: rkm2);

        }
        public static List<InlineKeyboardButton> FillButtons(List<Resume> list, int offset, string text, string type)
        {

            var btns = new List<InlineKeyboardButton>();

            if (offset == 0)
            {
                if (list.Count > 1)
                {
                    var btn = new InlineKeyboardButton("Next");
                    btn.CallbackData = $"{type}:{text}-next:{offset + 1}";
                    btns.Add(btn);
                }

            }
            else
            {
                if (list.Count == offset)
                {

                }
                else
                {
                    if (list.Count != offset + 1)
                    {
                        var btn = new InlineKeyboardButton("Next");
                        btn.CallbackData = $"{type}:{text}-next:{offset + 1}";
                        btns.Add(btn);
                    }
                }
            }
            return btns;
        }
        public static List<InlineKeyboardButton> FillButtons(List<Job> list, int offset, string text, string type)
        {

            var btns = new List<InlineKeyboardButton>();

            if (offset == 0)
            {
                if (list.Count > 1)
                {
                    var btn = new InlineKeyboardButton("Next");
                    btn.CallbackData = $"{type}:{text}-next:{offset + 1}";
                    btns.Add(btn);
                }

            }
            else
            {
                if (list.Count == offset)
                {
                }
                else
                {
                    if (list.Count != offset + 1)
                    {
                        var btn = new InlineKeyboardButton("Next");
                        btn.CallbackData = $"{type}:{text}-next:{offset + 1}";
                        btns.Add(btn);
                    }
                }
            }
            return btns;
        }

        public async static void SendFile(long id, InlineKeyboardMarkup rkm, string text, string name)
        {

            string p = Path.Combine($"\\usr\\documents\\{name}");

            try
            {

                using (FileStream stream = File.Open(p, FileMode.Open))
                {

                    InputOnlineFile iof = new InputOnlineFile(stream);
                    iof.FileName = name;


                    await Program.botClient.SendDocumentAsync(id, iof, caption: text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: rkm);

                    return;
                }
            }
            catch (System.Exception ex)
            {
                return;
            }

        }

        public async static void SendFile(Update update, User user, string type, int count, int max)
        {
            string name = "";
            if (update.Message.Document != null)
            {
                Random random = new Random();
                var r = random.Next(1111, 4444);
                name += r + update.Message.Document.FileName;
                try
                {
                    using (var stream = new FileStream("\\usr\\documents\\" + r + update.Message.Document.FileName, FileMode.Create))
                    {
                        await Program.botClient.GetInfoAndDownloadFileAsync(update.Message.Document.FileId, stream);
                    }
                    using (var db = new UserContext())
                    {
                        db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false).HaveFile = true;
                        db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false).FileName = r + update.Message.Document.FileName;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (update.Message.Photo != null)
            {
                Random r = new Random();
                var rand = r.Next(111111, 9999999);
                name += rand + ".jpg";

                try
                {
                    if (update.Message.Photo[update.Message.Photo.Length - 1].FileSize > 2097152)
                    {
                        throw new Exception("File is too biggest");
                    }

                    using (var stream = new FileStream("\\usr\\documents\\" + rand + ".jpg", FileMode.Create))
                    {
                        await Program.botClient.GetInfoAndDownloadFileAsync(update.Message.Photo[update.Message.Photo.Length - 1].FileId, stream);
                    }
                    using (var db = new UserContext())
                    {
                        if (type == "resume")
                        {
                            var resum = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                            resum.HaveFile = true;
                            db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false).FileName = rand.ToString() + ".jpg";
                        }
                        else
                        {
                            var ev = db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                            ev.HaveFile = true;
                            db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false).FileName = rand.ToString() + ".jpg";
                        }
                        db.SaveChanges();


                    }
                }
                catch (Exception ex)
                {
                    int x = 2;
                }
            }
            else if (update.Message.Video != null)
            {
                Random rand = new Random();
                var r = rand.Next(1111, 9999);
                name += r + update.Message.Video.FileName;


                if (update.Message.Video.FileSize > 10485760)
                {
                    throw new Exception("File is too biggest");
                }


                try
                {
                    using (var stream = new FileStream("\\usr\\documents\\" + r + update.Message.Video.FileName, FileMode.Create))
                    {
                        await Program.botClient.GetInfoAndDownloadFileAsync(update.Message.Video.FileId, stream);
                    }
                    using (var db = new UserContext())
                    {
                        db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false).HaveFile = true;
                        db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false).FileName = r + update.Message.Video.FileName;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    int x = 2;
                }
            }
            Thread.Sleep(500);


            string p = Path.Combine($"\\usr\\documents\\{name}");

            try
            {

                using (FileStream stream = File.Open(p, FileMode.Open))
                {

                    InputOnlineFile iof = new InputOnlineFile(stream);
                    iof.FileName = name;

                    string text = "";
                    if (type == "event")
                    {

                        var resume = new Event();
                        using (var db = new UserContext())
                        {
                            resume = db.Events.FirstOrDefault(m => m.UserId == update.Message.Chat.Id && m.IsDone == false);
                        }
                        text = Program.EventView;

                        text = text.Replace("[title]", resume.Title);
                        text = text.Replace("[username]", "@" + user.Username);
                        text = text.Replace("[description]", resume.Description);
                        text = text.Replace("[date]", resume.StartDate);

                    }
                    else if (type == "resume")
                    {
                        var resume = new Resume();
                        using (var db = new UserContext())
                        {
                            resume = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.Chat.Id && m.IsDone == false);
                        }
                        text = Program.ResumeView;

                        text = text.Replace("[title]", resume.Title);
                        text = text.Replace("[username]", "@" + user.Username);
                        text = text.Replace("[description]", resume.Description);
                        text = text.Replace("[level]", resume.Level);
                        text = text.Replace("[place]", resume.Place);

                        if (resume.Pay != 0)
                        {
                            text = text.Replace("[pay]", resume.Pay.ToString() + "$");
                        }
                        else if (resume.PayMin != 0)
                        {
                            text = text.Replace("[pay]", resume.PayMin.ToString() + " - " + resume.PayMax.ToString() + "$");
                        }
                        else
                        {
                            text = text.Replace("[pay]", "No expectations");
                        }
                    }

                    var btn11 = new KeyboardButton("Done");
                    var btn22 = new KeyboardButton("Back");
                    var btn33 = new KeyboardButton("Cancel");

                    var rkm2 = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn11, btn22, btn33 });
                    rkm2.ResizeKeyboard = true;
                    await Program.botClient.SendDocumentAsync(update.Message.From.Id, iof, caption: $"<b>{count + 1}/{max}</b>" + text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Great! You are one step away from placing!\n\nIf everything suits you click Done if not Go Back or Cancel", replyMarkup: rkm2);

                    return;
                }
            }
            catch (System.Exception ex)
            {
                return;
            }

        }
    }
}
