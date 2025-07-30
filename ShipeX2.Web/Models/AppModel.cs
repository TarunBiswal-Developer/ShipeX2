using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipeX2.Identity.Context;

namespace ShipeX2.Web.Models
{
    public class AppModel
    {

        public static async Task<List<SelectListItem>> RetrieveRoles ( ApplicationDbContext _context )
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Choose Role", Value = "" }
            };
            var roles = await _context.UserRoles.Where(x=> !x.Role.Contains("Super Admin")).ToListAsync();
            items.AddRange(roles.Select(role => new SelectListItem
            {
                Text = role.Role,
                Value = role.RoleId.ToString()
            }));
            return items;
        }

        public static async Task<List<SelectListItem>> RetrieveLabelPrinters ( ApplicationDbContext _context )
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Select Label Printer", Value = "" }
            };
            var printers = await _context.Printers.Where(p => p.PntType.Contains("LABEL") && p.Status).ToListAsync();
            items.AddRange(printers.Select(p => new SelectListItem
            {
                Text = p.PntAliasName,
                Value = p.PntId.ToString()
            }));
            return items;
        }

        public static async Task<List<SelectListItem>> RetrieveInvoicePrinters ( ApplicationDbContext _context )
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Select Invoice Printer", Value = "" }
            };
            var printers = await _context.Printers.Where(p => p.PntType.Contains("PACK_INV") && p.Status).ToListAsync();
            items.AddRange(printers.Select(p => new SelectListItem
            {
                Text = p.PntAliasName,
                Value = p.PntId.ToString()
            }));
            return items;
        }

        public static async Task<List<SelectListItem>> ClientListAsync ( ApplicationDbContext _context )
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Select Client ", Value = "" }
            };
            var clientsList = await _context.Clients.Where(p => p.Status == true).OrderBy(o => o.Name).ToListAsync();
            items.AddRange(clientsList.Select(p => new SelectListItem
            {
                Text = p.Name + " (" + p.ClientCode + ")",
                Value =  p.ClientId.ToString()
            }));
            return items;
        }

        //ship carrier list
        public static async Task<List<SelectListItem>> ShipCarrierListAsync ( ApplicationDbContext _context )
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Select Carrier", Value = "" }
            };
            var shipCarriers = await _context.ShipCarriers./*Where(p => p.Status == true).*/OrderBy(o => o.CarrierName).ToListAsync();
            items.AddRange(shipCarriers.Select(p => new SelectListItem
            {
                Text = p.CarrierName,
                Value = p.CarrierId.ToString()
            }));
            return items;
        }
    }
}
