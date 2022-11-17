using FreelanceBot.Database;
using FreelanceBot.Models;
using System.Linq;

namespace FreelanceBot.Services
{
    public class StageService
    {
        public void SetStage(long id, int stage)
        {
            using (var db = new UserContext())
            {
                var user = db.Users.SingleOrDefault(u => u.ChatId == id);
                if (user == null)
                {
                    return;
                }

                user.Stage = (Stage)stage;
                db.SaveChanges();
            }
        }
        public int GetStage(long id)
        {
            using (var db = new UserContext())
            {
                var stage = db.Users.FirstOrDefault(m => m.ChatId == id)?.Stage;
                return (int)stage.Value;
            }

        }
    }
}
