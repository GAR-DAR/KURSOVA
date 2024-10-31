using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KURSOVA.HelpWindows;

namespace KURSOVA.Exceptions
{
    public class Handlers
    {
            public static void HandleException(WorkersListException ex)
            {
                  ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                  errorWindow.ShowDialog();
            }
    }
    
}
