//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Web.UI.DataVisualization.Charting;
//using System.Drawing;

//********************************

//Don't Use this Object.

//********************************




//namespace MOE.Common.Business
//{
//    public class TMCChart
//    {
//        public Chart chart = new Chart();

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="graphStartDate"></param>
//        /// <param name="graphEndDate"></param>
//        /// <param name="signalId"></param>
//        /// <param name="location"></param>
//        /// <param name="direction"></param>
//        /// <param name="movement"></param>
//        /// <param name="thruMax"></param>
//        /// <param name="turnMax"></param>
//        /// <param name="detectors"></param>
//        /// <param name="plans"></param>
//        /// <param name="isThruMovement"></param>
//        /// <param name="showLaneVolumes"></param>
//        /// <param name="showTotalVolumes"></param>
//        /// <param name="binSize"></param>
//        /// 

//        public TMCChart(DateTime graphStartDate, DateTime graphEndDate, string signalId, string location, 
//            string direction, string movement, double thruMax, double turnMax,List<Models.Detectors> detectors, 
//            MOE.Common.Business.PlanCollection plans, bool isThruMovement, bool showLaneVolumes, bool showTotalVolumes, 
//            int binSize)
//        {

            
//            string extendedDirection = string.Empty;

//            //Set the chart properties
//            chart.ImageType = ChartImageType.Jpeg;
//            chart.Height = 367;
//            chart.Width = 734;

//            chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
//            chart.BorderlineColor = Color.Black;
//            chart.BorderlineWidth = 2;
//            chart.BorderlineDashStyle = ChartDashStyle.Solid;
//            //Set the chart title
//            chart.Titles.Add(location + "SIG#" + signalId.ToString() + "\n" + graphStartDate.ToString("f") + " - " + graphEndDate.ToString("f"));
//            chart.Titles.Add(" " + direction + " " + movement);

//            chart.Titles[1].Font = new Font(chart.Titles[0].Font, FontStyle.Bold);

//            ChartArea chartArea = new ChartArea();
//            chartArea.Name = "ChartArea1";


//            Legend chartLegend = new Legend();
//            chartLegend.Name = "MainLegend";
//            chartLegend.Docking = Docking.Left;
//           // chartLegend.Title = direction + " " + movement;
//            chartLegend.Docking = Docking.Bottom;
//            chart.Legends.Add(chartLegend);
            


//            chartArea.AxisX.Title = "Time (Hour of Day)";
//            chartArea.AxisX.Interval = 1;
//            chartArea.AxisX.IntervalType = DateTimeIntervalType.Hours;
//            chartArea.AxisX.LabelStyle.Format = "HH";
//            //   chartArea.AxisX.MajorTickMark.Enabled = true;
//            chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;

//            chartArea.AxisX2.Enabled = AxisEnabled.True;
//            chartArea.AxisX2.MajorTickMark.Enabled = true;
//            chartArea.AxisX2.IntervalType = DateTimeIntervalType.Hours;
//            chartArea.AxisX2.LabelStyle.Format = "HH";
//            chartArea.AxisX2.LabelAutoFitStyle = LabelAutoFitStyles.None;
//            chartArea.AxisX2.Interval = 1;


//            chartArea.AxisY.Title = "Volume (VPH)";
//            //chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
//            chartArea.AxisY.Interval = 100;
//            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;

//            if (movement.IndexOf("Thru") > -1)
//            {
               

//                if (thruMax > 0)
//                {
//                    chartArea.AxisY.Maximum = thruMax;
//                }
//                else
//                {
//                    chartArea.AxisY.Maximum = 1000;
//                }

//            }
//            else
//            {
                
//                if (turnMax > 0)
//                {
//                    chartArea.AxisY.Maximum = turnMax;
//                }
//                else
//                {
//                    chartArea.AxisY.Maximum = 300;
//                }
//            }

//            chart.ChartAreas.Add(chartArea);

//            Series TotalVolume = new Series();
//            TotalVolume.ChartType = SeriesChartType.Line;
//            TotalVolume.Name = "Total Volume";
//            TotalVolume.Color = Color.Black;
//            TotalVolume.BorderWidth = 2;


//            //Lane Series
//            Series L1 = new Series();
//            L1.ChartType = SeriesChartType.Line;
//            L1.Color = Color.DarkRed;
//            L1.Name = "Lane 1";
//            L1.XValueType = ChartValueType.DateTime;
//            L1.MarkerStyle = MarkerStyle.None;
//            L1.MarkerSize = 1;

//            Series L2 = new Series();
//            L2.ChartType = SeriesChartType.Line;
//            L2.Color = Color.DarkOrange;
//            L2.Name = "Lane 2";
//            L2.XValueType = ChartValueType.DateTime;


