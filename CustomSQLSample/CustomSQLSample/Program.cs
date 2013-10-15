using System;
using System.Collections.ObjectModel;

namespace CustomSQLSample
{
    class Program
    {
        static void Main()
        {
            var datasetAdapter = new DataSetAdapter();

            // get all states
            var allStates = datasetAdapter.getAll();
            
            Console.WriteLine("===== allStates");
            foreach (var state in allStates)
            {
                state.print();
            }

            // get by selection ... CustomSQL
            var selection = new Collection<string>
            {
                @"アラスカ州",
                @"マサチューセッツ州",
                @"オハイオ州",
            };

            Console.WriteLine("===== selection");
            foreach (var x in selection)
            {
                Console.WriteLine("\t{0}", x);
            }

            var selectedStates = datasetAdapter.getWithInOperator(selection);

            Console.WriteLine("===== selectedStates");
            foreach (var state in selectedStates)
            {
                state.print();
            }
        }
    }
}
