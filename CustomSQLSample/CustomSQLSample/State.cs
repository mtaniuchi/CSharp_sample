using System;

namespace CustomSQLSample
{
    public class State
    {
        public string Name { get; set; }
        public string Nicname { get; set; }
        public string Capital { get; set; }
        public string Metropolis { get; set; }

        public void print()
        {
            Console.WriteLine("{0}\t{1}\t{2}\t{3}",
                this.Name, this.Nicname, this.Capital, this.Metropolis);
        }
    }
}
