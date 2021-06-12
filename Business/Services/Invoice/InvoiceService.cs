using Core.UnitOfWork;
using Data.Context;
using Data.DbEntity;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork<PixelPlusContext> _unitOfWork;

        public InvoiceService(IUnitOfWork<PixelPlusContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<InvoiceResponseModel> GetInvoices(int userId)
        {
            if (userId == 0 || userId < 0)
                return new List<InvoiceResponseModel>();

            var subscriber = _unitOfWork.GetRepository<Subscribers>().GetFirstOrDefault(predicate: x => x.UserId == userId);

            if (subscriber == null)
                return new List<InvoiceResponseModel>();

            var invoices = (from i in _unitOfWork.GetRepository<Data.DbEntity.Invoice>().GetAll(x => x.UserId == userId).ToList()
                            select new InvoiceResponseModel()
                            {
                                Id = i.Id,
                                UserId = i.UserId,
                                InvoiceNumber = i.InvoiceNumber,
                                TotalPrice = i.TotalPrice,
                                TaxRate = i.TaxRate,
                                ExpriyDate = i.ExpriyDate,
                                IsPayment = i.IsPayment,
                                CreatedAt = i.CreatedAt,
                                Subscribers = new SubscribersResponseModel()
                                {
                                    Id = subscriber.Id,
                                    UserId = subscriber.UserId,
                                    NameSurname = subscriber.NameSurname,
                                    IdentityNumber = subscriber.IdentityNumber,
                                    Phone = subscriber.Phone,
                                    Address = subscriber.Address,
                                    SubscriberNumber = subscriber.SubscriberNumber,
                                    IsCancelled = subscriber.IsCancelled,
                                    IsRefund = subscriber.IsRefund,
                                    DepositFee = subscriber.DepositFee,
                                    CreatedAt = subscriber.CreatedAt,
                                }
                            }).ToList();

            return invoices;
        }

        public ReturnModel<object> InvoicePayment(InvoicePaymentRequestModel model)
        {
            ReturnModel<object> returnModel = new ReturnModel<object>();

            try
            {
                var invoice = _unitOfWork.GetRepository<Data.DbEntity.Invoice>().GetFirstOrDefault(predicate: x => x.InvoiceNumber == model.InvoiceNumber && !x.IsPayment);

                if (invoice != null)
                {
                    #region total price check
                    if (model.TotalPrice == 0M)
                    {
                        returnModel.IsSuccess = false;
                        returnModel.Message = "Ödemek istediğiniz miktar belirtilmemiştir.Lütfen tekrar deneyiniz";
                        return returnModel;
                    }

                    if (model.TotalPrice > invoice.TotalPrice)
                    {
                        returnModel.IsSuccess = false;
                        returnModel.Message = "Ödemek istediğiniz miktar fatura tutarından fazla olmamalıdır.Lütfen tekrar deneyiniz";
                        return returnModel;
                    }

                    if (model.TotalPrice < invoice.TotalPrice)
                    {
                        returnModel.IsSuccess = false;
                        returnModel.Message = "Ödemek istediğiniz miktar fatura tutarının altında olmamalıdır.Lütfen tekrar deneyiniz";
                        return returnModel;
                    }
                    #endregion

                    invoice.IsPayment = true;

                    _unitOfWork.GetRepository<Data.DbEntity.Invoice>().Update(invoice);
                    _unitOfWork.SaveChanges();

                    returnModel.IsSuccess = true;
                    returnModel.Message = "Faturanız ödeme işlemi başarıyla gerçekleştirilmiştir.";
                }
                else
                {
                    returnModel.Message = "Fatura Bulunamadı";
                }
            }
            catch (Exception ex)
            {
                returnModel.Message = "Hata oluştu";
            }

            return returnModel;
        }
    }
}
