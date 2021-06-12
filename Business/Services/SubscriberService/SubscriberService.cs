using Core.UnitOfWork;
using Data.Context;
using Data.DbEntity;
using Data.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace Business.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IUnitOfWork<PixelPlusContext> _unitOfWork;
        private readonly IInvoiceService _invoiceService;
        private readonly IUserService _userService;

        public SubscriberService(IUnitOfWork<PixelPlusContext> unitOfWork,IInvoiceService invoiceService, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _invoiceService = invoiceService;
            _userService = userService;
        }

        public ReturnModel<object> CreateSubscribe(SubscriberRequestModel model)
        {
            ReturnModel<object> returnModel = new ReturnModel<object>();
            Subscribers subscriber = new Subscribers();

            try
            {
                Random rnd = new Random();
                var user = _userService.CreateUser(model.Phone, rnd.Next(100000, 999999).ToString());

                if (user.IsSuccess)
                {
                    subscriber.UserId = user.Data.Id;
                    subscriber.SubscriberNumber = rnd.Next(100000, 999999);
                    subscriber.NameSurname = model.NameSurname;
                    subscriber.IdentityNumber = model.IdentityNumber;
                    subscriber.Phone = model.Phone;
                    subscriber.Address = model.Address;
                    subscriber.DepositFee = 500M;

                    _unitOfWork.GetRepository<Subscribers>().Insert(subscriber);
                    _unitOfWork.SaveChanges();

                    returnModel.Data = user.Data;
                    returnModel.IsSuccess = true;
                    returnModel.Message = "Abone oluşturuldu";
                }
                else
                {
                    returnModel.IsSuccess = false;
                    returnModel.Message = "Abone oluşturulamadı";
                }
            }
            catch (Exception ex)
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "Abone oluşturulamadı";
            }

            return returnModel;
        }

        public Subscribers GetSubscribe(int subscriberNumber) 
        {
            return _unitOfWork.GetRepository<Subscribers>().GetFirstOrDefault(predicate: x => x.SubscriberNumber == subscriberNumber);
        }

        public ReturnModel<object> SubscriberCancelled(int id)
        {
            ReturnModel<object> returnModel = new ReturnModel<object>();

            try
            {
                var subscribe = _unitOfWork.GetRepository<Subscribers>().GetFirstOrDefault(predicate: x => x.Id == id && !x.IsCancelled);

                if (subscribe != null)
                {
                    #region Is Not Payment Invoice Check
                    
                    var isNotPaymentInvoice = _invoiceService.GetInvoices(subscribe.UserId).Where(x => !x.IsPayment).ToList();

                    if (isNotPaymentInvoice.Any() && isNotPaymentInvoice.Count > 0)
                    {
                        returnModel.IsSuccess = false;
                        returnModel.Message = "Aboneye ait ödenmemiş fatura bulunmaktadır. Aboneliğin iptal edilebilmesi için faturalar ödendikten sonra lütfen tekrar deneyiniz";
                        return returnModel;
                    }

                    #endregion

                    subscribe.IsCancelled = true;
                    subscribe.IsRefund = true;

                    _unitOfWork.GetRepository<Subscribers>().Update(subscribe);
                    _unitOfWork.SaveChanges();

                    returnModel.IsSuccess = true;
                    returnModel.Message = "Abone iptal işlemi başarıyla gerçekleştirilmiştir.";
                }
                else
                {
                    returnModel.IsSuccess = false;
                    returnModel.Message = "Abone Bulunamadı";
                    return returnModel;
                }
            }
            catch (Exception ex)
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "Abonelik iptal edilirken hata oluştu";
            }

            return returnModel;
        }
    }
}