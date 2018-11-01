using System;

namespace WebScrapingNFCe.Models
{
    public class Empresa
    {   
        public Int64 CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }

    }
}

