using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace MFA.Entities.LogicModels
{
    public class ImageExtensions
    {
        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            using (var binaryReader = new BinaryReader(fileStream))
            {
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        /// <summary>
        /// Returns all the images present in a directory as a stream list.
        /// </summary>
        /// <param name="imageDirectory"></param>
        /// <returns></returns>
        public static List<Stream> GetAllImagesInADirectory(string imageDirectory)
        {
            var imageList = new List<Stream>();
            foreach (string imagePath in Directory.GetFiles(imageDirectory))
            {
                using (Stream imageStream = File.OpenRead(imagePath))
                {
                    imageList.Add(imageStream);
                }
            }
            return imageList;
        }

        /// <summary>
        /// Returns all the images uploaded as a stream list.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<Stream> GetAllUploadedImages(List<IFormFile> files)
        {
            var imageList = new List<Stream>();
            foreach (var file in files)
            {
                imageList.Add(file.OpenReadStream());
            }
            return imageList;
        }
    }
}
