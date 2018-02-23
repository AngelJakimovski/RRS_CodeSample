using System.Collections.Generic;
using OpenQA.Selenium;

namespace AARP.WebApi
{
    public interface IWebDriverApi
    {
        string BaseUrl { get; }
        IWebDriver WebDriver { get; }
        T LoadDataObject<T>(string dataApiUrl);
        IList<T> LoadDataList<T>(string dataApiUrl);

        T LoadDataObject_New<T>(string dataApiUrl) where T : new();

        IList<T> LoadDataList_New<T>(string dataApiUrl);
        void CleanUp();
    }
}
