using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebPulse_WebManager.Utility
{
    public static class ImageUtilities
    {
        #region Gravatar
        public static string GetGravatarImage(string email)
        {
            string url = string.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hashValue = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(email));
                url = "https://www.gravatar.com/avatar/" + BitConverter.ToString(hashValue).Replace("-", "").ToLower() + "?s=600&d=404";
            }

            if (UrlExists(url))
            {
                return url;
            }
            else
            {
                return string.Empty;
            }
        }

        private static bool UrlExists(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false; // 404 Not Found
                }
                else
                {
                    throw; // Handle other exceptions if needed
                }
            }
        }

        #endregion

        #region Encode/Decode

        public static byte[] EncodeImageToBytes(IFormFile formFile)
        {
            try
            {
                // Check if the form file is not null
                if (formFile == null)
                {
                    Console.WriteLine("Error: Form file is null.");
                    return null;
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Copy the content of the IFormFile to the MemoryStream
                    formFile.CopyTo(memoryStream);

                    // Check if the file is actually an image
                    try
                    {
                        using (Image image = Image.FromStream(memoryStream))
                        {
                            // Save the image to the MemoryStream in a specific format (e.g., JPEG)
                            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                            // Return the byte array
                            return memoryStream.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error converting form file to image: {ex.Message}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error encoding image: {ex.Message}");
                return null;
            }
        }

        #endregion
    }
}
