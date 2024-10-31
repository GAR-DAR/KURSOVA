using KURSOVA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KURSOVA.ViewModels
{
    public class EngineerVM : WorkerVM
    {
        private Engineer engineer;

        public EngineerVM() : base(new Engineer())
        {
            engineer = (Engineer)worker;
        }

        public EngineerVM(Engineer engineer) : base(engineer)
        {
            this.engineer = engineer;
        }

        public bool CanAdministrate
        {
            get { return engineer.CanAdministrate; }
            set
            {
                engineer.CanAdministrate = value;
                OnPropertyChanged("CanAdministrate");
            }
        }

        public string Category
        {
            get { return CategoryMap.FirstOrDefault(x => x.Value == engineer.Category).Key; }
            set
            {
                if (CategoryMap.TryGetValue(value, out var result))
                {
                    engineer.Category = result;
                    OnPropertyChanged("Category");
                }
            }
        }

        public int YearOfAtestation
        {
            get { return engineer.YearOfAtestation; }
            set
            {
                engineer.YearOfAtestation = value;
                OnPropertyChanged("YearOfAtestation");
            }
        }

        public string OfficeProgramsKnowledge
        {
            get { return string.Join(", ", engineer.OfficeProgramsKnowledge.Select(p => $"\"{p}\"")); }
            set
            {
                engineer.OfficeProgramsKnowledge = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                         .Select(p => p.Trim().Trim('"'))
                                                         .ToList();
                OnPropertyChanged("OfficeProgramsKnowledge");
            }
        }

        #region ComboBoxItems

        public static Dictionary<string, Engineer.EngineerCategoryEnum> CategoryMap = new Dictionary<string, Engineer.EngineerCategoryEnum>
        {
            { "---",   Engineer.EngineerCategoryEnum.None},
            { "Механік", Engineer.EngineerCategoryEnum.Mechanical },
            { "Електрик", Engineer.EngineerCategoryEnum.Electrical },
            { "Цивільний", Engineer.EngineerCategoryEnum.Civil },
            { "Хімічний", Engineer.EngineerCategoryEnum.Chemical }
        };

        public static List<string> Categories
        {
            get
            {
                return CategoryMap.Keys.ToList();
            }
        }


        #endregion
    }
}
