using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;

namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    public class MKDLicRequestAnswerAttachmentMap : BaseEntityMap<MKDLicRequestAnswerAttachment>
    {
        public MKDLicRequestAnswerAttachmentMap()
            : base("Вложения ответов", "GJI_MKD_LIC_REQUEST_ANSWER_FILES")
        {

        }

        protected override void Map()
        {
            this.Reference(x => x.MKDLicRequestAnswer, "Ответ").Column("MKD_LIC_REQUEST_ANSWER_ID").NotNull();
            this.Reference(x => x.FileInfo, "Описание файла").Column("FILE_INFO_ID").NotNull().Fetch();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(250).NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
        }
    }
}
