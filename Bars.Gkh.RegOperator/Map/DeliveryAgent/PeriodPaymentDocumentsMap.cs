/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PeriodPaymentDocumentsMap : BaseImportableEntityMap<PeriodPaymentDocuments>
///     {
///         public PeriodPaymentDocumentsMap() : base("REGOP_PERIOD_PAY_DOC")
///         {
///             Map(x => x.DocumentCode, "DOCUMENT_CODE");
///             References(x => x.File, "FILE_ID");
///             References(x => x.Period, "PERIOD_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Документы платежного агента за период по насПункту_улице_типЛС"</summary>
    public class PeriodPaymentDocumentsMap : BaseImportableEntityMap<PeriodPaymentDocuments>
    {
        
        public PeriodPaymentDocumentsMap() : 
                base("Документы платежного агента за период по насПункту_улице_типЛС", "REGOP_PERIOD_PAY_DOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Period, "Период начисления").Column("PERIOD_ID");
            Reference(x => x.File, "Ссылка на файл").Column("FILE_ID");
            Property(x => x.DocumentCode, "Код документа насПункту_улице_типЛС").Column("DOCUMENT_CODE").Length(250);
        }
    }
}
