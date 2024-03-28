namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVDISKVLICMap : BaseEntityMap<SMEVDISKVLIC>
    {
        
        public SMEVDISKVLICMap() : 
                base("Запрос к ВС ФНС по дисквал. лицам", "GJI_CH_SMEV_DISKVLIC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            Property(x => x.BirthPlace, "Место Рождения").Column("BIRTH_PLACE");
            Property(x => x.DisqDays, "Срок диксв. дней").Column("DISQ_DAYS");
            Property(x => x.DisqMonths, "Срок дискв. месяц").Column("DISQ_MONTHS");
            Property(x => x.DisqYears, "Срок дискв. лет").Column("DISQ_YEARS");
            Property(x => x.EndDisqDate, "Дата окончания дисквалификации").Column("END_DISQ_DATE");
            Property(x => x.FamilyName, "Фамилия").Column("FAMILY_NAME");
            Property(x => x.FirstName, "Имя").Column("FIRST_NAME");
            Property(x => x.Patronymic, "Отчество").Column("PATRONYMIC");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.RequestId, "Guid запроса").Column("REQUEST_ID");
            Property(x => x.FormDate, "Дата формирования сведений из Реестра дисквалифицированных лиц").Column("FORM_DATE");
            Property(x => x.RegNumber, "Регистрационный номер записи в Реестре дисквалифицированных лиц").Column("REG_NUMBER");
            Property(x => x.Article, "Ст. КоАП").Column("ARTICLE");
            Property(x => x.LawDate, "Дата вынесения постановления").Column("LAW_DATE");
            Property(x => x.LawName, "Наименование суда, вынесшего постановление о дисквалификации").Column("LAW_NAME");
            Property(x => x.CaseNumber, "Номер дела").Column("CASE_NUMBER");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
