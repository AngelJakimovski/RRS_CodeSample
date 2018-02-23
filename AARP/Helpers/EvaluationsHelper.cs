using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AARP.Services.Helpers
{
    public static class EvaluationsHelper
    {
        public static bool IsTechInterviewer(string techEmail)
        {
            return !GetNonTechEmail().Contains(techEmail);
        }

        /// <summary>
        /// This method is loading non tech emails from the local file, and puts them into a list
        /// </summary>
        /// <returns></returns>
        private static List<string> GetNonTechEmail()
        {
            var emailsText = File.ReadAllText(HttpContext.Current.Server.MapPath("nontechemails.txt"));

            List<string> result = new List<string>();
            result.AddRange(emailsText.SplitAndTrim(';'));
            return result;
        }
    }
}