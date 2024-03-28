namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEGRNLogMap : BaseEntityMap<SMEVEGRNLog>
    {
        
        public SMEVEGRNLogMap() : 
                base("Файл запроса к ВС Предоставления данных из ФГИС ЕГРП", "GJI_CH_SMEV_EGRN_LOG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVEGRN, "ЗАПРОС").Column("SMEV_EGRN_ID").NotNull();
            Property(x => x.OperationType, "Тип файла").Column("OP_TYPE").NotNull();
            Property(x => x.Login, "Тип файла").Column("USER_LOGIN").NotNull();
            Property(x => x.UserName, "Тип файла").Column("USER_NAME").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID");
        }
    }
}
