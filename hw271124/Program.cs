using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw271124
{
    internal class Program
    {
        static void Main(string[] args)
        {
            House house = new House();
            house.Work();
        }
    }

    interface IWorker
    {
        int Build();
    }

    interface IPart
    {
        string Name { get; }
        void Construct(int progress);

    }

    abstract class PartHouse
    {
        private int _progress = 0;
        private bool _isFinished = false;

        public virtual void ProgressCheck() //виртуальный метод можео перезаписывать
        {
            if (_progress >= 100)
            {
                _isFinished = true;
            }
        }

        public bool CheckSttatus()
        {
            return _isFinished;
        }

        public int GetProgress()
        {
            return _progress;
        }

        public void Construct(int progress)
        {
            _progress += progress;
            ProgressCheck();
        }
    }

    class House
    {
        List<PartHouse> _parts = new List<PartHouse>
            {
                new Basement(),
                new Wall(),
                new Wall(),
                new Wall(),
                new Wall(),
                new Window(),
                new Window(),
                new Window(),
                new Window(),
                new Roof(),
            };

        TeamLeader _teamLeader = new TeamLeader();

        public void Work()
        {
            _teamLeader.GenerateWorkers();

            for (int i = 0; i < _parts.Count; i++)
            {
                if (_parts[i].CheckSttatus() == false)
                {
                    MadeParts(_parts[i]);
                }
                if(i == UserUtils.GenerateRandomNumber(0, _parts.Count - 1))

                _teamLeader.CreateReport(_parts);
            }
        }

        private void MadeParts(PartHouse part)
        {
            while (part.CheckSttatus() == false)
            {
                foreach (Worker worker in _teamLeader.GetWorkers())
                {
                    part.Construct(worker.Build());
                }

                part.ProgressCheck();
            }
        }

    }

    class Basement : PartHouse, IPart
    {
        private string _name = "Фундамент";

        public string Name { get => _name; }
    }

    class Wall : PartHouse, IPart
    {
        private string _name = "Стена";

        public string Name { get => _name; }
    }

    class Window : PartHouse, IPart
    {
        private string _name = "Окно";

        public string Name { get => _name; }
    }

    class Roof : PartHouse, IPart
    {
        private string _name = "Крыша";

        public string Name { get => _name; }

    }

    class Worker : IWorker
    {
        public Worker()
        {
            Productivity = UserUtils.GenerateRandomNumber(1, 10);
        }
        public int Productivity { get; private set; }
        public int Build()
        {
            return Productivity;
        }
    }

    class TeamLeader
    {
        private List<Worker> _workers = new List<Worker>();

        public void GenerateWorkers()
        {
            for (int i = 0; i < UserUtils.GenerateRandomNumber(5, 10); i++)
            {
                _workers.Add(new Worker());
            }
        }

        public void CreateReport(List<PartHouse> parts)
        {
            int progress = 0;

            foreach (PartHouse part in parts)
            {
                progress += part.GetProgress();

                Console.WriteLine($"Прогресс {(part as IPart).Name} - {part.GetProgress()}");
            }

            Console.WriteLine($"Суммарный програесс - {progress / parts.Count}");
        }

        public List<Worker> GetWorkers()
        {
            return _workers;
        }
    }


    class UserUtils
    {
        private static Random rand = new Random();
        public static int GenerateRandomNumber(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue + 1);
        }
    }
}


