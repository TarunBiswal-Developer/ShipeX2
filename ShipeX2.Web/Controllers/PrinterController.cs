using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.DTOs;
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
        public async Task<IActionResult> PrinterList()
        {
            var model = await _printerServices.GetPrintersAsync();
            return View(model);
        }
        [HttpGet]
        public IActionResult CreatePrinter() => View();
        [HttpPost]
        public async Task<IActionResult> CreatePrinter([FromBody] PrinterModel model)
        {
            var result = await _printerServices.CreatePrinterAsync(model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> EditPrinter(long id)
        {
            var result = await _printerServices.GetPrinterByIdAsync(id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> EditPrinter([FromBody] PrinterModel model)
        {
            var result = await _printerServices.UpdatePrinterAsync(model);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> TogglePrinterStatus(long id)
        {
            var result = await _printerServices.TogglePrinterStatusAsync(id);
            return Json(result);
        }
        #endregion
    }
}
