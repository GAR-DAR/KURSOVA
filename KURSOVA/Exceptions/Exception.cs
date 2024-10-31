using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KURSOVA.Exceptions
{
    using System;

    public class WorkersListException : Exception
    { 
        public WorkersListException(string message)
            : base(message)
        {  
        }
    }
}
