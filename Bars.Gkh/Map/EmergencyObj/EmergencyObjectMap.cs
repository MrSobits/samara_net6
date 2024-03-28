/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.EmergencyObj
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг сущности "Аварийность жилого дома"
///     /// </summary>
///     public class EmergencyObjectMap : BaseGkhEntityMap<EmergencyObject>
///     {
///         public EmergencyObjectMap() : base("GKH_EMERGENCY_OBJECT")
///         {
///             Map(x => x.CadastralNumber, "CADASTRAL_NUMBER").Length(300);
///             Map(x => x.ActualInfoDate, "ACTUAL_INFO_DATE");
///             Map(x => x.DemolitionDate, "DEMOLITION_DATE");
///             Map(x => x.Description, "DESCRIPTION").Length(300);
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNumber, "DOCUMENT_NUM").Length(300);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.IsRepairExpedient, "IS_REPAIR_EXPEDIENT").Not.Nullable();
///             Map(x => x.LandArea, "LAND_AREA");
///             Map(x => x.ResettlementFlatArea, "RESETTLEMENT_FLAT_AREA");
///             Map(x => x.ResettlementFlatAmount, "RESETTLEMENT_FLAT_AMOUNT");
///             Map(x => x.ConditionHouse, "CONDITION_HOUSE").Not.Nullable().CustomType<ConditionHouse>();
/// 
///             References(x => x.RealityObject, "REALTITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ReasonInexpedient, "REASON_INEXPEDIENT_ID").Fetch.Join();
///             References(x => x.FurtherUse, "FURTHER_USE_ID").Fetch.Join();
///             References(x => x.FileInfo, "FILE_INFO_ID").Fetch.Join();
///             References(x => x.ResettlementProgram, "RESETTLEMENT_PROG_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Аварийность жилого дома"</summary>
    public class EmergencyObjectMap : BaseImportableEntityMap<EmergencyObject>
    {
        
        public EmergencyObjectMap() : 
                base("Аварийность жилого дома", "GKH_EMERGENCY_OBJECT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.CadastralNumber, "Кадастровый номер").Column("CADASTRAL_NUMBER").Length(300);
            this.Property(x => x.ActualInfoDate, "Дата актуальности информации").Column("ACTUAL_INFO_DATE");
            this.Property(x => x.DemolitionDate, "Планируемая дата сноса/реконструкции МКД").Column("DEMOLITION_DATE");
            this.Property(x => x.ResettlementDate, "Планируемая дата завершения переселения").Column("RESETTLEMENT_DATE");
            this.Property(x => x.FactDemolitionDate, "Фактическая дата сноса/реконструкции МКД").Column("FACT_DEMOLITION_DATE");
            this.Property(x => x.FactResettlementDate, "Фактическая дата завершения переселения").Column("FACT_RESETTLEMENT_DATE");

            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(300);
            this.Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");

            this.Property(x => x.EmergencyDocumentName, "Наименование документа, подтверждающего признание МКД аварийным").Column("EMG_DOCUMENT_NAME").Length(300);
            this.Property(x => x.EmergencyDocumentNumber, "Номер документа, подтверждающего признание МКД аварийным").Column("EMG_DOCUMENT_NUM").Length(300);
            this.Property(x => x.EmergencyDocumentDate, "Дата документа, подтверждающего признание МКД аварийным").Column("EMG_DOCUMENT_DATE");

            this.Property(x => x.IsRepairExpedient, "Ремонт целесообразен").Column("IS_REPAIR_EXPEDIENT").NotNull();
            this.Property(x => x.LandArea, "Площадь земельного участка").Column("LAND_AREA");
            this.Property(x => x.ResettlementFlatArea, "Площадь расселяемых жилых помещений").Column("RESETTLEMENT_FLAT_AREA");
            this.Property(x => x.ResettlementFlatAmount, "Кол-во расселяемых жилых помещений").Column("RESETTLEMENT_FLAT_AMOUNT");
            this.Property(x => x.InhabitantNumber, "Число жителей планируемых к переселению").Column("INHABITANT_NUMBER");
            this.Property(x => x.ConditionHouse, "Состояние дома").Column("CONDITION_HOUSE").NotNull();
            this.Property(x => x.ExemptionsBasis, "Основание изъятия").Column("EXEMPTIONS_BASIS");

            this.Reference(x => x.RealityObject, "Объект недвижимости").Column("REALTITY_OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.ReasonInexpedient, "Основание нецелесообразности").Column("REASON_INEXPEDIENT_ID").Fetch();
            this.Reference(x => x.FurtherUse, "Дальнейшее использование").Column("FURTHER_USE_ID").Fetch();
            this.Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
            this.Reference(x => x.EmergencyFileInfo, "Файл документа, подтверждающего признание МКД аварийным").Column("EMG_FILE_INFO_ID").Fetch();
            this.Reference(x => x.ResettlementProgram, "Программа переселения").Column("RESETTLEMENT_PROG_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
