namespace ShipeX2.Application.DTOs
{
    public class ImporterModel
    {
        public class ImporterExtended
        {
            public long ImporterId { get; set; }
            public string? BrokerName { get; set; }
            public string? ImporterName { get; set; }
            public string? ImporterCompany { get; set; }
            public string? Client { get; set; }
            public string? ImporterAddress1 { get; set; }
            public string? ImporterAddress2 { get; set; }
            public string? Country { get; set; }
            public string? ImporterState { get; set; }
            public string? ImporterCity { get; set; }
            public string? ImporterPostCode { get; set; }
            public string? ImporterPhone { get; set; }
            public bool Status { get; set; }
        }
    }
}
