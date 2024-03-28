namespace Bars.GkhDi.Report
{
    using System;
    using Gkh.Enums;

    public partial class DisclosureInfo731
    {
        public class ServicePprProxy
        {
            public long BaseServiceId { get; set; }

            public string Work { get; set; }

            public string Periodicity { get; set; }

            public decimal? PlanCost { get; set; }

            public decimal? FactCost { get; set; }

            public string ReasonRejection { get; set; }

            public DateTime? DateStart { get; set; }

            public DateTime? DateEnd { get; set; }

            public YesNoNotSet Sheduled { get; set; }
            
            public long? GroupWorkPprId { get; set; }
        }

        public class RepairPprProxy
        {
            public long BaseServiceId { get; set; }

            public YesNoNotSet Sheduled { get; set; }

            public string Name { get; set; }

            public decimal? Cost { get; set; }

            public decimal? FactCost { get; set; }

            public decimal? Volume { get; set; }

            public decimal? FactVolume { get; set; }

            public DateTime? DateStart { get; set; }

            public DateTime? DateEnd { get; set; }
        }

        public class PlanRepairWorkProxy
        {
            public long BaseServiceId { get; set; }

            public string Periodicity { get; set; }

            public string Work { get; set; }

            public decimal? Cost { get; set; }

            public decimal? FactCost { get; set; }

            public string ReasonRejection { get; set; }

            public DateTime? DateStart { get; set; }

            public DateTime? DateEnd { get; set; }
        }

        public class RepairWorkTo
        {
            public long BaseServiceId { get; set; }

            public string Name { get; set; }

            public decimal? PlanCost { get; set; }

            public decimal? FactCost { get; set; }

            public DateTime? DateStart { get; set; }

            public DateTime? DateEnd { get; set; }
        }

        protected class TariffForRsoProxy
        {
            public long BaseServiceId { get; set; }

            public decimal? Cost { get; set; }

            public string NumberAct { get; set; }

            public DateTime? DateAct { get; set; }

            public string Org { get; set; }
        }
    }
}