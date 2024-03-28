namespace Bars.GkhGji.Regions.Tatarstan.Map.RapidResponseSystem
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    public class RapidResponseSystemAppealResponseMap : BaseEntityMap<RapidResponseSystemAppealResponse>
    {
        /// <inheritdoc />
        public RapidResponseSystemAppealResponseMap()
            : base(nameof(RapidResponseSystemAppealResponse), "GJI_RAPID_RESPONSE_SYSTEM_APPEAL_RESPONSE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.Response, "Ответ").Column("RESPONSE");
            Property(x => x.Theme, "Тема").Column("THEME");
            Property(x => x.ResponseDate, "Дата ответа").Column("RESPONSE_DATE");
            Property(x => x.CarriedOutWork, "Проведенные работы").Column("CARRIED_OUT_WORK");
            Reference(x => x.RapidResponseSystemAppealDetails, "Детали обращения в СОПР").Column("RAPID_RESPONSE_SYSTEM_APPEAL_DETAILS_ID").NotNull();
            Reference(x => x.ResponseFile, "Файл").Column("FILE_ID");
        }
    }
}