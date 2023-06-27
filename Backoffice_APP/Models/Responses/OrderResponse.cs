using System;
using System.Collections.Generic;

namespace Backoffice_APP.Models.Responses
{
    public class OrderResponse
    {
        public List<Order> Message { get; set; }

        public OrderResponse()
        {
            Message = new List<Order>();
        }
    }

    public class Order
    {
        public string _Id { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public double WithCommissionAmount { get; set; }
        public double DeliveryAmount { get; set; }
    }
}
