namespace Bars.GkhCr.Modules.ClaimWork.Entities
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Enums;

    using Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// 
    /// </summary>
    public class ViewBuildContract : PersistentObject
    {
        public virtual long ClwId { get; set; }

        public virtual ClaimWorkDocumentType DocumentType { get; set; }

        public virtual string Builder { get; set; }

        public virtual string ContractName { get; set; }

        public virtual string ContractNum { get; set; }

        public virtual DateTime? ContractDate { get; set; }

        public virtual long RoId { get; set; }

        public virtual string Address { get; set; }

        public virtual long MuId { get; set; }

        public virtual string Municipality { get; set; }

        public virtual long? StlId { get; set; }

        public virtual string Settlement { get; set; }

        public virtual string JurSectorNumber { get; set; }

        public virtual DateTime? LawsuitDocDate { get; set; }

        public virtual string LawsuitDocNumber { get; set; }

        public virtual LawsuitConsiderationType WhoConsidered { get; set; }

        public virtual DateTime? DateOfAdoption { get; set; }

        public virtual DateTime? DateOfReview { get; set; }

        public virtual LawsuitResultConsideration ResultConsideration { get; set; }

        public virtual decimal? DebtSumApproved { get; set; }

        public virtual decimal? PenaltyDebtApproved { get; set; }

        public virtual LawsuitDocumentType TypeDocument { get; set; }

        public virtual DateTime? DateConsideration { get; set; }

        public virtual string NumberConsideration { get; set; }

        public virtual decimal? CbDebtSum { get; set; }

        public virtual decimal? CbPenaltyDebtSum { get; set; }

        public virtual DateTime? PretensionDocDate { get; set; }

        public virtual string PretensionDocNumber { get; set; }

        public virtual decimal? PretensionDebtSum { get; set; }

        public virtual decimal? PretensionPenaltyDebtSum { get; set; }
    }
}