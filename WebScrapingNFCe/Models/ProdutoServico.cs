using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScrapingNFCe.Models
{
    public class ProdutoServico
    {
        public string DescricaoProduto { get; set; }
        public decimal Qtd { get; set; }
        public string UnidadeComercial { get; set; }
        public decimal ValorTotal { get; set; }
        public string CodigoProduto { get; set; }
        public string CodigoNCM { get; set; }
        public string CodigoCEST { get; set; }
        public decimal ValorDesconto { get; set; }
        public string CodigoEANComercial { get; set; }        
        public decimal ValorUnitarioComercial { get; set; }        
        public decimal ValorAproxTributos { get; set; }

    }
}
