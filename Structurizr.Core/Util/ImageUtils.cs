using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Structurizr.Util
{
    public class ImageUtils
    {

        public static string GetContentType(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentException("A file must be specified.");
            }
            else if (Directory.Exists(file.FullName))
            {
                throw new ArgumentException(file.FullName + " is not a file.");
            }
            else if (!file.Exists)
            {
                throw new ArgumentException(file.FullName + " does not exist.");
            }

            string fileExtension = file.FullName.Substring(file.FullName.LastIndexOf(".") + 1).ToLower();
            if (fileExtension.Equals("jpg"))
            {
                fileExtension = "jpeg";
            }

            if (fileExtension == "png" || fileExtension == "jpeg" || fileExtension == "gif")
            {
                return "image/" + fileExtension;
            }
            else
            {
                throw new ArgumentException(file.FullName + " is not a supported image file.");
            }
        }

        public static string GetImageAsBase64(FileInfo file)
        {
            String contentType = GetContentType(file); // this does the file checks
            byte[] imageArray = File.ReadAllBytes(file.FullName);
            return Convert.ToBase64String(imageArray);
        }

        public static string GetImageAsDataUri(FileInfo file)
        {
            String contentType = GetContentType(file);
            String base64Content = GetImageAsBase64(file);

            return "data:" + contentType + ";base64," + base64Content;
        }

    }

}
