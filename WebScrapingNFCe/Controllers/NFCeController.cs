using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using WebScrapingNFCe.Helpers;
using WebScrapingNFCe.Models;

namespace WebScrapingNFCe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFCeController
    {
        [HttpGet]
        public string Get(string infoNota)
        {
            try
            {
                // Se a informação de entrada for por Link de QrCode
                if (infoNota.Contains("https:") || infoNota.Contains("www.sefaz.rs.gov.br"))
                {
                    infoNota = infoNota.Substring(infoNota.IndexOf("p=") + 2, 44); //Pega somente o número da chave de acesso
                }
                else
                {
                    if (!Regex.IsMatch(infoNota.Replace(" ", ""), @"[^\d]") && infoNota.Replace(" ", "").Length == 44)
                    {
                        infoNota = infoNota.Replace(" ", "");
                    }
                    else
                    {
                        if (infoNota.Replace(" ", "").Length < 44)
                        {
                            throw new ArgumentNullException("Quantidade", "Chave de Acesso inválida (menos do que 44 posições)");
                        }
                        else
                        {
                            throw new ArgumentNullException("Quantidade", "Chave de Acesso inválida (mais do que 44 posições)");
                        }

                        throw new ArgumentNullException("valor", "Chave de Acesso inválida");
                    }
                }

                return JsonConvert.SerializeObject(NFCeController.GetContentNFCeFromHtml(Helper.GetHtmlContent(infoNota)), Formatting.Indented).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }





        public static NFCe GetContentNFCeFromHtml(string htmlContent)
        {
            NFCe nota = new NFCe();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            nota.ChaveAcesso = doc.DocumentNode.SelectSingleNode("//fieldset").SelectSingleNode("//span").InnerHtml.Replace(" ", "").Trim();

            if (!String.IsNullOrWhiteSpace(Helper.ExtraiFieldNFe(doc, "tr/td[1]/span")))
                nota.Modelo = Convert.ToInt32(Helper.ExtraiFieldNFe(doc, "tr/td[1]/span"));

            if (!String.IsNullOrWhiteSpace(Helper.ExtraiFieldNFe(doc, "tr/td[3]/span")))
                nota.Numero = Convert.ToInt32(Helper.ExtraiFieldNFe(doc, "tr/td[3]/span"));

            if (!String.IsNullOrWhiteSpace(Helper.ExtraiFieldNFe(doc, "tr/td[2]/span")))
                nota.Serie = Convert.ToInt32(Helper.ExtraiFieldNFe(doc, "tr/td[2]/span"));

            nota.VersaoXML = doc.DocumentNode.SelectSingleNode("//fieldset").SelectNodes("//span")[2].InnerHtml.Trim();

            if (!String.IsNullOrWhiteSpace(Helper.ExtraiFieldNFe(doc, "tr/td[4]/span")))
                nota.DataEmissao = Convert.ToDateTime(Helper.ExtraiFieldNFe(doc, "tr/td[4]/span"));

            if (!String.IsNullOrWhiteSpace(Helper.ExtraiFieldNFe(doc, "tr/td[6]/span")))
                nota.ValorTotalNota = Convert.ToDecimal(Helper.ExtraiFieldNFe(doc, "tr/td[6]/span"));

            if (!String.IsNullOrWhiteSpace(doc.DocumentNode.SelectSingleNode("//*[@id=\"Totais\"]/fieldset/fieldset/table/tr[4]/td[3]/span").InnerHtml))
                nota.ValorDescontoTotalNota = Convert.ToDecimal(doc.DocumentNode.SelectSingleNode("//*[@id=\"Totais\"]/fieldset/fieldset/table/tr[4]/td[3]/span").InnerHtml.Trim());

            nota.LinkQrCode = doc.DocumentNode.SelectSingleNode("//*[@id=\"Inf\"]/fieldset/fieldset[2]/table/tr[1]/td/span").InnerHtml.Trim();

            nota.Empresa = EmpresaController.GetContentEmpresaFromHtml(htmlContent);

            nota.ListaProdutoServico = ProdutoServicoController.GetContentProdutoServicoFromHtml(htmlContent);

            nota.InformacaoAdicional = InformacaoAdicionalController.GetContentInformacaoAdicionalFromHtml(htmlContent);

            return nota;
        }
    }
}
