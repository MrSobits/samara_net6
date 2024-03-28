namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;

    using Entities;

    using NHibernate.Transform;

    /// <summary>
    /// Правило для создания документа ПИР
    /// </summary>
    public abstract class DocumentClwCreateRule : BaseClaimWorkDocRule
    {
        /// <inheritdoc />
        public override IDataResult CreateDocument(IEnumerable<BaseClaimWork> claimWorks)
        {
            var result = this.FormDocument(claimWorks);
            TransactionHelper.InsertInManyTransactions(this.Container, result, 1000, true, true);

            this.CreateDocumentDetail(result);

            return new BaseDataResult();
        }

        protected virtual void CreateDocumentDetail(IEnumerable<DocumentClw> claimWork)
        {
            var claimworkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var debtorCalcService = this.Container.Resolve<IDebtorCalcService>();
            var documentClwAccountDetailList = new List<DocumentClwAccountDetail>();

            using (this.Container.Using(claimworkAccountDetailDomain, debtorCalcService))
            {
                foreach (var documentClw in claimWork)
                {
                    if (documentClw == null || documentClw.Id == 0)
                    {
                        continue;
                    }

                    var personalAccountQuery = claimworkAccountDetailDomain.GetAll()
                        .Where(x => x.ClaimWork.Id == documentClw.ClaimWork.Id)
                        .Select(x => x.PersonalAccount);

                    documentClwAccountDetailList.AddRange(debtorCalcService.GetDebtorsInfo(personalAccountQuery).Values
                        .Select(x => new DocumentClwAccountDetail
                        {
                            Document = documentClw,
                            PersonalAccount = new BasePersonalAccount{ Id = x.PersonalAccountId },
                            DebtBaseTariffSum = x.DebtBaseTariffSum,
                            DebtDecisionTariffSum = x.DebtDecisionTariffSum,
                            DebtSum = x.DebtSum,
                            PenaltyDebtSum = x.PenaltyDebt,
                        }));
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, documentClwAccountDetailList);
        }

        protected virtual void CreateDocumentDetail(DocumentClw documentClw)
        {
            if (documentClw == null || documentClw.Id == 0 || documentClw.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.BuildContract)
            {
                return;
            }

            var claimworkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var debtorCalcService = this.Container.Resolve<IDebtorCalcService>();
            using (this.Container.Using(claimworkAccountDetailDomain, debtorCalcService))
            {
                var claimWorkQuery = claimworkAccountDetailDomain.GetAll()
                    .Where(x => x.ClaimWork.Id == documentClw.ClaimWork.Id);

                var docDetail = debtorCalcService.GetDebtorsInfo(claimWorkQuery.Select(x => x.PersonalAccount));

                var claimWorks = claimWorkQuery
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var debtor = docDetail.Get(x.PersonalAccount.Id);
                        return new DocumentClwAccountDetail
                        {
                            Document = documentClw,
                            PersonalAccount = x.PersonalAccount,
                            DebtDecisionTariffSum = debtor?.DebtDecisionTariffSum ?? 0,
                            DebtSum = debtor?.DebtSum ?? 0,
                            PenaltyDebtSum = debtor?.PenaltyDebt ?? 0,
                            DebtBaseTariffSum = debtor?.DebtBaseTariffSum ?? 0
                        };
                    })
                    .ToList();

                //var penaltyFormulaDict = this.GetPenaltyFormulaCache(claimWorks.Select(x => x.PersonalAccount.Id), DateTime.Today, 2);

                //foreach (var claimWork in claimWorks.Where(x => x.DebtSum != 0 && x.PenaltyDebtSum != 0))
                //{
                //    claimWork.PenaltyCalcFormula = penaltyFormulaDict.Get(claimWork.PersonalAccount.Id);
                //}

                TransactionHelper.InsertInManyTransactions(this.Container, claimWorks);
            }
        }

        private IDictionary<long, string> GetPenaltyFormulaCache(IEnumerable<long> accountIds, DateTime periodDate, int roundDecimals)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            using (var session = sessionProvider.OpenStatelessSession())
            {
                var sql = @"
SELECT id as Id, GET_PENALTY_FORMULA(CAST(id as bigint), CAST(:periodDate as date), :roundDecimals) as Formula 
FROM regop_pers_acc
WHERE id in (:accountIds);
";
                var ids = accountIds.ToArray();

                return session.CreateSQLQuery(sql)
                    .SetDateTime("periodDate", periodDate)
                    .SetInt32("roundDecimals", roundDecimals)
                    .SetParameterList("accountIds", ids)
                    .SetResultTransformer(Transformers.AliasToBean<FormulaDto>())
                    .List<FormulaDto>()
                    .ToDictionary(x => x.Id, x => x.Formula);
            }
        }

        private struct FormulaDto
        {
            /// <summary>
            /// Идентификатор ЛС
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Формула расчета пени
            /// </summary>
            public string Formula { get; set; }
        }
    }
}