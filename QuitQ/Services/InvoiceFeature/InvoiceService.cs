using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuitQ.Data;
using System.Reflection.Metadata;

namespace QuitQ.Services.InvoiceFeature
{
    public class InvoiceService:IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public byte[] GenerateInvoice(
     int orderId,
     int userId)
        {
            var order = _context.Orders
    .Include(o => o.User)
    .Include(o => o.Address)
    .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
    .FirstOrDefault(o =>
        o.OrderId == orderId &&
        o.UserId == userId);

            if (order == null)
                throw new Exception(
                    "Order not found.");

            var payment = _context.Payments
                .FirstOrDefault(p =>
                    p.OrderId == orderId);

            if (payment == null)
                throw new Exception(
                    "Payment not found.");

            if (!payment.PaymentStatus.Equals(
        "paid",
        StringComparison.OrdinalIgnoreCase))
                throw new Exception(
                    "Invoice available only after successful payment.");

            return QuestPDF.Fluent.Document.Create(
                container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);

                        page.Header()
                            .Text("QuitQ Invoice")
                            .FontSize(24)
                            .Bold();

                        page.Content()
                            .Column(col =>
                            {
                                col.Item().Text(
    $"Invoice No: INV-{order.OrderId:D6}");
                                col.Item().Text(
$"Customer: {order.User!.Name}");

                                col.Item().Text(
                                    $"Email: {order.User.Email}");
                                col.Item().Text(" ");

                                col.Item().Text("Shipping Address")
                                    .Bold();

                                col.Item().Text(
                                    order.Address!.FullAddress);

                                col.Item().Text(
                                    $"{order.Address.City}, {order.Address.State}");

                                col.Item().Text(
                                    $"{order.Address.Country} - {order.Address.Pincode}");

                                col.Item().Text(" ");

                                col.Item().Text(
                                    $"Order Date: {order.OrderDate}");
                                col.Item().Text("Products").Bold();

                                foreach (var item in order.OrderItems!)
                                {
                                    col.Item().Text(
                                        $"{item.Product!.ProductName} | Qty: {item.Quantity} | ₹{item.PriceAtPurchase}");
                                }

                                col.Item().Text(" ");

                                col.Item().Text(
                                    $"Amount: ₹{order.TotalAmount}");

                                col.Item().Text(
                                    $"Payment Method: {payment.PaymentMethod}");

                                col.Item().Text(
                                    $"Payment Status: {payment.PaymentStatus}");

                                col.Item().Text(
                                    $"Transaction ID: {payment.TransactionId}");

                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(
                                "Thank you for shopping with QuitQ");
                    });
                })
                .GeneratePdf();
        }
    }
}
