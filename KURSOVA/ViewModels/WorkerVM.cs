using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KURSOVA;
using KURSOVA.Model;

namespace KURSOVA.ViewModels
{
    public class WorkerVM : ObservableObject
    {
        protected Worker worker;

        public WorkerVM()
        {
            worker = new Worker();
        }

        public WorkerVM(Worker worker)
        {
            this.worker = worker;
        }

        public string Name
        {
            get { return worker.Name; }
            set
            {
                worker.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Age
        {
            get { return worker.Age; }
            set
            {
                worker.Age = value;
                OnPropertyChanged("Age");
            }
        }

        public int WorkExperience
        {
            get { return worker.WorkExperience; }
            set
            {
                worker.WorkExperience = value;
                OnPropertyChanged("WorkExperience");
            }
        }

        public static Dictionary<string, Worker.EducationEnum> EducationMap = new Dictionary<string, Worker.EducationEnum>
        {
            { "---", Worker.EducationEnum.None },
            { "Без освіти", Worker.EducationEnum.NoEducation },
            { "Повна загальна середня", Worker.EducationEnum.Secondary },
            { "Бакалавр", Worker.EducationEnum.Bachelor },
            { "Магістр", Worker.EducationEnum.Master },
        };

        public static List<string> Educations
        {
            get
            {
                return EducationMap.Keys.ToList();
            }
        }

        public string Education
        {
            get { return EducationMap.FirstOrDefault(x => x.Value == worker.Education).Key; }
            set
            {
                if (EducationMap.TryGetValue(value, out var result))
                {
                    worker.Education = result;
                    OnPropertyChanged("Education");
                }
            }
        }

    }
}
