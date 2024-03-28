namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для справочника Тип документа ЕГРН</summary>
    public class EGRNDocTypeMap : BaseEntityMap<EGRNDocType>
    {
        
        public EGRNDocTypeMap() : 
                base("Тип документа ЕГРН", "GJI_CH_DICT_EGRN_DOC_TYPE")
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
