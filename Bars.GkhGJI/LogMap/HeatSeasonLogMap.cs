namespace Bars.GkhGji.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;
    using B4.Utils;

    public class HeatSeasonLogMap : AuditLogMap<HeatSeason>
    {
        public HeatSeasonLogMap()
        {
            this.Name("Документы по подготовке к отопительному сезону");
            this.Description(x => x.ReturnSafe(y => y.RealityObject.Address));

            this.MapProperty(x => x.RealityObject, "RealityObject", "Адрес", x => x.Return(y => y.Address));
            this.MapProperty(x => x.Period, "Period", "Период", x => x.Return(y => y.Name));
        }
    }
}
