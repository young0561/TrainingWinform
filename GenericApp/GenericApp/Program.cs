using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericApp
{
    public class SimpleGeneric<T> {

        private T[] values;
        private int index;

        public SimpleGeneric(int len) {

            values = new T[len];
            index = 0;
        }

        public void Add(params T[] args) {
            foreach (T e in args)
            {
                values[index++] = e;
            }
        }

        public void Print() {
            foreach (T e in values)
            {
                Console.Write(e + "");
            }
                Console.WriteLine();
            
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            SimpleGeneric<Int32> gIntegers = new SimpleGeneric<Int32>(10);
            SimpleGeneric<double> gDouble = new SimpleGeneric<double>(10);


            gIntegers.Add(1, 2);
            gIntegers.Add(1, 2, 3, 4, 5, 6, 7);
            gIntegers.Add(0);
            gIntegers.Print();



            gDouble.Add(10.0, 20.0, 30.0);
            gDouble.Print();
                 
        }
    }
}
