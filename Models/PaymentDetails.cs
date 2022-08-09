using System;
using System.ComponentModel.DataAnnotations;

namespace ComponentProcessingService.Models
{
    public class PaymentDetails
    {
        [Key]
        [Required]
        public string RequestId { get; set; }
        [Required]        
        public string CardNbr { get; set; }        
        [Required]
        public string NameOnCard { get; set; }
        [Required]
        public string ValidThru { get; set; }
        [Required]
        public DateTime TxnDate { get; set; }

    }
}
