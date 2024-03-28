namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Enums;

    public class HeatInputInformation : BaseEntity
    {
        public virtual HeatInputPeriod HeatInputPeriod { get; set; }

        public virtual TypeHeatInputObject TypeHeatInputObject { get; set; }

        public virtual int Count { get; set; }

        public virtual int CentralHeating { get; set; }

        public virtual int IndividualHeating { get; set; }

        public virtual decimal Percent { get; set; }

        public virtual int NoHeating { get; set; }
    }
}