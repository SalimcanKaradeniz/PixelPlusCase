using Data.DbEntity;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface ISubscriberService
    {
        Subscribers GetSubscribe(int subscriberNumber);
        ReturnModel<object> CreateSubscribe(SubscriberRequestModel model);
        ReturnModel<object> SubscriberCancelled(int id);
    }
}
