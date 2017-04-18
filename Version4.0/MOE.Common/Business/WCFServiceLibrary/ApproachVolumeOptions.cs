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
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class ApproachVolumeOptions: MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [DataMember]
        public List<int> BinSizeList { get; set; }
        [DataMember]
        [Display(Name = "Show Directional Splits")]
        public bool ShowDirectionalSplits { get; set; }
        [DataMember]
        [Display(Name = "Show Total Volume")]
        public bool ShowTotalVolume { get; set; }
        [DataMember]
        [Display(Name = "Show NB/WB Volume")]
        public bool ShowNBWBVolume { get; set; }
        [DataMember]
        [Display(Name = "Show SB/EB Volume")]
        public bool ShowSBEBVolume { get; set; }
        [DataMember]
        [Display(Name = "Show TMC Detection")]
        public bool ShowTMCDetection { get; set; }
        [DataMember]
        [Display(Name = "Show Advance Detection")]
        public bool ShowAdvanceDetection { get; set; }

        public List<MOE.Common.Business.ApproachVolume.MetricInfo> MetricInfoList;

        

        public ApproachVolumeOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, int binSize, bool showDirectionalSplits,
            bool showTotalVolume, bool showNBWBVolume, bool showSBEBVolume, bool showTMCDetection, bool showAdvanceDetection)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;

            SelectedBinSize = binSize;
            ShowTotalVolume = showTotalVolume;
            ShowDirectionalSplits = showDirectionalSplits;
            ShowNBWBVolume = showNBWBVolume;
            ShowSBEBVolume = showSBEBVolume;
            ShowTMCDetection = showTMCDetection;
            ShowAdvanceDetection = showAdvanceDetection;
        }

        public ApproachVolumeOptions()
        {
            
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 7;
            SetDefaults();
        }

        public void SetDefaults()
        {
            YAxisMin = 0;
            YAxisMax = null;
            ShowDirectionalSplits = false;
            ShowTotalVolume = false;
            ShowNBWBVolume = true;
            ShowSBEBVolume = true;
            ShowTMCDetection = true;
            ShowAdvanceDetection = true;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            List<string> returnList = new List<string>();
            MetricInfoList = new List<ApproachVolume.MetricInfo>();          
            MOE.Common.Models.Repositories.ISignalsRepository signalsRepository = 
                MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            var signal = signalsRepository.GetSignalBySignalID(this.SignalID);    
            var volumeApproaches = new List<MOE.Common.Business.ApproachVolume.Approach>(); 
            
            
            foreach(MOE.Common.Models.Approach a in signal.Approaches)
            {
               
                volumeApproaches.Add(new ApproachVolume.Approach(signal.SignalID, a.DirectionType.Description));
            }
            if (volumeApproaches.Count > 0)
            {
               // Match Phase With Direction 
                int NBPhase = 0;
                int SBPhase = 0;
                int EBPhase = 0;
                int WBPhase = 0;

                NBPhase = (from k in volumeApproaches
                           where string.Compare(k.Direction, "Northbound", true) == 0
                           select k).Count();

                SBPhase = (from k in volumeApproaches
                           where string.Compare(k.Direction, "Southbound", true) == 0
                           select k).Count();

                EBPhase = (from k in volumeApproaches
                           where string.Compare(k.Direction, "Eastbound", true) == 0
                           select k).Count();

                WBPhase = (from k in volumeApproaches
                           where string.Compare(k.Direction, "Westbound", true) == 0
                           select k).Count();

                string location = GetSignalLocation();

                if (NBPhase > 0 | SBPhase > 0)
                {
                    if (ShowAdvanceDetection)
                    {
                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(
                                StartDate, EndDate, SignalID, location, "Northbound", "Southbound", this,
                                volumeApproaches, true);
                        


                        string chartName = CreateFileName();


                        //Save an image of the chart
                        AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                        

                        AVC.info.ImageLocation = (MetricWebPath + chartName);
                        MetricInfoList.Add(AVC.info);


                       // PlaceApproachVolumeMetrics(AVC);
                    }
                    if (ShowTMCDetection)
                    {
                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(StartDate, EndDate, SignalID,
                                location, "Northbound", "Southbound", this, volumeApproaches, false);

                        string chartName = CreateFileName();


                        //Save an image of the chart
                        AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                        AVC.info.ImageLocation = (MetricWebPath + chartName);
                        MetricInfoList.Add(AVC.info);

                       
                    }



                }

                if (EBPhase > 0 | WBPhase > 0)
                {
                    if (ShowAdvanceDetection)
                    {
                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(StartDate, EndDate,
                                SignalID, location, "Westbound", "Eastbound", this, volumeApproaches, true);

                        string chartName = CreateFileName();


                        //Save an image of the chart
                        AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                        AVC.info.ImageLocation = (MetricWebPath + chartName);
                        MetricInfoList.Add(AVC.info);

                       
                    }
                    if (ShowTMCDetection)
                    {
                        MOE.Common.Business.ApproachVolume.ApproachVolumeChart AVC = 
                            new MOE.Common.Business.ApproachVolume.ApproachVolumeChart(StartDate, EndDate,
                                SignalID, location, "Westbound", "Eastbound", this, volumeApproaches,false);

                        string chartName = CreateFileName();


                        //Save an image of the chart
                        AVC.Chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);


                        AVC.info.ImageLocation = (MetricWebPath + chartName);
                        MetricInfoList.Add(AVC.info);

                     
                    }
                }

            }
            return returnList;
        }



       
    }
}
