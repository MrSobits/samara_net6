namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEGRULMap : BaseEntityMap<SMEVEGRUL>
    {
        
        public SMEVEGRULMap() : 
                base("Запрос к ВС ЕРГЮЛ", "GJI_CH_SMEV_EGRUL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.INNReq, "ИНН запроса").Column("REQ_INN").NotNull();
            Property(x => x.AddressRegOrg, "Адрес регистрирующего органа").Column("REG_ORG_ADDRESS");
            Property(x => x.AddressUL, "Адрес ЮЛ").Column("ADDRESS_UL");
            Property(x => x.AuthorizedCapitalAmmount, "Размер уставного капиталла").Column("AUT_CAPITAL_AMMOUNT");
            Property(x => x.AuthorizedCapitalType, "Тип уставного капиталла").Column("CAPITAL_TYPE");
            Property(x => x.CodeRegOrg, "Код регистрирующего органа").Column("REG_ORG_CODE");
            Property(x => x.CreateWayName, "Способ образования ЮЛ").Column("CREATE_WAY_NAME");
            Property(x => x.FIO, "ФИО Должностного лица").Column("FIO");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.INN, "ИНН ЮЛ").Column("INN");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.KPP, "КПП").Column("KPP");
            Property(x => x.Name, "Полное наименование ЮЛ").Column("NAME");
            Property(x => x.OGRN, "ОРГН").Column("OGRN");
            Property(x => x.OGRNDate, "Дата присвоения ОГРН").Column("OGRN_DATE");
            Property(x => x.OKVEDCodes, "Коды ОКВЭД").Column("OKVED_CODES");
            Property(x => x.OKVEDNames, "Наименования ОКВЭД").Column("OKVED_NAMES");
            Property(x => x.OPFName, "Наименование организиционно-правовой формы").Column("OPF_NAME");
            Property(x => x.Pozition, "Должность").Column("POZITION");
            Property(x => x.RegOrgName, "Наименование регистрирующего органа").Column("REG_ORG_NAME");
            Property(x => x.ResponceDate, "Дата формирования выписки").Column("RESPONCE_DATE");
            Property(x => x.ShortName, "Краткое наименование ЮЛ").Column("SHORT_NAME");
            Property(x => x.StateNameUL, "Статус ЮЛ").Column("STATE_UL");
            Property(x => x.TypePozitionName, "Наименование вида должности").Column("TYPE_POZITION_NAME");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.InnOgrn, "InnOgrn").Column("INN_OGRN");
            Reference(x => x.XmlFile, "XML выписки").Column("XML_FILE");

        }
    }
}
