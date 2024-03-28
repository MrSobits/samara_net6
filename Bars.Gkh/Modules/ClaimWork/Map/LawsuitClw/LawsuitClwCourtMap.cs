/// <mapping-converter-backup>
/// namespace Bars.Gkh.ClaimWork.Map
/// {
///     using Bars.Gkh.Modules.ClaimWork.Entities;
///     using B4.DataAccess.ByCode;
/// 
///     public class LawsuitClwCourtMap : BaseEntityMap<LawsuitClwCourt>
///     {
///         public LawsuitClwCourtMap()
///             : base("CLW_LAWSUIT_COURT")
///         {
///             Map(x => x.LawsuitCourtType, "COURT_TYPE", true, (object)0);
///             Map(x => x.DocDate, "DOC_DATE");
///             Map(x => x.DocNumber, "DOC_NUMBER", false, 100);
///             Map(x => x.Description, "DESCRIPTION", false, 2000);
/// 
///             References(x => x.DocumentClw, "DOCUMENT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    
    
    /// <summary>Маппинг для "Исковое зявление"</summary>
    public class LawsuitClwCourtMap : BaseEntityMap<LawsuitClwCourt>
    {
        
        public LawsuitClwCourtMap() : 
                base("Исковое зявление", "CLW_LAWSUIT_COURT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DocumentClw, "ссылка на докуент").Column("DOCUMENT_ID").NotNull().Fetch();
            Property(x => x.LawsuitCourtType, "тип суда").Column("COURT_TYPE").DefaultValue(LawsuitCourtType.NotSet).NotNull();
            Property(x => x.DocDate, "Дата").Column("DOC_DATE");
            Property(x => x.DocNumber, "Номер").Column("DOC_NUMBER").Length(100);
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.PretensionType, "PretensionType").Column("PretensionType");
            Property(x => x.PretensionReciever, "PretensionReciever").Column("PretensionReciever");
            Property(x => x.PretensionDate, "PretensionDate").Column("PretensionDate");
            Property(x => x.PretensionResult, "PretensionResult").Column("PretensionResult");
            Property(x => x.PretensionReviewDate, "PretensionReviewDate").Column("PretensionReviewDate");
            Property(x => x.PretensionNote, "PretensionNote").Column("PretensionNote");
        }
    }
}
