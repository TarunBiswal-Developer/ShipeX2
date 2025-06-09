using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;

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
            List<ModelShipCarrier> model = new List<ModelShipCarrier>();    
            try
            {
                model = await _carrerServices.GetCarriersAsync();
            }
            catch (Exception ex)
            {
                 _logger.LogError("Error in Carrier Controller (CarrierApiList):" + ex.Message);
                throw;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCarrierApi()
        {
            try
            {

            }
            catch (Exception ex)
            {
               _logger.LogError("Error in Carrier Controller (CreateCarrierApi):" + ex.Message);
                throw;
            }
            return View();
        }


        #endregion
    }
}
