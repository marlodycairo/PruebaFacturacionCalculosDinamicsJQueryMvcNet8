using Microsoft.AspNetCore.Mvc;
using PruebaFacturacionCalculosDinamicsJQueryMvcNet8.IServices;

namespace PruebaFacturacionCalculosDinamicsJQueryMvcNet8.Controllers
{
    public class ProductoController(IProductoService productService) : Controller
    {
        private readonly IProductoService _productService = productService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProductById(int id)
        {
            var product = _productService.GetProductByIdAsync(id);

            return Json(product);
        }
    }
}
