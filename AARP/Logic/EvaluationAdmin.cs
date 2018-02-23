using AARP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AARP.Logic
{
    public static class EvaluationAdmin
    {
        public static List<AdminSettingsViewModel>GetAdminSettings()
        {
            List<AdminSettingsViewModel> result = new List<AdminSettingsViewModel>();
            using (var dbContext = new AARPDbContext())
            {
                var settings = dbContext.AdminSettings.ToList();

                if (settings.Count==0)
                {
                    var techSettings1 = new AdminSettingsViewModel
                    {
                        Id = 1,
                    };
                    result.Add(techSettings1);

                    var techSettings2 = new AdminSettingsViewModel
                    {
                        Id = 2,
                    };
                    result.Add(techSettings2);
                }
                else
                {
                    var techSettings1 = settings.Where(x => x.Id == 1).FirstOrDefault();
                    if (techSettings1!=null)
                    {
                        var techSettings1VM = new AdminSettingsViewModel()
                        {
                            Id = 1,
                            EmailTimer = techSettings1.EmailTimer,
                            FeedbackMessage = techSettings1.FeedbackMessage,
                            EnableReminder = techSettings1.ReminderID,
                            ReminderMessage = techSettings1.ReminderMessage
                        };
                        result.Add(techSettings1VM);
                    }
                    else
                    {
                        var techSettings1VM = new AdminSettingsViewModel()
                        {
                            Id = 1,
                        };
                        result.Add(techSettings1VM);
                    }
                    var techSettings2 = settings.Where(x => x.Id == 2).FirstOrDefault();
                    if (techSettings2 != null)
                    {
                        var techSettings2VM = new AdminSettingsViewModel()
                        {
                            Id = 2,
                            EmailTimer = techSettings2.EmailTimer,
                            FeedbackMessage = techSettings2.FeedbackMessage,
                            EnableReminder = techSettings2.ReminderID,
                            ReminderMessage = techSettings2.ReminderMessage
                        };
                        result.Add(techSettings2VM);
                    }
                    else
                    {
                        var techSettings2VM = new AdminSettingsViewModel()
                        {
                            Id = 2,
                        };
                        result.Add(techSettings2VM);
                    }
                }
            }
            return result;
        }

        public static bool SetAdminSettings(List<AdminSettingsViewModel> saveModel)
        {
            using (var dbContext = new AARPDbContext())
            {
                var settings = dbContext.AdminSettings.ToList();

                var techSettings1 = settings.Where(x => x.Id == 1).FirstOrDefault();
                if (techSettings1 != null)
                {
                    techSettings1.EmailTimer = saveModel[0].EmailTimer;
                    techSettings1.FeedbackMessage = saveModel[0].FeedbackMessage;
                    techSettings1.ReminderID = saveModel[0].EnableReminder;
                    techSettings1.ReminderMessage = saveModel[0].ReminderMessage;
                    dbContext.SaveChanges();
                }
                else
                {
                    techSettings1 = new AdminSetting()
                    {
                        Id = 1,
                        EmailTimer = saveModel[0].EmailTimer,
                        FeedbackMessage=saveModel[0].FeedbackMessage,
                        ReminderID=saveModel[0].EnableReminder,
                        ReminderMessage=saveModel[0].ReminderMessage
                    };
                    dbContext.AdminSettings.Add(techSettings1);
                    dbContext.SaveChanges();
                }
                var techSettings2 = settings.Where(x => x.Id == 2).FirstOrDefault();
                if (techSettings2 != null)
                {
                    techSettings2.EmailTimer = saveModel[1].EmailTimer;
                    techSettings2.FeedbackMessage = saveModel[1].FeedbackMessage;
                    techSettings2.ReminderID = saveModel[1].EnableReminder;
                    techSettings2.ReminderMessage = saveModel[1].ReminderMessage;
                    dbContext.SaveChanges();
                }
                else
                {
                    techSettings2 = new AdminSetting()
                    {
                        Id = 2,
                        EmailTimer = saveModel[1].EmailTimer,
                        FeedbackMessage = saveModel[1].FeedbackMessage,
                        ReminderID = saveModel[1].EnableReminder,
                        ReminderMessage = saveModel[1].ReminderMessage
                    };
                    dbContext.AdminSettings.Add(techSettings2);
                    dbContext.SaveChanges();
                }
            }

            return true;
        }
    }
}