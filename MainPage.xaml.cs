using SIMC.Models;
using SIMC.Services.Models;
using System.Security.Cryptography;

namespace SIMC
{

    public partial class MainPage : ContentPage
    {

        private static string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        public MainPage()
        {
            InitializeComponent();
        }

        private void UpdateGridImage(List<ImageItem> imageItems)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ImageGrid.ItemsSource = imageItems;
            });
        }

        private void UpdateStatusBar(string text = "")
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                lblStatus.Text = text;
            });
        }

        List<ImageItem> imageItems = new List<ImageItem>();
        private async Task LoadImage(string? keyword = null)
        {
            UpdateStatusBar($"Loading image...");
            var dbService = new DatabaseService();
            imageItems = new List<ImageItem>();
            if (null != keyword)
            {
                List<ImageData> images = await dbService.SearchByContentAsync(keyword);
                for (int i = 0; i < images.Count; i++)
                {
                    ImageData file = images[i];
                    UpdateStatusBar($"Loading: [{i + 1}/{images.Count}]{file.File}");
                    var imageItem = new ImageItem
                    {
                        ImageSource = ImageSource.FromFile(file.File),
                        Caption = file.Content
                    };
                    imageItems.Add(imageItem);
                }
                UpdateStatusBar("Setting item view source...");
                UpdateGridImage(imageItems);
                UpdateStatusBar();
            }
            else
            {
                UpdateStatusBar("Loading image from directory...");
                var imagePaths = Directory
                .GetFiles(FileSystem.Current.AppDataDirectory)
                .Where(file => ImageExtensions.Contains(Path.GetExtension(file).ToLower()))
                .ToList();
                for (int i = 0; i < imagePaths.Count; i++)
                {
                    string file = imagePaths[i];
                    UpdateStatusBar($"Loading: [{i + 1}/{imagePaths.Count}]{file}");
                    string caption = "...";
                    ImageData imageData = await dbService.GetImageByFileNameAsync(file);
                    if (imageData != null && imageData.Content != null)
                    {
                        caption = imageData.Content;
                    }
                    var imageItem = new ImageItem
                    {
                        ImageSource = ImageSource.FromFile(file),
                        Caption = caption
                    };
                    imageItems.Add(imageItem);
                }
                UpdateStatusBar("Setting item view source...");
                UpdateGridImage(imageItems);
                UpdateStatusBar();
                Image2Text(imagePaths);
            }
        }

        private async void Image2Text(List<string> imagePaths)
        {
            var dbService = new DatabaseService();
            using HttpClient client = new HttpClient();
            foreach (var imagePath in imagePaths)
            {
                if (await dbService.FileExistsAsync(imagePath))
                {
                    continue;
                }
                UpdateStatusBar($"Getting image content: {imagePath}");
                Image2TextResponse? image2TextResponse = await SIMC.Services.Image2Text.GetImageContent(imagePath, client);
                if (null != image2TextResponse)
                {
                    image2TextResponse.file = imagePath;
                    // Create a new image record
                    var newImage = new ImageData
                    {
                        File = image2TextResponse.file,
                        Content = image2TextResponse.content
                    };

                    // Insert it into the database
                    await dbService.SaveImageAsync(newImage);
                }
                UpdateStatusBar();
                await LoadImage(btnKeyword.Text);
            }
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            await LoadImage();
        }

        private async void BtnSelectFile_Clicked(object sender, EventArgs e)
        {
            var results = await FilePicker.Default.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Select images",
                FileTypes = FilePickerFileType.Images
            });
            if (results != null)
            {
                foreach (var file in results)
                {
                    await SaveFileWithHashedName(file);
                }
            }
            await LoadImage();
        }

        public async Task<string> SaveFileWithHashedName(FileResult file)
        {
            using var stream = await file.OpenReadAsync();

            // Compute SHA256 hash
            using var sha256 = SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(stream);
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            // Create new file path
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{hashString}{extension}";
            var destinationPath = Path.Combine(FileSystem.Current.AppDataDirectory, newFileName);
            if (!File.Exists(destinationPath))
            {
                // Reset stream and copy to destination
                UpdateStatusBar($"Saving file: {file.FullPath} -> {destinationPath}");
                stream.Position = 0;
                using var destinationStream = File.Create(destinationPath);
                await stream.CopyToAsync(destinationStream);
                UpdateStatusBar();
            }

            return destinationPath;
        }

        private static bool isSearching = false;
        private async void TxtKeyword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isSearching) return;
            UpdateStatusBar($"Searching");
            isSearching = true;
            Editor editor = (Editor)sender;
            await LoadImage(editor.Text);
            isSearching = false;
            UpdateStatusBar();
        }
    }
}
