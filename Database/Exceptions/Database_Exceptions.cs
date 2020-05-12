using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Exceptions
{
    public class Database_Exceptions:Exception
    {
        public Database_Exceptions(string message): base(message)
        {

        }
    }
}
