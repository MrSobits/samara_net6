/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount;
/// 
///     public class PersonalAccountCalcParam_tmpMap : BaseImportableEntityMap<PersonalAccountCalcParam_tmp>
///     {
///         public PersonalAccountCalcParam_tmpMap()
///             : base("REGOP_PERSACCALCPARAMTMP")
///         {
///             References(x => x.LoggedEntity, "LOGGED_ENTITY_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.PersonalAccount, "PERS_ACC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Временный объект для хранения использованных параметров. Создается при каждом вычислении тарифа и т.д. При закрытии периода используется как источник параметров для фиксирования, затем удаляется"</summary>
    public class PersonalAccountCalcParam_tmpMap : BaseEntityMap<PersonalAccountCalcParam_tmp>
    {
        
        public PersonalAccountCalcParam_tmpMap() : 
                base("Временный объект для хранения использованных параметров. Создается при каждом выч" +
                        "ислении тарифа и т.д. При закрытии периода используется как источник параметров " +
                        "для фиксирования, затем удаляется", "REGOP_PERSACCALCPARAMTMP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PersonalAccount, "PersonalAccount").Column("PERS_ACC_ID").NotNull().Fetch();
            Reference(x => x.LoggedEntity, "LoggedEntity").Column("LOGGED_ENTITY_ID").NotNull().Fetch();
        }
    }
}
