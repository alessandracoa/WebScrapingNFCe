using HtmlAgilityPack;
using NLog;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using WebScrapingNFCe.Controllers;
using WebScrapingNFCe.WebScraping;

namespace WebScrapingNFCe.Helpers
{
    public static class Helper
    {      
        public static string GetHtmlContent(string nroChaveAcesso)
        {
            WebScraping.WebScraping webScraping = new WebScraping.WebScraping();
            
            NameValueCollection parametros = new NameValueCollection();
            parametros.Add("HML", "false");
            parametros.Add("chaveNFe", nroChaveAcesso);
            parametros.Add("Action", "Avan%E7ar");

            HtmlDocument doc = new HtmlDocument();            
            
            var htmlContentInitial = webScraping.HttpPost(@"https://www.sefaz.rs.gov.br/ASP/AAE_ROOT/NFE/SAT-WEB-NFE-NFC_2.asp", parametros);
            
            var nroNF = htmlContentInitial.DocumentNode.InnerHtml.Substring(htmlContentInitial.DocumentNode.InnerHtml.IndexOf("&NF") + 1, 12); //Infomação necessário para "Visualizar em Abas" 

            NameValueCollection parametrosAba = new NameValueCollection();
            parametrosAba.Add("chaveNFe", nroChaveAcesso);
            parametrosAba.Add("HML", "false");
            parametrosAba.Add(nroNF, "");

            var htmlContentAbas = webScraping.HttpPost(@"https://www.sefaz.rs.gov.br/ASP/AAE_ROOT/NFE/SAT-WEB-NFE-COM_2.asp", parametrosAba);
            
            string uri = "https://www.sefaz.rs.gov.br/ASP/AAE_ROOT/NFE/SAT-WEB-NFE-COM_2.asp?chaveNFe=" +
                nroChaveAcesso +
                "&HML=false&NF" + nroNF;

            return htmlContentAbas.DocumentNode.InnerHtml.Trim();
        }


        public static string ExtraiFieldIdEmitente(HtmlDocument doc, string html)
        {
            string htmlIdEmitente = "//*[@id=\"Emitente\"]/fieldset/table/" + html;
            
            return doc.DocumentNode.SelectSingleNode(htmlIdEmitente) != null ? doc.DocumentNode.SelectSingleNode(htmlIdEmitente).InnerHtml.Trim().Replace("\n ", "") : "";
        }

        public static string ExtraiFieldIdInf(HtmlDocument doc, string html)
        {
            string htmlIdInf = "//*[@id=\"Inf\"]/fieldset/table/" + html;

            return doc.DocumentNode.SelectSingleNode(htmlIdInf) != null ? doc.DocumentNode.SelectSingleNode(htmlIdInf).InnerHtml.Trim().Replace("\n ", "") : "";
        }


        public static string ExtraiFieldNFe(HtmlDocument doc, string html)
        {
            string htmlIdNFe = "//*[@id=\"NFe\"]/fieldset[1]/table/" + html;
            
            return doc.DocumentNode.SelectSingleNode(htmlIdNFe) != null ? doc.DocumentNode.SelectSingleNode(htmlIdNFe).InnerHtml.Trim().Replace("\n ", "") : "";
        }


        #region Table's

        public static bool IsTableMaster(HtmlNode tableNode)
        {
            if (tableNode.GetAttributeValue("class", "").Equals("toggle box"))
            {
                return true;
            }

            return false;
        }

        public static bool IsTableDetail(HtmlNode tableNode)
        {
            if (tableNode.GetAttributeValue("class", "").Equals("toggable box"))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
