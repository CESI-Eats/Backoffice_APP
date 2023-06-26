using System;
using System.Collections.Generic;

namespace Backoffice_APP.Models.Responses
{
    public class PaymentResponse
    {
        public List<Payment> Message { get; set; }

        public PaymentResponse()
        {
            Message = new List<Payment>();
        }
    }

    public class Payment
    {
        public string _Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
