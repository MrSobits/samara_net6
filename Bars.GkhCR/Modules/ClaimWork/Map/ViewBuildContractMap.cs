/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Modules.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class ViewBuildContractMap : PersistentObjectMap<ViewBuildContract>
///     {
///         public ViewBuildContractMap()
///             : base("view_clw_build")
///         {
///             Map(x => x.ClwId, "clw_id");
///             Map(x => x.DocumentType, "document_type");
///             Map(x => x.Builder, "builder");
///             Map(x => x.ContractName, "contract_name");
///             Map(x => x.ContractNum, "contract_num");
///             Map(x => x.ContractDate, "contract_date");
/// 
///             Map(x => x.RoId, "ro_id");
///             Map(x => x.Address, "address");
///             Map(x => x.Municipality, "municipality");
///             Map(x => x.MuId, "mu_id");
///             Map(x => x.Settlement, "settlement");
///             Map(x => x.StlId, "stl_id");
/// 
///             Map(x => x.JurSectorNumber, "jursector_number");
///             Map(x => x.LawsuitDocDate, "ls_doc_date");
///             Map(x => x.LawsuitDocNumber, "ls_doc_number");
///             Map(x => x.WhoConsidered, "who_considered");
///             Map(x => x.DateOfAdoption, "date_of_adoption");
///             Map(x => x.DateOfReview, "date_of_rewiew");
///             Map(x => x.ResultConsideration, "result_consideration");
///             Map(x => x.DebtSumApproved, "debt_sum_approv");
///             Map(x => x.PenaltyDebtApproved, "penalty_debt_approv");
///             Map(x => x.TypeDocument, "law_type_document");
///             Map(x => x.DateConsideration, "date_considerat");
///             Map(x => x.NumberConsideration, "num_considerat");
///             Map(x => x.CbDebtSum, "cb_debt_sum");
///             Map(x => x.CbPenaltyDebtSum, "cb_penalty_debt_sum");
/// 
///             Map(x => x.PretensionDocDate, "pret_doc_date");
///             Map(x => x.PretensionDocNumber, "pret_doc_number");
///             Map(x => x.PretensionDebtSum, "pret_sum");
///             Map(x => x.PretensionPenaltyDebtSum, "pret_penalty_sum");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhCr.Modules.ClaimWork.Entities.ViewBuildContract"</summary>
    public class ViewBuildContractMap : PersistentObjectMap<ViewBuildContract>
    {
        
        public ViewBuildContractMap() : 
                base("Bars.GkhCr.Modules.ClaimWork.Entities.ViewBuildContract", "VIEW_CLW_BUILD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ClwId, "ClwId").Column("CLW_ID");
            Property(x => x.DocumentType, "DocumentType").Column("DOCUMENT_TYPE");
            Property(x => x.Builder, "Builder").Column("BUILDER").Length(250);
            Property(x => x.ContractName, "ContractName").Column("CONTRACT_NAME").Length(250);
            Property(x => x.ContractNum, "ContractNum").Column("CONTRACT_NUM").Length(250);
            Property(x => x.ContractDate, "ContractDate").Column("CONTRACT_DATE");
            Property(x => x.RoId, "RoId").Column("RO_ID");
            Property(x => x.Address, "Address").Column("ADDRESS").Length(250);
            Property(x => x.MuId, "MuId").Column("MU_ID");
            Property(x => x.Municipality, "Municipality").Column("MUNICIPALITY").Length(250);
            Property(x => x.StlId, "StlId").Column("STL_ID");
            Property(x => x.Settlement, "Settlement").Column("SETTLEMENT").Length(250);
            Property(x => x.JurSectorNumber, "JurSectorNumber").Column("JURSECTOR_NUMBER").Length(250);
            Property(x => x.LawsuitDocDate, "LawsuitDocDate").Column("LS_DOC_DATE");
            Property(x => x.LawsuitDocNumber, "LawsuitDocNumber").Column("LS_DOC_NUMBER").Length(250);
            Property(x => x.WhoConsidered, "WhoConsidered").Column("WHO_CONSIDERED");
            Property(x => x.DateOfAdoption, "DateOfAdoption").Column("DATE_OF_ADOPTION");
            Property(x => x.DateOfReview, "DateOfReview").Column("DATE_OF_REWIEW");
            Property(x => x.ResultConsideration, "ResultConsideration").Column("RESULT_CONSIDERATION");
            Property(x => x.DebtSumApproved, "DebtSumApproved").Column("DEBT_SUM_APPROV");
            Property(x => x.PenaltyDebtApproved, "PenaltyDebtApproved").Column("PENALTY_DEBT_APPROV");
            Property(x => x.TypeDocument, "TypeDocument").Column("LAW_TYPE_DOCUMENT");
            Property(x => x.DateConsideration, "DateConsideration").Column("DATE_CONSIDERAT");
            Property(x => x.NumberConsideration, "NumberConsideration").Column("NUM_CONSIDERAT").Length(250);
            Property(x => x.CbDebtSum, "CbDebtSum").Column("CB_DEBT_SUM");
            Property(x => x.CbPenaltyDebtSum, "CbPenaltyDebtSum").Column("CB_PENALTY_DEBT_SUM");
            Property(x => x.PretensionDocDate, "PretensionDocDate").Column("PRET_DOC_DATE");
            Property(x => x.PretensionDocNumber, "PretensionDocNumber").Column("PRET_DOC_NUMBER").Length(250);
            Property(x => x.PretensionDebtSum, "PretensionDebtSum").Column("PRET_SUM");
            Property(x => x.PretensionPenaltyDebtSum, "PretensionPenaltyDebtSum").Column("PRET_PENALTY_SUM");
        }
    }
}
