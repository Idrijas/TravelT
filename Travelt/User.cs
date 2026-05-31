using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelt
{
    public class User
    {

        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int UserId { get; set; }

        public string Bio {  get; set; } 
        public string ProfilePicture { get; set; }

        public string Role { get; set; }
        public int NationalityCountryId { get; set; }
        public System.Windows.Media.ImageSource ProfilePictureFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ProfilePicture)) return null;

                string basedir = System.AppDomain.CurrentDomain.BaseDirectory;
                string convert = ProfilePicture.Replace("/", "\\");
                string fullPath = System.IO.Path.Combine(basedir, convert);

                if (!System.IO.File.Exists(fullPath)) return null;

                try
                {
                    var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad; // breaks the file lock
                    bitmap.UriSource = new Uri(fullPath);
                    bitmap.EndInit();
                    bitmap.Freeze(); // Enhances performance and allows cross-thread UI safety
                    return bitmap;
                }
                catch
                {
                    return null;
                }
            }
        }


    }
}
