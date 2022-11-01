using FreelanceBot.Database;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBot.Helpers
{
    public class AdminTools
    {
        public static async void UpdatePack(Update update)
        {
            var text = update.Message.Text;
            var split = text.Split(" ");
            if(split.Length < 3)
            {
                await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Error. Example: /pack andreyglazev job");
                return;
            }

            var username = split[1];
            var type = split[2];

            Update(type, username);
         
        }
        private static async void Update(string type, string username)
        {
            switch (type)
            {
                case "job":
                    using (var db = new UserContext())
                    {
                        var user = db.Users.FirstOrDefault(m=>m.Username == username);
                        if(user == null)
                        {
                            return;
                        }

                        user.MaxJobs = user.MaxJobs + 5;
                        db.SaveChanges();
                        
                    }
                    break;
                case "event":
                    using (var db = new UserContext())
                    {
                        var user = db.Users.FirstOrDefault(m => m.Username == username);
                        if (user == null)
                        {
                            return;
                        }

                        user.MaxEvents = user.MaxEvents + 5;
                        db.SaveChanges();

                    }
                    break;
                case "resume":
                    using (var db = new UserContext())
                    {
                        var user = db.Users.FirstOrDefault(m => m.Username == username);
                        if (user == null)
                        {
                            return;
                        }

                        user.MaxResumes = user.MaxResumes + 5;
                        db.SaveChanges();

                    }
                    break;
                default:
                    break;
            }
        }
    }
}
