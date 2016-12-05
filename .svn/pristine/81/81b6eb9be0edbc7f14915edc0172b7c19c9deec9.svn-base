using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Preempt
{
    public class PreemptCycleEngine
    {
        

        public List<PreemptCycle> CreatePreemptCycle(MOE.Common.Business.ControllerEventLogs DTTB)
        {
             List<PreemptCycle> CycleCollection = new List<PreemptCycle>();
            PreemptCycle cycle = null;

            int x = 0;
            foreach (MOE.Common.Models.Controller_Event_Log row in DTTB.Events)
            {
                x++;
                //every preempt shoudl start with a call
                if (row.EventCode == 102 && cycle == null)
                {
                    cycle = new PreemptCycle();
                    cycle.InputOn = row.Timestamp;

                }

                //this thought isn't finished
                if (row.EventCode == 102 && cycle != null && CycleCollection.Count > 0)
                {
                    CycleCollection.Add(cycle);
                    cycle = new PreemptCycle();
                    cycle.InputOn = row.Timestamp;

                }

                if (row.EventCode == 103 && cycle != null && cycle.GateDown == DateTime.MinValue)
                {

                    cycle.GateDown = row.Timestamp;

                }

                //this is when the preempt call ends.
                if (row.EventCode == 104 && cycle != null)
                {

                    cycle.InputOff = row.Timestamp;

                }
                if (row.EventCode == 105 && cycle != null)
                {

                    cycle.EntryStarted = row.Timestamp;

                }
                if (row.EventCode == 106 && cycle != null)
                {

                    cycle.BeginTrackClearance = row.Timestamp;

                }
                //this is where the phase is actually served
                if (row.EventCode == 107 && cycle != null)
                {

                    cycle.BeginDwellService = row.Timestamp;

                }
                if (row.EventCode == 108 && cycle != null)
                {

                    cycle.LinkActive = row.Timestamp;

                }
                if (row.EventCode == 109 && cycle != null)
                {

                    cycle.LinkInactive = row.Timestamp;

                }
                if (row.EventCode == 110 && cycle != null)
                {

                    cycle.MaxPresenceExceeded = row.Timestamp;

                }
                // 111 can usually be considered "cycle complete"
                if (row.EventCode == 111 && cycle != null)
                {

                    cycle.BeginExitInterval = row.Timestamp;
                    CycleCollection.Add(cycle);
                    cycle = null;                                    


                }
                if (x == DTTB.Events.Count && cycle != null)
                {
                    CycleCollection.Add(cycle);
                    cycle = null;               
                }
               

                //if (row.EventCode == 112 && cycle != null)
                //{

                //    cycle.TSPCheckIn = row.TimeStamp;

                //}
                //if (row.EventCode == 113 && cycle != null)
                //{

                //    cycle.TSPAdjustToEarlyGreen = row.TimeStamp;

                //}
                //if (row.EventCode == 114 && cycle != null)
                //{

                //    cycle.TSPAdjustToExtendGreen = row.TimeStamp;

                //}
                //if (row.EventCode == 115 && cycle != null)
                //{

                //    cycle.TSPCheckout = row.TimeStamp;

                //}
 



            }
             
            return CycleCollection;
        }
    }
}
