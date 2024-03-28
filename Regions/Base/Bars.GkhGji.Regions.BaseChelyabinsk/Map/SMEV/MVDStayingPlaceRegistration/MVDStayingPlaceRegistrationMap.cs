namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class MVDStayingPlaceRegistrationMap : BaseEntityMap<MVDStayingPlaceRegistration>
    {
        
        public MVDStayingPlaceRegistrationMap() : 
                base("Запрос к ВС МВД", "GJI_CH_SMEV_MVDSTAY_PLACEREG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.AnswerInfo, "Результат").Column("ANSWER_INFO");
            Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            Property(x => x.BirthPlace, "Место рождения").Column("BIRTH_PLACE");
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Reference(x => x.FileInfo, "FileInfo").Column("FILE_ID");
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.IssueDate, "Дата выдачи").Column("ISSUE_DATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.Name, "Имя").Column("NAME");
            Property(x => x.PassportNumber, "Номер паспорта").Column("PASSPORT_NUM").Length(6);
            Property(x => x.PassportSeries, "Серия паспорта").Column("PASSPORT_SERIES").Length(4);
            Property(x => x.PatronymicName, "Отчество").Column("PATRONYMIC");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.Surname, "Фамилия").Column("SURNAME");
        }
    }
}
