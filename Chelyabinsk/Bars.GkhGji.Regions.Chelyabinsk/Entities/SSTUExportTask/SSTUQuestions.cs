using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    public class SSTUQuestions
    {
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// status
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// incomingDate
        /// </summary>
        public string incomingDate { get; set; }

        /// <summary>
        /// registrationDate
        /// </summary>
        public string registrationDate { get; set; }

        /// <summary>
        /// responseDate
        /// </summary>
        public string responseDate { get; set; }

        /// <summary>
        /// responseDate
        /// </summary>
       // public bool actionsTaken { get; set; }
        /// <summary>
        /// attachment
        /// </summary>
        public SSTUAttachment attachment { get; set; }

    }
}

