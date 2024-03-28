namespace Bars.GisIntegration.Base.Map.External.Housing.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.SourceReceiver
    /// </summary>
    public class SourceReceiverMap : BaseEntityMap<SourceReceiver>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SourceReceiverMap() :
            base("OKI_SOURCE_RECEIVER")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_SOURCE_RECEIVER_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.OkiObject, "OKI_OBJECT_ID");
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.References(x => x.ChildOkiObject, "CHILD_OKI_ID");
            this.Map(x => x.IsSource, "IS_SOURCE");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
