using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fShop.Models.ViewModels
{
    public class ApproveRequest
    {
        public int ProductId { get; set; }
        public bool IsApproved { get; set; }
    }
}