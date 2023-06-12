namespace Invoices_Manager_API.Models
{
    public class InvoiceWrapperModel
    {
        public InvoiceModel NewInvoice { get; set; } = default!;
        public string InvoiceFileBase64 { get; set; } = string.Empty;
    }
}
