namespace Bars.Gkh.ClaimWork.DocCreateRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;

    using Bars.B4.IoC;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Правило для создания Реструктуризации долга
    /// </summary>
    public class RestructDebtRule : BaseClaimWorkDocRule
    {
        public override string Id => this.GetType().Name;

        public override string Description => "Создание реструктуризации долга";

        public override string ActionUrl => "restructdebt";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.RestructDebt;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var restructDebtDomain = this.Container.ResolveDomain<RestructDebt>();
            using (this.Container.Using(restructDebtDomain))
            {
                var document = restructDebtDomain.GetAll()
                    .Where(x => x.DocumentType == ClaimWorkDocumentType.RestructDebt)
                    .FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);

                var debtorClaimWork = claimWork as DebtorClaimWork;

                if (document == null)
                {
                    document = new RestructDebt
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.RestructDebt,
                        DocumentDate = DateTime.Now,
                        BaseTariffDebtSum = debtorClaimWork.CurrChargeBaseTariffDebt,
                        DecisionTariffDebtSum = debtorClaimWork.CurrChargeDecisionTariffDebt,
                        DebtSum = debtorClaimWork.CurrChargeDebt,
                        PenaltyDebtSum = debtorClaimWork.CurrPenaltyDebt,
                        RestructSum = debtorClaimWork.CurrChargeDebt + debtorClaimWork.CurrPenaltyDebt,
                        DocumentState = RestructDebtDocumentState.Active
                    };

                    restructDebtDomain.Save(document);
                }

                return new BaseDataResult(document);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var restructDebtDomain = this.Container.ResolveDomain<RestructDebt>();
            using (this.Container.Using(restructDebtDomain))
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var claimWorkWithDoc = restructDebtDomain.GetAll()
                    .WhereContainsBulked(x => x.ClaimWork.Id, claimWorkIds)
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var documents = new List<RestructDebt>();

                foreach (var claimWork in claimWorks.Where(x => !claimWorkWithDoc.Contains(x.Id)))
                {
                    documents.Add(new RestructDebt
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.RestructDebt,
                        DocumentDate = DateTime.Now
                    });
                }

                return documents;
            }
        }
    }
}