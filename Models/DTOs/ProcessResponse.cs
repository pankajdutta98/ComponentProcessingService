using System;

namespace ComponentProcessingService.Models.DTOs
{
    public class ProcessResponse
    {
        public string RequestId { get; set; }
        public double ProcessingCharge { get; set; }
        public double PackagingCharge { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
