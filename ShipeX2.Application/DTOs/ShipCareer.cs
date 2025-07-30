namespace ShipeX2.Application.DTOs
{
    public class ShipCareer
    {
        public class ClientCarrier
        {
            public long? CarrierId { get; set; }
            public string CarrierName { get; set; }
            public string ApiKey1 { get; set; }
            public string ApiKey2 { get; set; }
            public string AccountNo { get; set; }
            public bool? Mode { get; set; }
            public string STDCode { get; set; }
            public bool? DefaultAccount { get; set; }
        }

        public class CarrierService
        {
            public long ServiceId { get; set; }
            public string ServiceName { get; set; }
            public string ServiceCode { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsDefault { get; set; }
            public long CarrierId { get; set; }
        }

        public class CarrierPacking
        {
            public long PackingId { get; set; }
            public string PackingName { get; set; }
            public decimal MaxWeight { get; set; }
            public string IsWeightInRange { get; set; } // 'Selected' or ''
        }

    }
}
