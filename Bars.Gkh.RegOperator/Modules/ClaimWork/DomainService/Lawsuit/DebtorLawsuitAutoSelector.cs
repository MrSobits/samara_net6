namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.Lawsuit
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Lawsuit;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;

    public class DebtorLawsuitAutoSelector : AbstractLawsuitAutoSelector
    {
        public static string Id => "DebtorLawsuitAutoSelector";

        private readonly IUnProxy unProxy;

        /// <summary>
        /// .ctor
        /// </summary>
        public DebtorLawsuitAutoSelector(IWindsorContainer container, IUnProxy unProxy) : base(container)
        {
            this.unProxy = unProxy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override bool Validate(Lawsuit lawsuit)
        {
            return lawsuit.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.Debtor;
        }

        /// <summary>
        /// Получить тип должника
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override DebtorType GetDebtorType(Lawsuit lawsuit)
        {
            var clw = (DebtorClaimWork)this.unProxy.GetUnProxyObject(lawsuit.ClaimWork);
            return clw.DebtorType;
        }

        /// <summary>
        /// Получить тип основания
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override ReasonType GetReasonType(Lawsuit lawsuit)
        {
            return ReasonType.Debtor;
        }

        /// <summary>
        /// Получить жилой дом
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override RealityObject GetRobject(Lawsuit lawsuit)
        {
            var clw = (DebtorClaimWork)this.unProxy.GetUnProxyObject(lawsuit.ClaimWork);

            return clw.DebtorType == DebtorType.Individual
                ? ((IndividualAccountOwner)this.unProxy.GetUnProxyObject(clw.AccountOwner)).RegistrationAddress
                : ((LegalClaimWork) clw).JurisdictionAddress;
        }

        /// <summary>
        /// Получить жилой дом
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        protected override List<long> GetRoIds(Lawsuit lawsuit)
        {
            var clw = (DebtorClaimWork)this.unProxy.GetUnProxyObject(lawsuit.ClaimWork);

            return clw.DebtorType == DebtorType.Individual
                ? clw.AccountDetails
                .Select(x => x.PersonalAccount.Room.RealityObject.Id).ToList()
                : new List<long>();
        }
    }
}