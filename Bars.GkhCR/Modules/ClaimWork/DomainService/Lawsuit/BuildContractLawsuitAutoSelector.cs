namespace Bars.GkhCr.Modules.ClaimWork.DomainService.Lawsuit
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Lawsuit;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.GkhCr.Modules.ClaimWork.Entities;
    using Castle.Windsor;

    /// <summary>
    /// 
    /// </summary>
    public class BuildContractLawsuitAutoSelector : AbstractLawsuitAutoSelector
    {
        private readonly IUnProxy _unProxy;

        /// <summary>
        /// 
        /// </summary>
        public static string Id
        {
            get { return "BuildContractLawsuitAutoSelector"; }
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public BuildContractLawsuitAutoSelector(IWindsorContainer container, IUnProxy unProxy)
            : base(container)
        {
            _unProxy = unProxy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override bool Validate(Lawsuit lawsuit)
        {
            return lawsuit.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.BuildContract;
        }

        /// <summary>
        /// Получить тип должника
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override DebtorType GetDebtorType(Lawsuit lawsuit)
        {
            return DebtorType.Legal;
        }

        /// <summary>
        /// Получить тип основания
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override ReasonType GetReasonType(Lawsuit lawsuit)
        {
            return ReasonType.Builder;
        }

        /// <summary>
        /// Получить жилой дом
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override RealityObject GetRobject(Lawsuit lawsuit)
        {
            return ((BuildContractClaimWork)_unProxy.GetUnProxyObject(lawsuit.ClaimWork)).BuildContract.ObjectCr.RealityObject;
        }

        protected override List<long> GetRoIds(Lawsuit lawsuit)
        {
            return new List<long>();
        }
    }
}