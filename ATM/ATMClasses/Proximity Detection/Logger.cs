using System.IO;
using ATMClasses.Interfaces;

namespace ATMClasses.Proximity_Detection
{
    public class Logger : ILog
    {
        public void LogSeparationEvent(ITracks track1, ITracks track2)
        {
            string output = "Timestamp: " + track1.FormattedTimestamp + "\t" +
                            track1.Tag + " and " + track2.Tag + " are breaking separation rules";

            using (StreamWriter outputFile = new StreamWriter(@"SeparatationEventLog.txt", true))
            {
                outputFile.WriteLine(output);
            }
        }
    }
}