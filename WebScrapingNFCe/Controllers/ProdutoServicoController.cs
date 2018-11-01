using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebScrapingNFCe.Helpers;
using WebScrapingNFCe.Models;

namespace WebScrapingNFCe.Controllers
{
    public class ProdutoServicoController
    {
        public static List<ProdutoServico> GetContentProdutoServicoFromHtml(string htmlContent)
        {
            List<ProdutoServico> listaProdutoServico = new List<ProdutoServico>();
            ProdutoServico resultProdutoServico = new ProdutoServico();
            bool extractedDataTableMaster = false;
            bool extractedDataTableDetail = false;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//*[@id=\"Prod\"]/fieldset/div/table"))
            {
                if (Helper.IsTableMaster(table))
                {
                    resultProdutoServico = ExtractInformationProductTabTableMaster(table);
                    extractedDataTableMaster = true;
                }
                else if (Helper.IsTableDetail(table))
                {
                    resultProdutoServico = ExtractInformationProductTabTabletail(table.SelectNodes("tr/td/table"), resultProdutoServico);
                    extractedDataTableDetail = true;
                }
                
                if (extractedDataTableMaster && extractedDataTableDetail)
                {
                    extractedDataTableMaster = extractedDataTableDetail = false;
                    listaProdutoServico.Add(resultProdutoServico);
                }
            }

            return listaProdutoServico;
        }

        #region Métodos para extração de informação de tabelas

        private static ProdutoServico ExtractInformationProductTabTabletail(HtmlNodeCollection tableColletion, ProdutoServico compraitem)
        {
            var table1 = tableColletion[0];
            compraitem.CodigoProduto = table1.SelectNodes("tr/td/span")[0].InnerHtml.Trim();
            compraitem.CodigoNCM = table1.SelectNodes("tr/td/span")[1].InnerHtml.Trim();
            compraitem.CodigoCEST = table1.SelectNodes("tr/td/span")[2].InnerHtml.Trim();
                        
            if (!String.IsNullOrWhiteSpace(table1.SelectNodes("tr/td/span")[9].InnerHtml.Trim()))
                compraitem.ValorDesconto = Convert.ToDecimal(table1.SelectNodes("tr/td/span")[9].InnerHtml.Trim());
            
            var table2 = tableColletion[1];
            compraitem.CodigoEANComercial = table2.SelectNodes("tr/td/span")[1].InnerHtml.Trim();
            compraitem.UnidadeComercial = table2.SelectNodes("tr/td/span")[2].InnerHtml.Trim();
            
            if (!String.IsNullOrWhiteSpace(table2.SelectNodes("tr/td/span")[7].InnerHtml.Trim()))
                compraitem.ValorUnitarioComercial = Convert.ToDecimal(table2.SelectNodes("tr/td/span")[7].InnerHtml.Trim());
                                    
            if (!String.IsNullOrWhiteSpace(table2.SelectNodes("tr/td/span")[11].InnerHtml))
                compraitem.ValorAproxTributos = Convert.ToDecimal(table2.SelectNodes("tr/td/span")[11].InnerHtml.Trim());
            

            return compraitem;
        }


        private static ProdutoServico ExtractInformationProductTabTableMaster(HtmlNode table)
        {
            Regex regex = new Regex(@"\s{2,}"); //Remove espaços a mais entre palavras 

            ProdutoServico produtoServico = new ProdutoServico();

            produtoServico.DescricaoProduto = regex.Replace(table.SelectNodes("tr/td")[1].InnerText.Replace("&amp;", ""), " ");

            produtoServico.Qtd = !String.IsNullOrWhiteSpace(table.SelectNodes("tr/td")[2].InnerText) ? Convert.ToDecimal(table.SelectNodes("tr/td")[2].InnerText) : decimal.MinValue;
            produtoServico.UnidadeComercial = table.SelectNodes("tr/td")[3].InnerText;

            if (!String.IsNullOrWhiteSpace(table.SelectNodes("tr/td")[4].InnerText))
                produtoServico.ValorTotal = Convert.ToDecimal(table.SelectNodes("tr/td")[4].InnerText);

            return produtoServico;
        }

        #endregion

    }
}
