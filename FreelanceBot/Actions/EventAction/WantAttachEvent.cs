using FreelanceBot.Database;
using FreelanceBot.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBot.Actions.EventAction
{
    public class WantAttachEvent : IBaseAction
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public async Task Start(Update update)
        {
            Program.stageService.SetStage(update.Message.From.Id, 21);


            DateTime date;

            try
            {
                date = DateTime.ParseExact(update.Message.Text.Trim(), "dd/MM/yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                logger.Error(ex  + "first");

                try
                {
                    date = DateTime.ParseExact(update.Message.Text.Trim(), "d/M/yyyy h:m tt", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex2)
                {

                    await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "You should enter date in format day/month/year hour:minutes AM/PM\nFor example: 3/11/2022 5:12 AM");
                    Program.stageService.SetStage(update.Message.From.Id, 20);
                    return;
                }
            }

            var all = new List<Event>();
            using (var db = new UserContext())
            {
                var ev = db.Events.FirstOrDefault(m => m.UserId == update.Message.From.Id && m.IsDone == false);
                all = db.Events.Where(m=>m.IsDone == true).ToList();
                foreach (var item in all)
                {
                    if(item.StartDate == null)
                    {
                        continue;
                    }
                    DateTime dbDate = new DateTime();

                    try
                    {
                        dbDate = DateTime.ParseExact(item.StartDate, "dd.MM.yyyy h:m", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex + "first2");

                        try
                        {
                            dbDate = DateTime.ParseExact(item.StartDate, "dd.MM.yyyy H:m", CultureInfo.InvariantCulture);

                          
                        }
                        catch (Exception ex2)
                        {
                            try
                            {
                                dbDate = DateTime.ParseExact(item.StartDate, "dd/MM/yyyy H:m", CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex3)
                            {
                                try
                                {
                                    dbDate = DateTime.ParseExact(item.StartDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                                }
                                catch (Exception ex4)
                                {

                                    continue;
                                }
                            }
                        }
                    
                     }


                    if(dbDate == date)
                    {
                        await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Sorry this time is booked. Try set other time, or date");
                        Program.stageService.SetStage(update.Message.From.Id, 20);
                        return;
                    }

                    if(dbDate < date)
                    {
                        if(dbDate.AddHours(1) >= date)
                        {
                            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Sorry this time is booked. Try set other time, or date");
                            Program.stageService.SetStage(update.Message.From.Id, 20);
                            return;
                        }
                    }else if (dbDate > date)
                    {
                        if(dbDate.AddHours(-1)  <= date)
                        {
                            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Sorry this time is booked. Try set other time, or date");
                            Program.stageService.SetStage(update.Message.From.Id, 20);
                            return;
                        }
                    }

                }
                if (ev == null)
                {
                    return;
                }

                ev.StartDate = date.ToString("dd/MM/yyyy HH:mm");
                db.SaveChanges();
            }
            var btn1 = new KeyboardButton("Yes");
            var btn2 = new KeyboardButton("No");


            var rkm = new ReplyKeyboardMarkup(new List<KeyboardButton>() { btn1, btn2 });
            rkm.ResizeKeyboard = true;
            await Program.botClient.SendTextMessageAsync(update.Message.From.Id, "Would you like to attach a video or image?", replyMarkup: rkm);
        }
    }
}
