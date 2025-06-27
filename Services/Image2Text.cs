using Newtonsoft.Json;
using SIMC.Services.Models;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text;

namespace SIMC.Services
{
    internal class Image2Text
    {
        public static string API_URL = "";
        public static string API_URL_LOCAL = "";
        public static string API_CHECKER = "";

        public static async Task<Image2TextResponse?> GetImageContent(string imagePath, HttpClient? client = null)
        {
            try
            {
                if (client == null)
                {
                    client = new HttpClient();
                }
                using Stream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                using MemoryStream memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var byteArray = memoryStream.ToArray();

                using MultipartFormDataContent formData = new MultipartFormDataContent();
                ByteArrayContent imageContent = new ByteArrayContent(byteArray);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                formData.Add(imageContent, "file", imagePath);
                var response = await client.PostAsync(API_URL, formData);
                var responseContent = await response.Content.ReadAsStringAsync();
                responseContent = Encoding.UTF8.GetString(Encoding.Default.GetBytes(responseContent));
                Image2TextResponse? image2TextResponse = JsonConvert.DeserializeObject<Image2TextResponse>(responseContent);
                return image2TextResponse;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
                return null;
            }
        }
    }
}
