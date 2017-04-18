using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MOE.Common.Business.CustomReport
{
    public class Phase : ControllerEventLogs
    {
        public enum Direction
        {
            Northbound,
            Southbound,
            Eastbound,
            Westbound
        }

        public String ApproachDirection
        {
            get;
            set;
        }

        private List<Models.Detector> _Detector;

        public List<Models.Detector> Detector
        {
            get { return _Detector; }
            set { _Detector = value; }
        }

        public bool IsOverlap
        {
            get;
            set;
        }

        private List<DateTime> _DetectorActivations;

        public List<DateTime> DetectorActivations
        {
            get { return _DetectorActivations; }
            set { _DetectorActivations = value; }
        }

        private List<CustomReport.Cycle> _Cycles = new List<Cycle>();

        public List<CustomReport.Cycle> Cycles
        {
            get { return _Cycles; }
            set { _Cycles = value; }
        }

        public string SignalID { get; set; }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        private int _PhaseNumber;

        public int PhaseNumber
        {
            get { return _PhaseNumber; }
            set { _PhaseNumber = value; }
        }

        //private List<List<Models.Controller_Event_Log>> _GraphEvents = 
        //    new List<List<Models.Controller_Event_Log>>();

        //public List<List<Models.Controller_Event_Log>> GraphEvents
        //{
        //    get { return _GraphEvents; }
        //    set { _GraphEvents = value; }
        //}

        //private List<Models.Controller_Event_Log> _GraphEvents;

        //public List<Models.Controller_Event_Log> GraphEvents
        //{
        //    get { return _GraphEvents; }
        //    set { _GraphEvents = value; }
        //}







        public Phase(List<Models.Detector> detectors, int phaseNumber, string signalID,
            DateTime startDate, DateTime endDate, List<int> eventCodes, int StartofCycleEvent)
            : base(signalID, startDate, endDate, phaseNumber, new List<int> { 1, 4, 5, 6, 7, 8, 9, 10, 61, 63, 64 })
        {
            startDate = startDate.AddMinutes(-1);
            endDate = endDate.AddMinutes(+1);

            _Detector = detectors;
            _SignalID = signalID;
            _StartDate = startDate;
            _EndDate = endDate;
            _PhaseNumber = phaseNumber;
            IsOverlap = false;
            SignalID = signalID;
            //ApproachDirection = direction;

            



            GetCycles(StartofCycleEvent);
        }

        private void GetCycles(int StartofCycleEvent)
        {
 

            if (Events.Exists(s => s.EventCode == 1))
            {
                IsOverlap = false;
                
                CustomReport.Cycle.TerminationCause termEvent = new Cycle.TerminationCause();
                termEvent = Cycle.TerminationCause.Unknown;

                for (int i = 0; i < Events.Count; i++)
                {
                    DateTime CycleStart = new DateTime();
                    DateTime changeToGreen = new DateTime();
                    DateTime beginYellowClear = new DateTime();
                    DateTime endYellowClear = new DateTime();
                    DateTime changeToRed = new DateTime();
                    DateTime greenTerm = new DateTime();
                    DateTime cycleEnd = new DateTime();
   
                    if (Events[i].EventCode == StartofCycleEvent)
                    {
                        if (i + 1 >= Events.Count)
                        {
                            break;
                        }
                        CycleStart = Events[i].Timestamp;
                        switch (Events[i].EventCode)
                            {
                                case 1:
                                    changeToGreen = Events[i].Timestamp;
                                    break;
                                case 4:
                                    termEvent = Cycle.TerminationCause.GapOut;
                                    break;
                                case 5:
                                    termEvent = Cycle.TerminationCause.MaxOut;
                                    break;
                                case 6:
                                    termEvent = Cycle.TerminationCause.ForceOff;
                                    break;
                                case 7:
                                    greenTerm = Events[i].Timestamp;
                                    break;
                                case 8:
                                    beginYellowClear = Events[i].Timestamp;
                                    break;
                                case 9:
                                    endYellowClear = Events[i].Timestamp;
                                    changeToRed = Events[i].Timestamp;
                                    break;
                                //case 10:
                                //    changeToRed = Events[i].Timestamp;
                                //    break;
                                }
                        
                        int s = i + 1;
                        
                        
                        while (Events[s].EventCode != StartofCycleEvent && s != Events.Count)
                        {
                            switch (Events[s].EventCode)
                            {
                                case 1:
                                    changeToGreen = Events[s].Timestamp;
                                    break;
                                case 4:
                                    termEvent = Cycle.TerminationCause.GapOut;
                                    break;
                                case 5:
                                    termEvent = Cycle.TerminationCause.MaxOut;
                                    break;
                                case 6:
                                    termEvent = Cycle.TerminationCause.ForceOff;
                                    break;
                                case 7:
                                    greenTerm = Events[s].Timestamp;
                                    break;
                                case 8:
                                    beginYellowClear = Events[s].Timestamp;
                                    break;
                                case 9:
                                    endYellowClear = Events[s].Timestamp;
                                    changeToRed = Events[s].Timestamp;
                                    break;
                                //case 10:
                                //    changeToRed = Events[s].Timestamp;
                                //    break;




                            }
                            s++;
                            if (s >= Events.Count)
                            {
                                i = s;

                                //deal with the very last cycle
                                if(
                                    CycleStart > DateTime.MinValue 
                                   )
                                {
                                    if (changeToGreen == DateTime.MinValue)
                                    { changeToGreen = CycleStart; }

                                    if (beginYellowClear == DateTime.MinValue)
                                    { beginYellowClear = CycleStart.AddSeconds(1); }

                                    if (endYellowClear == DateTime.MinValue)
                                    { endYellowClear = CycleStart.AddSeconds(4); }


                                    if (changeToRed == DateTime.MinValue)
                                    { changeToRed = CycleStart.AddSeconds(5); }


                                    if (Events.Last().EventCode == 1)
                                    {
                                        cycleEnd = Events.Last().Timestamp;
                                    }
                                    else
                                    {
                                        cycleEnd = this.EndDate;
                                    }

                                    CustomReport.Cycle _Cycle = new CustomReport.Cycle(CycleStart, changeToGreen,
                                   beginYellowClear, changeToRed, cycleEnd);
                                    _Cycle.EndYellowClearance = endYellowClear;
                                    _Cycle.TerminationEvent = termEvent;

                                    _Cycles.Add(_Cycle);

                                }
                                break;
                            }
                        }
                        
                        if (s >= Events.Count)
                        {
                            break;
                        }
                        
                        i = s-1;
                        cycleEnd = Events[s].Timestamp;

                       
                        if (
                            CycleStart > DateTime.MinValue &&
                            changeToGreen > DateTime.MinValue &&
                            beginYellowClear > DateTime.MinValue &&
                            changeToRed > DateTime.MinValue 
                            )
                        {
                            CustomReport.Cycle _Cycle = new CustomReport.Cycle(CycleStart, changeToGreen,
                           beginYellowClear, changeToRed, cycleEnd);
                            _Cycle.EndYellowClearance = endYellowClear;
                            _Cycle.TerminationEvent = termEvent;

                            _Cycles.Add(_Cycle);
                        }
                        //else 
                        
                    }



                }
            }



            //if (Events.Exists(s => s.EventCode == 64))
            //{
            //    IsOverlap = true;
            //    for (int i = 0; i < Events.Count - 4; i++)
            //    {
            //        if (Events[i].EventCode == 64 && Events[i + 1].EventCode == 61 &&
            //            Events[i + 2].EventCode == 63 && Events[i + 3].EventCode == 64)
            //        {
            //            if ((Events[i + 3].Timestamp - Events[i].Timestamp).TotalSeconds < 300)
            //            {
            //                _Cycles.Add(new CustomReport.Cycle(Events[i].Timestamp, Events[i + 1].Timestamp,
            //                Events[i + 2].Timestamp, Events[i + 3].Timestamp, Events[i + 3].Timestamp));

            //                i = i + 3;
            //            }
            //        }
            //    }
            //}
        }
    }
}
    


       
    

