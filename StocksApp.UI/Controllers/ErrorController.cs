using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace StocksApp.UI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("[action]")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature details = HttpContext.Features.GetRequiredFeature<IExceptionHandlerPathFeature>();
            ViewBag.ErrorMessage = "Error occurred";
            if (details != null && details.Error != null)
            {
                ViewBag.ErrorMessage = details.Error.Message;
            }
            return View();
        }
        [Route("[action]/{statusCode}")]
        public IActionResult StatusCode(int statusCode)
        {
            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = "The page you requested could not be found.";
            }
            else if (statusCode == 500)
            {
                ViewBag.ErrorMessage = "A server error occurred.";
            }
            else
            {
                ViewBag.ErrorMessage = $"An error occurred. Status Code: {statusCode}";
            }
            ViewBag.StatusCode = statusCode;
            return View();
        }
    }
}
