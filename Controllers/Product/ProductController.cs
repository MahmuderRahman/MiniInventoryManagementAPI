using Microsoft.AspNetCore.Mvc;

namespace MiniInventoryManagementAPI.Controllers.Product
{
    [Route("product")]
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("view")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
