namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVSNILSMap : BaseEntityMap<SMEVSNILS>
    {
        
        public SMEVSNILSMap() : 
                base("Запрос к ВС МВД", "GJI_CH_SMEV_SNILS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();          
            Property(x => x.BirthDate, "BirthDate").Column("BIRTH_DATE");
            Property(x => x.Country, "Country").Column("COUNTRY");
            Property(x => x.District, "District").Column("DISTRICT");
            Property(x => x.IssueDate, "IssueDate").Column("ISSUE_DATE");
            Property(x => x.Issuer, "Issuer").Column("ISSUER");
            Property(x => x.SNILS, "СНИЛС").Column("SNILS");
            Property(x => x.Surname, "Фамилия").Column("SURNAME");
            Property(x => x.Number, "Number").Column("NUMBER");
            Property(x => x.Region, "Region").Column("REGION");
            Property(x => x.Series, "Series").Column("SERIES");
            Property(x => x.Settlement, "Settlement").Column("SETTELMENT");
            Property(x => x.SMEVGender, "SMEVGender").Column("GENDER");
            Property(x => x.SnilsPlaceType, "SnilsPlaceType").Column("PLACE_TYPE");
            Property(x => x.Name, "Имя").Column("NAME");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.PatronymicName, "Отчество").Column("PATRONYMIC");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
