namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для справочника Объект запроса ЕГРН </summary>
    public class EGRNObjectTypeMap : BaseEntityMap<EGRNObjectType>
    {
        
        public EGRNObjectTypeMap() : 
                base("Объект запроса ЕГРН", "GJI_CH_DICT_EGRN_OBJECT_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
            Property(x => x.Description, "Комментарий").Column("DESCRIPTION");
        }
    }
}
