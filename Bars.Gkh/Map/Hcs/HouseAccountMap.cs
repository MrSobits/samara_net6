/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Hcs
/// {
///     using Bars.Gkh.Enums;
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities.Hcs;
/// 
///     public class HouseAccountMap : BaseImportableEntityMap<HouseAccount>
///     {
///         public HouseAccountMap() : base("HCS_HOUSE_ACCOUNT")
///         {
///             Map(x => x.OwnerNumber, "OWNER_NUMBER");
///             Map(x => x.PaymentCode, "PAYMENT_CODE");
///             Map(x => x.Apartment, "APARTMENT");
///             Map(x => x.Living, "LIVING");
///             Map(x => x.ResidentsCount, "RESIDENTS_COUNT");
///             Map(x => x.HouseStatus, "HOUSE_STATUS");
///             Map(x => x.ApartmentArea, "APARTMENT_AREA");
///             Map(x => x.LivingArea, "LIVING_AREA");
///             Map(x => x.RoomsCount, "ROOMS_COUNT");
///             Map(x => x.AccountState, "ACCOUNT_STATE");
///             Map(x => x.Privatizied, "PRIVATIZED");
///             Map(x => x.TemporaryGoneCount, "TEMPORARY_GONE_COUNT");
///             Map(x => x.OpenAccountDate, "OPEN_ACC_DATE");
///             Map(x => x.CloseAccountDate, "CLOSE_ACC_DATE");
///             Map(x => x.OwnershipPercentage, "PERCENTAGE");
///             Map(x => x.ContractDate, "CONTRACT_DATE");
///             Map(x => x.HouseAccountNumber, "HCS_NUMBER");
///             Map(x => x.OwnerName, "OWNER_NAME");
///             Map(x => x.OwnerType, "OWNER_TYPE").Not.Nullable().CustomType<OwnerType>();
///             Map(x => x.PersonalAccountNum, "PERS_ACC_NUM").Length(30);
/// 
///             References(x => x.FiasMailingAddress, "FIAS_MAIL_ADDRESS_ID").Fetch.Join().Cascade.Delete();
///             References(x => x.FiasFullAddress, "FIAS_FULL_ADDRESS_ID").Fetch.Join().Cascade.Delete();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Hcs
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Hcs;
    
    
    /// <summary>Маппинг для "Лицевой счет дома"</summary>
    public class HouseAccountMap : BaseImportableEntityMap<HouseAccount>
    {
        
        public HouseAccountMap() : 
                base("Лицевой счет дома", "HCS_HOUSE_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OwnerNumber, "Номер собственника (1, 2, 3, ..)").Column("OWNER_NUMBER");
            Property(x => x.PaymentCode, "Платежный код").Column("PAYMENT_CODE");
            Property(x => x.Apartment, "Номер квартиры").Column("APARTMENT");
            Property(x => x.Living, "Жилое помещение").Column("LIVING");
            Property(x => x.ResidentsCount, "Количество проживающих/прописанных").Column("RESIDENTS_COUNT");
            Property(x => x.HouseStatus, "Статус жилья").Column("HOUSE_STATUS");
            Property(x => x.ApartmentArea, "Общая площадь квартиры").Column("APARTMENT_AREA");
            Property(x => x.LivingArea, "Жилая площадь").Column("LIVING_AREA");
            Property(x => x.RoomsCount, "Количество комнат").Column("ROOMS_COUNT");
            Property(x => x.AccountState, "Состояние счета").Column("ACCOUNT_STATE");
            Property(x => x.Privatizied, "Приватизированная (да/нет)").Column("PRIVATIZED");
            Property(x => x.TemporaryGoneCount, "Количество временно убывших").Column("TEMPORARY_GONE_COUNT");
            Property(x => x.OpenAccountDate, "Дата открытия ЛС").Column("OPEN_ACC_DATE");
            Property(x => x.CloseAccountDate, "Дата закрытия ЛС").Column("CLOSE_ACC_DATE");
            Property(x => x.OwnershipPercentage, "Доля собственности").Column("PERCENTAGE");
            Property(x => x.ContractDate, "Дата заключения договора").Column("CONTRACT_DATE");
            Property(x => x.HouseAccountNumber, "Номер").Column("HCS_NUMBER");
            Property(x => x.OwnerName, "Имя собственника (ФИО/Наименование организации)").Column("OWNER_NAME");
            Property(x => x.OwnerType, "Тип собственника").Column("OWNER_TYPE").NotNull();
            Property(x => x.PersonalAccountNum, "Лицевой счет").Column("PERS_ACC_NUM").Length(30);
            Reference(x => x.FiasMailingAddress, "Почтовый адрес ФИАС").Column("FIAS_MAIL_ADDRESS_ID").Fetch();
            Reference(x => x.FiasFullAddress, "Полный адрес ФИАС").Column("FIAS_FULL_ADDRESS_ID").Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
