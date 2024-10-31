using KURSOVA.HelpWindows;
using KURSOVA.HelpWindows.Info;
using KURSOVA.Model;
using KURSOVA.ViewModels;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KURSOVA
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public WorkersListVM workersList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            workersList = new WorkersListVM();
            DataContext = workersList;

         
            if (workersList.Workers == null)
            {
                workersList.Workers = new ObservableCollection<WorkerVM>();
            }

            workersList.WorkersChanged += WorkersList_WorkersChanged;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WorkersListView.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            LoadWorkers();
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (WorkersListView.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                LoadWorkers();
            }
        }

        private void Workers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LoadWorkers();
        }

        private void WorkersList_WorkersChanged(object sender, EventArgs e)
        {
            if (WorkersListView.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                LoadWorkers();
            }
        }

        #region List Item Helpers

        private void LoadWorkers()
        {
            foreach (var item in WorkersListView.Items)
            {
                if (item is WorkerVM worker)
                {
                    var listViewItem = WorkersListView.ItemContainerGenerator.ContainerFromItem(worker) as ListViewItem;

                    if (listViewItem != null)
                    {
                        var editButton = FindVisualChild<Button>(listViewItem, "EditButton");
                        if (editButton != null)
                        {
                            editButton.Click -= EditButton_Click;
                            editButton.Click += EditButton_Click;
                        }

                        var workerTypeButton = FindVisualChild<Button>(listViewItem, "WorkerTypeButton");
                        if (workerTypeButton != null)
                        {
                            if (worker is TeacherVM)
                            {
                                workerTypeButton.Style = (Style)FindResource("TeacherButton");
                            }
                            else if (worker is EngineerVM)
                            {
                                workerTypeButton.Style = (Style)FindResource("EngineerButton");
                                workerTypeButton.Width = 28;
                                workerTypeButton.Height = 28;
                                workerTypeButton.Margin = new Thickness(0, 0, 5, 0);
                            }
                        }
                    }
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var w = button?.DataContext as WorkerVM;

            if (w != null)
            {
                if (w is TeacherVM teacher)
                {
                    T_Info infoWindow = new T_Info();
                    infoWindow.DataContext = w;
                    infoWindow.ShowDialog();
                }
                else if (w is EngineerVM engineer)
                {
                    E_Info e_Info = new E_Info();
                    e_Info.DataContext = w;
                    e_Info.ShowDialog();
                }
                else
                {
                    W_Info w_Info = new W_Info();
                    w_Info.DataContext = w;
                    w_Info.ShowDialog();
                }
            }
        }



        private T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild && (child as FrameworkElement)?.Name == childName)
                {
                    return tChild;
                }

                var result = FindVisualChild<T>(child, childName);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        #endregion

        #region Sidebar

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterW filterWindow = new FilterW();
            filterWindow.DataContext = workersList;
            filterWindow.ShowDialog();
        }

        private void GroupingButton_Click(object sender, RoutedEventArgs e)
        {
            GroupingW groupingWindow = new GroupingW();
            groupingWindow.DataContext = workersList;
            groupingWindow.ShowDialog();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "JSON files (*.json)|*.json";
            openFileDialog.Title = "Select a JSON file";


            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                workersList.Deserialize(filePath);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Save as JSON file"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                workersList.Serialize(filePath);
            }
        }

        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {
            AddWorker addWorker = new AddWorker();
            addWorker.DataContext = workersList;
            addWorker.ShowDialog();
        }

        #endregion

        #region General

        private void OpenSliderButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Slider.Visibility = Visibility.Visible;


                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.25))
                };

                DoubleAnimation blurAnimation = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromSeconds(0.25))
                };

                if (Slider.Width == 0)
                {
                    widthAnimation.From = 0;
                    widthAnimation.To = 140;

                    blurAnimation.From = 0;
                    blurAnimation.To = 10;
                }
                else
                {
                    widthAnimation.From = 140;
                    widthAnimation.To = 0;

                    blurAnimation.From = 10;
                    blurAnimation.To = 0;
                }


                Slider.BeginAnimation(WidthProperty, widthAnimation);
                BlurEffect.BeginAnimation(BlurEffect.RadiusProperty, blurAnimation);

                OnPropertyChanged(nameof(Slider));
                OnPropertyChanged(nameof(BlurEffect));
                OnPropertyChanged(nameof(UserControlBorder));

            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    #endregion
}