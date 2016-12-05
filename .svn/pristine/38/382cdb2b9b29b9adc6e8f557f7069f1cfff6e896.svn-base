using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOE.Common;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using Microsoft.AspNet.Identity;
using MOE.Common.Models;
using System.Data.Entity.Validation;
using System.Configuration;




namespace SPMWatchDogNew
{
    class Program
    {
        static public ConcurrentBag<MOE.Common.Models.Signal> signalsWithRecords = 
            new ConcurrentBag<MOE.Common.Models.Signal>();
        static public ConcurrentBag<MOE.Common.Models.Signal> signalsNoRecords = 
            new ConcurrentBag<MOE.Common.Models.Signal>();
        static public ConcurrentBag<MOE.Common.Models.Signal> errorSignals = 
            new ConcurrentBag<MOE.Common.Models.Signal>();
        static public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> ForceOffErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        static public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> MaxOutErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        static public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> LowHitCountErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        static public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> MissingRecords = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        static public ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> StuckPedErrors = 
            new ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent>();
        static public int consecutiveCount =0;
        static public int minPhaseTerminations = 0;
        static public double percentThreshold = 0;
        static public MOE.Common.Models.Repositories.ISPMWatchDogErrorEventRepository er;
        static public List<SPMWatchDogErrorEvent> RecordsFromTheDayBefore = new List<SPMWatchDogErrorEvent>();
        static public DateTime StartTime { get; set; }
        static public DateTime EndTime { get; set; }
        static public DateTime AnalysisStart { get; set; }
        static public DateTime AnalysisEnd { get; set; }


        static void Main(string[] args)
        {

            //try
            //{
             consecutiveCount = Convert.ToInt32(ConfigurationManager.AppSettings["ConsecutiveCount"]);
             minPhaseTerminations = Convert.ToInt32(ConfigurationManager.AppSettings["MinPhaseTerminations"]);
            percentThreshold = Convert.ToDouble(ConfigurationManager.AppSettings["PercentThreshold"]);
            er = MOE.Common.Models.Repositories.SPMWatchDogErrorEventRepositoryFactory.Create();
            MOE.Common.Models.Repositories.ISignalsRepository sr = 
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signals = sr.EagerLoadAllSignals();
 
                ParallelOptions options = new ParallelOptions();
                options.MaxDegreeOfParallelism = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreads"]);

                StartTime = DateTime.Today;
                EndTime = DateTime.Today;


                // find the analysis timespan
                if (args.Length > 0)
                {

                    try
                    {
                        StartTime = DateTime.Parse(args[0]);
                        EndTime = StartTime;
                    }
                    catch
                    {
                        Console.WriteLine("The Arguments were invalid.");
                    }
                }
                RecordsFromTheDayBefore =
                er.GetSPMWatchDogErrorEventsBetweenDates(StartTime.AddDays(-1), StartTime.AddMinutes(-1));



                int endHourInt = 0;
                int startHourInt = 0;

                if (Convert.ToInt32(ConfigurationManager.AppSettings["StartHour"]) > -1 && Convert.ToInt32(ConfigurationManager.AppSettings["StartHour"]) < 24)
                {
                    startHourInt = Convert.ToInt32(ConfigurationManager.AppSettings["StartHour"]);
                }
                else
                {
                    startHourInt = 1;
                }

                if (Convert.ToInt32(ConfigurationManager.AppSettings["EndHour"]) > -1 && Convert.ToInt32(ConfigurationManager.AppSettings["EndHour"]) < 24
                    && Convert.ToInt32(ConfigurationManager.AppSettings["EndHour"]) > Convert.ToInt32(ConfigurationManager.AppSettings["StartHour"]))
                {
                    endHourInt = Convert.ToInt32(ConfigurationManager.AppSettings["EndHour"]);
                }
                else
                {
                    endHourInt = 5;
                }



                TimeSpan startHour = new TimeSpan(startHourInt, 0, 0);
                TimeSpan endHour = new TimeSpan(endHourInt, 0, 0);

                AnalysisStart = StartTime.Date + startHour;
                AnalysisEnd = EndTime.Date + endHour;





                Parallel.ForEach(signals, options, signal =>
                {
                    MOE.Common.Models.Repositories.IControllerEventLogRepository CELR =
                        MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();


                    List<MOE.Common.Models.Controller_Event_Log> CEDT = 
                        CELR.GetTop1000SignalEventsBetweenDates(signal.SignalID, StartTime.AddDays(-1), StartTime);


                    if (CEDT.Count > 500)
                    {
                        Console.WriteLine("Signal " + signal.SignalID + "Has Current records");
                        signalsWithRecords.Add(signal);
                        CEDT = null;

                    }
                    else
                    {
                        List<MOE.Common.Models.Controller_Event_Log> YCEDT = 
                            CELR.GetTop1000SignalEventsBetweenDates(signal.SignalID, StartTime.AddDays(-2), StartTime.AddDays(-1));
                        //if (YCEDT.Count > 500)
                        //{
                            Console.WriteLine("Signal " + signal.SignalID + "Does Not Have Current records");
                            signalsNoRecords.Add(signal);
                            //ErrorRecord error = new ErrorRecord(signal.SignalID, signal.PrimaryName, signal.SecondaryName,
                            //       "0", 0, 0, "0", 0, "Missing Records");

                            MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();

                            error.SignalID = signal.SignalID;
                            error.DetectorID = "0";
                            error.Phase = 0;
                            error.Direction = "";
                            error.TimeStamp = StartTime;
                            error.Message = "Missing Records";
                            error.ErrorCode = 1;

                            MissingRecords.Add(error);
                            YCEDT = null;
                        }
                    //}


                }
               );



          
                Parallel.ForEach(signalsWithRecords, options, signal =>
                //foreach(var signal in signalsWithRecords)
                {
                    MOE.Common.Business.AnalysisPhaseCollection APcollection = 
                        new MOE.Common.Business.AnalysisPhaseCollection(signal.SignalID, 
                            AnalysisStart, AnalysisEnd, consecutiveCount);

                    foreach (MOE.Common.Business.AnalysisPhase phase in APcollection.Items)
                    //Parallel.ForEach(APcollection.Items, options,phase =>
                    {
                            CheckForMaxOut(phase, signal);
                            CheckForForceOff(phase, signal);
                            CheckForStuckPed(phase, signal);
                    }

                    // );

                    //Look for low hits on a PCD detector
                    CheckForLowDetectorHits(signal);



                }
                );

                CreateAndSendEmail();
            //}
            //catch (Exception ex)
            //{
            //    MOE.Common.Models.Repositories.IApplicationEventRepository er = 
            //        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();

            //    er.QuickAdd("SPMWatchDog", "MainLoop", "MainLoop", MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
            //    throw;
            //}
        
        }