//            Series L3 = new Series();
//            L3.ChartType = SeriesChartType.Line;
//            L3.Color = Color.Blue;
//            L3.Name = "Lane 3";
//            L3.XValueType = ChartValueType.DateTime;
//            L3.BorderDashStyle = ChartDashStyle.Dot;
//            L3.BorderWidth = 2;

//            Series L4 = new Series();
//            L4.ChartType = SeriesChartType.Line;
//            L4.Color = Color.DarkRed;
//            L4.Name = "Lane 4";
//            L4.XValueType = ChartValueType.DateTime;
//            L4.BorderDashStyle = ChartDashStyle.Dot;
//            L4.BorderWidth = 2;

//            Series LT = new Series();
//            LT.ChartType = SeriesChartType.Line;
//            LT.Color = Color.DarkOrange;
//            LT.Name = "Thru Left";
//            LT.XValueType = ChartValueType.DateTime;
//            LT.BorderDashStyle = ChartDashStyle.Dot;
//            LT.BorderWidth = 2;

//            Series RT = new Series();
//            RT.ChartType = SeriesChartType.Line;
//            RT.Color = Color.Blue;
//            RT.Name = "Thru Right";
//            RT.XValueType = ChartValueType.DateTime;
//            RT.BorderDashStyle = ChartDashStyle.Solid;
//            RT.BorderWidth = 1;

//            //Add the Posts series to ensure the chart is the size of the selected timespan
//            Series testSeries = new Series();
//            testSeries.IsVisibleInLegend = false;
//            testSeries.ChartType = SeriesChartType.Point;
//            testSeries.Color = Color.White;
//            testSeries.Name = "Posts";
//            testSeries.XValueType = ChartValueType.DateTime;

            
//            chart.Series.Add(TotalVolume);
//            chart.Series.Add(L1);
//            chart.Series.Add(L2);
//            chart.Series.Add(L3);
//            chart.Series.Add(L4);
//            chart.Series.Add(LT);
//            chart.Series.Add(RT);
//            chart.Series.Add(testSeries);

//            //Add points at the start and and of the x axis to ensure
//            //the graph covers the entire period selected by the user
//            //whether there is data or not
//            chart.Series["Posts"].Points.AddXY(graphStartDate, 0);
//            chart.Series["Posts"].Points.AddXY(graphEndDate.AddMinutes(5), 0);



//            AddDataToChart(graphStartDate, graphEndDate, detectors, signalId, plans, isThruMovement, showLaneVolumes, showTotalVolumes, binSize);
        
//        }



//        private void AddDataToChart(DateTime startDate, DateTime endDate, List<Models.Detectors> detectors, 
//            string signalId, MOE.Common.Business.PlanCollection plans, bool isThruMovement, 
//            bool showLaneVolumes, bool showTotalVolumes, int binSize)
//        {

//            SortedDictionary<DateTime, int> MovementTotals = new SortedDictionary<DateTime, int>();
//            SortedDictionary<string, int> laneTotals = new SortedDictionary<string, int>();
//            int totalVolume = 0;
//            int laneCount = 0;



//            if (detectors.Count > 0)
//            {

//                foreach (Models.Detectors detector in detectors)
//                {
//                    // THIS IS WHERE WE SORT DETECTORS INTO LANES
//                    //The 'detectors' collection is already sperated by movement, and must now be sorted into
//                    //lanes for graphing
//                    if (detector.Lane.IndexOf("1") != -1 && detector.Lane.IndexOf("TL") == -1 && detector.Lane.IndexOf("TR") == -1)
//                    {
//                        //The total number of lanes by movement will be used in a calculation later.
//                        laneCount++;
//                        foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
//                        {
//                            if (showLaneVolumes)
//                            {
//                                chart.Series["Lane 1"].Points.AddXY(volume.XAxis, volume.YAxis);
//                            }

//                            //we need ot track the total number of cars (volume) for this movement.
//                            //this uses a time/int dictionary.  The volume record for a given time is contibuted to by each lane.
//                            //Then the movement total can be plotted on the graph
//                            if (MovementTotals.ContainsKey(volume.XAxis))
//                            {
//                                MovementTotals[volume.XAxis] += volume.YAxis;
//                            }
//                            else
//                            {
//                                MovementTotals.Add(volume.XAxis, volume.YAxis);
//                            }

//                            //One of the calculations requires total volume by lane.  This if statment keeps a 
//                            //running total of that volume and stores it in a dictonary with the lane number.
//                            if (laneTotals.ContainsKey("L1"))
//                            {
//                                laneTotals["L1"] += volume.YAxis;
//                            }
//                            else
//                            {
//                                laneTotals.Add("L1", volume.YAxis);
//                            }
//                        }
//                    }


