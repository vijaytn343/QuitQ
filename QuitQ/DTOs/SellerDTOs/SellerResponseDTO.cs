namespace QuitQ.DTOs.SellerDTOs
{
    public class SellerResponseDTO
    {
        public int SellerId { get; set; }

        public int UserId { get; set; }

        public string StoreName { get; set; } = string.Empty;

        public string? GSTNumber { get; set; }

        public string? BusinessEmail { get; set; }

        public string? AccountHolderName { get; set; }

        public string? AccountNumber { get; set; }

        public string? IFSCCode { get; set; }

        public string? BankName { get; set; }

        public string? UserName { get; set; }
    }
}
