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
    /// Правило создание постановления об ограничении выезда из РФ
    /// </summary>
    public class DepartureRestrictionRule : BaseClaimWorkDocRule
    {
        public override string Id => "DepartureRestrictionRule"; 

        public override string Description => "Создание постановления об ограничении выезда из РФ";

        public override string ActionUrl => "departrestrict";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.DepartureRestriction;
        
        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();
            var departureRestrictionDomain = this.Container.ResolveDomain<DepartureRestriction>();
            var claimWorkInfoService = this.Container.Resolve<IUtilityDebtorClaimWorkInfoService>();

            try
            {
                var dict = new DynamicDictionary();
                claimWorkInfoService.GetInfo(claimWork, dict);

                var departureRestriction = departureRestrictionDomain.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);
                if (departureRestriction == null)
                {
                    var execProcess = executoryProcessDomain.GetAll().First(x => x.ClaimWork.Id == claimWork.Id);
                    
                    departureRestriction = new DepartureRestriction
                    {
                        ClaimWork = claimWork,
                        DocumentType = this.ResultTypeDocument,
                        JurInstitution = execProcess.JurInstitution,
                        Creditor = execProcess.Creditor,
                        AccountOwner = dict.GetAs<string>("AccountOwner"),
                        OwnerType = dict.GetAs<OwnerType>("OwnerType")
                    };

                    departureRestrictionDomain.Save(departureRestriction);
                }

                return new BaseDataResult(departureRestriction);
            }
            finally
            {
                this.Container.Release(executoryProcessDomain);
                this.Container.Release(claimWorkInfoService);
                this.Container.Release(departureRestrictionDomain);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();
            var departureRestrictionDomain = this.Container.ResolveDomain<DepartureRestriction>();
            var claimWorkInfoService = this.Container.Resolve<IUtilityDebtorClaimWorkInfoService>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var departureRestrictionWithDoc = departureRestrictionDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var result = new List<DepartureRestriction>();

                var jurInstitutionDict = executoryProcessDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .GroupBy(x => x.ClaimWork.Id)
                    .ToDictionary(x => x.Key, x => x.First()
                    .JurInstitution);

                foreach (var claimWork in claimWorks.Where(x => !departureRestrictionWithDoc.Contains(x.Id)))
                {
                    var dict = new DynamicDictionary();

                    claimWorkInfoService.GetInfo(claimWork, dict);

                    result.Add(new DepartureRestriction
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
                this.Container.Release(departureRestrictionDomain);
            }
        }
    }
}