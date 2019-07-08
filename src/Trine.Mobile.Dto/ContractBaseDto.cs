using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Dto
{
    public class ContractBaseDto
    {
        public string Content { get; set; }
        public bool IsSignedByCommercial { get; set; }
        public bool IsSignedByConsultant { get; set; }
        public bool IsSignedByCustomer { get; set; }
        public ContratStatus Status { get; set; }

        public enum ContratStatus
        {
            CREATED, // Généré et En attente de signature des trois parties
            CONFIRMED, // Signé et valide
            REVOKED, // Révoqué
        }
    }
}
