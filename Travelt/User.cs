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
        public string ProfilePictureFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ProfilePicture)) return null;

                string basedir = System.AppDomain.CurrentDomain.BaseDirectory;
                string convert = ProfilePicture.Replace("/", "\\");
                return System.IO.Path.Combine(basedir, convert);
            }
        }


    }
}
