using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using KURSOVA.Exceptions;
using KURSOVA.Model;


namespace KURSOVA.ViewModels
{
    public class WorkersList : INotifyPropertyChanged
    {
        private List<Worker> _workers { get; set; }
        private List<Worker> _filteredWorkers { get; set; }


        public List<Worker> Workers
        {
            get => _workers;
            set
            {
                _workers = value;
                OnPropertyChanged(nameof(Workers));
            }
        }

        public List<Worker> FilteredWorkers
        {
            get => _filteredWorkers;
            set
            {
                _filteredWorkers = value;
                OnPropertyChanged(nameof(FilteredWorkers));
            }
        }


        public WorkersList()
        {
            Workers = new List<Worker>();
            FilteredWorkers = new List<Worker>();

            FilteredWorkers = Workers;

           
        }

        public void Reset()
        {
            FilteredWorkers = Workers;
            OnPropertyChanged("FilteredWorkers");
        }

        #region Grouping

        public void Sort()
        {
            var sortedWorkers = Workers
                                .OrderBy(w => w.GetType().Name) // Sort by type name
                                .ThenBy(w => w is Teacher ? 0 : 1) // Ensure Teachers are grouped together
                                .ThenBy(w => w is Teacher teacher ? (int)teacher.ScientificDegree : 0) // Sort teachers by ScientificDegreeEnum
                                .ThenBy(w => w is Teacher ? string.Empty : w.Name) // Sort non-teachers by name
                                .ToList();

            Workers = sortedWorkers;
            FilteredWorkers = sortedWorkers;
            OnPropertyChanged(nameof(Workers));
            OnPropertyChanged(nameof(FilteredWorkers));
        }



        #endregion

        #region Filters

        public void AgeFilter(int from, int to)
        {
            _filteredWorkers = _filteredWorkers.Where(w => w.Age >= from && w.Age <= to).ToList();
            OnPropertyChanged(nameof(FilteredWorkers));
        }

        public void WorkExperienceFilter(int from, int to)
        {
            _filteredWorkers = _filteredWorkers.Where(w => w.WorkExperience >= from && w.WorkExperience <= to).ToList();
            OnPropertyChanged(nameof(FilteredWorkers));
        }

        public void NameFilter(string name)
        {
            _filteredWorkers = _filteredWorkers.Where(w => w.Name.Contains(name)).ToList();
            OnPropertyChanged(nameof(FilteredWorkers));
        }

        public void ScientificDegreeFilter(string scientificDegree)
        {
            if (TeacherVM.ScientificDegreeMap.TryGetValue(scientificDegree, out var degree))
            {
                _filteredWorkers = _filteredWorkers.OfType<Teacher>()
                                                   .Where(t => t.ScientificDegree == degree)
                                                   .Cast<Worker>()
                                                   .ToList();
                OnPropertyChanged(nameof(FilteredWorkers));
            }
        }


        public void CategoryFilter(string category)
        {
            if (EngineerVM.CategoryMap.TryGetValue(category, out var engineerCategory))
            {
                _filteredWorkers = _filteredWorkers.OfType<Engineer>()
                                                   .Where(e => e.Category == engineerCategory)
                                                   .Cast<Worker>()
                                                   .ToList();
                OnPropertyChanged(nameof(FilteredWorkers));
            }
        }

        public void OfficeProgramsFilter(string officePrograms)
        {
            _filteredWorkers = _filteredWorkers.OfType<Engineer>()
                                               .Where(e => {
                                                   var engineerVM = new EngineerVM(e);
                                                   return engineerVM.OfficeProgramsKnowledge.Contains(officePrograms);
                                               })
                                               .Cast<Worker>()
                                               .ToList();
            OnPropertyChanged(nameof(FilteredWorkers));
        }

        public void PositionFilter(string position)
        {
            _filteredWorkers = _filteredWorkers.OfType<Teacher>()
                                               .Where(t => t.Position == position)
                                               .Cast<Worker>()
                                               .ToList();
            OnPropertyChanged(nameof(FilteredWorkers));
        }

        public void CanAdministrateFilter(bool canAdministrate)
        {
            _filteredWorkers = _filteredWorkers.OfType<Engineer>()
                                               .Where(e => e.CanAdministrate == canAdministrate)
                                               .Cast<Worker>()
                                               .ToList();
            OnPropertyChanged(nameof(FilteredWorkers));
        }

        #endregion

        #region Serialization

        public void Serialize(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() },
                    TypeInfoResolver = new DefaultJsonTypeInfoResolver
                    {
                        Modifiers = { PolymorphicTypeInfoResolver }
                    }
                };
                var jsonString = JsonSerializer.Serialize(FilteredWorkers, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                Handlers.HandleException(new WorkersListException($"Помилка сереалізації: {ex.Message}"));
            }

        }

        public void Deserialize(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() },
                        TypeInfoResolver = new DefaultJsonTypeInfoResolver
                        {
                            Modifiers = { PolymorphicTypeInfoResolver }
                        }
                    };
                    var jsonString = File.ReadAllText(filePath);
                    var deserializedList = JsonSerializer.Deserialize<List<Worker>>(jsonString, options);

                    if (deserializedList != null)
                    {
                        Workers.AddRange(deserializedList);
                        FilteredWorkers = new List<Worker>(Workers);
                        OnPropertyChanged(nameof(Workers));
                        OnPropertyChanged(nameof(FilteredWorkers));
                    }
                }
                catch (Exception ex)
                {
                    Handlers.HandleException(new WorkersListException($"Помилка десереалізації: {ex.Message}"));
                }

            }
        }


        private static void PolymorphicTypeInfoResolver(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Type == typeof(Worker))
            {
                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "$type",
                    DerivedTypes =
            {
                new JsonDerivedType(typeof(Teacher), "Teacher"),
                new JsonDerivedType(typeof(Engineer), "Engineer")
            }
                };
            }
        }


        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
