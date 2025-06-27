using SIMC.Models;
using SQLite;

namespace SIMC
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "simc.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ImageData>().Wait();
        }

        public Task<int> SaveImageAsync(ImageData image) =>
            _database.InsertAsync(image);

        public Task<List<ImageData>> GetImagesAsync() =>
            _database.Table<ImageData>().ToListAsync();

        public Task<ImageData> GetImageByFileNameAsync(string fileName)
        {
            return _database.Table<ImageData>()
                            .Where(i => i.File == fileName)
                            .FirstOrDefaultAsync();
        }

        public async Task<bool> FileExistsAsync(string fileName)
        {
            var count = await _database.Table<ImageData>()
                                       .Where(i => i.File == fileName)
                                       .CountAsync();
            return count > 0;
        }

        public Task<List<ImageData>> SearchByContentAsync(string keyword)
        {
            return _database.Table<ImageData>()
                            .Where(i => null != i.Content && i.Content.ToLower().Contains(keyword.ToLower()))
                            .ToListAsync();
        }
    }
}
