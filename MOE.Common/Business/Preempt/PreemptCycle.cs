using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business.Preempt
{
    public class PreemptCycle
    {
       // public enum CycleState { InputOn, GateDown, InputOff, BeginTrackClearance, EntryStarted  };
        
        public DateTime WarningInput { get; set; }
        public DateTime InputOn { get; set; }
        public DateTime InputOff { get; set; }
        public DateTime GateDown { get; set; }
        public DateTime EntryStarted { get; set; }
        public DateTime BeginTrackClearance { get; set; }
        public DateTime BeginDwellService { get; set; }
        public DateTime LinkActive { get; set; }
        public DateTime LinkInactive { get; set; }
        public DateTime MaxPresenceExceeded { get; set; }
        public DateTime BeginExitInterval { get; set; }
        public DateTime TSPCheckIn { get; set; }
        public DateTime TSPAdjustToEarlyGreen { get; set; }
        public DateTime TSPAdjustToExtendGreen { get; set; }
        public DateTime TSPCheckout { get; set; }

      
        public Double TimeToService{
            get{


                if (BeginDwellService > DateTime.MinValue && InputOn > DateTime.MinValue && BeginDwellService > InputOn)
                {
                    return (BeginExitInterval - InputOn).TotalSeconds;
                }
                else
                {
                    return 0;
                }
               }
        }

        public Double DwellTime
        {
            get
            {
                if (BeginExitInterval > DateTime.MinValue && BeginDwellService > DateTime.MinValue && BeginExitInterval > BeginDwellService)
                {
                    return (BeginExitInterval - BeginDwellService).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }
        


        public Double TimeToEndCall
        {
            get
            {

                if (InputOff > DateTime.MinValue && InputOn > DateTime.MinValue && InputOff > InputOn)
                {
                    return (InputOff - InputOn).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Double TimeToCallMaxOut
        {
            get
            {

                if (InputOff > DateTime.MinValue && MaxPresenceExceeded > DateTime.MinValue && MaxPresenceExceeded > InputOn)
                {
                    return (MaxPresenceExceeded - InputOn).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }


        public Double TimeToEndOfEntryDelay
        {
            get
            {

                if (InputOn > DateTime.MinValue && EntryStarted > DateTime.MinValue && EntryStarted > InputOn)
                {
                    return (EntryStarted - InputOn).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Double TimeToTrackClear
        {
            get
            {

                if (InputOn > DateTime.MinValue && BeginTrackClearance > DateTime.MinValue && BeginTrackClearance > InputOn)
                {
                    return (BeginTrackClearance - InputOn).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Double TimeToGateDown
        {
            get
            {

                if (InputOn > DateTime.MinValue && GateDown > DateTime.MinValue && GateDown > InputOn)
                {
                    return (GateDown - InputOn).TotalSeconds;
                }
                else
                {
                    return 0;
                }
            }
        }
        
    }
}
