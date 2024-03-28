namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Domain;

    using Entities;
    using Enums;
    using Gkh.ClaimWork.Entities;

    /// <summary>
    /// Правило для создания претензии ПИР
    /// </summary>
    public class PretensionRule : DocumentClwCreateRule
    {
        public override string Id => "PretensionCreateRule";

        public override string Description => "Создание претензии ПИР";

        public override string ActionUrl => "pretension";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.Pretension;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var pretensionDomain = this.Container.ResolveDomain<PretensionClw>();
            var claimWorkInfoServices = this.Container.ResolveAll<IClaimWorkInfoService>();
            try
            {
                var dict = new DynamicDictionary();

                claimWorkInfoServices.ForEach(x => x.GetInfo(claimWork, dict));
                var pretension = pretensionDomain.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);

                if (pretension == null)
                {
                    this.Container.InTransaction(() =>
                    {
                        pretension = new PretensionClw
                        {
                            ClaimWork = claimWork,
                            DocumentType = ClaimWorkDocumentType.Pretension,
                            DebtBaseTariffSum = dict.GetAs<decimal>("CurrChargeBaseTariffDebt"),
                            DebtDecisionTariffSum = dict.GetAs<decimal>("CurrChargeDecisionTariffDebt"),
                            Sum = dict.GetAs<decimal>("CurrChargeDebt"),
                            Penalty = dict.GetAs<decimal>("CurrPenaltyDebt"),
                            DocumentDate = DateTime.Now
                        };


                        pretensionDomain.Save(pretension);

                        this.CreateDocumentDetail(pretension);
                    });
                }

                return new BaseDataResult(pretension);
            }
            finally
            {
                this.Container.Release(pretensionDomain);
                this.Container.Release(claimWorkInfoServices);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var pretensionDomain = this.Container.ResolveDomain<PretensionClw>();
            var claimWorkInfoServices = this.Container.ResolveAll<IClaimWorkInfoService>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var claimWorkWithDoc = pretensionDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var result = new List<PretensionClw>();

                foreach (var claimWork in claimWorks.Where(x => !claimWorkWithDoc.Contains(x.Id)))
                {
                    var dict = new DynamicDictionary();
                    var work = claimWork;
                    claimWorkInfoServices.ForEach(x => x.GetInfo(work, dict));

                    result.Add(new PretensionClw
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.Pretension,
                        DebtBaseTariffSum = dict.GetAs<decimal>("CurrChargeBaseTariffDebt"),
                        DebtDecisionTariffSum = dict.GetAs<decimal>("CurrChargeDecisionTariffDebt"),
                        Sum = dict.GetAs<decimal>("CurrChargeDebt"),
                        Penalty = dict.GetAs<decimal>("CurrPenaltyDebt"),
                        DocumentDate = DateTime.Now
                    });
                }

                return result;
            }
            finally
            {
                this.Container.Release(pretensionDomain);
                this.Container.Release(claimWorkInfoServices);
            }
        }
    }
}