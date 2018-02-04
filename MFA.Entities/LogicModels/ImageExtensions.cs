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
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            using (var binaryReader = new BinaryReader(fileStream))
            {
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
