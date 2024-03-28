namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class AppealCitsAttachmentMap : BaseEntityMap<AppealCitsAttachment>
    {
        public AppealCitsAttachmentMap()
            : base("Скан(ы) обращения", "GJI_APPEAL_CITIZENS_FILES")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(500).NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);

            this.Property(x => x.Hash, "Hash").Column("HASH");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ Guid").Column("GIS_GKH_GUID").Length(36);

            this.Reference(x => x.AppealCits, "Обращение граждан").Column("CITIZENS_ID");
            this.Reference(x => x.FileInfo, "Описание файла").Column("FILE_ID").Fetch();
        }
    }
}
