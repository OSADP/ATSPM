using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOE.Common.Business
{
    public class AnalysisPhase
    {

        private int phaseNumber;
        public int PhaseNumber
        {
            get
            {
                return phaseNumber;
            }
        }



        private string signalId;
        public string SignalID
        {
            get
            {
                return signalId;
            }
        }

        private double percentMaxOuts;
        public double PercentMaxOuts
        {
            get
            {
                return percentMaxOuts;
            }
        }

        private double percentForceOffs;
        public double PercentForceOffs
        {
            get
            {
                return percentForceOffs;
            }
        }

        private int totalPhaseTerminations;
        public int TotalPhaseTerminations
        {
            get
            {
                return totalPhaseTerminations;
            }
        }

        public string Direction { get; set; }
        public bool IsOverlap { get; set; }

        public List<Business.ControllerEvent> PedestrianEvents = new List<ControllerEvent>();
        public List<Business.ControllerEvent> TerminationEvents = new List<ControllerEvent>();
        public List<Business.ControllerEvent> ConsecutiveGapOuts = new List<ControllerEvent>();
        public List<Business.ControllerEvent> ConsecutiveMaxOut = new List<ControllerEvent>();
        public List<Business.ControllerEvent> ConsecutiveForceOff = new List<ControllerEvent>();
        public List<Business.ControllerEvent> UnknownTermination = new List<ControllerEvent>();
        public Business.AnalysisPhaseCycleCollection Cycles;

        public List<Models.Controller_Event_Log> FindTerminationEvents(MOE.Common.Business.ControllerEventLogs terminationeventstable, int phasenumber)
        {
            List<Models.Controller_Event_Log> events = (from row in terminationeventstable.Events
                          where row.EventParam == phasenumber && (row.EventCode == 4 || 
                          row.EventCode == 5 || row.EventCode == 6)
                          orderby row.Timestamp
                          select row).ToList();

                        return events;
        }

        public List<Models.Controller_Event_Log> FindPedEvents(MOE.Common.Business.ControllerEventLogs terminationeventstable, int phasenumber)
        {
            List<Models.Controller_Event_Log> events = (from row in terminationeventstable.Events
                         where row.EventParam == phasenumber && (row.EventCode == 21 || row.EventCode == 23)
                          orderby row.Timestamp
                          select row).ToList();

            return events;
        }

        public List<Models.Controller_Event_Log> FindPhaseEvents(MOE.Common.Business.ControllerEventLogs PhaseEventsTable, int PhaseNumber)
        {
            List<Models.Controller_Event_Log> events = (from row in PhaseEventsTable.Events
                                                        where row.EventParam == PhaseNumber
                                                        orderby row.Timestamp
                                                        select row).ToList();

            return events;
        }
        
        /// <summary>
        /// Constructor used for Phase Termination Chart
        /// </summary>
        /// <param name="phasenumber"></param>
        /// <param name="terminationeventstable"></param>
        /// <param name="consecutiveCount"></param>
        public AnalysisPhase(int phasenumber, MOE.Common.Business.ControllerEventLogs terminationeventstable, int consecutiveCount)
        {

            this.phaseNumber = phasenumber;
            var termRow = FindTerminationEvents(terminationeventstable, phaseNumber);

            var pedRow = FindPedEvents(terminationeventstable, phaseNumber);

            foreach (MOE.Common.Models.Controller_Event_Log row in termRow)
            {
                Business.ControllerEvent tEvent = new ControllerEvent(row.Timestamp, row.EventCode);
                TerminationEvents.Add(tEvent);
            }


            foreach (MOE.Common.Models.Controller_Event_Log row in pedRow)
            {
                Business.ControllerEvent tEvent = new ControllerEvent(row.Timestamp, row.EventCode);
                PedestrianEvents.Add(tEvent);
            }



            ConsecutiveGapOuts = FindConsecutiveEvents(TerminationEvents, 4, consecutiveCount);
            ConsecutiveMaxOut = FindConsecutiveEvents(TerminationEvents, 5, consecutiveCount);
            ConsecutiveForceOff = FindConsecutiveEvents(TerminationEvents, 6, consecutiveCount);
            UnknownTermination = FindConsecutiveEvents(TerminationEvents, 7, consecutiveCount);
            percentMaxOuts = FindPercentageConsecutiveEvents(TerminationEvents, 5, consecutiveCount);
            percentForceOffs = FindPercentageConsecutiveEvents(TerminationEvents, 6, consecutiveCount);
            totalPhaseTerminations = TerminationEvents.Count;
    
        }

        
        /// <summary>
        /// Constructor Used for Split monitor
        /// </summary>
        /// <param name="phasenumber"></param>
        /// <param name="signalID"></param>
        /// <param name="CycleEventsTable"></param>
        public AnalysisPhase(int phasenumber, string signalID, MOE.Common.Business.ControllerEventLogs CycleEventsTable)
        {
            MOE.Common.Models.Repositories.ISignalsRepository repository =
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = repository.GetSignalBySignalID(signalID);     
            this.phaseNumber = phasenumber;
            this.signalId = signalID;
            this.IsOverlap = false;
            List<Models.Controller_Event_Log> PedEvents = FindPedEvents(CycleEventsTable, phasenumber);
            List<Models.Controller_Event_Log> PhaseEvents = FindPhaseEvents(CycleEventsTable, phasenumber);
            Cycles = new AnalysisPhaseCycleCollection(phasenumber, signalId, PhaseEvents, PedEvents);
            Models.Approach approach = signal.Approaches.Where(a => a.ProtectedPhaseNumber == phasenumber).FirstOrDefault();
            if (approach != null)
            {
                this.Direction = approach.DirectionType.Description;
            }
            else
            {
                this.Direction = "Unknown";
            }            
        }

        private List<Business.ControllerEvent> FindConsecutiveEvents(List<Business.ControllerEvent> terminationEvents, 
            int eventtype, int consecutiveCount)
        {
            List<Business.ControllerEvent> ConsecutiveEvents = new List<ControllerEvent>();
            int runningConsecCount = 0;
            // Order the events by datestamp
            var eventsInOrder = terminationEvents.OrderBy(TerminationEvent => TerminationEvent.TimeStamp);
            foreach (Business.ControllerEvent termEvent in eventsInOrder)
            {
                if (termEvent.EventCode == eventtype)
                {
                    runningConsecCount++;
                }
                else
                {
                    runningConsecCount = 0;
                }

                if (runningConsecCount >= consecutiveCount)
                {
                    ConsecutiveEvents.Add(termEvent);
                }
            }
            return ConsecutiveEvents;
        }


        private double FindPercentageConsecutiveEvents(List<Business.ControllerEvent> terminationEvents, int eventtype, 
            int consecutiveCount)
        {
            double percentile = 0;
            double total = terminationEvents.Count();
            //Get all termination events of the event type
            int terminationEventsOfType = terminationEvents.Where(
                TerminationEvent => TerminationEvent.EventCode == eventtype).Count();

            if (terminationEvents.Count() > 0)
            {
                percentile= terminationEventsOfType / total;
            }
            return percentile;
        }
    }
}