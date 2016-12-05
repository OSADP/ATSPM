using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;


namespace MOE.Common.Business.Preempt
{
    public class PreemptDetailChart
        {
        public Chart chart = new Chart();


        public PreemptDetailChart(DateTime graphStartDate, DateTime graphEndDate,
            string signalid, string location, MOE.Common.Business.ControllerEventLogs DTTB)
        {

            int PreemptNumber = 0;
            if (DTTB.Events.Count > 0)
            {
                MOE.Common.Models.Controller_Event_Log r = DTTB.Events[0] as MOE.Common.Models.Controller_Event_Log;
                PreemptNumber = r.EventParam;
            }
            TimeSpan reportTimespan = graphEndDate - graphStartDate;

            //Set the chart properties
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 350;
            chart.Width = 1100;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.BorderSkin.BorderColor = Color.Black;
            chart.BorderSkin.BorderWidth = 1;

            //Set the chart title
            chart.Titles.Add("Preemption Details");
            chart.Titles.Add(location + "Signal " + signalid + " " + "\n" + graphStartDate.ToString("f") +
                " - " + graphEndDate.ToString("f"));

            if (PreemptNumber > 0)
            {
                chart.Titles.Add(" Preempt Number: " + PreemptNumber.ToString());
            }

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";

            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Title = "Seconds Since Request";
            chartArea.AxisY.Title = "";
            chartArea.AxisY.Interval = 10;
            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            if (reportTimespan.Days < 1)
            {
                if (reportTimespan.Hours > 1)
                {
                    chartArea.AxisX.Interval = 1;
                }
                else
                {
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";
                }
            }

            chart.ChartAreas.Add(chartArea);


            //Add the point series

            Series TimeToServiceSeries = new Series();
            TimeToServiceSeries.ChartType = SeriesChartType.Column; 
            TimeToServiceSeries.Color = Color.Yellow;
            TimeToServiceSeries.Name = "Time to Service";
            TimeToServiceSeries.XValueType = ChartValueType.DateTime;

            Series DwellTimeSeries = new Series();
            DwellTimeSeries.ChartType = SeriesChartType.Column;
            DwellTimeSeries.Color = Color.Green;
            DwellTimeSeries.Name = "Dwell Time";
            DwellTimeSeries.XValueType = ChartValueType.DateTime;

            Series EntryDelaySeries = new Series();
            EntryDelaySeries.ChartType = SeriesChartType.Column;
            EntryDelaySeries.Color = Color.Red;
            EntryDelaySeries.Name = "Entry Delay";
            EntryDelaySeries.XValueType = ChartValueType.DateTime;

            Series EndCallSeries = new Series();
            EndCallSeries.ChartType = SeriesChartType.Point;          
            EndCallSeries.BorderDashStyle = ChartDashStyle.Dash;           
            EndCallSeries.Color = Color.Black;
            EndCallSeries.Name = "End Call";
            EndCallSeries.XValueType = ChartValueType.DateTime;

            Series CallMaxOutSeries = new Series();
            CallMaxOutSeries.ChartType = SeriesChartType.Point;
            CallMaxOutSeries.BorderDashStyle = ChartDashStyle.Dash;
            CallMaxOutSeries.MarkerStyle = MarkerStyle.Cross;
            CallMaxOutSeries.Color = Color.Red;
            CallMaxOutSeries.Name = "Call Max Out";
            CallMaxOutSeries.XValueType = ChartValueType.DateTime;

            Series TrackClearSeries = new Series();
            TrackClearSeries.ChartType = SeriesChartType.Point;
            TrackClearSeries.BorderDashStyle = ChartDashStyle.Dash;           
            TrackClearSeries.Color = Color.Green;
            TrackClearSeries.Name = "Track Clear";
            TrackClearSeries.XValueType = ChartValueType.DateTime;

            Series GateDownSeries = new Series();
            GateDownSeries.ChartType = SeriesChartType.Point;
            GateDownSeries.BorderDashStyle = ChartDashStyle.Dash;
            GateDownSeries.MarkerStyle = MarkerStyle.Circle;
            GateDownSeries.Color = Color.Black;
            GateDownSeries.Name = "Gate Down";
            GateDownSeries.XValueType = ChartValueType.DateTime;



            //Add the Posts series to ensure the chart is the size of the selected timespan
            Series posts = new Series();
            posts.IsVisibleInLegend = false;
            posts.ChartType = SeriesChartType.Point;
            posts.Color = Color.White;
            posts.Name = "Posts";
            posts.XValueType = ChartValueType.DateTime;

            
            chart.Series.Add(TimeToServiceSeries);
            chart.Series.Add(DwellTimeSeries);
            chart.Series.Add(EntryDelaySeries);
            chart.Series.Add(EndCallSeries);
            chart.Series.Add(CallMaxOutSeries);
            chart.Series.Add(TrackClearSeries);
            chart.Series.Add(GateDownSeries);
            chart.Series.Add(posts);

            //Add points at the start and and of the x axis to ensure
            //the graph covers the entire period selected by the user
            //whether there is data or not
            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
            chart.Series["Posts"].Points.AddXY(graphEndDate, 0);

            AddDataToChart(chart, graphStartDate, graphEndDate, DTTB, signalid);
            PlanCollection plans = new PlanCollection(graphStartDate, graphEndDate, signalid);
            SetSimplePlanStrips(plans, chart, graphStartDate, graphEndDate, DTTB);

        }

