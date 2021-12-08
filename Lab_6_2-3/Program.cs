using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab_6_2_3
{
    public interface ISignal
    {
        int year { get; set; }
        string name { get; set; }
        int diameter { get; set; }
        int frequency { get; set; }
    }

    class Signal : ISignal, IComparable<Signal>
    {
        public int year { get; set; }
        public string name { get; set; }
        public int diameter { get; set; }
        public int frequency { get; set; }

        public Signal(int year, string name, int diameter, int frequency)
        {
            this.year = year;
            this.name = name;
            this.diameter = diameter;
            this.frequency = frequency;
        }

        public string Info
        {
            get { return $"{year} {name}"; }
        }

        public int CompareTo(Signal other)
        {
            return string.Compare(other.Info, Info, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            return $"{year}, {name}, {diameter}, {frequency}";
        }
    }

    class CollectionType<T> : IEnumerable<T> where T : Signal
    {
        List<T> list = new List<T>();

        public CollectionType()
        {
            list = new List<T>();
        }

        public int Count
        {
            get { return list.Count; }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return list[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException();
                }
                list[index] = value;
            }
        }

        public void Add(T signal)
        {
            list.Add(signal);
        }

        public T Remove(T signal)
        {
            var element = list.FirstOrDefault(h => h == signal);
            if (element != null)
            {
                list.Remove(element);
                return element;
            }
            throw new NullReferenceException();
        }

        public void Sort()
        {
            list.Sort();
        }

        public T GetByName(string name)
        {
            return list.FirstOrDefault(h => string.Compare(h.Info, name, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Signal sgn1 = new Signal(1960, "Дрейк", 26, 1420);
            Signal sgn2 = new Signal(1970, "Троїцкий", 14, 1875);
            Signal sgn3 = new Signal(1978, "Хоровiц", 300, 1665);
            Signal sgn4 = new Signal(1981, "Барановський", 180, 1774);

            CollectionType<Signal> collection = new CollectionType<Signal>();
            collection.Add(sgn1);
            collection.Add(sgn2);
            collection.Add(sgn3);
            collection.Add(sgn4);
            collection.Remove(sgn4);

            Console.WriteLine("Рiк | Науковий керiвник | Дiаметр антени (м) | Робоча частота (Мгц)");
            foreach (Signal s in collection)
            {
                Console.WriteLine(s.ToString());
            }

            Signal sgn5 = new Signal(1980, "Саржков", 45, 1256);
            Signal sgn6 = new Signal(1983, "Лузкович", 155, 1733);
            Signal sgn7 = new Signal(1988, "Прокопенко", 288, 1888);

            CollectionType<Signal> collection2 = new CollectionType<Signal>();
            collection.Add(sgn5);
            collection.Add(sgn6);
            collection.Add(sgn7);

            var list = new List<CollectionType<Signal>>();
            list.Add(collection);
            list.Add(collection2);

            Console.WriteLine("\nOrderBy:");
            var order = collection.OrderBy(h => h.diameter).ThenBy(h => h.year);
            foreach (var signal in order)
            {
                Console.WriteLine(signal);
            }

            Console.WriteLine("\nWhere:");
            var where = collection.Where(h => (h.diameter >= 100 && h.frequency > 1450) || h.Info.StartsWith("L"));
            foreach (var signal in where)
            {
                Console.WriteLine(signal.ToString());
            }

            Console.WriteLine("\nSelect:");
            var select = collection.Select((h, i) => new { ID = i + 1, h.Info });
            foreach (var s in select)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("\nSkip:");
            var skip = collection.Skip(3);
            foreach (var signal in skip)
            {
                Console.WriteLine(signal);
            }

            Console.WriteLine("\nTake:");
            var take = collection.Take(3);
            foreach (var signal in take)
            {
                Console.WriteLine(signal);
            }

            Console.WriteLine("\nConcat:");
            var concat = collection.Concat(collection2);
            foreach (var signal in concat)
            {
                Console.WriteLine(signal);
            }

            Console.WriteLine("\nFirst:");
            var first = collection.First(h => h.Info.Length > 5);
            Console.WriteLine(first);

            Console.Write("\nMin: ");
            var min = collection.Min(h => h.frequency);
            Console.WriteLine(min);

            Console.Write("\nMax: ");
            var max = collection.Max(h => h.frequency);
            Console.WriteLine(max);

            Console.WriteLine("\nAll and Any:");
            var allAny = list.First(c => c.All(h => h.diameter >= 14) && c.Any(h => h is Signal)).Select(h => h.Info).OrderByDescending(s => s);
            foreach (var str in allAny)
            {
                Console.WriteLine(str);
            }

            Console.WriteLine("\nContains:");
            var contains = list.Where(c => c.Contains(sgn1)).SelectMany(c => c.SelectMany(h => h.Info.Split(' '))).Distinct().OrderBy(s => s).ToList();
            foreach (var str in contains)
            {
                Console.WriteLine(str);
            }
        }
    }
}
