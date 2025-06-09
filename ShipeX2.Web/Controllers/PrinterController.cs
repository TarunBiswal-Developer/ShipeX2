using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
    public class PrinterController : Controller
    {
        private readonly IPrinterServices _printerServices;
        public PrinterController ( IPrinterServices printerServices )
        {
            _printerServices = printerServices;
        }
        #region Printer crud operations
        
        [HttpGet]
        public async Task<IActionResult> PrinterList ()
        {
            var model = await _printerServices.GetPrintersAsync();
            return View(model);
        }




        #endregion
    }
}
