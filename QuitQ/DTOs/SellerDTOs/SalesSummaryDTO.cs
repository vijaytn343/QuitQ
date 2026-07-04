namespace QuitQ.DTOs.SellerDTOs
{
    public class SalesSummaryDTO
    {

        public decimal TotalRevenue { get; set; }

        public decimal WeeklyRevenue { get; set; }

        public decimal MonthlyRevenue { get; set; }

        public int ProductsSold { get; set; }

        public string TopProduct { get; set; } = "";
        public string MonthlyTopProduct { get; set; } = "N/A";
    }
}
