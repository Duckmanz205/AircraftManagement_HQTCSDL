using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyMayBay.Models
{
    public class MyTicketsViewModel
    {
        public List<OrderTicketViewModel> UnpaidOrders { get; set; } = new List<OrderTicketViewModel>();
        public List<OrderTicketViewModel> PaidOrders { get; set; } = new List<OrderTicketViewModel>();
    }
}
