namespace QuitQ.DTOs.SellerDTOs
{
    public class SalesReportDTO
    {
        public string ProductName { get; set; } = string.Empty;

        public int QuantitySold { get; set; }

        public decimal Revenue { get; set; }
    }
}