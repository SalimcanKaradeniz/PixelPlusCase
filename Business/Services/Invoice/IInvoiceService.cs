using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IInvoiceService
    {
        List<InvoiceResponseModel> GetInvoices(int userId);
        ReturnModel<object> InvoicePayment(InvoicePaymentRequestModel model);
    }
}
