using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KURSOVA.Model;

namespace KURSOVA.ViewModels
{
    public class TeacherVM : WorkerVM
    {
        private Teacher teacher;

        public TeacherVM() : base(new Teacher())
        {
            teacher = (Teacher)worker;
        }

        public TeacherVM(Teacher teacher) : base(teacher)
        {
            this.teacher = teacher;
        }


        public string Position
        {
            get { return teacher.Position; }
            set
            {
                teacher.Position = value;
                OnPropertyChanged("Position");
            }
        }



        public string ScientificDegree
        {
            get { return ScientificDegreeMap.FirstOrDefault(x => x.Value == teacher.ScientificDegree).Key; }
            set
            {
                if (ScientificDegreeMap.TryGetValue(value, out var result))
                {
                    teacher.ScientificDegree = result;
                    OnPropertyChanged("ScientificDegree");
                }
            }
        }

        public string Rank
        {
            get { return RankMap.FirstOrDefault(x => x.Value == teacher.Rank).Key; }
            set
            {
                if (RankMap.TryGetValue(value, out var result))
                {
                    teacher.Rank = result;
                    OnPropertyChanged("Rank");
                }
            }
        }

        public string ScientificWorks
        {
            get { return string.Join(", ", teacher.ScientificWorks.Select(work => $"\"{work}\"")); }
            set
            {
                teacher.ScientificWorks = value.Split(new[] { "\", \"" }, StringSplitOptions.None)
                                               .Select(work => work.Trim('"'))
                                               .ToList();
                OnPropertyChanged("ScientificWorks");
            }
        }

        #region ComboBoxItems

        public static Dictionary<string, Teacher.ScientificDegreeEnum> ScientificDegreeMap = new Dictionary<string, Teacher.ScientificDegreeEnum>
        {
            { "---", Teacher.ScientificDegreeEnum.None },
            { "Доктор філософії", Teacher.ScientificDegreeEnum.DoctorofPhilosophy },
            { "Доктор наук", Teacher.ScientificDegreeEnum.DoctorofScience }
        };

        public static Dictionary<string, Teacher.RankEnum> RankMap = new Dictionary<string, Teacher.RankEnum>
        {
            { "---", Teacher.RankEnum.None },
            { "Доцент", Teacher.RankEnum.Docent },
            { "Старший викладач", Teacher.RankEnum.SeniorTeacher },
            { "Професор", Teacher.RankEnum.Professor }
        };

        public static List<string> ScientificDegrees
        {
            get
            {
                return ScientificDegreeMap.Keys.ToList();
            }
        }

        public static List<string> Ranks
        {
            get
            {
                return RankMap.Keys.ToList();
            }
        }

        #endregion
    }
}
