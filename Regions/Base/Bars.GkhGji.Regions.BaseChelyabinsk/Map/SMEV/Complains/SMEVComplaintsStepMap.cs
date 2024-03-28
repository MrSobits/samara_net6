namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVComplaintsStepMap : BaseEntityMap<SMEVComplaintsStep>
    {
        
        public SMEVComplaintsStepMap() : 
                base("", "SMEV_CH_COMPLAINTS_STEP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVComplaints, "AppealCits").Column("COMPLAINT_ID").NotNull();
            Reference(x => x.FileInfo, "AppealCits").Column("FILE_ID");
            Property(x => x.Reason, "Код").Column("REASON");
            Property(x => x.AddDocList, "Код").Column("DOCLIST");
            Property(x => x.NewDate, "Номер проверки").Column("NEW_DATE");
            Property(x => x.DOPetitionResult, "Пояснительный текст к жалобе").Column("TYPE_RESULT");
            Property(x => x.YesNo, "Пояснительный текст к жалобе").Column("SENDED");
            Property(x => x.DOTypeStep, "Номер проверки").Column("TYPE_STEP");
        }
    }
}
