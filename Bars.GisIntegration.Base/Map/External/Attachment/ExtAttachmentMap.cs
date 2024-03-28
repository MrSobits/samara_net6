namespace Bars.GisIntegration.Base.Map.External.Attachment
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Attachment;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.Attachment
    /// </summary>
    public class ExtAttachmentMap : BaseEntityMap<ExtAttachment>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExtAttachmentMap() :
            base("ATTACHMENT")
        {
            //Устанавливаем схему MASTER
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("ATTACHMENT_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.Name, "NAME");
            this.Map(x => x.Note, "NOTE");
            this.Map(x => x.Guid, "GUID");
            this.Map(x => x.Hash, "HASH");
            this.References(x => x.FileInfo, "FILE_INFO_ID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
