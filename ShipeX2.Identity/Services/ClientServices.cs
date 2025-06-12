using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShipeX2.Application.Interfaces;
using ShipeX2.Identity.Context;
using static ShipeX2.Application.DTOs.ClientModel;

namespace ShipeX2.Identity.Services
{
    public class ClientServices : IClientServices
    {
		private readonly ApplicationDbContext _context;
		private readonly ILogger<ClientServices> _logger;

		public ClientServices(ApplicationDbContext context, ILogger<ClientServices> logger)
		{
			_context = context;
			_logger = logger;
        }

        public async Task<List<ClientViewModel>> GetAllClientsAsync ()
        {
			try
			{
                var clients = await _context.Clients
                                .Select(c => new ClientViewModel
                                {
                                    ClientId = (int)c.ClientId,
                                    Name = c.Name,
                                    PrinterIP = c.PrinterIP,
                                    ClientCode = c.ClientCode,
                                    CompanyName = c.CompanyName,
                                    Phone1 = c.Phone1,
                                    Phone2 = c.Phone2,
                                    Email = c.Email,
                                    Addresline1 = c.Addresline1,
                                    Addresline2 = c.Addresline2,
                                    City = c.City,
                                    State = c.State,
                                    Country = c.Country,
                                    PostalCode = c.PostalCode,
                                    ShipName = c.ShipName,
                                    ShipCompany = c.ShipCompany,
                                    ShipAddress1 = c.ShipAddress1,
                                    ShipAddress2 = c.ShipAddress2,
                                    ShipCity = c.ShipCity,
                                    ShipState = c.ShipState,
                                    ShipCounty = c.ShipCounty,
                                    ShipPostcode = c.ShipPostcode,
                                    ShipPhone1 = c.ShipPhone1,
                                    ShipPhone2 = c.ShipPhone2,
                                    ShipEmailId = c.ShipEmailId,
                                    Status = c.Status,
                                    CreatedBy = c.CreatedBy,
                                    CreatedDate = c.CreatedDate,
                                    ModifiedBy = c.ModifiedBy,
                                    ModifiedDate = c.ModifiedDate,
                                    STDCode = c.STDCode
                                })
                                .OrderByDescending(c => c.Name)
                                .ToListAsync();
                return clients;

            }
            catch (Exception ex)
			{
                _logger.LogError(ex, "Error retrieving clients");
                throw;
			}
        }
    }
}
