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
    public class TMCOptions: MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [DataMember]
        public List<int> BinSizeList { get; set; }

        [DataMember]
        [Display(Name = "Show Lane Volume")]
        public bool ShowLaneVolumes { get; set; }
        [DataMember]
        [Display(Name = "Show Total Volume")]
        public bool ShowTotalVolumes { get; set; }

        public TMCOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double y2AxisMax,
            int binSize, bool showPlanStatistics, bool showVolumes, int metricTypeID, bool showLaneVolumes, bool showTotalVolumes)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            SelectedBinSize = binSize;
            MetricTypeID = metricTypeID;
            ShowLaneVolumes = showLaneVolumes;
            ShowTotalVolumes = showTotalVolumes;
        }
        public TMCOptions()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            MetricTypeID = 5;
            SetDefaults();
        }
        public void SetDefaults()
        {
            Y2AxisMax = 300;
            YAxisMax = 1000;
            ShowLaneVolumes = true;
            ShowTotalVolumes = true;
        }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();

            MOE.Common.Models.Repositories.ISignalsRepository repository =
            MOE.Common.Models.Repositories.SignalsRepositoryFactory.Create();
            Common.Models.Signal signal = repository.GetSignalBySignalID(SignalID);


            MOE.Common.Business.PlanCollection plans = new MOE.Common.Business.PlanCollection(StartDate, EndDate, SignalID);


            MOE.Common.Models.Repositories.ILaneTypeRepository ltr = MOE.Common.Models.Repositories.LaneTypeRepositoryFactory.Create();
            List<Common.Models.LaneType> laneTypes = ltr.GetAllLaneTypes();

            MOE.Common.Models.Repositories.IMovementTypeRepository mtr = MOE.Common.Models.Repositories.MovementTypeRepositoryFactory.Create();
            List<Common.Models.MovementType> movementTypes = mtr.GetAllMovementTypes();

            MOE.Common.Models.Repositories.IDirectionTypeRepository dtr = MOE.Common.Models.Repositories.DirectionTypeRepositoryFactory.Create();
            List<Common.Models.DirectionType> directions = dtr.GetAllDirections();

            CreateLaneTypeCharts(signal, "Vehicle", laneTypes, movementTypes, directions, plans);
            CreateLaneTypeCharts(signal, "Exit", laneTypes, movementTypes, directions, plans);
            CreateLaneTypeCharts(signal, "Bike", laneTypes, movementTypes, directions, plans);


            return ReturnList;
            
        }

        private void CreateLaneTypeCharts(Common.Models.Signal signal, string laneTypeDescription, 
            List<Common.Models.LaneType> laneTypes, List<Common.Models.MovementType> movementTypes, 
            List<Common.Models.DirectionType> directions, MOE.Common.Business.PlanCollection plans)
        {
            foreach (Common.Models.DirectionType direction in directions)
            {
                 List<Models.Approach> approaches = (from r in signal.Approaches
                              where r.DirectionType.DirectionTypeID == direction.DirectionTypeID
                              select r).ToList();

                 List<Models.Detector> DetectorsByDirection = new List<Models.Detector>();

                 foreach (Models.Approach a in approaches)
                 {
                     foreach(Models.Detector d in a.Detectors)
                     {
                         if(d.DetectorSupportsThisMetric(5))
                         {
                             DetectorsByDirection.Add(d);
                         }
                     }
                 }


                //Loop through the major movement types
                 List<int> movementTypeIdsSorted = new List<int> { 3, 1, 2 };
                foreach(int x in movementTypeIdsSorted)
                {
                Common.Models.LaneType lanetype = (from r in laneTypes
                                                   where r.Description == laneTypeDescription
                                                  select r).FirstOrDefault();

                Common.Models.MovementType movementType = (from r in movementTypes
                                                           where r.MovementTypeID == x
                                                           select r).FirstOrDefault();

                List<Models.Detector> DetectorsForChart = (from r in DetectorsByDirection
                                                          where r.MovementType.MovementTypeID == movementType.MovementTypeID
                                                          && r.LaneType.LaneTypeID == lanetype.LaneTypeID
                                                          select r).ToList();

                    //movement type 1 is the thru movement.  We have to add the thru/turn lanes to the thru movment count.

                    if(x == 1)
                    {
                        List<Models.Detector> turnthrudetectors = (from r in DetectorsByDirection
                                                                   where (r.MovementType.MovementTypeID == 4 || r.MovementType.MovementTypeID == 5)
                                                                   && r.LaneType.LaneTypeID == lanetype.LaneTypeID
                                                                   select r).ToList();

                        if (turnthrudetectors != null && turnthrudetectors.Count > 0)
                        {
                            DetectorsForChart.AddRange(turnthrudetectors);
                        }
                    }

                if (DetectorsForChart.Count > 0)
                {
                    MOE.Common.Business.TMCMetric TMCchart = new MOE.Common.Business.TMCMetric(StartDate, EndDate, signal, direction, DetectorsForChart, lanetype, movementType, YAxisMax, Y2AxisMax, ShowLaneVolumes, ShowTotalVolumes, SelectedBinSize);
                    Chart chart = TMCchart.chart;
                    SetSimplePlanStrips(plans, chart, StartDate);
                    //Create the File Name

                    string chartName = CreateFileName();

                    //Save an image of the chart
                    chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                    ReturnList.Add(MetricWebPath + chartName);
                }
                        
                }
                
            }
        }




        private void SetSimplePlanStrips(MOE.Common.Business.PlanCollection plans, Chart chart, DateTime StartDate)
                {
                    PlanCollection.SetSimplePlanStrips(plans, chart, StartDate);
                }
    }
}
