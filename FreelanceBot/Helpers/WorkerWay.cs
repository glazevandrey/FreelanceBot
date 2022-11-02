using FreelanceBot.Actions;
using FreelanceBot.Actions.BossSearchingActions;
using FreelanceBot.Actions.JobActions;
using FreelanceBot.Actions.ResumeActions;
using FreelanceBot.Actions.WorkerActions;
using FreelanceBot.Database;
using FreelanceBot.Models;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace FreelanceBot.Helpers
{
    public class WorkerWay
    {
        public List<IBaseAction> Start(Update update, FreelanceBot.Models.User user)
        {
            var result = new List<IBaseAction>();
            if (update.Message.Text == "Publish a resume")
            {
                result.Add(new MakeAResumeAction());
            }
            if (update.Message.Text == "Job searching")
            {
                result.Add(new WhatTypeWorkerSearch());
            }
            if (user.Stage == Models.Stage.ВыберитеЗаголовокРезюме)
            {
                result.Add(new DescriptionResumeAction());
            }
            if (user.Stage == Models.Stage.ВашеОписаниеРезюме)
            {
                result.Add(new LevelResumeAction());
            }
            if (user.Stage == Models.Stage.ВашУровень)
            {
                result.Add(new PayAction());
            }
            if (user.Stage == Models.Stage.ВашаЖелаемаяЗП)
            {
                result.Add(new YourPlaceAction());
            }
            if (user.Stage == Models.Stage.ВашГород)
            {
                result.Add(new WantDocAction());
            }
            if (user.Stage == Models.Stage.ЖелаетеПрикрепить)
            {
                if(update.Message.Text == "Yes")
                {
                    result.Add(new AttachFileAction());
                }
                else
                {
                    using (var db = new UserContext())
                    {
                        var resume = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                        resume.HaveFile = false;
                        resume.FileName = null;
                        db.SaveChanges();
                    }
                    result.Add(new PreviewResumeAction());
                }
            }
            if (user.Stage == Models.Stage.Прикрепить)
            {
                result.Add(new PreviewResumeAction());
            }
            if (user.Stage == Models.Stage.Предпоказ)
            {
                if (update.Message.Text == "Done")
                {
                    var resume = new Resume();
                    using (var db = new UserContext())
                    {
                        resume = db.Resumes.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                        resume.IsDone = true;
                        db.SaveChanges();
                    }
                    result.Add(new MainMenuWorkerAction());

                    SendToChannel.Send(resume, user);

                }
            }


            if (user.Stage == Models.Stage.ВыберитеТипПоискаБосс)
            {
                result.Add(ChooseSearch(update.Message.Text));
            }

            if (user.Stage == Models.Stage.ВыберитеСпециализациюПоискБосс)
            {
                result.Add(new ResultSeaSpecWorkerAction());
            }
            if (user.Stage == Models.Stage.ВведитеТекстПоискБосс)
            {
                result.Add(new ResultSeaTextWorkerAction());
            }
            if (user.Stage == Models.Stage.ВведитеСтоимостьПоиск)
            {
                result.Add(new ResultSeaByPayWorkerAction());
            }
            if (user.Stage == Models.Stage.ВведитеГород)
            {
                result.Add(new ResultSeaByPlaceWorkerAction());
            }
            if (user.Stage == Models.Stage.ВведитеЛевел)
            {
                result.Add(new ResultSeaLevelWorkerAction());
            }

            if (update.Message.Text == "Back")
            {
                result = new List<IBaseAction>();
                result.Add(BackAction.Back(user));
            }
            if (update.Message.Text == "Cancel" || update.Message.Text == "To main menu")
            {
                result = new List<IBaseAction>();
                result.Add(new MainMenuWorkerAction());
            }
            return result;
        }
        private IBaseAction ChooseSearch(string name)
        {

            switch (name)
            {
                case "By speciality":
                    return new SearchingBySpecWorkerAction();
                case "By text":
                    return new SearchingByTextWorkerAction();
                case "By level":
                    return new SeachingByLevelWorkerAction();
                case "By salary":
                    return new SearchingByPayWorkerAction();
                case "By country/city":
                    return new SeachingByPlaceWorkerAction();
                default:
                    break;
            }
            return new MainMenuWorkerAction();
        }

    }
}
