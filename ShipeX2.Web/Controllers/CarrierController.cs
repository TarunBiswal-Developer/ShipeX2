using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin")]
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

        [HttpGet]
        public async Task<IActionResult> EditCarrierApi(long id)
        {
            var model = await _carrerServices.GetCarrierByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCarrierApi([FromBody] ModelShipCarrier model)
        {
            var result = await _carrerServices.UpdateCarrierAsync(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ModeChange ( long id )
        {
            var result = await _carrerServices.ToggleModeAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus ( long id )
        {
            var result = await _carrerServices.ToggleCarrierStatusAsync(id);
            return Json(result);
        }

        #endregion
    }
}
