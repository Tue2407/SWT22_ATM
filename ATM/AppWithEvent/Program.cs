using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Data;
using ATMClasses.Decoding;
using TransponderReceiver;

namespace AppWithEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            ITransponderReceiver transponderDataReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            var decoder = new DecodingWithEvent(transponderDataReceiver);

            decoder.TrackDataReady += (o, trackArgs) => PrintTracks(trackArgs.TrackData);

            System.Console.ReadLine();
        }

        private static void PrintTracks(List<TrackData> tracks)
        {
            foreach (var track in tracks)
            {
                System.Console.WriteLine(track);                
            }
        }
    }
}
