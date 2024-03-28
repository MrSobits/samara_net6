/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "решение об отмены предписания ГЖИ"
///     /// </summary>
///     public class PrescriptionCancelMap : BaseGkhEntityMap<PrescriptionCancel>
///     {
///         public PrescriptionCancelMap() : base("GJI_PRESCRIPTION_CANCEL")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.DateCancel, "DATE_CANCEL");
///             Map(x => x.Reason, "REASON").Length(2000);
///             Map(x => x.IsCourt, "IS_COURT").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.TypeCancel, "TYPE_PRESCRIPTION").CustomType<TypePrescriptionCancel>();
///             Map(x => x.DateDecisionCourt, "DATE_DECISION_COURT");
///             Map(x => x.PetitionNumber, "PETITION_NUMBER");
///             Map(x => x.PetitionDate, "DATE_PETITION");
///             Map(x => x.DescriptionSet, "DESCRIPTION_SET");
///             Map(x => x.Prolongation, "TYPE_PROLONG").CustomType<TypeProlongation>();
///             Map(x => x.DateProlongation, "DATE_PROLONG");
/// 
///             References(x => x.Prescription, "PRESCRIPTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.IssuedCancel, "ISSUED_CANCEL_ID").Fetch.Join();
///             References(x => x.DecisionMakingAuthority, "DECIS_MAKE_AUTH_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Решение об отмене в предписании ГЖИ"</summary>
    public class PrescriptionCancelMap : BaseEntityMap<PrescriptionCancel>
    {
        
        public PrescriptionCancelMap() : 
                base("Решение об отмене в предписании ГЖИ", "GJI_PRESCRIPTION_CANCEL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Номер").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.DateCancel, "Дата отмены").Column("DATE_CANCEL");
            Property(x => x.Reason, "Причина отмены").Column("REASON").Length(2000);
            Property(x => x.IsCourt, "Отменено судом").Column("IS_COURT").NotNull();
            Property(x => x.TypeCancel, "Тип решения").Column("TYPE_PRESCRIPTION");
            Property(x => x.DateDecisionCourt, "Дата вступления в силу решения суда").Column("DATE_DECISION_COURT");
            Property(x => x.PetitionNumber, "Номер ходатайства").Column("PETITION_NUMBER");
            Property(x => x.PetitionDate, "Дата ходатайства").Column("DATE_PETITION");
            Property(x => x.DescriptionSet, "Установлено").Column("DESCRIPTION_SET");
            Property(x => x.Prolongation, "Продлено").Column("TYPE_PROLONG");
            Property(x => x.DateProlongation, "Продлить до").Column("DATE_PROLONG");
            Reference(x => x.Prescription, "Предписание").Column("PRESCRIPTION_ID").NotNull().Fetch();
            Reference(x => x.IssuedCancel, "Дл, вынесшее решение").Column("ISSUED_CANCEL_ID").Fetch();
            Reference(x => x.DecisionMakingAuthority, "Орган, вынесший решение").Column("DECIS_MAKE_AUTH_ID").Fetch();
        }
    }
}
