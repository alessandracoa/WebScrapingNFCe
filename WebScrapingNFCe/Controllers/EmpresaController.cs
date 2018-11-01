using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebScrapingNFCe.Helpers;
using WebScrapingNFCe.Models;

namespace WebScrapingNFCe.Controllers
{
    public class EmpresaController
    {
        public static Empresa GetContentEmpresaFromHtml(string htmlContent)
        {            
            Empresa empresa = new Empresa();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            
            if (!String.IsNullOrWhiteSpace(Helper.ExtraiFieldIdEmitente(doc, "tr[2]/td[1]/span")))
                empresa.CNPJ = Convert.ToInt64(Helper.ExtraiFieldIdEmitente(doc, "tr[2]/td[1]/span").Replace("-", "").Replace("/", "").Replace(".", ""));

            empresa.RazaoSocial = Helper.ExtraiFieldIdEmitente(doc, "tr[1]/td[1]/span");
            empresa.NomeFantasia = Helper.ExtraiFieldIdEmitente(doc, "tr[1]/td[2]/span");
            empresa.InscricaoEstadual = Helper.ExtraiFieldIdEmitente(doc, "tr[6]/td[1]/span");

            empresa.Telefone = Helper.ExtraiFieldIdEmitente(doc, "tr[4]/td[2]/span");

            empresa.Endereco = EnderecoController.GetContentEnderecoFromHtml(htmlContent);

            return empresa;
        }

    }
}
