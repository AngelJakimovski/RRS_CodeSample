using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AARP
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
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "nontechemails.txt");
            var emailsText = File.ReadAllText(path);

            List<string> result = new List<string>();
            result.AddRange(emailsText.Split(';'));
            return result;
        }
    }
}