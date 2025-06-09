namespace ShipeX2.Application.DTOs
{
    public class ModelLogin
    {
        public string Id { get; set; }
        public string? Userid { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string PrinterIP { get; set; }
        public string? Captcha { get; set; }
        public string? SubCaptcha { get; set; }
        public string? Captchavaidation { get; set; }
    }
}
