namespace Bars.GkhGji.Regions.Habarovsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVChangePremisesStateFileMap : BaseEntityMap<SMEVChangePremisesStateFile>
    {
        
        public SMEVChangePremisesStateFileMap() : 
                base("Запрос к ВС СГИО о переводe статуса помещения", "GJI_CH_SMEV_CHANGE_PREM_STATE_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVChangePremisesState, "ЗАПРОС").Column("SMEV_CHANGE_PREM_STATE_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
