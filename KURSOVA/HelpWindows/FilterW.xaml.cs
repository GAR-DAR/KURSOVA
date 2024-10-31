using KURSOVA.Exceptions;
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
    /// Interaction logic for FilterWi.xaml
    /// </summary>
    public partial class FilterW : Window
    {
        public FilterW()
        {
            InitializeComponent();
        }

        private void WorkerButton_Click(object sender, RoutedEventArgs e)
        {
            WorkerFilter.Visibility = Visibility.Visible;
            TeacherFilter.Visibility = Visibility.Collapsed;
            EngineerFilter.Visibility = Visibility.Collapsed;
        }

        private void TeacherButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherFilter.Visibility = Visibility.Visible;
            WorkerFilter.Visibility = Visibility.Collapsed;
            EngineerFilter.Visibility = Visibility.Collapsed;
        }

        private void EngineerButton_Click(object sender, RoutedEventArgs e)
        {
           EngineerFilter.Visibility = Visibility.Visible;
           WorkerFilter.Visibility = Visibility.Collapsed;
           TeacherFilter.Visibility = Visibility.Collapsed;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

            if (DataContext is WorkersListVM viewModel)
            {
                // Reset Worker filters
                viewModel.W_nameFilter = string.Empty;
                viewModel.W_ageFromFilter = string.Empty;
                viewModel.W_ageToFilter = string.Empty;
                viewModel.W_workExperienceFromFilter = string.Empty;
                viewModel.W_workExperienceToFilter = string.Empty;

                // Reset Teacher filters
                viewModel.T_positionFilter = string.Empty;
                viewModel.T_scientificDegreeFilter = string.Empty;

                // Reset Engineer filters
                viewModel.E_categoryFilter = string.Empty;
                viewModel.E_officeProgramsFilter = string.Empty;
                viewModel.E_canAdministrateFilter = false;

                viewModel.ResetFilterList();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is WorkersListVM viewModel)
                {
                    viewModel.ResetFilterList();

                    // Check if any Teacher filters are used
                    bool isTeacherFilterUsed = !string.IsNullOrWhiteSpace(viewModel.T_positionFilter) ||
                                               !string.IsNullOrWhiteSpace(viewModel.T_scientificDegreeFilter);

                    // Check if any Engineer filters are used
                    bool isEngineerFilterUsed = !string.IsNullOrWhiteSpace(viewModel.E_categoryFilter) ||
                                                !string.IsNullOrWhiteSpace(viewModel.E_officeProgramsFilter) ||
                                                viewModel.E_canAdministrateFilter;

                    // Clear Engineer filters if Teacher filters are used
                    if (isTeacherFilterUsed && isEngineerFilterUsed)
                    {
                        
                        throw new WorkersListException("Не можна використовувати фільтрування вчителів і інженерів одночасно.");

                        viewModel.E_categoryFilter = string.Empty;
                        viewModel.E_officeProgramsFilter = string.Empty;
                        viewModel.E_canAdministrateFilter = false;
                        viewModel.T_positionFilter = string.Empty;
                        viewModel.T_scientificDegreeFilter = string.Empty;
                    }

                    // Apply Worker filters
                    if (!string.IsNullOrWhiteSpace(viewModel.W_ageFromFilter) || !string.IsNullOrWhiteSpace(viewModel.W_ageToFilter))
                    {
                        if (!int.TryParse(viewModel.W_ageFromFilter, out int ageFrom))
                        {
                            throw new WorkersListException("Вивід у полі Вік від є помилковим");
                        }
                        if (!int.TryParse(viewModel.W_ageToFilter, out int ageTo))
                        {
                            throw new WorkersListException("Вивід у полі Вік до є помилковим");
                        }
                        else if (ageFrom > ageTo || ageFrom < 0 || ageTo < 0)
                        {
                            throw new WorkersListException("Вивід у полі Вік до є помилковим");
                        }
                            viewModel.AgeFilter(ageFrom, ageTo);
                    }

                    if (!string.IsNullOrWhiteSpace(viewModel.W_workExperienceFromFilter) || !string.IsNullOrWhiteSpace(viewModel.W_workExperienceToFilter))
                    {
                        if (!int.TryParse(viewModel.W_workExperienceFromFilter, out int workExperienceFrom))
                        {
                            throw new WorkersListException("Вивід у полі Стаж роботи від є помилковим");
                        }
                        if (!int.TryParse(viewModel.W_workExperienceToFilter, out int workExperienceTo))
                        {
                            throw new WorkersListException("Вивід у полі Стаж роботи до є помилковим");
                        }
                        else if (workExperienceFrom > workExperienceTo || workExperienceFrom < 0 || workExperienceTo < 0)
                        {
                            throw new WorkersListException("Вивід у полі Стаж роботи до є помилковим");
                        }
                        viewModel.WorkExperienceFilter(workExperienceFrom, workExperienceTo);
                    }

                    if (!string.IsNullOrEmpty(viewModel.W_nameFilter))
                    {
                        viewModel.NameFilter(viewModel.W_nameFilter);
                    }

                    // Apply Teacher filters if used
                    if (isTeacherFilterUsed)
                    {
                        if (!string.IsNullOrEmpty(viewModel.T_positionFilter))
                        {
                            viewModel.PositionFilter(viewModel.T_positionFilter);
                        }
                        if (!string.IsNullOrEmpty(viewModel.T_scientificDegreeFilter))
                        {
                            viewModel.ScientificDegreeFilter(viewModel.T_scientificDegreeFilter);
                        }
                    }
                    // Apply Engineer filters if used
                    if (isEngineerFilterUsed)
                    {
                        if (!string.IsNullOrEmpty(viewModel.E_categoryFilter))
                        {
                            viewModel.CategoryFilter(viewModel.E_categoryFilter);
                        }
                        if (!string.IsNullOrEmpty(viewModel.E_officeProgramsFilter))
                        {
                            viewModel.OfficeProgramsFilter(viewModel.E_officeProgramsFilter);
                        }
                        if (viewModel.E_canAdministrateFilter)
                        {
                            viewModel.CanAdministrateFilter(viewModel.E_canAdministrateFilter);
                        }
                    }
                }
                this.Close();
            }
            catch (WorkersListException ex)
            {
                Handlers.HandleException(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch
            {
                MessageBox.Show("Помилка");
            }
        }
    }
}