using System;
using System.Collections.Generic;
using System.Configuration;
using AARP.WebApi.Helpers;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using AARP.WebApi.Models.RecruiterBoxConfig;
using RestSharp;
using RestSharp.Authenticators;

namespace AARP.WebApi
{
    public class RecruiterBoxPhantomJsApi : IWebDriverApi
    {
        private PhantomJSDriver _phantom;
        
        private static volatile RecruiterBoxPhantomJsApi _instance;

        private static readonly object SyncRoot = new object();
        private readonly object _networkSync = new object();

        #region string settings
        private readonly string _phantomJsUserAgent = ConfigurationManager.AppSettings["phantomJsUserAgent"];
        #endregion

        public string BaseUrl => ConfigurationManager.AppSettings["recruiterBoxRoot"];
        public string BaseUrlv2 => ConfigurationManager.AppSettings["recruiterBoxv2"];
        public string RecruiterBoxAPIKey => ConfigurationManager.AppSettings["recruiterBoxAPIKey"];
        public IWebDriver WebDriver => _phantom;

        public static RecruiterBoxPhantomJsApi Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new RecruiterBoxPhantomJsApi();
                        }
                    }
                }
                return _instance;
            }
        }

        private RecruiterBoxPhantomJsApi()
        {
            var configs = new ConfigurationsModelWebApi();
            InitPhantom(configs.RecruiterBoxConfigs.Login, configs.RecruiterBoxConfigs.Password);
        }

        private void InitPhantom(string login, string password)
        {
            var service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;
            service.LoadImages = false;
            service.ProxyType = "none";
            service.SslProtocol = "any";

            var options = new PhantomJSOptions();
            options.AddAdditionalCapability("phantomjs.page.settings.userAgent", _phantomJsUserAgent);

            _phantom = new PhantomJSDriver(service, options);
            _phantom.Navigate().GoToUrl($"{BaseUrl}/accounts/login");


            _phantom.FindElement(By.Id("input_login_email")).SendKeys(login);
            _phantom.FindElement(By.Id("input_login_password")).SendKeys(password);
            _phantom.FindElement(By.Id("signin_submit")).Click();
            new WebDriverWait(_phantom, TimeSpan.FromSeconds(10)).Until(
                ExpectedConditions.UrlContains($"{BaseUrl}/app/"));
        }

        public void Reinitialize(string login, string password)
        {
            lock (_networkSync)
            {
                _phantom.Quit();
                InitPhantom(login, password);
            }
        }

        public bool ConnectionTest(string login, string password)
        {
            var service = PhantomJSDriverService.CreateDefaultService();
            service.IgnoreSslErrors = true;
            service.LoadImages = false;
            service.ProxyType = "none";
            service.SslProtocol = "any";

            var options = new PhantomJSOptions();
            options.AddAdditionalCapability("phantomjs.page.settings.userAgent", _phantomJsUserAgent);

            var phantomCheckConnection = new PhantomJSDriver(service, options);
            phantomCheckConnection.Navigate().GoToUrl($"{BaseUrl}/accounts/login");

            phantomCheckConnection.FindElement(By.Id("input_login_email")).SendKeys(login);
            phantomCheckConnection.FindElement(By.Id("input_login_password")).SendKeys(password);
            phantomCheckConnection.FindElement(By.Id("signin_submit")).Click();

            Func < IWebDriver, bool> del = ExpectedConditions.UrlContains($"{BaseUrl}/app/");
            var result = del.Invoke(phantomCheckConnection);
            phantomCheckConnection.Quit();
            return result;
        }
        
        public T LoadDataObject<T>(string dataApiUrl)
        {
            lock (_networkSync)
            {
                if (_phantom != null)
                {
                    _phantom.Navigate().GoToUrl($"{BaseUrl}/{dataApiUrl}");
                    return JsonConvert.DeserializeObject<T>(_phantom.FindElementByTagName("body").Text,
                        new TimestampJsonConverter(), new BooleanConverter());
                }
                return default(T);
            }
        }

        public IList<T> LoadDataList<T>(string dataApiUrl)
        {
            var info = LoadDataObject<RecruiterBoxData<T>>(dataApiUrl);
            var result = info?.Objects;
            while (!string.IsNullOrEmpty(info?.Meta?.NextUrl))
            {
                info = LoadDataObject<RecruiterBoxData<T>>(info.Meta.NextUrl);
                if (info?.Objects != null)
                {
                    result.AddRange(info.Objects);
                }
            }
            return result;
        }

        public T LoadDataObject_New<T>(string dataApiUrl) where T : new()
        {
            lock (_networkSync)
            {
                var client = new RestClient
                {
                    BaseUrl = new Uri(BaseUrlv2),
                    Authenticator = new HttpBasicAuthenticator(RecruiterBoxAPIKey, "")
                };

                var request = new RestRequest(dataApiUrl, Method.GET);

                request.AddHeader("content-type", "application/json");
                var response = client.Execute<T>(request);

                return response.Data;
            }
        }

        public IList<T> LoadDataList_New<T>(string dataApiUrl)
        {
            var info = LoadDataObject_New<RecruiterBoxData<T>>(dataApiUrl);
            var result = info?.Objects;
            while (!string.IsNullOrEmpty(info?.Meta?.NextUrl))
            {
                info = LoadDataObject<RecruiterBoxData<T>>(info.Meta.NextUrl);
                if (info?.Objects != null)
                {
                    result.AddRange(info.Objects);
                }
            }
            return result;
        }

        public void CleanUp()
        {
            lock (_networkSync)
            {
                _phantom.Quit();
            }
        }
    }
}