namespace ShipeX2.Application.DTOs
{
    public class ClientDatabase
    {
        public int Id { get; set; }
        public required string ClientId { get; set; }
        public required string DatabaseName { get; set; }
    }
}
