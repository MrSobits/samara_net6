namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.AttachmentField"</summary>
    public class AttachmentFieldMap : BaseEntityMap<AttachmentField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AttachmentFieldMap()
            : base("Sobits.GisGkh.Entities", AttachmentFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_ATTACHMENT_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(AttachmentField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(AttachmentField.Name).ToLower()).NotNull();
            this.Property(x => x.Description, "Описание").Column(nameof(AttachmentField.Description).ToLower());
            this.Reference(x => x.Attachment, "Файл приложения").Column(nameof(AttachmentField.Attachment).ToLower()).Fetch();
            this.Property(x => x.Hash, "Hash").Column(nameof(AttachmentField.Hash).ToLower());
            this.Property(x => x.Guid, "Guid").Column(nameof(AttachmentField.Guid).ToLower()).NotNull();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class AttachmentFieldNhMapping : ClassMapping<AttachmentField>
    {
        public AttachmentFieldNhMapping()
        {
            this.Schema(AttachmentFieldMap.SchemaName);
        }
    }
}