using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelt.Service
{
    public class UserService
    {

        public bool Login(string email, string password)
        {   

            // Dries this is a note for you and me:

            // TEMPORARY: Mock login logic for testing UI functionality.
            // TODO: Replace this with a database query (SQL) once backend is connected.

            if (email == "test@test.com" && password == "1234")
            {
                return true;
            }
            return false;
        }



    }
}
