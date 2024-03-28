namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEGRIPMap : BaseEntityMap<SMEVEGRIP>
    {
        
        public SMEVEGRIPMap() : 
                base("Запрос к ВС ЕГРИП", "GJI_CH_SMEV_EGRIP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.INNReq, "ИНН запроса").Column("REQ_INN").NotNull();
            Property(x => x.AddressRegOrg, "Адрес регистрирующего органа").Column("REG_ORG_ADDRESS");
            Property(x => x.CodeRegOrg, "Код регистрирующего органа").Column("REG_ORG_CODE");
            Property(x => x.CreateWayName, "Способ образования ЮЛ").Column("CREATE_WAY_NAME");
            Property(x => x.FIO, "ФИО Должностного лица").Column("FIO");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.OGRN, "ОРГН").Column("OGRN");
            Property(x => x.OGRNDate, "Дата ОРГН").Column("OGRN_DATE");
            Property(x => x.Citizenship, "Гражданство").Column("CITIZENSHIP");
            Property(x => x.IPType, "Тип ИП").Column("IP_TYPE");
            Property(x => x.RegionName, "Наименование региона").Column("REGION_NAME");
            Property(x => x.RegionType, "Тип региона").Column("REGION_TYPE");
            Property(x => x.CityName, "Наименование населённого пункта").Column("CITY_NAME");
            Property(x => x.CityType, "Тип населённого пункта").Column("CITY_TYPE");
            Property(x => x.OKVEDCodes, "Коды ОКВЭД").Column("OKVED_CODES");
            Property(x => x.OKVEDNames, "Наименования ОКВЭД").Column("OKVED_NAMES");
            Property(x => x.RegOrgName, "Наименование регистрирующего органа").Column("REG_ORG_NAME");
            Property(x => x.ResponceDate, "Дата формирования выписки").Column("RESPONCE_DATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.InnOgrn, "InnOgrn").Column("INN_OGRN");
            Reference(x => x.XmlFile, "XML выписки").Column("XML_FILE");

        }
    }
}
