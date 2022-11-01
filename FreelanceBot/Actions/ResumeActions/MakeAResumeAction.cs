using FreelanceBot.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.ResumeActions
{
    public class MakeAResumeAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 10);
            int max = 0;
            using (var db = new UserContext())
            {
                max = db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id).MaxResumes;

                if (db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false) != null)
                {

                }
                else
                {
                    if (db.Resumes.Where(m => m.UserId == update.Message.From.Id).ToList().Count == max)
                    {
                        var btn1 = new InlineKeyboardButton("Order a new package");

                        if (update.Message.From.Username == null || update.Message.From.Username == "")
                        {
                            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "You need to fill out your telegram username");
                            return;
                        }
                        else
                        {
                            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Your order was sended to admin. Wait a message!\nThank you");
                        }

                        btn1.CallbackData = $"wantPack:{update.Message.From.Id}:{update.Message.From.Username}:resume";

                        var rkm2 = new InlineKeyboardMarkup(btn1);

                        await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "You have reached the maximum number of resumes", replyMarkup: rkm2);
                        Program.stageService.SetStage(update.Message.From.Id, 2);

                        return;
                    }

                    db.Resumes.Add(new Models.Resume() { UserId = update.Message.From.Id });
                }
                db.SaveChanges();
            }

            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            var list = new List<string>();
            foreach (var item in Program.config.Specialization)
            {
                list.Add(item);
            }

            list.Add("Back");

            var rkm = new ReplyKeyboardMarkup("");
            for (var Index = 0; Index < list.Count; Index++)
            {
                cols.Add(new KeyboardButton(list[Index]));
                //if (Index % 2!= 0) continue;
                rows.Add(cols.ToArray());
                cols = new List<KeyboardButton>();
            }

            rkm.Keyboard = rows.ToArray();
            rkm.ResizeKeyboard = true;


            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Select the resume title from the suggested ones:", replyMarkup: rkm);
        }
    }
}
