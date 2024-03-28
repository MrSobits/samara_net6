namespace Bars.GisIntegration.Base.Map.External.Rso
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Rso;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.RsoCompany
    /// </summary>
    public class RsoCompanyMap : BaseEntityMap<RsoCompany>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RsoCompanyMap() :
            base("RSO_COMPANY")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("RSO_COMPANY_ID");
                m.Generator(Generators.Native);
            });
            References(x => x.Contragent, "CONTRAGENT_ID");
            References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.FirgpDocNumber, "FIRGP_DOC_NUMBER");
            this.Map(x => x.FirgpIncludedOn, "FIRGP_INCLUDED_ON");
            this.Map(x => x.RsemDocNumber, "RSEM_DOC_NUMBER");
            this.Map(x => x.RsemIncludedOn, "RSEM_INCLUDED_ON");
            this.Map(x => x.DispatchAddress, "DISPATCH_ADDRESS");
            this.Map(x => x.DispatchPhone, "DISPATCH_PHONE");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }
    }
}
