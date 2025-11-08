using Microsoft.AspNetCore.Mvc;

namespace MiniInventoryManagementAPI.Controllers.Order
{
    [Route("order")]
    public class OrderController : Controller
    {
        [HttpGet]
        [Route("view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
