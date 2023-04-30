namespace Invoices_Manager_API.Models
{
    public class InvoiceWrapperModel
    {
        public InvoiceModel NewInvoice { get; set; }
        public string InvoiceFileBase64 { get; set; }
    }
}
