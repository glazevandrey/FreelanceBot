using FreelanceBot.Actions;
using FreelanceBot.Actions.BossSearchingActions;
using FreelanceBot.Actions.JobActions;
using FreelanceBot.Actions.ResumeActions;
using FreelanceBot.Actions.WorkerActions;
using FreelanceBot.Database;
using FreelanceBot.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;

namespace FreelanceBot.Helpers
{
    public class SetWay
    {
        public List<IBaseAction> GetInline(Telegram.Bot.Types.Update update)
        {
            IBaseAction type;
            List<IBaseAction> result = new List<IBaseAction>();

            var user = new User();
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(m=>m.ChatId == update.CallbackQuery.From.Id);
            }
           if(update.CallbackQuery != null)
            {
                if (update.CallbackQuery.Data.Contains("wantPack"))
                {
                    SendToChannel.SendNewPackage(update.CallbackQuery.Data);
                    return result;
                }
            }
            if(user.Role == Role.Заказчик)
            {
                if (update.CallbackQuery.Data.Contains("spec"))
                {
                    result.Add(new ResultSeaSpecAction());
                }
                if (update.CallbackQuery.Data.Contains("text"))
                {
                    result.Add(new ResultSeaTextAction());
                }
                if (update.CallbackQuery.Data.Contains("pay"))
                {
                    result.Add(new ResultSeaByPayAction());
                }
                if (update.CallbackQuery.Data.Contains("level"))
                {
                    result.Add(new ResultSeaLevelAction());
                }
                if (update.CallbackQuery.Data.Contains("place"))
                {
                    result.Add(new ResultSeaByPlaceAction());
                }
            }
            else
            {
                if (update.CallbackQuery.Data.Contains("spec"))
                {
                    result.Add(new ResultSeaSpecWorkerAction());
                }
                if (update.CallbackQuery.Data.Contains("text"))
                {
                    result.Add(new ResultSeaTextWorkerAction());
                }
                if (update.CallbackQuery.Data.Contains("pay"))
                {
                    result.Add(new ResultSeaByPayWorkerAction());
                }
                if (update.CallbackQuery.Data.Contains("level"))
                {
                    result.Add(new ResultSeaLevelWorkerAction());
                }
                if (update.CallbackQuery.Data.Contains("place"))
                {
                    result.Add(new ResultSeaByPlaceWorkerAction());
                }
            }
            

            return result;
        }

        public List<IBaseAction> Get(Telegram.Bot.Types.Update update)
        {
            IBaseAction type;
            List<IBaseAction> result = new List<IBaseAction>();
            if(update.Message == null)
            {
                return result;
            }

           
            if(update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo ||
                update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Video ||
                update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                var usr = new User();
                using (var db = new UserContext())
                {
                    usr = db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id);
                }

                if(usr.Role == Role.Исполнитель)
                {
                    WorkerWay way = new WorkerWay();
                    return way.Start(update, usr);
                }
                else
                {
                    BossWay way = new BossWay();
                    return way.Start(update, usr);
                }
                
            }  
            
            if (update.Message.Text.StartsWith("/start"))
            {

                var usr = new User();
                using (var db = new UserContext())
                {
                    usr = db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id);
                }

                if(usr == null)
                {
                    type = new StartAction();
                    result.Add(type);
                    return result;
                }
                else
                {
                    if(usr.Role == Role.Исполнитель)
                    {
                        result.Add(new MainMenuWorkerAction());
                        return result;
                    }
                    else
                    {
                        result.Add(new MainMenuBossAction());
                        return result;
                    }
                }

            }

            if (update.Message.Text.StartsWith("/pack"))
            {
                AdminTools.UpdatePack(update);
                return result;
            }


            if (update.Message.Text == "Change me")
            {
                type = new ChangeClassAction();
                result.Add(type);
                return result;
            }
            if (update.Message.Text == "Settings")
            {
                type = new SettingInAction();
                result.Add(type);
                return result;
            }


            if (update.Message.Text == "I am a candidate")
            {
                type = new MainMenuWorkerAction();
                result.Add(type);
                return result;
            }
            if (update.Message.Text == "I am an employer")
            {
                type = new MainMenuBossAction();
                result.Add(type);
                return result;
            }



            var user = new User();
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(m => m.ChatId == update.Message.From.Id);
            }

            if (user.Role == Models.Role.Заказчик)
            {
                BossWay way = new BossWay();
                return way.Start(update, user);
            }
            else
            {
                WorkerWay way = new WorkerWay();
                return way.Start(update, user);
            }
        }
    }
}
