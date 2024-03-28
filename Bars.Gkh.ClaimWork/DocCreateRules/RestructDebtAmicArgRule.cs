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
    using Bars.Gkh.Utils;

    /// <summary>
    /// Правило для создания Реструктуризации по мировому соглашению
    /// </summary>
    public class RestructDebtAmicArgRule : BaseClaimWorkDocRule
    {
        public override string Id => this.GetType().Name;

        public override string Description => "Создание реструктуризации по мировому соглашению";

        public override string ActionUrl => "restructdebtamicagr";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.RestructDebtAmicAgr;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var restructDebtDomain = this.Container.ResolveDomain<RestructDebt>();
            var lawsuitDomain = this.Container.ResolveDomain<Lawsuit>();
            using (this.Container.Using(restructDebtDomain, lawsuitDomain))
            {
                var document = restructDebtDomain.GetAll()
                    .Where(x => x.DocumentType == ClaimWorkDocumentType.RestructDebtAmicAgr)
                    .FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);

                var lawsuitDebt = lawsuitDomain.GetAll()
                    .Where(x => x.ClaimWork.Id == claimWork.Id)
                    .Select(x => new
                    {
                        BaseTariffDebtSum = x.DebtSumApproved.Value,
                        DecisionTariffDebtSum = 0,
                        DebtSum = x.DebtSumApproved.Value,
                        PenaltyDebtSum = x.PenaltyDebtApproved.Value,
                        x.LawsuitDocType
                    })
                    .FirstOrDefault();

                if (lawsuitDebt.LawsuitDocType != LawsuitDocumentType.AmicableAgreement)
                {
                    return BaseDataResult.Error("Результатом рассмотрения искового заявление доджно быть 'Мировое соглашение'");
                }

                if (document == null)
                {
                    document = new RestructDebt
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.RestructDebtAmicAgr,
                        DocumentDate = DateTime.Now,
                        BaseTariffDebtSum = lawsuitDebt.BaseTariffDebtSum,
                        DecisionTariffDebtSum = lawsuitDebt.DecisionTariffDebtSum,
                        DebtSum = lawsuitDebt.DebtSum,
                        PenaltyDebtSum = lawsuitDebt.PenaltyDebtSum,
                        RestructSum = lawsuitDebt.DebtSum + lawsuitDebt.PenaltyDebtSum,
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
                        DocumentType = ClaimWorkDocumentType.RestructDebtAmicAgr,
                        DocumentDate = DateTime.Now
                    });
                }

                return documents;
            }
        }
    }
}