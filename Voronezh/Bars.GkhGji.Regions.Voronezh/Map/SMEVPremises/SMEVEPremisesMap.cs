namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVPremises;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEPremisesMap : BaseEntityMap<SMEVPremises>
    {
        
        public SMEVEPremisesMap() : 
                base("Запрос к ВС помещения", "GJI_SMEV_PREMISES")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.AnswerFile, "Файл").Column("ANS_FILE_ID").Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.ActDate, "Дата акта").Column("ACT_DATE").NotNull();
            Property(x => x.ActDepartment, "Департамент").Column("ACT_DEPARTMENT");
            Property(x => x.ActName, "Наименование").Column("ACT_NAME");
            Property(x => x.ActNumber, "Номер Акта").Column("ACT_NUMBER");
            Property(x => x.OKTMO, "ОКТМО").Column("OKTMO");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
