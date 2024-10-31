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

namespace KURSOVA.HelpWindows
{
    /// <summary>
    /// Interaction logic for AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        public AddWorker()
        {
            InitializeComponent();
        }

        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is WorkersListVM workersList)
            {
                workersList.AddWorker();
                this.Close();
            }
        }

        private void AddEngineer_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is WorkersListVM workersList)
            {
                workersList.AddEngineer();
                this.Close();
            }
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is WorkersListVM workersList)
            {
                workersList.AddTeacher();
                this.Close();
            }
        }
    }
}
