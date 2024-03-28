namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class AppealExecutionTypeMap : BaseEntityMap<AppealExecutionType>
    {
        
        public AppealExecutionTypeMap() : 
                base("Способ исполнения", "GJI_DICT_APPEAL_EXECUTION_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
        }
    }
}
