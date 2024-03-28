/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount;
/// 
///     public class ViewPersonalAccountMap : PersistentObjectMap<ViewPersonalAccount>
///     {
///         public ViewPersonalAccountMap()
///             : base("view_regop_personal_account")
///         {
///             Map(x => x.RoomId, "room_id");
///             Map(x => x.RoId, "ro_id");
///             Map(x => x.MuId, "mu_id");
///             Map(x => x.SettleId, "stl_id");
///             Map(x => x.OwnerId, "owner_id");
///             Map(x => x.PrivilegedCategoryId, "priv_cat_id");
/// 
///             Map(x => x.Address, "address");
///             Map(x => x.Municipality, "municipality");
///             Map(x => x.Settlement, "settlement");
///             Map(x => x.RoomAddress, "room_address");
///             Map(x => x.RoomNum, "croom_num");
///             Map(x => x.PlaceName, "place_name");
///             Map(x => x.StreetName, "street_name");
/// 
///             Map(x => x.AccountOwner, "owner_name");
///             Map(x => x.OwnerType, "owner_type");
/// 
///             Map(x => x.Area, "carea");
/// 
///             Map(x => x.PersonalAccountNum, "acc_num");
///             Map(x => x.AreaShare, "area_share");
///             Map(x => x.OpenDate, "open_date");
///             Map(x => x.CloseDate, "close_date");
///             Map(x => x.PersAccNumExternalSystems, "external_number");
///             Map(x => x.HasCharges, "has_charges");
/// 
///             References(x => x.State, "state_id");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PersonalAccount.ViewPersonalAccount"</summary>
    public class ViewPersonalAccountMap : PersistentObjectMap<ViewPersonalAccount>
    {
        
        public ViewPersonalAccountMap() : 
                base("Bars.Gkh.RegOperator.Entities.PersonalAccount.ViewPersonalAccount", "VIEW_REGOP_PERSONAL_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RoomId, "RoomId").Column("ROOM_ID");
            Property(x => x.RoId, "RoId").Column("RO_ID");
            Property(x => x.MuId, "MuId").Column("MU_ID");
            Property(x => x.SettleId, "SettleId").Column("STL_ID");
            Property(x => x.OwnerId, "OwnerId").Column("OWNER_ID");
            Property(x => x.PrivilegedCategoryId, "PrivilegedCategoryId").Column("PRIV_CAT_ID");
            Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            Property(x => x.Municipality, "Municipality").Column("MUNICIPALITY").Length(250);
            Property(x => x.Settlement, "Settlement").Column("SETTLEMENT").Length(250);
            Property(x => x.RoomAddress, "RoomAddress").Column("ROOM_ADDRESS").Length(250);
            Property(x => x.RoomNum, "RoomNum").Column("CROOM_NUM").Length(250);
            Property(x => x.PlaceName, "PlaceName").Column("PLACE_NAME").Length(250);
            Property(x => x.StreetName, "StreetName").Column("STREET_NAME").Length(250);
            Property(x => x.AccountOwner, "AccountOwner").Column("OWNER_NAME").Length(250);
            Property(x => x.OwnerType, "OwnerType").Column("OWNER_TYPE");
            Property(x => x.Area, "Area").Column("CAREA");
            Property(x => x.PersonalAccountNum, "PersonalAccountNum").Column("ACC_NUM").Length(250);
            Property(x => x.AreaShare, "AreaShare").Column("AREA_SHARE");
            Property(x => x.OpenDate, "OpenDate").Column("OPEN_DATE");
            Property(x => x.CloseDate, "CloseDate").Column("CLOSE_DATE");
            Reference(x => x.State, "State").Column("STATE_ID");
            Property(x => x.HasCharges, "HasCharges").Column("HAS_CHARGES");
            Property(x => x.PersAccNumExternalSystems, "PersAccNumExternalSystems").Column("EXTERNAL_NUMBER").Length(250);
        }
    }
}
