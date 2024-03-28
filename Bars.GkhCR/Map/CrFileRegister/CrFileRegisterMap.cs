namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Исполнитель документа ГЖИ"</summary>
    public class CrFileRegisterMap : BaseEntityMap<CrFileRegister>
    {
        
        public CrFileRegisterMap() : 
                base("Реестр файлов ОКР", "CR_FILE_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Дом").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.File, "Архив").Column("FILE_ID").Fetch();
            Property(x => x.DateFrom, "Дата c").Column("DATE_FROM");
            Property(x => x.DateTo, "Дата по").Column("DATE_TO");
        }
    }
}
