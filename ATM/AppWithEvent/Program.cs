using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Data;
using ATMClasses.Decoding;
using ATMClasses.Filtering;
using ATMClasses.Interfaces;
using ATMClasses.Output;
using ATMClasses.TrackUpdate;
using TransponderReceiver;
//Denne solution bruger vi
namespace AppWithEvent
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Tilføjet printer til output
            IOutput Output = new Output();
            Print Printer;
            IMonitors monitor = new Monitor();
            ITransponderReceiver transponderDataReceiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            IUpdate update;
            ICalcVelocity calcVelocity = new CalcVelocity();;
            ICalcCourse calcCourse = new CalcCourse();

            var decoder = new DecodingWithEvent(transponderDataReceiver);
            //Kaldet bliver lagt her til eventet. Som en slags subscriber.
            update = new Update(decoder);
            //decoder.TrackDataReady += (o, trackArgs) => Printer = new Print(update,calculator,monitor,Output,trackArgs.TrackData);
            decoder.TrackDataReadyForCalculation += (o, trackArgs) => Printer = new Print(update, calcCourse, calcVelocity, monitor, Output, trackArgs.TrackData);

            System.Console.ReadLine();
        }
    }
}

//private static void PrintTracks(List<ITracks> tracks)
//{
//    foreach (var track in tracks)
//    {
//        //Tilsat filtering
//        IMonitors monitor = new Monitor();
//        if (monitor.MonitorFlight(track) == true)
//        {
//            System.Console.WriteLine(track);
//        }
//        else
//        {

//        }

//    }
//}