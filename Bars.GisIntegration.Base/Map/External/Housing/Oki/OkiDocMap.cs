namespace Bars.GisIntegration.Base.Map.External.Housing.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.OkiDocMap
    /// </summary>
    public class OkiDocMap : BaseEntityMap<OkiDoc>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkiDocMap() :
            base("OKI_DOC")
        {
            //Устанавливаем схему
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKI_DOC_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.References(x => x.OkiObject, "OKI_OBJECT_ID");
            this.References(x => x.OkiDocType, "OKI_DOC_TYPE_ID");
            this.References(x => x.Attachment, "ATTACHMENT_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
