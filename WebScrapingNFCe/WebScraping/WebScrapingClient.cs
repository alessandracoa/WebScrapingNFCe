using System;
using System.Net;

namespace WebScrapingNFCe.WebScraping
{
    public class WebScrapingClient : WebClient
    {
        public WebScrapingClient()
        {
            this.Headers.Add("Accept", "*/*");
            this.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
        }

        public CookieContainer _cookie = new CookieContainer();
        public bool _allowAutoRedirect;

        protected override WebRequest GetWebRequest(Uri address)
        {

            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).ServicePoint.Expect100Continue = false;
                (request as HttpWebRequest).CookieContainer = _cookie;
                (request as HttpWebRequest).KeepAlive = false;
                (request as HttpWebRequest).AllowAutoRedirect = _allowAutoRedirect;
            }

            return request;
        }
    }
}
