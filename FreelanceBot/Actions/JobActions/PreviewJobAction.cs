using FreelanceBot.Database;
using FreelanceBot.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = FreelanceBot.Models.User;

namespace FreelanceBot.Actions.JobActions
{
    public class PreviewJobAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 9);
            Job job = new Job();
            User user = new Models.User();
            int count = 0;
            int max = 0;
            using (var db = new UserContext())
            {
                job = db.Jobs.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                if (job == null)
                {
                    return;
                }
                count = db.Jobs.Where(m => m.UserId == update.Message.From.Id && m.IsDone == true).ToList().Count();


                user = db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id);
                if (update.Message.Text != "Back")
                {
                    job.TypeJob = update.Message.Text;
                    db.SaveChanges();
                }

            }
            max = user.MaxJobs;
            var btn1 = new KeyboardButton("Done");
            var btn2 = new KeyboardButton("Back");
            var btn3 = new KeyboardButton("Cancel");

            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2, btn3 });
            rkm.ResizeKeyboard = true;


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
                text = text.Replace("[pay]", job.PayMin.ToString() + " - " + job.PayMax.ToString() + "$");
            }
            else
            {
                text = text.Replace("[pay]", "No expectations");
            }

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, $"<b>({count + 1}/{max})</b>\n\n" + text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Great! You are one step away from placing a job.\n\nIf everything suits you click Done if not Go Back or Cancel", replyMarkup: rkm);

        }
    }
}
