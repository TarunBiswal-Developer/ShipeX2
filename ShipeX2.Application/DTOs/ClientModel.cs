namespace ShipeX2.Application.DTOs
{
    public class ClientModel
    {
        public class ClientViewModel
        {
            public int ClientId { get; set; }
            public string? Name { get; set; }
            public string? PrinterIP { get; set; }
            public string? ClientCode { get; set; }
            public string? CompanyName { get; set; }
            public string? Phone1 { get; set; }
            public string? Phone2 { get; set; }
            public string? Email { get; set; }
            public string? Addresline1 { get; set; }
            public string? Addresline2 { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? Country { get; set; }
            public string? PostalCode { get; set; }
            public string? ShipName { get; set; }
            public string? ShipCompany { get; set; }
            public string? ShipAddress1 { get; set; }
            public string? ShipAddress2 { get; set; }
            public string? ShipCity { get; set; }
            public string? ShipState { get; set; }
            public string? ShipCounty { get; set; }
            public string? ShipPostcode { get; set; }
            public string? ShipPhone1 { get; set; }
            public string? ShipPhone2 { get; set; }
            public string? ShipEmailId { get; set; }
            public bool Status { get; set; }
            public long? CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public long? ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }

            public string? STDCode { get; set; }
        }

    }
}
