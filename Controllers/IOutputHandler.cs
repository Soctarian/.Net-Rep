using System;
using System.Collections.Generic;
using System.Text;

namespace Controllers
{
    public interface IOutputHandler
    {
        void Output(string Info);
    }

    class ConsoleOutputHandler : IOutputHandler
    {
        public void Output(string Info)
        {
            Console.WriteLine(Info);
        }
    }

    class DataBaseLogHandler : IOutputHandler
    {
        public void Output(string Info)
        {
            //... dbSet..
        }
    }

}
