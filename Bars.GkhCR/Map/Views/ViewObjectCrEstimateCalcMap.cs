namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на Объект КР"</summary>
    public class ViewSpecialObjCrEstimateCalcMap : PersistentObjectMap<ViewSpecialObjCrEstimateCalc>
    {
        public ViewSpecialObjCrEstimateCalcMap() : 
                base("Вьюха на Объект КР", "VIEW_CR_OBJECT_EST_CALC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.RealityObjName, "Жилой дом").Column("ADDRESS");
            this.Property(x => x.RealityObjectId, "Id жилого дома").Column("REALITY_OBJECT_ID");
            this.Property(x => x.ObjectCrId, "Объект КР").Column("OBJECT_ID");
            this.Property(x => x.MunicipalityId, "Муниципальное образование Id").Column("MUNICIPALITY_ID");
            this.Property(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_NAME");
            this.Property(x => x.SettlementId, "Муниципальный район Id").Column("SETTLEMENT_ID");
            this.Property(x => x.SettlementName, "Муниципальный район Название").Column("SETTLEMENT_NAME");
            this.Property(x => x.TypeWorkCrCount, "Количество Видов Работ по ОКР").Column("CNT_TW");
            this.Property(x => x.EstimateCalculationsCount, "Количество Сметных расчетов по ОКР").Column("CNT_EST_CALC");
            this.Property(x => x.ProgramCrId, "Программа КР").Column("PROGRAM_ID");
            this.Property(x => x.ProgramCrName, "Программа КР").Column("PROGRAM_NAME");
        }
    }
}
