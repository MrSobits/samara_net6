﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    public class SSTUAppeal
    {
        /// <summary>
        /// departmentId
        /// </summary>
        public string departmentId { get; set; }

        /// <summary>
        /// isDirect
        /// </summary>
        public bool isDirect { get; set; }

        /// <summary>
        /// format
        /// </summary>
        public string format { get; set; }

        /// <summary>
        /// number
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// createDate
        /// </summary>
        public string createDate { get; set; }

        /// <summary>
        /// name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// address
        /// </summary>
        public string address { get; set; }

        ///// <summary>
        ///// email
        ///// </summary>
        //public string email { get; set; }

        /// <summary>
        /// questions
        /// </summary>
        public SSTUQuestions[] questions { get; set; }


    }

    public class SSTUAppealCD
    {
        /// <summary>
        /// departmentId
        /// </summary>
        public string departmentId { get; set; }

        /// <summary>
        /// isDirect
        /// </summary>
        public bool isDirect { get; set; }

        /// <summary>
        /// format
        /// </summary>
        public string format { get; set; }

        /// <summary>
        /// number
        /// </summary>
        public string number { get; set; }

        ///// <summary>
        ///// createDate
        ///// </summary>
        public string createDate { get; set; }

        /// <summary>
        /// name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// address
        /// </summary>
        public string address { get; set; }

        ///// <summary>
        ///// email
        ///// </summary>
        //public string email { get; set; }

        /// <summary>
        /// questions
        /// </summary>
        public SSTUQuestions[] questions { get; set; }
    }
}
