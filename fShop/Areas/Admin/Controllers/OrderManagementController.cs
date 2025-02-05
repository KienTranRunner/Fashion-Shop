using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace fShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagementController : Controller
    {
        private readonly fShopContext _context;

        public OrderManagementController(fShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Admin/OrderManagement/GetAll")]
        public IActionResult GetAll()
        {
            var orderList = _context.Orders.ToList();

            return Json(orderList);
        }
        [HttpGet]
        [Route("Admin/OrderManagement/Detail/{orderId}")]
        public IActionResult GetOrderDetail(int orderId)
        {
            var orderDetail = _context.Orders
                .Where(o => o.OrderId == orderId)
                .Select(o => new
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,

                    // Thông tin khách hàng
                    Customer = new
                    {
                        UserId = o.User.UserId,
                        FullName = o.User.FullName,
                        Email = o.User.Email,
                        PhoneNumber = o.User.PhoneNumber,
                        Address = o.User.Address
                    },

                    // Danh sách sản phẩm trong đơn hàng
                    Products = o.OrderDetails.Select(od => new
                    {
                        ProductId = od.Product.ProductId,
                        ProductName = od.Product.NameProduct,
                        Price = od.Price,
                        Quantity = od.Quantity,
                        Total = od.Price * od.Quantity
                    }).ToList()
                })
                .FirstOrDefault();

            if (orderDetail == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });
            }

            return Json(orderDetail);
        }
        [HttpPost]
        [Route("Admin/OrderManagement/UpdateStatus")]
        public IActionResult UpdateStatus([FromBody] OrderStatusUpdateModel model)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == model.OrderId);
            if (order == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng!" });
            }

            order.Status = model.Status;
            _context.SaveChanges();

            return Json(new { message = "Cập nhật trạng thái thành công!" });
        }

        public class OrderStatusUpdateModel
        {
            public int OrderId { get; set; }
            public string Status { get; set; }
        }


    }
}