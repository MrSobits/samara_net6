namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Архив значений в мониторинге СМР"</summary>
    public class BuildControlTypeWorkSmrFileMap : BaseImportableEntityMap<BuildControlTypeWorkSmrFile>
    {
        
        public BuildControlTypeWorkSmrFileMap() : 
                base("Стройконтроль работы объекта КР", "CR_OBJ_CMP_BUILD_CONTR_FILE")
        {
        }

        protected override void Map()
        {
            Property(x => x.Description, "Примечание").Column("DESCRIPTION");
            Reference(x => x.BuildControlTypeWorkSmr, "Отчет СК").Column("SK_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.VideoLink, "Дистационное мероприятие").Column("VIDEOLINK");
        }
    }
}
