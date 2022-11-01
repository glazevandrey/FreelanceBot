using FreelanceBot.Database;
using FreelanceBot.Models;
using System.IO;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Telegram.Bot.Types.InputFiles;
using User = FreelanceBot.Models.User;
using File = System.IO.File;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace FreelanceBot.Helpers
{
    public class SendToChannel
    {
        //test
        //public static long _channelId = -1001862646659;

        public static long _channelId = -1001874357683;


        public static long _adminId;

        public static async void SendNewPackage(string data)
        {
            var split = data.Split(":");
            if(split.Length < 4)
            {
                return;
            }


            var id = split[1];
            var name = split[2];
            var type = split[3];

            await Program.botClient.SendTextMessageAsync(_adminId, $"<b>New {type} pack order: @{name}</b>", Telegram.Bot.Types.Enums.ParseMode.Html);


        }
        public static async void SendEventToAdmin(Event evnt, User user)
        {
            string text2 = Program.EventView;

            text2 = text2.Replace("[title]", evnt.Title);
            text2 = text2.Replace("[username]",  "@" + user.Username);
            text2 = text2.Replace("[description]", evnt.Description);
            text2 = text2.Replace("[date]", evnt.StartDate);


            await Program.botClient.SendTextMessageAsync(_adminId, text2, Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        public static async void Send(Resume resume, User user)
        {

            string text = Program.ResumeView;

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

            if (!resume.HaveFile)
            {
                await Program.botClient.SendTextMessageAsync(_channelId, text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                return;
            }

            SendFile(resume.FileName, text);

        }
        public static void Send(Event evnt, User user)
        {
            string text = Program.EventView;

            text = text.Replace("[title]", evnt.Title);
            text = text.Replace("[username]", "@" + user.Username);
            text = text.Replace("[description]", evnt.Description);
            text = text.Replace("[date]", evnt.StartDate);

            if (!evnt.HaveFile)
            {
                Program.botClient.SendTextMessageAsync(_channelId, text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                return;
            }

            SendFile(evnt.FileName, text);
        }
        public static void Send(Job job, User user)
        {

            string text = Program.JobView;

            text = text.Replace("[title]", job.Title);
            text = text.Replace("[username]", "@" + user.Username);
            text = text.Replace("[description]", job.Description);
            text = text.Replace("[level]", job.Level);
            text = text.Replace("[place]", job.Place);
            text = text.Replace("[type]", job.TypeJob);
            if (job.Pay != 0)
            {
                text = text.Replace("[pay]", job.Pay.ToString() + "$");
            }
            else if (job.PayMin != 0)
            {
                text = text.Replace("[pay]", job.PayMin.ToString() + " - "+ job.PayMax.ToString() + "$");
            }
            else
            {
                text = text.Replace("[pay]", "No expectations");
            }

            Program.botClient.SendTextMessageAsync(_channelId, text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

        }

        public async static void SendFile(string fileName, string caption)
        {
            try
            {
                string p = Path.Combine($"\\usr\\documents\\{fileName}");
                using (FileStream stream = File.Open(p, FileMode.Open))
                {

                    InputOnlineFile iof = new InputOnlineFile(stream);
                    iof.FileName = fileName;
                    await Program.botClient.SendDocumentAsync(_channelId, iof, caption: caption, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                }
            }
            catch (System.Exception ex)
            {
                return;
            }
        }
    }
}
