using FreelanceBot.Actions;
using FreelanceBot.Actions.BossSearchingActions;
using FreelanceBot.Actions.EventAction;
using FreelanceBot.Actions.JobActions;
using FreelanceBot.Actions.ResumeActions;
using FreelanceBot.Database;
using FreelanceBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Update = Telegram.Bot.Types.Update;

namespace FreelanceBot.Helpers
{
    public class BossWay
    {
        public List<IBaseAction> Start(Update update, FreelanceBot.Models.User user)
        {
            var result = new List<IBaseAction>();
            if (update.Message.Text == "Post a job")
            {
                result.Add(new MakeJobAction());
            }
            if (user.Stage == Models.Stage.ВыберитеЗаголовокРаботы)
            {
                result.Add(new DescriptionJobAction());
            }
            if (user.Stage == Models.Stage.ВведитеОписаниеРаботы)
            {
                result.Add(new ChooseLevelAction());
            }
            if (user.Stage == Models.Stage.ВыберитеУровеньСпециалистаРаботы)
            {
                result.Add(new PayCostAction());
            }
            if (user.Stage == Models.Stage.ВыберитеЗарплатуРаботы)
            {
                result.Add(new WorkPlaceAction());
            }
            if (user.Stage == Models.Stage.ВыберитеМестоРаботы)
            {
                result.Add(new DistantOrOfficeAction());
            }
            if (user.Stage == Models.Stage.ВыберитеОфисИлиУдалработы)
            {
                result.Add(new PreviewJobAction());
            }
            if (user.Stage == Models.Stage.ПредПоказРаботы)
            {
                if(update.Message.Text == "Done")
                {
                    var job = new Job();
                    using (var db = new UserContext())
                    {
                        job = db.Jobs.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                        job.IsDone = true;
                        db.SaveChanges();
                    }
                    result.Add(new MainMenuBossAction());

                    SendToChannel.Send(job, user);
                }
            }

            if (update.Message.Text == "Post event")
            {
                result.Add(new MakeTitleEventAction());
            }
            if (user.Stage == Models.Stage.ВведитеЗаголовокЕвента)
            {
                result.Add(new MakeDescriptionEvent());
            }
            if (user.Stage == Models.Stage.ВведитеОписаниеЕвента)
            {
                result.Add(new ChooseDateEvent());
            }
            if (user.Stage == Models.Stage.ВведитеДатуЕвента)
            {
                result.Add(new WantAttachEvent());
            }
            if (user.Stage == Models.Stage.ХотитеФайл)
            {
                if(update.Message.Text == "Yes")
                {
                    result.Add(new SendDocEventAction());
                }
                else
                {
                    using (var db = new UserContext())
                    {
                        var resume = db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                        resume.HaveFile = false;
                        resume.FileName = null;
                        db.SaveChanges();
                    }
                    result.Add(new PreviewEventAction());
                }
            }
            if (user.Stage == Models.Stage.ПрикрепитьФайл)
            {
                result.Add(new PreviewEventAction());
            }
            if (user.Stage == Models.Stage.ПредпоказЕвент)
            {
                if (update.Message.Text == "Done")
                {
                    Event evnt = new Event();
                    using (var db = new UserContext())
                    {
                        evnt = db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                        evnt.IsDone = true;
                        db.SaveChanges();
                    }

                    result.Add(new MainMenuBossAction());

                    //SendToChannel.Send(evnt, user);
                    Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Thank you! Your event request was sent to the admin of channel  \"Jobs & Events in IT\". He will contact you soon");
                    SendToChannel.SendEventToAdmin(evnt, user);
                }
            }
           

            if (update.Message.Text == "Resume search")
            {
                result.Add(new WhatTypeAction());
            }
            if (user.Stage == Models.Stage.ВыберитеТипПоискаБосс)
            {
                result.Add(ChooseSearch(update.Message.Text));
            }

            if (user.Stage == Models.Stage.ВыберитеСпециализациюПоискБосс)
            {
                result.Add(new ResultSeaSpecAction());
            }
            if (user.Stage == Models.Stage.ВведитеТекстПоискБосс)
            {
                result.Add(new ResultSeaTextAction());
            }
            if (user.Stage == Models.Stage.ВведитеСтоимостьПоиск)
            {
                result.Add(new ResultSeaByPayAction());
            }
            if (user.Stage == Models.Stage.ВведитеГород)
            {
                result.Add(new ResultSeaByPlaceAction());
            }
            if (user.Stage == Models.Stage.ВведитеЛевел)
            {
                result.Add(new ResultSeaLevelAction());
            }



            if (update.Message.Text == "Back")
            {
                result = new List<IBaseAction>();
                result.Add(BackAction.Back(user));
            }
            if (update.Message.Text == "To main menu")
            {


                result = new List<IBaseAction>();
                result.Add(new MainMenuBossAction());
            }
            if (update.Message.Text == "Cancel")
            {
                using (var db = new UserContext())
                {
                    var job = db.Jobs.FirstOrDefault(m => m.UserId == user.ChatId && m.IsDone == false);
                    if(job == null)
                    {
                        var evnt = new Event();
                        evnt = db.Events.FirstOrDefault(m => m.UserId == user.ChatId && m.IsDone == false);
                        if(evnt == null)
                        {
                            result = new List<IBaseAction>();
                            result.Add(new MainMenuBossAction());
                            return result;
                        }
                        else
                        {
                            db.Events.Remove(evnt);

                        }
                    }
                    else
                    {
                        db.Jobs.Remove(job);
                    }

                    db.SaveChanges();

                }

                result = new List<IBaseAction>();
                result.Add(new MainMenuBossAction());
            }
            return result;
        }
        private IBaseAction ChooseSearch(string name)
        {
            
            switch (name)
            {
                case "By speciality":
                    return new SearchingBySpecAction();
                case "By text":
                    return new SearchingByTextAction();
                case "By level":
                    return new SeachingByLevelAction();
                case "By salary":
                    return new SearchingByPayAction();
                case "By country/city":
                    return new SeachingByPlaceAction();
                default:
                    break;
            }
            return null;
        }
    }
}
