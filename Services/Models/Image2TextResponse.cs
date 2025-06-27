namespace SIMC.Services.Models
{
    public class Image2TextResponse
    {
        public string? file { set; get; }
        public string? content { get; set; }
        public string? message { get; set; }
        public double time { get; set; }
    }
}
