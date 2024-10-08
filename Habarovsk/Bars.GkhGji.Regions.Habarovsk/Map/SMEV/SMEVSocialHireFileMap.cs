namespace Bars.GkhGji.Regions.Habarovsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVSocialHireFileMap : BaseEntityMap<SMEVSocialHireFile>
    {
        
        public SMEVSocialHireFileMap() : 
                base("Файл запроса к ВС", "GJI_CH_SMEV_SOCIAL_HIRE_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVSocialHire, "ЗАПРОС").Column("SMEV_SOCIAL_HIRE_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
