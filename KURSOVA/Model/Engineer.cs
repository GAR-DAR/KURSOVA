using KURSOVA.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KURSOVA.Model
{
    public class Engineer : Worker
    {
        public enum EngineerCategoryEnum
        {
            None,
            Mechanical,
            Electrical,
            Civil,
            Chemical
        }

        private bool _canAdministrate;
        private EngineerCategoryEnum _category;
        private int _yearOfAtestation;
        private List<string> _officeProgramsKnowledge;

        public bool CanAdministrate
        {
            get { return _canAdministrate; }
            set
            {
                _canAdministrate = value;
                OnPropertyChanged("CanAdministrate");
            }
        }

        public EngineerCategoryEnum Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        public int YearOfAtestation
        {
            get { return _yearOfAtestation; }
            set
            {
                try
                {
                    if (value < 1920 || value > 2024)
                    {
                        throw new WorkersListException("Рік адміністрації не може бути меншим ніж 1920 і більшим ніж 2024.");
                    }
                    _yearOfAtestation = value;
                }
                catch (WorkersListException ex)
                {
                    Handlers.HandleException(ex);
                }
                catch
                {
                    MessageBox.Show("Y");
                }

                OnPropertyChanged("YearOfAtestation");
            }
        }


        public List<string> OfficeProgramsKnowledge
        {
            get { return _officeProgramsKnowledge; }
            set
            {
                _officeProgramsKnowledge = value;
                OnPropertyChanged("OfficeProgramsKnowledge");
            }
        }

        public Engineer(string name, int age, EducationEnum education, int workExperience, bool canAdministrate, EngineerCategoryEnum category, int yearOfAtestation, List<string> officeProgramsKnowledge)
            : base(name, age, education, workExperience)
        {
            CanAdministrate = canAdministrate;
            Category = category;
            YearOfAtestation = yearOfAtestation;
            OfficeProgramsKnowledge = officeProgramsKnowledge;
        }

        public Engineer() : base()
        {
            CanAdministrate = false;
            Category = EngineerCategoryEnum.Mechanical;
            YearOfAtestation = DateTime.Now.Year;
            OfficeProgramsKnowledge = new List<string>();
        }

        public Engineer(Engineer other) : base(other)
        {
            CanAdministrate = other.CanAdministrate;
            Category = other.Category;
            YearOfAtestation = other.YearOfAtestation;
            OfficeProgramsKnowledge = new List<string>(other.OfficeProgramsKnowledge);
        }

        #region Output

        public override string ToString()
        {
            return base.ToString() + string.Format("Can Administrate: {4} " +
                                                   "\n Category: {5} \n Year Of Atestation: {6} \n Office Programs Knowledge: {7} \n\n",
                                                   Name, Age, Education, WorkExperience, CanAdministrate, Category, YearOfAtestation,
                                                   string.Join(", ", OfficeProgramsKnowledge));
        }

        #endregion
    }
}
