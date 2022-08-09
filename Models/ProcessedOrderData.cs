using System;
using System.ComponentModel.DataAnnotations;

namespace ComponentProcessingService.Models
{
    public class ProcessedOrderData
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        [MaxLength(15)]
        public string RequestId { get; set; }
        public string customerName { get; set; }
        [MaxLength(10)]
        public string CustContactNo { get; set; }
        public char ComponentType { get; set; }

        public string ComponentName { get; set; }
        public float Quantity { get; set; }
        public double ProcessingCharge { get; set; }
        public double PckgngAndDlvryCharge { get; set; }
        public DateTime DateOfDelivery { get; set; }        
        public DateTime RequestDate{ get; set; }

    }
}
