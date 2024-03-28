namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Enums;
    using Gkh.ClaimWork.Entities;

    //using Mono.Security.X509;

    /// <summary>
    /// Правило для создания акта выявления нарушений ПИР
    /// </summary>
    public class ActViolIdentificationRule : BaseClaimWorkDocRule
    {
        public override string Id => "ActViolIdentificationCreateRule";

        public override string Description => "Создание акта выявления нарушений ПИР";

        public override string ActionUrl => "actviolidentification";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.ActViolIdentification;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var actViolIdentifDomain = this.Container.ResolveDomain<ActViolIdentificationClw>();

            try
            {
                var actViolIdentif = actViolIdentifDomain.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);

                if (actViolIdentif == null)
                {
                    actViolIdentif = new ActViolIdentificationClw
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.ActViolIdentification,
                        DocumentDate = DateTime.Now
                    };

                    actViolIdentifDomain.Save(actViolIdentif);
                }

                return new BaseDataResult(actViolIdentif);
            }
            finally
            {
                this.Container.Release(actViolIdentifDomain);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var actViolIdentifDomain = this.Container.ResolveDomain<ActViolIdentificationClw>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var claimWorkWithDoc = actViolIdentifDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var result = new List<ActViolIdentificationClw>();

                foreach (var claimWork in claimWorks.Where(x => !claimWorkWithDoc.Contains(x.Id)))
                {
                    result.Add(new ActViolIdentificationClw
                    {
                        ClaimWork = claimWork,
                        DocumentType = ClaimWorkDocumentType.ActViolIdentification,
                        DocumentDate = DateTime.Now
                    });
                }

                return result;
            }
            finally
            {
                this.Container.Release(actViolIdentifDomain);
            }
        }
    }
}