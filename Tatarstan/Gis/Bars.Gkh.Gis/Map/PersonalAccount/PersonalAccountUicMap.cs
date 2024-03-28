/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.PersonalAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class PersonalAccountUicMap : BaseEntityMap<PersonalAccountUic>
///     {
///         public PersonalAccountUicMap()
///             : base("GIS_UIC_PERSONAL_ACCOUNT")
///         {
///             Map(x => x.Uic, "UIC");
///             Map(x => x.PersonalAccountId, "PERSONAL_ACC_ID");
///             Map(x => x.AccountNumber, "ACCOUNT_NUMBER");
///             Map(x => x.FlatNumber, "FLAT_NUMBER");
/// 
///             References(x => x.HouseRegister, "HOUSE_GIS_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.PersonalAccount.PersonalAccountUic"</summary>
    public class PersonalAccountUicMap : BaseEntityMap<PersonalAccountUic>
    {
        
        public PersonalAccountUicMap() : 
                base("Bars.Gkh.Gis.Entities.PersonalAccount.PersonalAccountUic", "GIS_UIC_PERSONAL_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Uic, "Uic").Column("UIC").Length(250);
            Reference(x => x.HouseRegister, "HouseRegister").Column("HOUSE_GIS_ID");
            Property(x => x.PersonalAccountId, "PersonalAccountId").Column("PERSONAL_ACC_ID");
            Property(x => x.AccountNumber, "AccountNumber").Column("ACCOUNT_NUMBER");
            Property(x => x.FlatNumber, "FlatNumber").Column("FLAT_NUMBER").Length(250);
        }
    }
}
