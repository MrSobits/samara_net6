/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
///     using Bars.GkhCr.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Лицевой счет"
///     /// </summary>
///     public class PersonalAccountMap : BaseGkhEntityMap<PersonalAccount>
///     {
///         public PersonalAccountMap() : base("CR_OBJ_PERS_ACCOUNT")
///         {
///             Map(x => x.FinanceGroup, "TYPE_FIN_GROUP").Not.Nullable().CustomType<TypeFinanceGroup>();
///             Map(x => x.Closed, "CLOSED").Not.Nullable();
///             Map(x => x.Account, "ACCOUNT").Length(300);
/// 
///             References(x => x.ObjectCr, "OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Лицевой счет"</summary>
    public class PersonalAccountMap : BaseImportableEntityMap<PersonalAccount>
    {
        
        public PersonalAccountMap() : 
                base("Лицевой счет", "CR_OBJ_PERS_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.FinanceGroup, "Группа финансирования").Column("TYPE_FIN_GROUP").NotNull();
            Property(x => x.Closed, "Счет закрыт").Column("CLOSED").NotNull();
            Property(x => x.Account, "Лицевой счет").Column("ACCOUNT").Length(300);
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
        }
    }
}
