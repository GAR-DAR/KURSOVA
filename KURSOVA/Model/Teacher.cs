using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KURSOVA.Model
{
    public class Teacher : Worker
    {
        public enum ScientificDegreeEnum
        {
            None,
            DoctorofPhilosophy,
            DoctorofScience
        }

        public enum RankEnum
        {
            None,
            Docent,
            SeniorTeacher,
            Professor
            
        }

        private string _position;
        private ScientificDegreeEnum _degree;
        private RankEnum _rank;
        private List<string> _scientificWorks;

        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        public ScientificDegreeEnum ScientificDegree
        {
            get { return _degree; }
            set
            {
                _degree = value;
                OnPropertyChanged("ScientificDegree");
            }
        }

        public RankEnum Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                OnPropertyChanged("Rank");
            }
        }

        public List<string> ScientificWorks
        {
            get { return _scientificWorks; }
            set
            {
                _scientificWorks = value;
                OnPropertyChanged("ScientificWorks");
            }
        }

        public Teacher(string name, int age, EducationEnum education, int workExperience, ScientificDegreeEnum degree, RankEnum rank, List<string> scientificWorks, string position)
       : base(name, age, education, workExperience)
        {
            ScientificDegree = degree;
            Rank = rank;
            ScientificWorks = scientificWorks;
            Position = position;
        }

        public Teacher() : base()
        {
            ScientificDegree = ScientificDegreeEnum.None;
            Rank = RankEnum.None;
            ScientificWorks = new List<string>();
        }

        public Teacher(Teacher other) : base(other)
        {
            ScientificDegree = other.ScientificDegree;
            Rank = other.Rank;
            ScientificWorks = new List<string>(other.ScientificWorks);
            Position = other.Position;
        }

        #region Output

        public override string ToString()
        {
            return base.ToString() + string.Format("Position: {4} " +

                                          "\n Degree: {5} \n Rank: {6} \n Scientific Works: {7} \n\n",
                                           Name, Age, Education, WorkExperience, Position, ScientificDegree, Rank,
                                           string.Join(", ", ScientificWorks));

        }

        #endregion

    }

}