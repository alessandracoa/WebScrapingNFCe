using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;
using WebScrapingNFCe.Helpers;
using WebScrapingNFCe.Models;

namespace WebScrapingNFCe.Controllers
{
    public class InformacaoAdicionalController
    {
        public static InformacaoAdicional GetContentInformacaoAdicionalFromHtml(string htmlContent)
        {
            Regex regex = new Regex(@"\s{2,}"); //Remove espaços a mais entre palavras 

            InformacaoAdicional infoAdicional = new InformacaoAdicional();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            infoAdicional.FormatoImpressaoDANFE = Helper.ExtraiFieldIdInf(doc, "tr/td/span");

            if (!String.IsNullOrWhiteSpace(Helper.ExtraiFieldIdInf(doc, "tr[2]/td[1]/span")))
                infoAdicional.DataEntradaContigencia = Convert.ToDateTime(Helper.ExtraiFieldIdInf(doc, "tr[2]/td[1]/span"));

            infoAdicional.Justificativa = Helper.ExtraiFieldIdInf(doc, "tr[2]/td[2]/span");
            infoAdicional.InfoComplementar = regex.Replace(doc.DocumentNode.SelectSingleNode("//*[@id=\"Inf\"]/fieldset/fieldset[1]/table/tr/td/span/div").InnerHtml.Trim(), " ");

            return infoAdicional;

        }

    }
}
