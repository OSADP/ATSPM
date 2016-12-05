using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class SplitFailOptions : MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "First Seconds Of Red")]
        public int FirstSecondsOfRed { get; set; }
        [DataMember]
        [Display(Name = "Show Fail Lines")]
        public bool ShowFailLines { get; set; }
        [DataMember]
        [Display(Name = "Show Average Lines")]
        public bool ShowAvgLines { get; set; }
        [DataMember]
        [Display(Name = "Show Percent Fail Lines")]
        public bool ShowPercentFailLines { get; set; }


        public SplitFailOptions(string signalID, DateTime startDate, DateTime endDate,
            int metricTypeID, int firstSecondsOfRed, bool showFailLines, bool showAvgLines, bool showPercentFailLine)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;
            MetricTypeID = metricTypeID;
            FirstSecondsOfRed = firstSecondsOfRed;
            ShowFailLines = showFailLines;
            ShowAvgLines = showAvgLines;
            ShowPercentFailLines = showPercentFailLine;
        }
        public SplitFailOptions()
        {
            MetricTypeID = 12;
            SetDefaults();
        }
        public void SetDefaults()
        {
            FirstSecondsOfRed = 5;
            ShowFailLines = true;
            ShowAvgLines = true;
            ShowPercentFailLines = false;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnString = new List<string>();

            MOE.Common.Models.Repositories.IDetectorRepository gdr = MOE.Common.Models.Repositories.DetectorRepositoryFactory.Create();
            List<MOE.Common.Models.Detector> detectors = gdr.GetDetectorsBySignalIDAndMetricType(SignalID, 12);

            int maxPhase = 0;
            foreach (MOE.Common.Models.Detector d in detectors)
            {
                if (d.Approach.ProtectedPhaseNumber > maxPhase)
                {
                    maxPhase = d.Approach.ProtectedPhaseNumber;
                }
            }

            for (int x = 1; x < maxPhase + 1; x++)
            {
                var phaseExists = from row in detectors
                                  where (row.Approach.ProtectedPhaseNumber == x)
                                  select row;
                if (phaseExists.Count() > 0)
                {
                    MOE.Common.Business.CustomReport.Phase phase = new MOE.Common.Business.CustomReport.Phase(detectors, x, SignalID, StartDate, EndDate, new List<int> { 1, 4, 5, 6, 7, 8, 9, 10, 61, 63, 64 }, 1);

                    phase.ApproachDirection = phaseExists.FirstOrDefault().Approach.DirectionType.Description;

                    string location = GetSignalLocation();
                    string chartName = CreateFileName();

                    MOE.Common.Business.SplitFail.SplitFailChart sfChart = new MOE.Common.Business.SplitFail.SplitFailChart(StartDate, EndDate, phase, location, FirstSecondsOfRed, 
                        ShowFailLines, ShowAvgLines, ShowPercentFailLines, YAxisMax);
                   

                    System.Threading.Thread.Sleep(300);

                    chartName = chartName.Replace(".",  (phase.ApproachDirection + ".")) ;
                    //System.IO.FileStream stream = new System.IO.FileStream((MetricFileLocation + chartName), System.IO.FileAccess = System.IO.FileAccess.Write);
                    try
                    {
                    sfChart.chart.SaveImage(MetricFileLocation +  chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                    }
                    catch
                    {
                        try{
                            sfChart.chart.SaveImage(MetricFileLocation +  chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);
                        }
                        catch
                        {
                            Models.Repositories.IApplicationEventRepository appEventRepository =
                             Models.Repositories.ApplicationEventRepositoryFactory.Create();
                            Models.ApplicationEvent applicationEvent = new ApplicationEvent();
                            applicationEvent.ApplicationName = "SPM Website";
                            applicationEvent.Description = MetricType.ChartName + " Failed While Saving File";
                            applicationEvent.SeverityLevel = ApplicationEvent.SeverityLevels.Medium;
                            applicationEvent.Timestamp = DateTime.Now;
                            appEventRepository.Add(applicationEvent);
                        }
                    }

                    // sfChart.chart.SaveImage(@"C:\temp\sfchart.jpeg" + x.ToString(), System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                    returnString.Add(MetricWebPath + chartName);
                }
            }
            return returnString;
        }
    }
}
