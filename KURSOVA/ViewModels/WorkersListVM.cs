using KURSOVA.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KURSOVA.ViewModels
{
    public class WorkersListVM : ObservableObject
    {
        private WorkersList _workersList;

        private ObservableCollection<WorkerVM> _workers;
        private ObservableCollection<WorkerVM> _filteredWorkers;

        public ObservableCollection<WorkerVM> Workers
        {
            get => _workers;
            set
            {
                _workers = value;
                OnPropertyChanged(nameof(Workers));
            }
        }

        public ObservableCollection<WorkerVM> FilteredWorkers
        {
            get => _filteredWorkers;
            set
            {
                _filteredWorkers = value;
                OnPropertyChanged(nameof(FilteredWorkers));
            }
        }

        public WorkersListVM()
        {
            _workersList = new WorkersList();
            Workers = new ObservableCollection<WorkerVM>(_workersList.FilteredWorkers.Select(CreateWorkerVM));
            FilteredWorkers = new ObservableCollection<WorkerVM>(_workersList.FilteredWorkers.Select(CreateWorkerVM));
            Workers.CollectionChanged += Workers_CollectionChanged;
            FilteredWorkers.CollectionChanged += Workers_CollectionChanged;
        }

        private void UpdateFilteredWorkers()
        {
            Workers.Clear();
            foreach (var worker in _workersList.Workers)
            {
                Workers.Add(CreateWorkerVM(worker));
            }

            FilteredWorkers.Clear();

            foreach (var worker in _workersList.FilteredWorkers)
            {
                FilteredWorkers.Add(CreateWorkerVM(worker));
            }

            OnPropertyChanged(nameof(FilteredWorkers));
            OnPropertyChanged(nameof(Workers));
            OnPropertyChanged(nameof(_workersList.Workers));
            OnPropertyChanged(nameof(_workersList.FilteredWorkers));
            WorkersChanged?.Invoke(this, EventArgs.Empty);
        }



        public event EventHandler WorkersChanged;

        private void Workers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Workers));
            OnPropertyChanged(nameof(FilteredWorkers));
        }

        #region Filtration Properties


        private string _w_nameFilter;
        private string _W_ageFromFilter;
        private string _W_ageToFilter;
        private string _W_workExperienceFromFilter;
        private string _W_workExperienceToFilter;

        private string _T_positionFilter;
        private string _T_scientificDegreeFilter;

        private string _E_categoryFilter;
        private string _E_officeProgramsFilter;
        private bool _E_canAdministrateFilter;


       public string W_nameFilter
        {
            get { return _w_nameFilter; }
            set
            {
                _w_nameFilter = value;
                OnPropertyChanged("W_nameFilter");
            }
        }

        public string W_ageFromFilter
        {
            get { return _W_ageFromFilter; }
            set
            {
                _W_ageFromFilter = value;
                OnPropertyChanged("W_ageFromFilter");
            }
        }

        public string W_ageToFilter
        {
            get { return _W_ageToFilter; }
            set
            {
                _W_ageToFilter = value;
                OnPropertyChanged("W_ageToFilter");
            }
        }

        public string W_workExperienceFromFilter
        {
            get { return _W_workExperienceFromFilter; }
            set
            {
                _W_workExperienceFromFilter = value;
                OnPropertyChanged("W_workExperienceFromFilter");
            }
        }

        public string W_workExperienceToFilter
        {
            get { return _W_workExperienceToFilter; }
            set
            {
                _W_workExperienceToFilter = value;
                OnPropertyChanged("W_workExperienceToFilter");
            }
        }

        public string T_positionFilter
        {
            get { return _T_positionFilter; }
            set
            {
                _T_positionFilter = value;
                OnPropertyChanged("T_positionFilter");
            }
        }

        public string T_scientificDegreeFilter
        {
            get { return _T_scientificDegreeFilter; }
            set
            {
                _T_scientificDegreeFilter = value;
                OnPropertyChanged("T_scientificDegreeFilter");
            }
        }

        public string E_categoryFilter
        {
            get { return _E_categoryFilter; }
            set
            {
                _E_categoryFilter = value;
                OnPropertyChanged("E_categoryFilter");
            }
        }

        public string E_officeProgramsFilter
        {
            get { return _E_officeProgramsFilter; }
            set
            {
                _E_officeProgramsFilter = value;
                OnPropertyChanged("E_officeProgramsFilter");
            }
        }

        public bool E_canAdministrateFilter
        {
            get { return _E_canAdministrateFilter; }
            set
            {
                _E_canAdministrateFilter = value;
                OnPropertyChanged("E_canAdministrateFilter");
            }
        }


        public void ResetFilterList()
        {
            _workersList.Reset();
            
           UpdateFilteredWorkers();
        }

        public void NameFilter(string name)
        {
            _workersList.NameFilter(name);

            UpdateFilteredWorkers();
        }

        public void AgeFilter(int from, int to)
        {
           _workersList.AgeFilter(from, to);

           UpdateFilteredWorkers();
        }

        public void WorkExperienceFilter(int from, int to)
        {
            _workersList.WorkExperienceFilter(from, to);

            UpdateFilteredWorkers();
        }


        public void PositionFilter(string position)
        {
            _workersList.PositionFilter(position);
            UpdateFilteredWorkers();
        }

        public void ScientificDegreeFilter(string scientificDegree)
        {
            _workersList.ScientificDegreeFilter(scientificDegree);
            UpdateFilteredWorkers();
        }


        public void CategoryFilter(string category)
        {
            _workersList.CategoryFilter(category);
            UpdateFilteredWorkers();
        }

        public void OfficeProgramsFilter(string officePrograms)
        {
            _workersList.OfficeProgramsFilter(officePrograms);
            UpdateFilteredWorkers();
        }

        public void CanAdministrateFilter(bool canAdministrate)
        {
            _workersList.CanAdministrateFilter(canAdministrate);
            UpdateFilteredWorkers();
        }

        #endregion

        public void Sort()
        {
            _workersList.Sort();
            UpdateFilteredWorkers();
        }



        #region Helpers

        private WorkerVM CreateWorkerVM(Worker worker)
        {
            return worker switch
            {
                Teacher teacher => new TeacherVM(teacher),
                Engineer engineer => new EngineerVM(engineer),
                _ => new WorkerVM(worker)
            };
        }

        public T GetWorkerVM<T>(int index) where T : WorkerVM
        {
            return Workers[index] as T;
        }

        public void AddWorker()
        {
            var newWorker = new Worker();
            _workersList.Workers.Add(newWorker);
            _workersList.FilteredWorkers = _workersList.Workers;

            Workers.Add(CreateWorkerVM(newWorker));
            FilteredWorkers.Add(CreateWorkerVM(newWorker));

            UpdateFilteredWorkers();

            
        }

        public void AddEngineer()
        {
            var newEngineer = new Engineer();
            _workersList.Workers.Add(newEngineer);
            _workersList.FilteredWorkers = _workersList.Workers;

            Workers.Add(CreateWorkerVM(newEngineer));
            FilteredWorkers.Add(CreateWorkerVM(newEngineer));

            UpdateFilteredWorkers();

        }

        public void AddTeacher()
        {
            var newTeacher = new Teacher();

            _workersList.Workers.Add(newTeacher);
            _workersList.FilteredWorkers = _workersList.Workers;

            Workers.Add(CreateWorkerVM(newTeacher));
            FilteredWorkers.Add(CreateWorkerVM(newTeacher));

            UpdateFilteredWorkers();
        }



        public void DeleteWorker(WorkerVM workerVM)
        {
            Worker worker = null;

            if (workerVM is TeacherVM teacherVM)
            {
                worker = _workersList.Workers.OfType<Teacher>().FirstOrDefault(t => t.Name == teacherVM.Name && t.Age == teacherVM.Age && t.WorkExperience == teacherVM.WorkExperience);
            }
            else if (workerVM is EngineerVM engineerVM)
            {
                worker = _workersList.Workers.OfType<Engineer>().FirstOrDefault(e => e.Name == engineerVM.Name && e.Age == engineerVM.Age && e.WorkExperience == engineerVM.WorkExperience);
            }
            else
            {
                worker = _workersList.Workers.FirstOrDefault(w => w.Name == workerVM.Name && w.Age == workerVM.Age && w.WorkExperience == workerVM.WorkExperience);
            }

            if (worker != null)
            {
                _workersList.Workers.Remove(worker);
                _workersList.FilteredWorkers.Remove(worker);
                Workers.Remove(workerVM);
                FilteredWorkers.Remove(workerVM);
                OnPropertyChanged(nameof(Workers));
                OnPropertyChanged(nameof(FilteredWorkers));
            }
        }

        #endregion

        public void Serialize(string filePath)
        {
            _workersList.Serialize(filePath);
        }

        public void Deserialize(string filePath)
        {
            _workersList.Deserialize(filePath);
            Workers = new ObservableCollection<WorkerVM>(_workersList.Workers.Select(CreateWorkerVM));
            FilteredWorkers = new ObservableCollection<WorkerVM>(_workersList.FilteredWorkers.Select(CreateWorkerVM));
            OnPropertyChanged(nameof(Workers));
            OnPropertyChanged(nameof(FilteredWorkers));
        }

    }


}
