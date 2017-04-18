using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class TestMetricTypeRepository : IMetricTypeRepository
    {
        Models.SPM db = new SPM();

        public List<Models.MetricType> GetAllMetrics()
        {
            List<Models.MetricType> results = new List<MetricType>();
            MetricType purduePhase = new MetricType();
            purduePhase.MetricID = 1;
            purduePhase.ChartName = "Purdue Phase Termination";
            purduePhase.ShowOnWebsite = true;
            purduePhase.DetectionTypeID = 1;
            results.Add(purduePhase);

            MetricType split = new MetricType();
            split.MetricID = 2;
            split.ChartName = "Split Monitor";
            split.ShowOnWebsite = true;
            split.DetectionTypeID = 1;
            results.Add(split);

            MetricType ped = new MetricType();
            ped.MetricID = 3;
            ped.ChartName = "Pedestrian Delay";
            ped.ShowOnWebsite = true;
            ped.DetectionTypeID = 1;
            results.Add(ped);

            MetricType preempt = new MetricType();
            preempt.MetricID = 4;
            preempt.ChartName = "Preemption Details";
            preempt.ShowOnWebsite = true;
            preempt.DetectionTypeID = 1;
            results.Add(preempt);

            MetricType tmc = new MetricType();
            tmc.MetricID = 5;
            tmc.ChartName = "Turning Movement Counts";
            tmc.ShowOnWebsite = true;
            tmc.DetectionTypeID = 4;
            results.Add(tmc);

            MetricType pcd = new MetricType();
            pcd.MetricID = 6;
            pcd.ChartName = "Purdue Coordination Diagram";
            pcd.ShowOnWebsite = true;
            pcd.DetectionTypeID = 2;
            results.Add(pcd);

            MetricType approach = new MetricType();
            approach.MetricID = 7;
            approach.ChartName = "Approach Volume";
            approach.ShowOnWebsite = true;
            approach.DetectionTypeID = 2;
            results.Add(approach);

            MetricType delay = new MetricType();
            delay.MetricID = 8;
            delay.ChartName = "Approach Delay";
            delay.ShowOnWebsite = true;
            delay.DetectionTypeID = 2;
            results.Add(delay);

            MetricType aor = new MetricType();
            aor.MetricID = 9;
            aor.ChartName = "Arrivals on Red";
            aor.ShowOnWebsite = true;
            aor.DetectionTypeID = 2;
            results.Add(aor);

            MetricType speed = new MetricType();
            speed.MetricID = 10;
            speed.ChartName = "Approach Speed";
            speed.ShowOnWebsite = true;
            speed.DetectionTypeID = 3;
            results.Add(speed);

            MetricType yra = new MetricType();
            yra.MetricID = 11;
            yra.ChartName = "Yellow and Red Actuations";
            yra.ShowOnWebsite = true;
            yra.DetectionTypeID = 5;
            results.Add(yra);


            MetricType splitFail = new MetricType();
            splitFail.MetricID = 12;
            splitFail.ChartName = "Purdue Split Failure";
            splitFail.ShowOnWebsite = true;
            splitFail.DetectionTypeID = 6;
            results.Add(splitFail);

            return results;
        }

        public List<MetricType> GetAllToDisplayMetrics()
        {
            List<Models.MetricType> results = new List<MetricType>();
            MetricType purduePhase = new MetricType();
            purduePhase.MetricID = 1;
            purduePhase.ChartName = "Purdue Phase Termination";
            purduePhase.ShowOnWebsite = true;
            purduePhase.DetectionTypeID = 1;
            results.Add(purduePhase);

            MetricType split = new MetricType();
            split.MetricID = 2;
            split.ChartName = "Split Monitor";
            split.ShowOnWebsite = true;
            split.DetectionTypeID = 1;
            results.Add(split);

            MetricType ped = new MetricType();
            ped.MetricID = 3;
            ped.ChartName = "Pedestrian Delay";
            ped.ShowOnWebsite = true;
            ped.DetectionTypeID = 1;
            results.Add(ped);

            MetricType preempt = new MetricType();
            preempt.MetricID = 4;
            preempt.ChartName = "Preemption Details";
            preempt.ShowOnWebsite = true;
            preempt.DetectionTypeID = 1;
            results.Add(preempt);

            MetricType tmc = new MetricType();
            tmc.MetricID = 5;
            tmc.ChartName = "Turning Movement Counts";
            tmc.ShowOnWebsite = true;
            tmc.DetectionTypeID = 4;
            results.Add(tmc);

            MetricType pcd = new MetricType();
            pcd.MetricID = 6;
            pcd.ChartName = "Purdue Coordination Diagram";
            pcd.ShowOnWebsite = true;
            pcd.DetectionTypeID = 2;
            results.Add(pcd);

            MetricType approach = new MetricType();
            approach.MetricID = 7;
            approach.ChartName = "Approach Volume";
            approach.ShowOnWebsite = true;
            approach.DetectionTypeID = 2;
            results.Add(approach);

            MetricType delay = new MetricType();
            delay.MetricID = 8;
            delay.ChartName = "Approach Delay";
            delay.ShowOnWebsite = true;
            delay.DetectionTypeID = 2;
            results.Add(delay);

            MetricType aor = new MetricType();
            aor.MetricID = 9;
            aor.ChartName = "Arrivals on Red";
            aor.ShowOnWebsite = true;
            aor.DetectionTypeID = 2;
            results.Add(aor);

            MetricType speed = new MetricType();
            speed.MetricID = 10;
            speed.ChartName = "Approach Speed";
            speed.ShowOnWebsite = true;
            speed.DetectionTypeID = 3;
            results.Add(speed);

            MetricType yra = new MetricType();
            yra.MetricID = 11;
            yra.ChartName = "Yellow and Red Actuations";
            yra.ShowOnWebsite = true;
            yra.DetectionTypeID = 5;
            results.Add(yra);


            MetricType splitFail = new MetricType();
            splitFail.MetricID = 12;
            splitFail.ChartName = "Purdue Split Failure";
            splitFail.ShowOnWebsite = true;
            splitFail.DetectionTypeID = 6;
            results.Add(splitFail);
            return results;
        }

        public List<MetricType> GetBasicMetrics()
        {
            List<Models.MetricType> results = new List<MetricType>();
            MetricType purduePhase = new MetricType();
            purduePhase.MetricID = 1;
            purduePhase.ChartName = "Purdue Phase Termination";
            purduePhase.ShowOnWebsite = true;
            purduePhase.DetectionTypeID = 1;
            results.Add(purduePhase);

            MetricType split = new MetricType();
            split.MetricID = 2;
            split.ChartName = "Split Monitor";
            split.ShowOnWebsite = true;
            split.DetectionTypeID = 1;
            results.Add(split);

            MetricType ped = new MetricType();
            ped.MetricID = 3;
            ped.ChartName = "Pedestrian Delay";
            ped.ShowOnWebsite = true;
            ped.DetectionTypeID = 1;
            results.Add(ped);

            MetricType preempt = new MetricType();
            preempt.MetricID = 4;
            preempt.ChartName = "Preemption Details";
            preempt.ShowOnWebsite = true;
            preempt.DetectionTypeID = 1;
            results.Add(preempt);

            return results;
        }

        public List<MetricType> GetMetricsByIDs(List<int> metricIDs)
        {
            return db.MetricTypes.Where(x => metricIDs.Contains(x.MetricID)).ToList();
        }
        public MetricType GetMetricsByID(int metricID)
        {
            return db.MetricTypes.Find(metricID);
        }

        public List<Models.MetricType> GetMetricTypesByMetricComment(Models.MetricComment metricComment)
        {
            return db.MetricTypes.Where(x => metricComment.MetricTypeIDs.Contains(x.MetricID)).ToList();
        }
    }
}
