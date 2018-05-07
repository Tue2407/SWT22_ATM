using System;
using ATMClasses.Interfaces;

namespace ATMClasses.Output
{
    public class Output : IOutput
    {
        public void OutputLine(string line)
        {

            Console.WriteLine(line);
        }
    }
}