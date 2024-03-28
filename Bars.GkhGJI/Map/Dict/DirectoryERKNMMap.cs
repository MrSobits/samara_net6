namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    using Entities;

    /// <summary>Маппинг для справочника Категория подателей заявления </summary>
    public class DirectoryERKNMMap : BaseEntityMap<DirectoryERKNM>
    {
        
        public DirectoryERKNMMap() : 
                base("Справочники ЕРКНМ", "GJI_DICT_ERKNMM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
            Property(x => x.CodeERKNM, "Код ЕРКНМ").Column("CODE_ERKNM");
            Property(x => x.EntityName, "Наименование объекта в системе").Column("ENTITY_NAME");
        }
    }
}
