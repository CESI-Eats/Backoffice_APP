using Backoffice_APP.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Backoffice_APP.Services
{
    public class GetPaymentsService : BaseService
    {
        public async Task<PaymentResponse> GetPayments()
        {
            try
            {
                PaymentResponse paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(await Get(ConfigurationManager.AppSettings["payments_url"]));
                return paymentResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
