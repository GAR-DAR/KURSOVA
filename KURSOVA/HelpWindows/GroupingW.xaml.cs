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
   
    public partial class GroupingW : Window
    {
        public GroupingW()
        {
            InitializeComponent();
        }


        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is WorkersListVM viewModel)
            {
                viewModel.Sort();
            }

            this.Close();
        }


    }
}
