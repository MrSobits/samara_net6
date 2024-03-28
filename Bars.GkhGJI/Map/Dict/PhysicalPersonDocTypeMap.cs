namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для справочника типов документа физлица</summary>
    public class PhysicalPersonDocTypeMap : BaseEntityMap<PhysicalPersonDocType>
    {
        
        public PhysicalPersonDocTypeMap() : 
                base("Тип документа физлица", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
        }
    }
}
