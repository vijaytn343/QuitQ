using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuitQ.Models;

namespace QuitQ.Documents
{
    public class InvoiceDocument : IDocument
    {
        private readonly Order _order;

        public InvoiceDocument(Order order)
        {
            _order = order;
        }

        public DocumentMetadata GetMetadata()
            => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Text("QuitQ")
                            .FontSize(28)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        row.RelativeItem()
                            .AlignRight()
                            .Text("INVOICE")
                            .FontSize(24)
                            .Bold();
                    });

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().LineHorizontal(1);

                        column.Item().Text($"Invoice No: INV-{_order.OrderId}");
                        column.Item().Text($"Order ID: {_order.OrderId}");
                        column.Item().Text($"Date: {_order.OrderDate:dd/MM/yyyy}");
                        column.Item().Text($"Status: {_order.OrderStatus}");

                        column.Item().PaddingTop(15);

                        column.Item()
                            .Text("Customer Details")
                            .Bold()
                            .FontSize(16);

                        column.Item()
                            .Text(_order.User?.Name ?? "");

                        column.Item()
                            .Text($"{_order.Address?.FullAddress}");

                        column.Item()
                            .Text($"{_order.Address?.City}, {_order.Address?.State} - {_order.Address?.Pincode}");

                        column.Item().PaddingTop(20);

                        column.Item()
                            .Text("Products")
                            .Bold()
                            .FontSize(16);

                        foreach (var item in _order.OrderItems)
                        {
                            column.Item()
                                .Border(1)
                                .Padding(10)
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Text(item.Product?.ProductName ?? "");

                                    row.ConstantItem(50)
                                        .AlignRight()
                                        .Text($"Qty: {item.Quantity}");

                                    row.ConstantItem(100)
                                        .AlignRight()
                                        .Text($"₹{item.PriceAtPurchase * item.Quantity}");
                                });
                        }

                        column.Item()
                            .PaddingTop(20)
                            .AlignRight()
                            .Text($"Grand Total: ₹{_order.TotalAmount}")
                            .Bold()
                            .FontSize(18);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text("Thank you for shopping with QuitQ");
            });
        }
    }
}