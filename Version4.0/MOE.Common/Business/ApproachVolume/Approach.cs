using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.ApproachVolume
{
    public class Approach
    {
        public string SignalID
        {
            get;
            set;
        }

        public string Direction
        {
            get;
            set;
        }

        private DetectorCollection detectors;
        public DetectorCollection Detectors
        {
            get { return detectors; }
        }

        private VolumeCollection volume;
        public VolumeCollection Volume
        {
            get { return volume; }
        }

        private ControllerEventLogs detectorevents;
        public ControllerEventLogs DetectorEvents
        {
            get { return detectorevents;  }
        }

        public Approach(string signalID, string direction)
        {
            SignalID = signalID;
            Direction = direction;

            detectors = new DetectorCollection(signalID, direction);

            detectorevents = null;

        }

        public void SetDetectorEvents(DateTime startdate, DateTime enddate, bool has_pcd, bool has_tmc)
        {
            detectorevents = detectors.CombineDetectorDataByApproachAndType(startdate, enddate, this.SignalID, this.Direction, has_pcd, has_tmc);


        }

        public void SetVolume(DateTime startDate, DateTime endDate,
            int binSize)
        {
           
            if (this.DetectorEvents.Events != null)
            {

                volume = new VolumeCollection(startDate, endDate, this.DetectorEvents.Events, binSize);
            }

        }

    }
}
