namespace ComponentProcessingService.Models.DTOs
{
    public class ProcessRequest
    {
        public string? CustName { get; set; }
        public string? ContactNumber { get; set; }
        //public DefectiveComponentDetail defectiveComponentDetail { get; set; }
        public char? ComponentType { get; set; }
        public string? CompName { get; set; }
        public float? Qty { get; set; }

    }

    public class DefectiveComponentDetail
    {
        public char Type { get; set; }
        public string Name { get; set; }
        public float Qty { get; set; }
    }
}
