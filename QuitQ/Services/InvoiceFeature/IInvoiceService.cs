namespace QuitQ.Services.InvoiceFeature
{
    public interface IInvoiceService
    {
        byte[] GenerateInvoice(int orderId, int userId);
    }
}