//                    //repete for lane 2
//                    if (detector.Lane.IndexOf("2") != -1 && detector.Lane.IndexOf("TL") == -1 && detector.Lane.IndexOf("TR") == -1)
//                    {
//                        laneCount++;
//                        foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
//                        {
//                            if (showLaneVolumes)
//                            {
//                                chart.Series["Lane 2"].Points.AddXY(volume.XAxis, volume.YAxis);
//                            }
//                            if (MovementTotals.ContainsKey(volume.XAxis))
//                            {
//                                MovementTotals[volume.XAxis] += volume.YAxis;
//                            }
//                            else
//                            {
//                                MovementTotals.Add(volume.XAxis, volume.YAxis);
//                            }

//                            if (laneTotals.ContainsKey("L2"))
//                            {
//                                laneTotals["L2"] += volume.YAxis;
//                            }
//                            else
//                            {
//                                laneTotals.Add("L2", volume.YAxis);
//                            }
//                        }
//                    }



//                    //repete for lane 3
//                    if (detector.Lane.IndexOf("3") != -1 && detector.Lane.IndexOf("TL") == -1 && detector.Lane.IndexOf("TR") == -1)
//                    {
//                        laneCount++;
//                        foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
//                        {
//                            if (showLaneVolumes)
//                            {
//                                chart.Series["Lane 3"].Points.AddXY(volume.XAxis, volume.YAxis);
//                            }
//                            if (MovementTotals.ContainsKey(volume.XAxis))
//                            {
//                                MovementTotals[volume.XAxis] += volume.YAxis;
//                            }
//                            else
//                            {
//                                MovementTotals.Add(volume.XAxis, volume.YAxis);
//                            }

//                            if (laneTotals.ContainsKey("L3"))
//                            {
//                                laneTotals["L3"] += volume.YAxis;
//                            }
//                            else
//                            {
//                                laneTotals.Add("L3", volume.YAxis);
//                            }
//                        }
//                    }



//                    //repete for lane 4
//                    if (detector.Lane.IndexOf("4") != -1 && detector.Lane.IndexOf("TL") == -1 && detector.Lane.IndexOf("TR") == -1)
//                    {
//                        laneCount++;
//                        foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
//                        {
//                            if (showLaneVolumes)
//                            {
//                                chart.Series["Lane 4"].Points.AddXY(volume.XAxis, volume.YAxis);
//                            }
//                            if (MovementTotals.ContainsKey(volume.XAxis))
//                            {
//                                MovementTotals[volume.XAxis] += volume.YAxis;
//                            }
//                            else
//                            {
//                                MovementTotals.Add(volume.XAxis, volume.YAxis);
//                            }

//                            if (laneTotals.ContainsKey("L4"))
//                            {
//                                laneTotals["L4"] += volume.YAxis;
//                            }
//                            else
//                            {
//                                laneTotals.Add("L4", volume.YAxis);
//                            }
//                        }
//                    }



//                    // "TL" and "TR" are for the Left Thru and Right Thru shared lanes.
//                    // "These lanes get counted with the thru lanes for total approach volume, but must be charted seperatly.
//                    if (detector.Lane.IndexOf("TL") != -1 && isThruMovement)
//                    {
//                        laneCount++;
//                        foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
//                        {
//                            if (showLaneVolumes)
//                            {
//                                chart.Series["Thru Left"].Points.AddXY(volume.XAxis, volume.YAxis);
//                            }
//                            if (MovementTotals.ContainsKey(volume.XAxis))
//                            {
//                                MovementTotals[volume.XAxis] += volume.YAxis;
//                            }
//                            else
//                            {
//                                MovementTotals.Add(volume.XAxis, volume.YAxis);
//                            }

//                            if (laneTotals.ContainsKey("LT"))
//                            {
//                                laneTotals["LT"] += volume.YAxis;
//                            }
//                            else
//                            {
//                                laneTotals.Add("LT", volume.YAxis);
//                            }
//                        }
//                    }



//                    //repete for RT
//                    if (detector.Lane.IndexOf("TR") != -1 && isThruMovement)
//                    {
//                        laneCount++;
//                        foreach (MOE.Common.Business.Volume volume in detector.Volumes.Items)
//                        {
//                            if (showLaneVolumes)
//                            {
//                                chart.Series["Thru Right"].Points.AddXY(volume.XAxis, volume.YAxis);
//                            }
//                            if (MovementTotals.ContainsKey(volume.XAxis))
//                            {
//                                MovementTotals[volume.XAxis] += volume.YAxis;
//                            }
//                            else
//                            {
//                                MovementTotals.Add(volume.XAxis, volume.YAxis);
//                            }