        private static void CreateAndSendEmail()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            MOE.Common.Models.SPM db = new MOE.Common.Models.SPM();
            var userStore = new UserStore<SPMUser>(db);
            var userManager = new UserManager<SPMUser>(userStore);

            List<SPMUser> users = (from u in userManager.Users
                                   where u.RecieveAlerts == true
                                   select u).ToList();



            foreach (SPMUser user in users)
            {
                message.To.Add(user.Email);
            }


            message.To.Add(ConfigurationManager.AppSettings["ToAddress"]);

    
            message.Subject = "ATSPM Alerts for " + StartTime.ToShortDateString();
            message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["FromAddress"]);

            string missingErrors = SortAndAddToMessage(MissingRecords);
            string forceErrors = SortAndAddToMessage(ForceOffErrors);
            string maxErrors = SortAndAddToMessage(MaxOutErrors);
            string countErrors = SortAndAddToMessage(LowHitCountErrors);
            string stuckpedErrors = SortAndAddToMessage(StuckPedErrors);
            

            if (MissingRecords.Count > 0 && missingErrors != "")
            {
                message.Body += " \n --The following signals had too few records in the database on " + 
                    StartTime.AddDays(-1).Date.ToShortDateString() + ": \n";
                message.Body += missingErrors;
            }
            else
            {

                message.Body += "\n --No new missing record errors were found on " +
                    StartTime.AddDays(-1).ToShortDateString() + ". \n";
            }

            if (ForceOffErrors.Count > 0 && forceErrors != "")
            {
                message.Body += " \n --The following signals had too many force off occurrences between " +
                ConfigurationManager.AppSettings["StartHour"].ToString() + ":00 and " +
                ConfigurationManager.AppSettings["EndHour"].ToString() + ":00: \n";
                message.Body += forceErrors;
            }
            else
            {

                message.Body += "\n --No new force off errors were found between " +
                ConfigurationManager.AppSettings["StartHour"].ToString() + ":00 and " +
                ConfigurationManager.AppSettings["EndHour"].ToString() + ":00. \n";
            }

            if (MaxOutErrors.Count > 0 && maxErrors != "")
            {
                message.Body += " \n --The following signals had too many max out occurrences between " +
                ConfigurationManager.AppSettings["StartHour"].ToString() + ":00 and " +
                ConfigurationManager.AppSettings["EndHour"].ToString() + ":00: \n";
                message.Body += maxErrors;
            }
            else
            {

                message.Body += "\n --No new max out errors were found between " +
                ConfigurationManager.AppSettings["StartHour"].ToString() + ":00 and " +
                ConfigurationManager.AppSettings["EndHour"].ToString() + ":00. \n";
            }

