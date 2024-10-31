using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Reflection;
using KURSOVA.Exceptions;
using System.Windows;


namespace KURSOVA.Model
{
   
    public class Worker : INotifyPropertyChanged
    {
        public enum EducationEnum
        {
            None,
            NoEducation,
            Secondary,
            Bachelor,
            Master
        }

        private string _name;
        private int _age;
        private EducationEnum _education;
        private int _workExperience;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public int Age
        {
            get { return _age; }
            set
            {
                try
                {
                    if (value < 0)
                    {
                        throw new WorkersListException("Вік не може бути від'ємним.");
                    }
                    _age = value;


                }
                catch (WorkersListException ex)
                {
                    Handlers.HandleException(ex);
                }
                catch
                {
                    MessageBox.Show("Невідома помилка");
                }

                OnPropertyChanged("Age");
            }
        }
        public EducationEnum Education
        {
            get { return _education; }
            set
            {
                _education = value;
                OnPropertyChanged(nameof(Education));
            }
        }

        public int WorkExperience
        {
            get { return _workExperience; }
            set
            {
                try
                {
                    if (value < 0)
                    {
                        throw new WorkersListException("Досвід роботи не може бути від'ємним.");
                    }
                    _workExperience = value;
                }
                catch (WorkersListException ex)
                {
                    Handlers.HandleException(ex);
                }
                catch
                {
                    MessageBox.Show("Невідома помилка");
                }

                OnPropertyChanged("WorkExperience");
            }
        }

        public Worker(string name, int age, EducationEnum education, int workExperience)
        {
                Name = name;
                Age = age;
                Education = education;
                WorkExperience = workExperience;
            
        }

        public Worker() : this(" ", 0, EducationEnum.NoEducation, 0) { }

        public Worker(Worker other)
        {
            Name = other.Name;
            Age = other.Age;
            Education = other.Education;
            WorkExperience = other.WorkExperience;
        }

        #region Output

        public override string ToString()
        {
            return string.Format("Name: {0} \n Age: {1} \n Education: {2} \n Work Experience: {3} \n",
                Name, Age, Education, WorkExperience);
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
