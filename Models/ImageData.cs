using SQLite;

namespace SIMC.Models
{
    public class ImageData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? File { get; set; }
        public string? Content { get; set; }
    }
}