            if (LowHitCountErrors.Count > 0 && countErrors != "")
            {
                message.Body += " \n --The following signals had unusually low advanced detection counts on " +
                     StartTime.AddDays(-1).ToShortDateString() + " between " +
                ConfigurationManager.AppSettings["PreviousDayPMPeakStart"].ToString() + ":00 and " +
                ConfigurationManager.AppSettings["PreviousDayPMPeakEnd"].ToString() + ":00: \n";
                message.Body += countErrors;
            }
            else
            {
                message.Body += "\n --No new low advanced detection count errors on " +
                     StartTime.AddDays(-1).ToShortDateString() + " between " +
                ConfigurationManager.AppSettings["PreviousDayPMPeakStart"].ToString() + ":00 and " +
                ConfigurationManager.AppSettings["PreviousDayPMPeakEnd"].ToString() + ":00. \n";
            }
            if (StuckPedErrors.Count > 0 && stuckpedErrors != "")
            {
                message.Body += " \n --The following signals have high pedestrian activation occurrences between " +
                ConfigurationManager.AppSettings["StartHour"].ToString() + ":00 and " +
                ConfigurationManager.AppSettings["EndHour"].ToString() + ":00: \n";
                message.Body += stuckpedErrors;
            }
            else
            {

                message.Body += "\n --No new high pedestrian activation errors between " +
                ConfigurationManager.AppSettings["StartHour"] + ":00 and " +
                ConfigurationManager.AppSettings["EndHour"] + ":00. \n";
            }

