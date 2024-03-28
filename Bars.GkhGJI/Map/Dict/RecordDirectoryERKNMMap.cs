namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    using Entities;

    /// <summary>Запись справочники ЕРКНМ </summary>
    public class RecordDirectoryERKNMMap : BaseEntityMap<RecordDirectoryERKNM>
    {
        
        public RecordDirectoryERKNMMap() : 
                base("Запись справочники ЕРКНМ", "GJI_DICT_REC_ERKNMM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
            Property(x => x.CodeERKNM, "Код ЕРКНМ").Column("CODE_ERKNM");
            Property(x => x.EntityId, "Айди объекта в системе").Column("ENTITY_ID");
            Property(x => x.IdentERKNM, "Идентификатор ЕРКНМ").Column("IDENT_ERKNM");
            Property(x => x.IdentSMEV, "Идентификатор СМЭВ").Column("IDENT_SMEV");
            Reference(x => x.DirectoryERKNM, "Идентификатор объекта в системе").Column("DICT_ERKNMM_ID");
        }
    }
}
