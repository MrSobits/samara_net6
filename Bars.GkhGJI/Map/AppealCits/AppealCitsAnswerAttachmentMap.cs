using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.Map
{
    public class AppealCitsAnswerAttachmentMap : BaseEntityMap<AppealCitsAnswerAttachment>
    {
        public AppealCitsAnswerAttachmentMap()
            : base("Вложения ответов", "GJI_APPEAL_CITIZEN_ANSWER_FILES")
        {

        }

        protected override void Map()
        {
            this.Reference(x => x.AppealCitsAnswer, "Ответ").Column("ANSWER_ID").NotNull();
            this.Reference(x => x.FileInfo, "Описание файла").Column("FILE_INFO_ID").NotNull().Fetch();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(250).NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.UniqueName, "Путь к файлу на ftp").Column("UNIQUE_NAME").Length(300);
        }
    }
}
