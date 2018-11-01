using HtmlAgilityPack;
using NLog;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace WebScrapingNFCe.WebScraping
{
    public class WebScraping
    {
        WebScrapingClient WebScrapingClient;

        public WebScraping()
        {
            WebScrapingClient = new WebScrapingClient();
            Logger logger = LogManager.GetCurrentClassLogger();
        }

        public HtmlDocument HttpGet(string url)
        {
            lock (this)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(WebScrapingClient.DownloadString(url));

                return htmlDocument;
            }
        }

        public HtmlDocument HttpPost(string url, NameValueCollection parametros)
        {
            string postData = "";
            var htmlDocument = new HtmlDocument();

            foreach (string key in parametros.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "="
                      + HttpUtility.UrlEncode(parametros[key]) + "&";
            }

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.Method = "POST";

            byte[] data = Encoding.ASCII.GetBytes(postData);

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";
            myHttpWebRequest.Accept = "*/*";
            myHttpWebRequest.UseDefaultCredentials = true;
            myHttpWebRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;


            using (Stream requestStream = myHttpWebRequest.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF7);

                string pageContent = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();

                htmlDocument.LoadHtml(pageContent);
            }

            return htmlDocument;
        }

    }
}
