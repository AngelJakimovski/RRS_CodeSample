namespace AARP.WebApi.Models.RecruiterBoxConfig
{
    using System.Configuration;

    public class RecruiterBoxConfigsWebApi : ConfigSectionWebApi
    {
        public RecruiterBoxConfigsWebApi()
        {
            Login = ConfigurationManager.AppSettings["defaultRecruiterBoxLogin"];
            Password = ConfigurationManager.AppSettings["defaultRecruiterBoxPassword"];
        }

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
