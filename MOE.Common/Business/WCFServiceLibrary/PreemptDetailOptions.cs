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

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PreemptDetailOptions: MetricOptions
    {
        public PreemptDetailOptions( string signalID, DateTime startDate, DateTime endDate)
        {
            SignalID = signalID;
            StartDate = startDate;
            EndDate = endDate;

        }
        public override List<string> CreateMetric()
        {
            base.CreateMetric();


            List<string> returnList = new List<string>();
            List<MOE.Common.Business.ControllerEventLogs> tables = new List<MOE.Common.Business.ControllerEventLogs>();

            MOE.Common.Business.ControllerEventLogs eventsTable = new MOE.Common.Business.ControllerEventLogs();
            eventsTable.FillforPreempt(SignalID, StartDate, EndDate);

                MOE.Common.Business.ControllerEventLogs t1 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t2 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t3 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t4 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t5 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t6 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t7 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t8 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t9 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t10 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t11 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t12 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t13 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t14 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t15 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t16 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t17 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t18 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t19 = new MOE.Common.Business.ControllerEventLogs();
                MOE.Common.Business.ControllerEventLogs t20 = new MOE.Common.Business.ControllerEventLogs();


                foreach (MOE.Common.Models.Controller_Event_Log row in eventsTable.Events)
                {
                    switch (row.EventParam)
                    {
                        case 1:
                            t1.Events.Add(row);
                            break;
                        case 2:
                            t2.Events.Add(row);
                            break;
                        case 3:
                            t3.Events.Add(row);
                            break;
                        case 4:
                            t4.Events.Add(row);
                            break;
                        case 5:
                            t5.Events.Add(row);
                            break;
                        case 6:
                            t6.Events.Add(row);
                            break;
                        case 7:
                            t7.Events.Add(row);
                            break;
                        case 8:
                            t8.Events.Add(row);
                            break;
                        case 9:
                            t9.Events.Add(row);
                            break;
                        case 10:
                            t10.Events.Add(row);
                            break;
                        case 11:
                            t11.Events.Add(row);
                            break;
                        case 12:
                            t12.Events.Add(row);
                            break;
                        case 13:
                            t13.Events.Add(row);
                            break;
                        case 14:
                            t14.Events.Add(row);
                            break;
                        case 15:
                            t15.Events.Add(row);
                            break;
                        case 16:
                            t16.Events.Add(row);
                            break;
                        case 17:
                            t17.Events.Add(row);
                            break;
                        case 18:
                            t18.Events.Add(row);
                            break;
                        case 19:
                            t19.Events.Add(row); 
                            break;
                        case 20:
                            t20.Events.Add(row);
                            break;


                    }
                }

                if (t1.Events.Count > 0)
                {
                    tables.Add(t1);

                }
                if (t2.Events.Count > 0)
                {
                    tables.Add(t2);

                }
                if (t3.Events.Count > 0)
                {
                    tables.Add(t3);

                }
                if (t4.Events.Count > 0)
                {
                    tables.Add(t4);

                }
                if (t5.Events.Count > 0)
                {
                    tables.Add(t5);

                }
                if (t6.Events.Count > 0)
                {
                    tables.Add(t6);

                }
                if (t7.Events.Count > 0)
                {
                    tables.Add(t7);

                }
                if (t8.Events.Count > 0)
                {
                    tables.Add(t8);

                }
                if (t9.Events.Count > 0)
                {
                    tables.Add(t9);

                }
                if (t10.Events.Count > 0)
                {
                    tables.Add(t10);

                }
                if (t11.Events.Count > 0)
                {
                    tables.Add(t11);

                }
                if (t12.Events.Count > 0)
                {
                    tables.Add(t12);

                }
                if (t13.Events.Count > 0)
                {
                    tables.Add(t13);

                }
                if (t14.Events.Count > 0)
                {
                    tables.Add(t14);

                }
                if (t15.Events.Count > 0)
                {
                    tables.Add(t15);

                }
                if (t16.Events.Count > 0)
                {
                    tables.Add(t16);

                }
                if (t17.Events.Count > 0)
                {
                    tables.Add(t17);

                }
                if (t18.Events.Count > 0)
                {
                    tables.Add(t18);

                }
                if (t19.Events.Count > 0)
                {
                    tables.Add(t19);

                }
                if (t20.Events.Count > 0)
                {
                    tables.Add(t20);

                }
                string location = GetSignalLocation();

                foreach (MOE.Common.Business.ControllerEventLogs t in tables)
                {
                    MOE.Common.Business.Preempt.PreemptDetailChart detailchart = new MOE.Common.Business.Preempt.PreemptDetailChart(StartDate, EndDate, SignalID, location, t);

                    Chart chart = detailchart.chart;
                   string chartName = CreateFileName();



                            //Save an image of the chart
                   chart.SaveImage(MetricFileLocation + chartName, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Jpeg);

                   returnList.Add(MetricWebPath + chartName);
                }
                return returnList;
            }
        }

 
    }
