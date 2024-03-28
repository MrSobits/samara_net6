namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Решение суда ГЖИ"</summary>
    public class MKDLicTypeRequestMap : BaseEntityMap<MKDLicTypeRequest>
    {
        
        public MKDLicTypeRequestMap() : 
                base("Тип запроса на внесение изменения в реестр лицензий", "GJI_DICT_MKD_LIC_TYPE_REQUEST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Code, "Код").Column("CODE").Length(5);
        }
    }
}