//                            if (laneTotals.ContainsKey("RT"))
//                            {
//                                laneTotals["RT"] += volume.YAxis;
//                            }
//                            else
//                            {
//                                laneTotals.Add("RT", volume.YAxis);
//                            }
//                        }
//                    }


//                }

                

//                int binMultiplier = 60 / binSize;

//                //get the total volume for the approach
//                foreach (KeyValuePair<DateTime, int> totals in MovementTotals)
//                {
//                    if (showTotalVolumes)
//                    {
//                        chart.Series["Total Volume"].Points.AddXY(totals.Key, totals.Value);
//                    }
//                    totalVolume += (totals.Value);
//                }


//                int highLaneVolume = laneTotals.Values.Max();
                
                
//                KeyValuePair<DateTime, int> peakHourValue = findPeakHour(MovementTotals, binMultiplier);
//                int PHV = peakHourValue.Value;
//                DateTime peakHour = peakHourValue.Key;
//                int PeakHourMAXVolume = 0;

//                string fluPlaceholder = "";

//                if (laneCount > 0 && highLaneVolume > 0)
//                {
//                    double fLU = Convert.ToDouble(totalVolume) / (Convert.ToDouble(laneCount) * Convert.ToDouble(highLaneVolume));
//                    fluPlaceholder = SetSigFigs(fLU, 2).ToString();
//                }
//                else
//                {
//                    fluPlaceholder = "Not Available";
//                }



//                string PHFPlaceholder = "";
//                for (int i = 0; i < binMultiplier; i++)
//                {
//                    if (MovementTotals.ContainsKey(peakHour.AddMinutes(i * binSize)))
//                    {
//                    if (PeakHourMAXVolume < (MovementTotals[peakHour.AddMinutes(i * binSize)]))
//                    {
//                    PeakHourMAXVolume = MovementTotals[peakHour.AddMinutes(i * binSize)];
//                    }
//                    }
//                }

//                if (PeakHourMAXVolume > 0)
//                {
//                double PHF = SetSigFigs( Convert.ToDouble(PHV) / (Convert.ToDouble(PeakHourMAXVolume) * Convert.ToDouble(binMultiplier)), 2);
//                    PHFPlaceholder = PHF.ToString();
//                }
//                else
//                {
//                    PHFPlaceholder = "Not Avialable";
//                }

//                string peakHourString = peakHour.ToShortTimeString() + " - " + peakHour.AddHours(1).ToShortTimeString();

//                string titleString = " TV: " + (totalVolume/binMultiplier).ToString() + " PH: " + peakHourString + " PHV: " + (PHV / binMultiplier).ToString() + " VPH \n " +
//                    " PHF: " + PHFPlaceholder + "        fLU: " + fluPlaceholder;
//                chart.Titles.Add(titleString);

//                foreach (Series series in chart.Series)
//                {
//                    if (series.Points.Count < 1)
//                    {
//                        series.IsVisibleInLegend = false;
//                    }
//                    else
//                    {
//                        series.IsVisibleInLegend = true;
//                    }
//                }
//                chart.Series["Posts"].IsVisibleInLegend = false;

//                if (laneCount == 1)
//                {
//                    chart.Series["Total Volume"].Enabled = false;
//                }

//            }
//        }

//        private KeyValuePair<DateTime, int> findPeakHour(SortedDictionary<DateTime, int> dirVolumes, int binMultiplier)
//        {
//            int subTotal = 0;
//            KeyValuePair<DateTime, int> peakHourValue = new KeyValuePair<DateTime, int>();

//            DateTime startTime = new DateTime();
//            SortedDictionary<DateTime, int> iteratedVolumes = new SortedDictionary<DateTime, int>();

//            for (int i = 0; i < (dirVolumes.Count - (binMultiplier - 1)); i++)
//            {
//                startTime = dirVolumes.ElementAt(i).Key;
//                subTotal = 0;
//                for (int x = 0; x < binMultiplier; x++)
//                {
//                    subTotal = subTotal + dirVolumes.ElementAt(i + x).Value;
//                }

//                iteratedVolumes.Add(startTime, subTotal);

//            }

//            //Find the highest value in the iterated Volumes dictionary.
//            //This should bee the peak hour.
//            foreach (KeyValuePair<DateTime, int> kvp in iteratedVolumes)
//            {
//                if (kvp.Value > peakHourValue.Value)
//                {

//                    peakHourValue = kvp;
//                }
//            }

//            return peakHourValue;
//        }


//        private static double SetSigFigs(double d, int digits)
//        {

//            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);

//            return scale * Math.Round(d / scale, digits);
//        }

//    }
//}
