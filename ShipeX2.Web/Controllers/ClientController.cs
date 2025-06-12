using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;

namespace ShipeX2.Web.Controllers
{
    [Authorize(Roles = "Super Admin, Admin")]
    public class ClientController : Controller
    {
        private readonly IClientServices _clientServices;

        public ClientController(IClientServices clientServices)
        {
            _clientServices = clientServices;
        }

        [HttpGet]
        public async Task<IActionResult> ManageClients()
        {
            var clients = await _clientServices.GetAllClientsAsync();
            return View(clients);
        }
    }
}
