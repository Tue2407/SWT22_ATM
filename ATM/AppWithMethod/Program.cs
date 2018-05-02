using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Decoding;
using ATMClasses.Interfaces;
using TransponderReceiver;

namespace AppWithMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            ITransponderReceiver transponderDataReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            ITrackReceiver trackReceiver = new TrackDataReceiver();

            var decoder = new DecodingWithMethod(transponderDataReceiver, trackReceiver);

            System.Console.ReadLine();
        }
    }
}
