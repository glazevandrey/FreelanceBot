using FreelanceBot.Actions.JobActions;
using FreelanceBot.Database;
using FreelanceBot.Helpers;
using FreelanceBot.Models;
using FreelanceBot.Parsers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace FreelanceBot.Quartz
{
    public class QuartzService : IQuartzService
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public QuartzService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task ParseJob()
        {
            var parser = new ITJobSite();
            var jobs =  parser.ParseNewJobs();
            foreach (var item in jobs)
            {
                foreach (var job in item.Value)
                {
                    string text = Program.ParsedJobView;

                    var jobEn = new Job();

                    jobEn.Description = job.body;
                    jobEn.Title = item.Key;
                    jobEn.Pay = 0;
                    if (job.locations != null)
                    {
                        jobEn.Place = job.locations[0].name;
                    }
                    else
                    {
                        jobEn.Place = "No matter";
                    }

                    if (job.body.ToLower().Contains("senior"))
                    {
                        jobEn.Level = "Senior";
                    }else if (job.body.ToLower().Contains("middle"))
                    {
                        jobEn.Level = "Middle";

                    }
                    else if(job.body.ToLower().Contains("team lead"))
                    {
                        jobEn.Level = "Team Lead";

                    }
                    else if (job.body.ToLower().Contains("junior"))
                    {
                        jobEn.Level = "Junior";

                    }
                    else if (job.body.ToLower().Contains("intern"))
                    {
                        jobEn.Level = "Intern";
                    }else
                    {
                        jobEn.Level = "No matter";

                    }

                    if (job.body.ToLower().Contains("remote"))
                    {
                        jobEn.TypeJob = "Remote work";

                    }
                    else if (job.body.ToLower().Contains("office"))
                    {
                        jobEn.TypeJob = "Office work";

                    }
                    else
                    {
                        jobEn.TypeJob = "No matter";

                    }
                    jobEn.UserId = 0;

                    if(job.company.phone != null)
                    {
                        jobEn.Contact = "Company phone: " + job.company.phone;

                    }

                    if (job.company.email != null)
                    {
                        jobEn.Contact += "\nCompany email: " + job.company.email;

                    }
                   



                    var desc = Regex.Replace(jobEn.Description, @"<p.*?>", String.Empty, RegexOptions.CultureInvariant).Replace("<p>", "\n").Replace("</p>", string.Empty);
                    desc = Regex.Replace(desc, @"<ul.*?>", String.Empty, RegexOptions.CultureInvariant).Replace("<ul>", string.Empty).Replace("</ul>", string.Empty);
                    desc = Regex.Replace(desc, @"<li.*?>", String.Empty, RegexOptions.CultureInvariant).Replace("<li>", string.Empty).Replace("</li>", string.Empty);
                    desc = desc.Replace("<bold>", "\n<bold>");
                    desc = desc.Replace("<b>", "\n<b>");
                    desc = desc.Replace("<strong>", "\n<strong>");
                    desc = desc.Replace("<br>", "\n");
                    desc = desc.Replace("</br>", string.Empty);
                    desc = desc.Replace("<br />", "\n");
                    desc = desc.Replace("<br/>", "\n");

                    text = text.Replace("[title]", jobEn.Title);
                    text = text.Replace("[contact]", jobEn.Contact);
                    text = text.Replace("[description]", desc);
                    text = text.Replace("[level]", jobEn.Level);
                    text = text.Replace("[place]", jobEn.Place);
                    text = text.Replace("[type]", jobEn.TypeJob);


                    
                    using (var db = new UserContext())
                    {
                       
                        try
                        {
                            if (db.Jobs.FirstOrDefault(m => m.Description == jobEn.Description) != null)
                            {
                                continue;
                            }
                            jobEn.IsDone = true;
                            db.Jobs.Add(jobEn);
                            db.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            
                            throw ex;
                        }
                    }
                    try
                    {
                        await Program.botClient.SendTextMessageAsync(SendToChannel._channelId, text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                    }
                    catch (Exception ex)
                    {
                        if(ex.Message.ToLower().Contains("too long"))
                        {
                            var text1 = text.Substring(0,text.Length/2);
                            var text2 = text.Replace(text1, string.Empty);
                            await Program.botClient.SendTextMessageAsync(SendToChannel._channelId, text1, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                            Thread.Sleep(100);
                            try
                            {
                                await Program.botClient.SendTextMessageAsync(SendToChannel._channelId, text2, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                            }
                            catch (Exception ex2)
                            {
                                Thread.Sleep(5000);
                                await Program.botClient.SendTextMessageAsync(SendToChannel._channelId, text2, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                            }

                        }

                        continue;
                    }
                    Thread.Sleep(10000);
                }
                Thread.Sleep(30000);

            }

        }
    }
}
