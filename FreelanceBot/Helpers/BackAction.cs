using FreelanceBot.Actions;
using FreelanceBot.Actions.EventAction;
using FreelanceBot.Actions.JobActions;
using FreelanceBot.Actions.ResumeActions;
using FreelanceBot.Database;
using FreelanceBot.Models;
using System.Linq;

namespace FreelanceBot.Helpers
{
    public class BackAction
    {
        public static IBaseAction Back(User user)
        {
            if(user.Role == Role.Заказчик)
            {
                switch (user.Stage)
                {
                    case Stage.ВыберитеЗаголовокРаботы:
                        using (var db = new UserContext())
                        {
                            var job = db.Jobs.FirstOrDefault(m => m.UserId == user.ChatId && m.IsDone == false);
                            db.Jobs.Remove(job);
                            db.SaveChanges();
                        }
                        return new MainMenuBossAction();
                    case Stage.ВведитеОписаниеРаботы:
                        return new MakeJobAction();
                    case Stage.ВыберитеУровеньСпециалистаРаботы:
                        return new DescriptionJobAction();
                    case Stage.ВыберитеЗарплатуРаботы:
                        return new ChooseLevelAction();
                    case Stage.ВыберитеМестоРаботы:
                        return new PayCostAction();
                    case Stage.ВыберитеОфисИлиУдалработы:
                        return new WorkPlaceAction();
                    case Stage.ПредПоказРаботы:
                        return new DistantOrOfficeAction();
                    case Stage.ВведитеЗаголовокЕвента:
                        using (var db = new UserContext())
                        {
                            var resume = db.Events.FirstOrDefault(m => m.UserId == user.ChatId && m.IsDone == false);
                            db.Events.Remove(resume);
                            db.SaveChanges();
                        }
                        return new MainMenuBossAction();
                    case Stage.ВведитеОписаниеЕвента:
                        return new MakeTitleEventAction();
                    case Stage.ВведитеДатуЕвента:
                        return new MakeDescriptionEvent();
                    case Stage.ХотитеФайл:
                        return new ChooseDateEvent();
                    case Stage.ПрикрепитьФайл:
                        return new WantAttachEvent();
                    case Stage.ПредпоказЕвент:
                        return new ChooseDateEvent();
                    default:
                        return new MainMenuBossAction();
                }
            }
            else
            {
                switch (user.Stage)
                {
                    case Stage.ВыберитеЗаголовокРезюме:
                        using (var db = new UserContext())
                        {
                            var resume = db.Resumes.FirstOrDefault(m => m.UserId == user.ChatId && m.IsDone == false);
                            db.Resumes.Remove(resume);
                            db.SaveChanges();
                        }
                        return new MainMenuWorkerAction();
                    case Stage.ВашеОписаниеРезюме:
                        return new MakeAResumeAction();
                    case Stage.ВашУровень:
                        return new DescriptionResumeAction();
                    case Stage.ВашаЖелаемаяЗП:
                        return new LevelResumeAction();
                    case Stage.ВашГород:
                        return new PayAction();
                    case Stage.ЖелаетеПрикрепить:
                        return new YourPlaceAction();
                    case Stage.Прикрепить:
                        return new WantDocAction();
                    case Stage.Предпоказ:
                        return new YourPlaceAction();
                    default:
                        return new MainMenuWorkerAction();
                }
            }
        }
    }
}
