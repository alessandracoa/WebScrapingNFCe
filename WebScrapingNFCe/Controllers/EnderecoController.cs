using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebScrapingNFCe.Helpers;
using WebScrapingNFCe.Models;

namespace WebScrapingNFCe.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {        
        public static Endereco GetContentEnderecoFromHtml(string htmlContent)
        {            
            Regex regex = new Regex(@"\s{2,}");  //Remove espaços a mais entre palavras 
            Endereco endereco = new Endereco();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            endereco.Rua = regex.Replace(Helper.ExtraiFieldIdEmitente(doc, "tr[2]/td[2]/span"), " ");
            endereco.Bairro = regex.Replace(Helper.ExtraiFieldIdEmitente(doc, "tr[3]/td[1]/span"), " ");
            endereco.Cidade = regex.Replace(Helper.ExtraiFieldIdEmitente(doc, "tr[4]/td[1]/span"), " ");
            endereco.Estado = Helper.ExtraiFieldIdEmitente(doc, "tr[5]/td[1]/span");
            endereco.CEP = Helper.ExtraiFieldIdEmitente(doc, "tr[3]/td[2]/span");
             
            return endereco;
        }
    }
}
