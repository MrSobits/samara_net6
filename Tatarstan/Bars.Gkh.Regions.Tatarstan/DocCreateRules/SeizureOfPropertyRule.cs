namespace Bars.Gkh.Regions.Tatarstan.DocCreateRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    /// <summary>
    /// Создание постановления о наложении ареста на имущество
    /// </summary>
    public class SeizureOfPropertyRule : BaseClaimWorkDocRule
    {
        public override string Id => "SeizureOfPropertyRule";

        public override string Description => "Создание постановления о наложении ареста на имущество";

        public override string ActionUrl => "propseizure";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.SeizureOfProperty;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();
            var seizureOfPropertyDomain = this.Container.ResolveDomain<SeizureOfProperty>();
            var claimWorkInfoService = this.Container.Resolve<IUtilityDebtorClaimWorkInfoService>();

            try
            {
                var dict = new DynamicDictionary();
                claimWorkInfoService.GetInfo(claimWork, dict);

                var seizureOfProperty = seizureOfPropertyDomain.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);
                if (seizureOfProperty == null)
                {
                    var execProcess = executoryProcessDomain.GetAll().First(x => x.ClaimWork.Id == claimWork.Id);

                    seizureOfProperty = new SeizureOfProperty
                    {
                        ClaimWork = claimWork,
                        DocumentType = this.ResultTypeDocument,
                        JurInstitution = execProcess.JurInstitution,
                        Creditor = execProcess.Creditor,
                        AccountOwner = dict.GetAs<string>("AccountOwner"),
                        OwnerType = dict.GetAs<OwnerType>("OwnerType")
                    };

                    seizureOfPropertyDomain.Save(seizureOfProperty);
                }

                return new BaseDataResult(seizureOfProperty);
            }
            finally
            {
                this.Container.Release(executoryProcessDomain);
                this.Container.Release(claimWorkInfoService);
                this.Container.Release(seizureOfPropertyDomain);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();
            var seizureOfPropertyDomain = this.Container.ResolveDomain<SeizureOfProperty>();

            var claimWorkInfoService = this.Container.Resolve<IUtilityDebtorClaimWorkInfoService>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var seizureOfPropertyWithDoc = seizureOfPropertyDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var result = new List<SeizureOfProperty>();

                var jurInstitutionDict = executoryProcessDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .GroupBy(x => x.ClaimWork.Id)
                    .ToDictionary(x => x.Key, x => x.First().JurInstitution);

                foreach (var claimWork in claimWorks.Where(x => !seizureOfPropertyWithDoc.Contains(x.Id)))
                {
                    var dict = new DynamicDictionary();

                    claimWorkInfoService.GetInfo(claimWork, dict);

                    result.Add(new SeizureOfProperty
                    {
                        ClaimWork = claimWork,
                        DocumentType = this.ResultTypeDocument,
                        AccountOwner = dict.GetAs<string>("AccountOwner"),
                        OwnerType = dict.GetAs<OwnerType>("OwnerType"),
                        JurInstitution = jurInstitutionDict[claimWork.Id]
                    });
                }

                return result;
            }
            finally
            {
                this.Container.Release(executoryProcessDomain);
                this.Container.Release(claimWorkInfoService);
                this.Container.Release(seizureOfPropertyDomain);
            }
        }       
    }
}