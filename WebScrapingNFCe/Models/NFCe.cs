using System;
using System.Collections.Generic;

namespace WebScrapingNFCe.Models
{
    public class NFCe
    {                
        public string ChaveAcesso { get; set; }        
        public int Modelo { get; set; }
        public int Serie { get; set; }
        public int Numero { get; set; }
        public string VersaoXML { get; set; }
        public DateTime DataEmissao { get; set; }
        public decimal ValorTotalNota { get; set; }
        public decimal ValorDescontoTotalNota { get; set; }
        public string LinkQrCode { get; set; }
        public Empresa Empresa { get; set; }
        public List<ProdutoServico> ListaProdutoServico { get; set; }        
        public InformacaoAdicional InformacaoAdicional { get; set; }
    }
}
