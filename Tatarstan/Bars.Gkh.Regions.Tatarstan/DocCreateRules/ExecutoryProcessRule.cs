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
    /// Правило создания документа "Исполнительное производство"
    /// </summary>
    public class ExecutoryProcessRule : BaseClaimWorkDocRule
    {
        public override string Id => "ExecutoryProcessCreateRule";

        public override string Description => "Создание исполнительного производства";

        public override string ActionUrl => "execprocess";

        public override ClaimWorkDocumentType ResultTypeDocument => ClaimWorkDocumentType.ExecutoryProcess;

        /// <inheritdoc />
        public override IDataResult CreateDocument(BaseClaimWork claimWork)
        {
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();
            var claimWorkInfoServices = this.Container.ResolveAll<IUtilityDebtorClaimWorkInfoService>();

            try
            {
                var dict = new DynamicDictionary();
                claimWorkInfoServices.ForEach(x => x.GetInfo(claimWork, dict));

                var executoryProcess = executoryProcessDomain.GetAll().FirstOrDefault(x => x.ClaimWork.Id == claimWork.Id);
                if (executoryProcess == null)
                {
                    executoryProcess = new ExecutoryProcess
                    { 
                        ClaimWork = claimWork,
                        RealityObject = claimWork.RealityObject,
                        DocumentType = this.ResultTypeDocument,           
                        DebtSum = dict.GetAs<decimal>("DebtSum"),
                        AccountOwner = dict.GetAs<string>("AccountOwner"),
                        OwnerType = dict.GetAs<OwnerType>("OwnerType")
                    };
                    
                    executoryProcessDomain.Save(executoryProcess);
                }

                return new BaseDataResult(executoryProcess);
            }
            finally
            {
                this.Container.Release(executoryProcessDomain);
                this.Container.Release(claimWorkInfoServices);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<DocumentClw> FormDocument(IEnumerable<BaseClaimWork> claimWorks, bool fillDebts = true)
        {
            var executoryProcessDomain = this.Container.ResolveDomain<ExecutoryProcess>();
            var claimWorkInfoServices = this.Container.ResolveAll<IUtilityDebtorClaimWorkInfoService>();

            try
            {
                var claimWorkIds = claimWorks.Select(x => x.Id).ToArray();

                var executoryProcessWithDoc = executoryProcessDomain.GetAll()
                    .Where(x => claimWorkIds.Contains(x.ClaimWork.Id))
                    .Select(x => x.ClaimWork.Id)
                    .ToArray();

                var result = new List<ExecutoryProcess>();

                foreach (var claimWork in claimWorks.Where(x => !executoryProcessWithDoc.Contains(x.Id)))
                {
                    var dict = new DynamicDictionary();

                    claimWorkInfoServices.ForEach(x => x.GetInfo(claimWork, dict));

                    result.Add(new ExecutoryProcess
                    {
                        ClaimWork = claimWork,
                        DocumentType = this.ResultTypeDocument,
                        DebtSum = dict.GetAs<decimal>("DebtSum"),
                        AccountOwner = dict.GetAs<string>("AccountOwner"),
                        OwnerType = dict.GetAs<OwnerType>("OwnerType")
                    });
                }
                return result;
            }
            finally
            {
                this.Container.Release(executoryProcessDomain);
                this.Container.Release(claimWorkInfoServices);
            }
        }
    }
}