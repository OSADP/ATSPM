using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MOE.Common.Business.PEDDelay
{
    public class PEDDelayChart
    {
        public Chart chart = new Chart();
        private PedPhase _PedPhase;

        public PEDDelayChart(PedPhase pp, string location, double? yAxisMaximum, double? y2AxisMaximum, DateTime startDate, DateTime endDate)
        {
            _PedPhase = pp;
            double y = 0;
            string extendedDirection = string.Empty;
            TimeSpan reportTimespan = endDate - startDate;

            //Set the chart properties
            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
            chart.ImageType = ChartImageType.Jpeg;
            chart.Height = 450;
            chart.Width = 1100;
            

            //Set the chart title
            chart.Titles.Add("Pedestrian Delay\n" + location + "Signal " + pp.SignalID + " "
                + "\n" + pp.StartDate.ToString("f") +
                " - " + pp.EndDate.ToString("f") + "\nPhase " + pp.PhaseNumber.ToString() +
                "\n\n" + pp.PedActuations.ToString() + "-Ped Acutations(PA) " +
                DateTime.Today.AddMinutes(pp.MinDelay / 60).ToString("mm:ss") + "-Min Delay " +
                DateTime.Today.AddMinutes(pp.MaxDelay / 60).ToString("mm:ss") + "-Max Delay " +
                DateTime.Today.AddMinutes(pp.AverageDelay / 60).ToString("mm:ss") + "-Average Delay(AD)");

            //Create the chart legend
            Legend chartLegend = new Legend();
            chartLegend.Name = "MainLegend";
            chartLegend.Docking = Docking.Left;
            chart.Legends.Add(chartLegend);


            //Create the chart area
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "ChartArea1";
            chartArea.AxisY.Title = "Pedestrian Delay\nby Actuation(minutes)";
            chartArea.AxisY.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisY.Interval = 1;
            chartArea.AxisY.Minimum = DateTime.Today.ToOADate();
            chartArea.AxisY.LabelStyle.Format = "mm:ss";
            if (yAxisMaximum != null)
            {
                chartArea.AxisY.Maximum = DateTime.Today.AddMinutes(yAxisMaximum.Value).ToOADate();
            }

            //chartArea.AxisY2.Title = "Total Hourly Delay(minutes)";
            //chartArea.AxisY2.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisY2.LabelStyle.Format = "mm:ss";
            //chartArea.AxisY2.Minimum = DateTime.Today.ToOADate();
            //chartArea.AxisY2.Interval = 5;
            //chartArea.AxisY2.MajorGrid.Enabled = false;
            //chartArea.AxisY2.MinorGrid.Enabled = false;
            //chartArea.AxisY2.Enabled = AxisEnabled.True;
            //chartArea.AxisY2.Maximum = DateTime.Today.AddMinutes(y2AxisMaximum).ToOADate();

            chartArea.AxisX.Title = "Time (Hour of Day)";
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX.LabelStyle.Format = "HH";
            chartArea.AxisX2.LabelStyle.Format = "HH";
            chartArea.AxisX.Minimum = _PedPhase.StartDate.ToOADate();
            chartArea.AxisX.Maximum = _PedPhase.EndDate.ToOADate();
            if (reportTimespan.Days < 1)
            {
                if (reportTimespan.Hours > 1)
                {
                    chartArea.AxisX2.Interval = 1;
                    chartArea.AxisX.Interval = 1;
                }
                else
                {
                    chartArea.AxisX.LabelStyle.Format = "HH:mm";
                    chartArea.AxisX2.LabelStyle.Format = "HH:mm";
                }
            }
            chartArea.AxisX2.Enabled = AxisEnabled.True;
            chartArea.AxisX2.MajorTickMark.Enabled = true;
            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chartArea.AxisX2.Minimum = _PedPhase.StartDate.ToOADate();
            chartArea.AxisX2.Maximum = _PedPhase.EndDate.ToOADate();

            chart.ChartAreas.Add(chartArea);


            //Add the point series

            Series PedestrianDelaySeries = new Series();
            PedestrianDelaySeries.ChartType = SeriesChartType.Column;            
            PedestrianDelaySeries.BorderDashStyle = ChartDashStyle.Dash;
            PedestrianDelaySeries.Color = Color.Blue;
            PedestrianDelaySeries.Name = "Pedestrian Delay\nby Actuation";
            PedestrianDelaySeries.XValueType = ChartValueType.DateTime;
            

            //Series HourlyTotalSeries = new Series();
            //HourlyTotalSeries.ChartType = SeriesChartType.Line;
            //HourlyTotalSeries.Color = Color.OliveDrab;
            //HourlyTotalSeries.Name = "Total Hourly Delay";
            //HourlyTotalSeries.XValueType = ChartValueType.DateTime;
            //HourlyTotalSeries.YAxisType = AxisType.Secondary;
            //HourlyTotalSeries.BorderWidth = 2;
            

            
            
            chart.Series.Add(PedestrianDelaySeries);
            chart.Series["Pedestrian Delay\nby Actuation"]["PixelPointWidth"] = "2";
            //chart.Series.Add(HourlyTotalSeries);
            

            AddDataToChart();
            SetPlanStrips();
        }



        protected void AddDataToChart()
        {   
            foreach(PedPlan pp in _PedPhase.Plans)     
            {
                foreach(PedCycle pc in pp.Cycles)
                {
                    chart.Series["Pedestrian Delay\nby Actuation"].Points.AddXY(pc.BeginWalk, DateTime.Today.AddMinutes(pc.Delay / 60));
                }
            }

            ////add the first points so that we can make the hourly totals flat
            //if (_PedPhase.HourlyTotals.Count > 0)
            //{
            //    chart.Series["Total Hourly Delay"].Points.AddXY(_PedPhase.StartDate.ToOADate(),
            //        DateTime.Today);
            //    //chart.Series["Hourly Delay"].Points.AddXY(_PedPhase.StartDate.ToOADate(), 
            //    //    DateTime.Today.AddMinutes(_PedPhase.HourlyTotals[0].Delay / 60));

            //    //double lastDelay = 0;
            //    for (int i = 0; i < _PedPhase.HourlyTotals.Count; i++)                   
            //    {
            //        chart.Series["Total Hourly Delay"].Points.AddXY(_PedPhase.HourlyTotals[i].Hour,
            //            DateTime.Today.AddMinutes(_PedPhase.HourlyTotals[i].Delay / 60));
            //        chart.Series["Total Hourly Delay"].Points.AddXY(_PedPhase.HourlyTotals[i].Hour.AddHours(1),
            //            DateTime.Today.AddMinutes(_PedPhase.HourlyTotals[i].Delay / 60));
            //    }
            //}

            //chart.Titles.Add("Test");           
        }


        protected void SetPlanStrips()
        {
            int backGroundColor = 1;
            foreach (PedPlan plan in _PedPhase.Plans)
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
                stripline.IntervalOffset = (plan.StartDate - _PedPhase.StartDate).TotalHours;
                stripline.StripWidth = (plan.EndDate - plan.StartDate).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartDate.ToOADate();
                Plannumberlabel.ToPosition = plan.EndDate.ToOADate();
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

                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 3;

                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                CustomLabel pedActuationsLabel = new CustomLabel();
                pedActuationsLabel.FromPosition = plan.StartDate.ToOADate();
                pedActuationsLabel.ToPosition = plan.EndDate.ToOADate();
                pedActuationsLabel.Text = plan.PedActuations + " PA";
                pedActuationsLabel.LabelMark = LabelMarkStyle.LineSideMark;
                pedActuationsLabel.RowIndex = 2;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(pedActuationsLabel);
                

                CustomLabel avgDelayLabel = new CustomLabel();
                avgDelayLabel.FromPosition = plan.StartDate.ToOADate();
                avgDelayLabel.ToPosition = plan.EndDate.ToOADate();
                avgDelayLabel.Text = Math.Round(plan.AvgDelay / 60).ToString() + " AD";
                avgDelayLabel.RowIndex = 1;
                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(avgDelayLabel);
               
                //Change the background color counter for alternating color
                backGroundColor++;

            }
        }

        private static int RoundToNearest(int iNumberToRound, int iToNearest)
        {
            //int iToNearest = 100;
            int iNearest = 0;
            bool bIsUpper = false;

            int iRest = iNumberToRound % iToNearest;
            if (iNumberToRound == 550) bIsUpper = true;

            if (bIsUpper == true)
            {
                iNearest = (iNumberToRound - iRest) + iToNearest;
                return iNearest;
            }
            else if (iRest > (iToNearest / 2))
            {
                iNearest = (iNumberToRound - iRest) + iToNearest;
                return iNearest;
            }
            else if (iRest < (iToNearest / 2))
            {
                iNearest = (iNumberToRound - iRest);
                return iNearest;
            }

            return 0;
        }

    }
}



