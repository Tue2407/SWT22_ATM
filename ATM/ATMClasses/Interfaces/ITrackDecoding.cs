﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Data;

namespace ATMClasses.Interfaces
{
    public class TrackDataEventArgs : EventArgs
    {
        public List<ITracks> TrackData { get;  }
        public TrackDataEventArgs(List<ITracks> trackData)
        {
            TrackData = trackData;
        }
    }

    public interface ITrackDecoding
    {
        event EventHandler<TrackDataEventArgs> TrackDataReady;
    }
}
