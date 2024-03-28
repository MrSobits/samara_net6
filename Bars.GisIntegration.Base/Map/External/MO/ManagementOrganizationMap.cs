namespace Bars.GisIntegration.Base.Map.External.MO
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Mo;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.ManagementOrganization
    /// </summary>
    public class ManagementOrganizationMap : BaseEntityMap<ManagementOrganization>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ManagementOrganizationMap() :
            base("MANAG_ORG")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("MANAG_ORG_ID");
                m.Generator(Generators.Native);
            });

            References(x => x.Contragent, "CONTRAGENT_ID");
            References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.ChairmanFio, "CHAIRMAN_FIO");
            this.Map(x => x.ChairmanPhone, "CHAIRMAN_PHONE");
            this.Map(x => x.ManagStaffCnt, "MANAG_STAFF_CNT");
            this.Map(x => x.EngineerCnt, "ENGINEER_CNT");
            this.Map(x => x.WorkerCnt, "WORKER_CNT");
            this.Map(x => x.RfCapitalShare, "RF_CAPITAL_SHARE");
            this.Map(x => x.MoCapitalShare, "MO_CAPITAL_SHARE");
            this.Map(x => x.IsTsg, "IS_TSG");
            this.Map(x => x.DispatchAddress, "DISPATCH_ADDRESS");
            this.Map(x => x.DispatchPhone, "DISPATCH_PHONE");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
