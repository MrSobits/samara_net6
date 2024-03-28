namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVNDFLMap : BaseEntityMap<SMEVNDFL>
    {
        
        public SMEVNDFLMap() : 
                base("Запрос к ВС ФНС по 2-ндфл", "GJI_CH_SMEV_NDFL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.DocumentCode, "Код документа").Column("DOCUMENT_CODE");
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            Property(x => x.PeriodYear, "Отчетный год").Column("PERIOD_YEAR");
            Property(x => x.ServiceCode, "Код (идентификатор) государственной услуги").Column("SERVICE_CODE");
            Property(x => x.SNILS, "СНИЛС").Column("SNILS");
            Property(x => x.RegDate, "Дата заявления").Column("REG_DATE");
            Property(x => x.FamilyName, "Фамилия").Column("FAMILY_NAME");
            Property(x => x.FirstName, "Имя").Column("FIRST_NAME");
            Property(x => x.Patronymic, "Отчество").Column("PATRONYMIC");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.RequestId, "Guid запроса").Column("REQUEST_ID");
            Property(x => x.SeriesNumber, "Серия").Column("SERIES_NUMBER");
            Property(x => x.RegNumber, "Регистрационный номер записи в Реестре дисквалифицированных лиц").Column("REG_NUMBER");
            Property(x => x.INNUL, "ИННЮЛ").Column("INNUL");
            Property(x => x.KPP, "КПП").Column("KPP");
            Property(x => x.OrgName, "Наименование организации").Column("ORG_NAME");
            Property(x => x.Rate, "Ставка").Column("RATE");
            Property(x => x.RevenueCode, "КодДохода").Column("REVENUE_CODE");
            Property(x => x.Month, "Месяц").Column("MONTH");
            Property(x => x.RevenueSum, "СумДохода").Column("REVENUE_SUM");
            Property(x => x.RecoupmentCode, "КодВычета").Column("RECOUPMENT_CODE");
            Property(x => x.RecoupmentSum, "СумВычета").Column("RECOUPMENT_SUM");
            Property(x => x.DutyBase, "НалБаза").Column("DUTY_BASE");
            Property(x => x.DutySum, "Сумма налога исчисленная").Column("DUTY_SUM");
            Property(x => x.UnretentionSum, "Сумма налога, не удержанная налоговым агентом").Column("UNRETENTION_SUM");
            Property(x => x.RevenueTotalSum, "СумДохОбщ").Column("REVENUE_TOTAL_SUM");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