        protected void AddDataToChart(Chart chart, DateTime startDate,
     DateTime endDate, MOE.Common.Business.ControllerEventLogs DTTB, string signalid)
        {
            PreemptCycleEngine engine = new PreemptCycleEngine();
            List<PreemptCycle> cycles = engine.CreatePreemptCycle(DTTB);

            foreach(PreemptCycle cycle in cycles)
            {
               
                chart.Series["Time to Service"].Points.AddXY(cycle.InputOn, cycle.TimeToService);
                chart.Series["Dwell Time"].Points.AddXY(cycle.InputOn, cycle.DwellTime);
                chart.Series["Entry Delay"].Points.AddXY(cycle.InputOn, cycle.TimeToEndOfEntryDelay);
                chart.Series["End Call"].Points.AddXY(cycle.InputOn, cycle.TimeToEndCall);
                chart.Series["Call Max Out"].Points.AddXY(cycle.InputOn, cycle.TimeToCallMaxOut);
                chart.Series["Track Clear"].Points.AddXY(cycle.InputOn, cycle.TimeToTrackClear);
                chart.Series["Gate Down"].Points.AddXY(cycle.InputOn, cycle.TimeToGateDown);
                

            }

            //RemoveErrantPoints(chart, startDate, endDate);



        }

        //private void  RemoveErrantPoints(Chart chart, DateTime startDate,
        //                DateTime endDate)
        //{
        //    foreach(Series series in chart.Series)
        //    {
                

        //        foreach(DataPoint point in series.Points)
        //        {
        //            if(point.XValue < startDate.ToOADate() || point.XValue > endDate.ToOADate())
        //            {
        //                series.Points.Remove(point);
        //            }
        //        }
        //    }
        //}

        protected void SetSimplePlanStrips(MOE.Common.Business.PlanCollection planCollection, Chart chart, DateTime graphStartDate, DateTime graphEndDate, MOE.Common.Business.ControllerEventLogs DTTB)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in planCollection.PlanList)
            {
                if (plan.StartTime > graphStartDate && plan.EndTime < graphEndDate)
                {
                    StripLine stripline = new StripLine();
                    //Creates alternating backcolor to distinguish the plans
                    if (backGroundColor % 2 == 0)
                    {
                        stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                    }
                    else
                    {
                        stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
                    }

                    //Set the stripline properties
                    stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                    stripline.Interval = 1;
                    stripline.IntervalOffset = (plan.StartTime - graphStartDate).TotalHours;
                    stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                    stripline.StripWidthType = DateTimeIntervalType.Hours;

                    chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                    //Add a corrisponding custom label for each strip
                    CustomLabel Plannumberlabel = new CustomLabel();
                    Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                    Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
                    switch (plan.PlanNumber)
                    {
                        case 254:
                            Plannumberlabel.Text = "Free";
                            break;
                        case 255:
                            Plannumberlabel.Text = "Flash";
                            break;
                        case 0:
                            Plannumberlabel.Text = "Unknown";
                            break;
                        default:
                            Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

                            break;
                    }
                    Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                    Plannumberlabel.ForeColor = Color.Black;
                    Plannumberlabel.RowIndex = 6;


                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(Plannumberlabel);

                    CustomLabel planPreemptsLabel = new CustomLabel();
                    planPreemptsLabel.FromPosition = plan.StartTime.ToOADate();
                    planPreemptsLabel.ToPosition = plan.EndTime.ToOADate();

                    var c = from r in DTTB.Events
                            where r.EventCode == 107 && r.Timestamp > plan.StartTime && r.Timestamp < plan.EndTime
                            select r;



                    string preemptCount = c.Count().ToString();
                    planPreemptsLabel.Text = "Preempts Serviced During Plan: " + preemptCount;
                    planPreemptsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                    planPreemptsLabel.ForeColor = Color.Red;
                    planPreemptsLabel.RowIndex = 7;

                    chart.ChartAreas[0].AxisX2.CustomLabels.Add(planPreemptsLabel);

                    backGroundColor++;

                }
            }
        }

        }
}
