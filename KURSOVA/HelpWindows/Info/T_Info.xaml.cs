using KURSOVA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KURSOVA.HelpWindows.Info
{
    /// <summary>
    /// Interaction logic for T_Info.xaml
    /// </summary>
    public partial class T_Info : Window
    {
        public T_Info()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is WorkerVM workerVM)
            {
                if (Application.Current.MainWindow.DataContext is WorkersListVM workersListVM)
                {
                    workersListVM.DeleteWorker(workerVM);
                }
            }
            this.Close();
        }
    }
}
