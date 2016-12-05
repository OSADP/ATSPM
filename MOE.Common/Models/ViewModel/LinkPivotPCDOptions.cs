﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace MOE.Common.Models.ViewModel
{
    public class LinkPivotPCDOptions
    {
        public List<DateTime> Dates { get; set; }
        [Required]
        [Display(Name="Y-Axis")]
        public int YAxis { get; set; }
        public string SignalId { get; set; }
        public string DownSignalId { get; set; }
        [Required]
        public int Delta { get; set; }
        public string DownDirection { get; set; }
        public string UpstreamDirection { get; set; }        
        public List<DateTime> SelectedStartDate { get; set; }
        [Required]
        public DateTime SelectedEndDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