            SendMessage(message);
        }

        private static void CheckForLowDetectorHits(MOE.Common.Models.Signal signal)
        {
            List<MOE.Common.Models.Detector> detectors = signal.GetDetectorsForSignalThatSupportAMetric(6);

            //Parallel.ForEach(detectors, options, detector =>
            foreach (MOE.Common.Models.Detector detector in detectors)
            {
                try
                {
                    int channel = detector.DetChannel;
                    string direction = detector.Approach.DirectionType.Description;
                    int PreviousDayPMPeakStart = 
                        Convert.ToInt32(ConfigurationManager.AppSettings["PreviousDayPMPeakStart"]);
                    DateTime start = StartTime.AddDays(-1).Date.AddHours(PreviousDayPMPeakStart);
                    int PreviousDayPMPeakEnd =
                        Convert.ToInt32(ConfigurationManager.AppSettings["PreviousDayPMPeakEnd"]);
                    DateTime end = StartTime.AddDays(-1).Date.AddHours(PreviousDayPMPeakEnd);
                    int currentVolume = detector.GetVolumeForPeriod(start, end);
                    //Compare collected hits to low hit threshold, 
                    if (currentVolume < Convert.ToInt32(ConfigurationManager.AppSettings["LowHitThreshold"]))
                    {
                        MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();

                        error.SignalID = signal.SignalID;
                        error.DetectorID = detector.DetectorID;
                        error.Phase = detector.Approach.ProtectedPhaseNumber;
                        error.TimeStamp = StartTime;
                        error.Direction = detector.Approach.DirectionType.Description;
                        error.Message = "Count: "+ currentVolume.ToString();
                        error.ErrorCode = 2;

                        if (!LowHitCountErrors.Contains(error))
                        {
                            LowHitCountErrors.Add(error);
                        }
                    }
                }

                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository er =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();

                    er.QuickAdd("SPMWatchDog", "Program", "CheckForLowDetectorHits", 
                        MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, detector.DetectorID + "-" + ex.Message);
                }
            }
            //);
        }

        private static void CheckForStuckPed(MOE.Common.Business.AnalysisPhase phase,  MOE.Common.Models.Signal signal)
        {
            if (phase.PedestrianEvents.Count > 200)
            {
                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();

                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = StartTime;
                error.Direction = phase.Direction??"";
                error.Message = phase.PedestrianEvents.Count.ToString() +
                        " Pedestrian Activations";
                error.ErrorCode = 3;


                if (!StuckPedErrors.Contains(error))
                {
                    Console.WriteLine("Signal " + signal.SignalID + phase.PedestrianEvents.Count.ToString() + 
                        " Pedestrian Activations");
                    StuckPedErrors.Add(error);
                }
            }
        }

        private static void CheckForForceOff(MOE.Common.Business.AnalysisPhase phase, MOE.Common.Models.Signal signal)
        {
            if (phase.PercentForceOffs > percentThreshold && phase.TotalPhaseTerminations > minPhaseTerminations)
            {

                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();

                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = StartTime;
                error.Direction = phase.Direction??"";
                error.Message = "Force Offs " + Math.Round(phase.PercentForceOffs * 100, 1).ToString() + "%";
                error.ErrorCode = 4;


                if (!ForceOffErrors.Contains(error))
                {
                    ForceOffErrors.Add(error);
                }

            }
        }

        private static void CheckForMaxOut(MOE.Common.Business.AnalysisPhase phase,
             MOE.Common.Models.Signal signal)
        {
            if (phase.PercentMaxOuts > percentThreshold && phase.TotalPhaseTerminations > minPhaseTerminations)
            {
                MOE.Common.Models.SPMWatchDogErrorEvent error = new MOE.Common.Models.SPMWatchDogErrorEvent();
                error.SignalID = signal.SignalID;
                error.Phase = phase.PhaseNumber;
                error.TimeStamp = StartTime;
                error.Direction = phase.Direction??"";
                error.Message = "Max Outs " + Math.Round(phase.PercentMaxOuts * 100, 1).ToString() + "%";
                error.ErrorCode = 5;
               
                    if (MaxOutErrors.Count == 0 || !MaxOutErrors.Contains(error))
                    {
                        Console.WriteLine("Signal " + signal.SignalID + "Has MaxOut Errors");
                        MaxOutErrors.Add(error);
                    }
                

            }
        }

        static private void SendMessage( System.Net.Mail.MailMessage message)
        {
            MOE.Common.Models.Repositories.IApplicationEventRepository er =
                                MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["EmailServer"]);
            try
            {
            Console.WriteLine("Sent message to: " + message.To.ToString() + "\nMessage text: " + message.Body + "\n");
                smtp.Send(message);
                System.Threading.Thread.Sleep(5000);
                er.QuickAdd("SPMWatchDog", "Program", "SendMessage",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.Information, 
                    "Email Sent Successfully to: " + message.To.ToString());
            }
            catch(Exception ex)
            {                            
                er.QuickAdd("SPMWatchDog", "Program", "SendMessage",
                    MOE.Common.Models.ApplicationEvent.SeverityLevels.Medium, ex.Message);
            }
        }

        static private string SortAndAddToMessage(ConcurrentBag<MOE.Common.Models.SPMWatchDogErrorEvent> errors)
        {
            List<MOE.Common.Models.SPMWatchDogErrorEvent> SortedErrors = 
                errors.OrderBy(x => x.SignalID).ThenBy(x => x.Phase).ToList();

                string ErrorMessage = "";

                foreach (MOE.Common.Models.SPMWatchDogErrorEvent error in SortedErrors)
                {

                    //compare to error log to see if this was failing yesterday

                    if (FindMatchingErrorInErrorTable(error) == false)
                    {

                        MOE.Common.Models.Repositories.ISignalsRepository signalRepository =
                            MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
                        var signal = signalRepository.GetSignalBySignalID(error.SignalID);
                        //   Add to email if it was not failing yesterday
                        ErrorMessage += error.SignalID.ToString();
                        ErrorMessage += " - ";
                        ErrorMessage += signal.PrimaryName;
                        ErrorMessage += " & ";
                        ErrorMessage += signal.SecondaryName;
                        if (error.Phase > 0)
                        {
                            ErrorMessage += " - Phase ";                        
                            ErrorMessage += error.Phase;
                        }
                        ErrorMessage += " (" + error.Message + ")";
                        ErrorMessage += "\n";
                        //}

                    }
                }
                    try
                    {
                        er.AddList(errors.ToList());

                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }

                return ErrorMessage;
                    
            }
    
        static private int FindChannel(string SignalID, int Phase)
        {
            
            MOE.Common.Models.Repositories.ISignalsRepository smh = MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            MOE.Common.Models.Signal sig = smh.GetSignalBySignalID(SignalID);

            var dets = sig.GetDetectorsForSignalByPhaseNumber(Phase);

            if (dets.Count() > 0)
            {
                return dets.FirstOrDefault().DetChannel;
            }
            else
            {
                return 0;
            }

        }



        static private bool FindMatchingErrorInErrorTable(SPMWatchDogErrorEvent error)
        {
            var MatchingRecord = (from r in RecordsFromTheDayBefore
                                 where error.SignalID == r.SignalID
                                 && error.DetectorID == r.DetectorID
                                 && error.ErrorCode == r.ErrorCode
                                 && error.Phase == r.Phase
                                 select r).FirstOrDefault();

            if(MatchingRecord != null)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        static private string FindDetector(MOE.Common.Models.Signal Signal, int Channel)
        {


            try
            {
                MOE.Common.Models.Detector gd = Signal.GetDetectorForSignalByChannel(Channel);

                if (gd != null)
                {
                    return gd.DetectorID;
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }
        }

        }


}

    


