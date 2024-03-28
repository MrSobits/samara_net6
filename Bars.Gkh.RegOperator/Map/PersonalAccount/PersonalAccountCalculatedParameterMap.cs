/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PersonalAccountCalculatedParameterMap : BaseImportableEntityMap<PersonalAccountCalculatedParameter>
///     {
///         public PersonalAccountCalculatedParameterMap() : base("REGOP_PERS_ACC_CALC_PARAM")
///         {
///             References(x => x.LoggedEntity, "LOGGED_ENTITY_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.PersonalAccount, "PERS_ACC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PersonalAccountCalculatedParameter"</summary>
    public class PersonalAccountCalculatedParameterMap : BaseImportableEntityMap<PersonalAccountCalculatedParameter>
    {
        
        public PersonalAccountCalculatedParameterMap() : 
                base("Bars.Gkh.RegOperator.Entities.PersonalAccountCalculatedParameter", "REGOP_PERS_ACC_CALC_PARAM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PersonalAccount, "PersonalAccount").Column("PERS_ACC_ID").NotNull().Fetch();
            Reference(x => x.LoggedEntity, "LoggedEntity").Column("LOGGED_ENTITY_ID").NotNull().Fetch();
        }
    }
}
