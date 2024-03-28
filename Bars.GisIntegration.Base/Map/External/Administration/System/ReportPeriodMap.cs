namespace Bars.GisIntegration.Base.Map.External.Administration.System
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.ReportPeriod
    /// </summary>
    public class ReportPeriodMap : BaseEntityMap<ReportPeriod>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ReportPeriodMap() :
            base("REPORT_PERIOD")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("REPORT_PERIOD_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.ReportMonth, "REPORT_MONTH");
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
        }
    }
}
