using System;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AARP.Services.Extensions
{
    public static class UrlShortener
    {
        public static string GetShortUrl(string longUrl)
        {
            UrlshortenerService service = new UrlshortenerService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyByVwLFuOW52I7RH3MAD--Q0uhbg-KktMM"
            });

            var m = new Google.Apis.Urlshortener.v1.Data.Url();
            m.LongUrl = longUrl;
            return service.Url.Insert(m).Execute().Id;
        }
    }
}
