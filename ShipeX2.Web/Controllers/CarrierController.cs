using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Application.Wrappers;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles ="Super Admin,Admin")]
    public class CarrierController : Controller
    {
        private readonly ILogger<CarrierController> _logger;
        private readonly ICarrierListServices _carrerServices;
        public CarrierController ( ILogger<CarrierController> logger, ICarrierListServices carrier )
        {
            _logger = logger;
            _carrerServices = carrier;
        }

        #region Carrer API

        [HttpGet]
        public async Task<IActionResult> CarrierApiList ()
        {
            var model = await _carrerServices.GetCarriersAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateCarrierApi () => View();

        [HttpPost]
        public async Task<IActionResult> CreateCarrierApi ( [FromBody] ModelShipCarrier model )
        {
            var result = await _carrerServices.CreateCarrierAsync(model);
            return Json(result);
        }





        #endregion
    }
}
