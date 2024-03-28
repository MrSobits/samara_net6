namespace Bars.GkhDi.Report
{
    public class RepairWorkDetailProxy
    {
        public long RealityObjectId { get; set; }

        public long BaseServiceId { get; set; }

        public string Name { get; set; }

        public decimal? PlanVolume { get; set; }

        public decimal? FactVolume { get; set; }

        public string UnitMeasure { get; set; }

        public long? GroupWorkPprId { get; set; }
    }
}
