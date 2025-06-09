namespace ShipeX2.Application.DTOs
{
    public class PrinterModel
    {
        public long PntId { get; set; }
        public string PntAliasName { get; set; }
        public string PntType { get; set; }
        public string PntMode { get; set; }
        public string PntIP { get; set; }
        public string PntIdentifier { get; set; }
        public bool Status { get; set; }
        public long Createdby { get; set; }
        public DateTime? Createddate { get; set; }
        public long? Modifiedby { get; set; }
        public DateTime? Modifieddate { get; set; }
        public string CupsPrinterName { get; set; }
    }
}
