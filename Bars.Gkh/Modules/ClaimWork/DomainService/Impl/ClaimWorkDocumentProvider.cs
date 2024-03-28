namespace Bars.Gkh.Modules.ClaimWork.DomainService.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.B4.Modules.States;
    using Castle.Core.Internal;
    using Castle.Windsor;
    using Domain;
    using Entities;
    using Enums;

    public class ClaimWorkDocumentProvider : IClaimWorkDocumentProvider
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult CreateDocument(BaseParams baseParams)
        {
            var claimWorkId = baseParams.Params.GetAsId("claimWorkId");
            var ruleId = baseParams.Params.GetAs("ruleId", string.Empty);

            if (claimWorkId == 0)
            {
                return new BaseDataResult(false, "Необходимо указать основание ПИР");
            }

            if (ruleId.IsEmpty())
            {
                return new BaseDataResult(false, "Не указан идентификатор правила формирования документа");
            }

            var rules = this.Container.ResolveAll<IClaimWorkDocRule>();
            var claimWorServices = this.Container.ResolveAll<IBaseClaimWorkService>();
            var baseClaimWorkDomain = this.Container.ResolveDomain<BaseClaimWork>();
            var states = this.Container.ResolveDomain<State>();

            try
            {
                var rule = rules.FirstOrDefault(x => x.Id == ruleId);

                var claimWork = baseClaimWorkDomain.Get(claimWorkId);

                if (rule == null)
                {
                    return new BaseDataResult(false, $"По идентификатору {ruleId} не удалось определить правило формирования документа ПИР");
                }

                if (claimWork == null)
                {
                    return new BaseDataResult(false, "Не удалось определить основание");
                }

                var claimWorService = claimWorServices.FirstOrDefault(x => x.ClaimWorkTypeBase == claimWork.ClaimWorkTypeBase);

                rule.SetParams(baseParams);
                baseParams.Params.SetValue("id", claimWorkId);

                baseParams.Params.Add("setstate", (int)rule.ResultTypeDocument);

                var result = rule.CreateDocument(claimWork);

                //claimWorService?.UpdateStates(baseParams);
                
                return result;
            }
            finally
            {
                this.Container.Release(baseClaimWorkDomain);
                this.Container.Release(rules);
                this.Container.Release(claimWorServices);
            }
        }
    }
}
