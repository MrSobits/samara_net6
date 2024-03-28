namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVFNSLicRequestMap : BaseEntityMap<SMEVFNSLicRequest>
    {
        
        public SMEVFNSLicRequestMap() : 
                base("Запрос к ВС ФНС", "GJI_CH_SMEV_FNS_LIC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.ManOrgLicense, "Лицензия").Column("LICENSE_ID").Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE");
            Property(x => x.FNSLicRequestType, "Тип запроса").Column("REQ_TYPE");
            Property(x => x.FNSLicDecisionType, "Тип решения").Column("DES_TYPE");
            Property(x => x.INN, "ИНН").Column("INN");
            Property(x => x.NameUL, "НаимЮЛПолн").Column("NAME_UL");
            Property(x => x.OGRN, "ОГРН").Column("OGRN");
            Property(x => x.KindLic, "ВидЛиц").Column("KIND_LIC");
            Property(x => x.DateLic, "ДатаЛиц").Column("DATE_LIC");
            Property(x => x.DateStartLic, "ДатаНачЛиц").Column("DATE_START_LIC");
            Property(x => x.DateEndLic, "ДатаОкончЛиц").Column("DATE_END_LIC");
            Property(x => x.FirstName, "FirstName ФЛ ИП").Column("FIRST_NAME");
            Property(x => x.FamilyName, "FamilyName ФЛ ИП").Column("FAMILY_NAME");
            Property(x => x.NumLic, "НомЛиц").Column("NUM_LIC");
            Property(x => x.SerLic, "СерЛиц").Column("SER_LIC");
            Property(x => x.SLVDCode, "КодСЛВД").Column("SLVD_CODE");
            Property(x => x.VDName, "НаимВД").Column("VD_NAME");
            Property(x => x.PrAction, "ПрДейств").Column("PR_ACTION");
            Property(x => x.Address, "АдресТекст").Column("ADDRESS");
            Property(x => x.DecisionKind, "ВидРеш").Column("DEC_KIND");
            Property(x => x.DecisionDateStart, "ДатаНачРеш").Column("DEC_START_DATE");
            Property(x => x.DecisionDateEnd, "ДатаОкончРеш").Column("DEC_END_DATE");
            Property(x => x.DecisionDate, "ДатаРеш").Column("DEC_DATE");
            Property(x => x.DecisionNum, "НомРеш").Column("DEC_NUM");
            Property(x => x.DecisionOrgLic, "ЛицОргРеш").Column("DEC_ORG_LIC");
            Property(x => x.LicOrgINN, "ИННЛО").Column("LIC_ORG_INN");
            Property(x => x.LicOrgFullName, "НаимЛОПолн").Column("LIC_ORG_FULL_NAME");
            Property(x => x.LicOrgShortName, "НаимЛОСокр").Column("LIC_ORG_SHORT_NAME");
            Property(x => x.LicOrgOGRN, "ОГРНЛО").Column("LIC_ORG_OGRN");
            Property(x => x.LicOrgOKOGU, "ОКОГУ").Column("LIC_ORG_OKOGU");
            Property(x => x.LicOrgRegion, "Регион").Column("LIC_ORG_REGION");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.IdDoc, "IdDoc").Column("ID_DOC");
            Property(x => x.FNSLicPersonType, "Тип лицензиата").Column("PERSON_TYPE");
            Property(x => x.DeleteIdDoc, "Удаление iddoc").Column("DELETE_ID_DOC");
        }
    }
}
