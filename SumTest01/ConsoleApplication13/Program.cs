using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApplication13
{
    class Program
    {
        static void Main(string[] args)
        {
            var datas = new ArrayList {100, 10000, 100000, 1000000, 5000000};

            for (int i = 0; i < datas.Count; i++)
            {
                // data
                var cnt = (int)datas[i];
                var list = new List<decimal>();
                for (int j = 0; j < cnt; j++)
                {
                    list.Add(j);
                }

                Console.WriteLine("========== count={0:##,###}", cnt);

                var sw = new Stopwatch();

                // non-Linq
                sw.Start();
                var result_foreach = Sum_A(list);
                sw.Stop();
                Console.WriteLine("result_foreach={0} : {1} msec", result_foreach, sw.ElapsedMilliseconds);

                sw.Restart();
                var result_for = Sum_B(list);
                sw.Stop();
                Console.WriteLine("result_for={0} : {1} msec", result_for, sw.ElapsedMilliseconds);

                // Linq
                sw.Restart();
                var result_Linq = Sum_Linq(list);
                sw.Stop();
                Console.WriteLine("result_Linq={0} : {1} msec", result_Linq, sw.ElapsedMilliseconds);
            }
        }

        public static decimal Sum_A(List<decimal> list)
        {
            var sum = 0m;
            foreach (var x in list)
            {
                sum += x;
            }
            return sum;
        }

        public static decimal Sum_B(List<decimal> list)
        {
            var cnt = list.Count;
            var sum = 0m;
            for (int i = 0; i < cnt; i++)
            {
                sum += list[i];
            }
            return sum;
        }

        public static decimal Sum_Linq(List<decimal> list)
        {
            return list.Sum();
        }
    }
}
