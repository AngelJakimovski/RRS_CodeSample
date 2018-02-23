using AARP.Models;
using AARP.Services.AppConfigs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services
{
    public class InterviewerProvider : BaseModelProvider<Interviewer>
    {
        public List<Interviewer> GetAll()
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Interviewers.ToList();
            }
        }

        public List<Interviewer> SearchInteviewer(string key)
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Interviewers.Where(p => p.InterviewerName.ToLower().Contains(key.ToLower())).ToList();
            }
        }

        public Interviewer GetById(int id)
        {
            using (var dbContext = new AARPDbContext())
            {
                return dbContext.Interviewers.Where(p => p.Id == id).FirstOrDefault();
            }
        }
    }
}
