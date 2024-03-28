namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;
    using Enums;
    using Gkh.Utils;
    using Castle.Windsor;
    using Entities;

    /// <summary>
    /// Параметр очередности (Выбранный способ управления МКД)
    /// </summary>
    public abstract class BaseCtrlMethodRoPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public IWindsorContainer Container { get; set; }

        public virtual string Id
        {
            get { return "SelectedControlMethodRealObj"; }
        }

        public virtual string Name
        {
            get { return "Выбранный способ управления МКД"; }
        }

        public Type EnumType
        {
            get
            {
                return typeof (TypeSelectedControlMethod);
            }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Qualit; }
        }

        private HashSet<long> RealObjDirectManag { get; set; }

        private HashSet<long> RealObjSelectedByOwners { get; set; }

        private HashSet<long> RealObjSelectedByLocalGov { get; set; }

        public virtual object GetValue(IStage3Entity obj)
        {
            return GetValueInternal(obj);
        }

        private object GetValueInternal(IStage3Entity obj)
        {
            if (RealObjDirectManag == null || RealObjSelectedByOwners == null || RealObjSelectedByLocalGov == null)
            {
                GetInfo();
            }

            if (RealObjDirectManag.Contains(obj.RealityObject.Id)
                || RealObjSelectedByOwners.Contains(obj.RealityObject.Id))
            {
                return TypeSelectedControlMethod.SelectedByOwner;
            }

            if (RealObjSelectedByLocalGov.Contains(obj.RealityObject.Id))
            {
                return TypeSelectedControlMethod.SelectedByLocalGov;
            }

            return TypeSelectedControlMethod.NotSelected;
        }

        private void GetInfo()
        {
            var moContractRealObjDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            var moContractOwnersDomain = Container.ResolveDomain<ManOrgContractOwners>();

            try
            {
                var currentDate = DateTime.Today;
                var moContractRealObjQuery = moContractRealObjDomain.GetAll()
                    .Where(x => (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= currentDate)
                                && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate));

                RealObjDirectManag = moContractRealObjQuery
                    .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                    .Select(x => x.RealityObject.Id)
                    .ToHashSet();

                RealObjSelectedByOwners = moContractRealObjQuery
                    .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                    .Where(x => moContractOwnersDomain.GetAll()
                        .Where(y => y.Id == x.ManOrgContract.Id)
                        .Any(y => y.ContractFoundation == ManOrgContractOwnersFoundation.OwnersMeetingProtocol))
                    .Select(x => x.RealityObject.Id)
                    .ToHashSet();

                RealObjSelectedByLocalGov = moContractRealObjQuery
                    .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                    .Where(x => moContractOwnersDomain.GetAll()
                        .Where(y => y.Id == x.ManOrgContract.Id)
                        .Any(y => y.ContractFoundation == ManOrgContractOwnersFoundation.OpenTenderResult))
                    .Select(x => x.RealityObject.Id)
                    .ToHashSet();
            }
            finally
            {
                Container.Release(moContractOwnersDomain);
                Container.Release(moContractRealObjDomain);
            }
        }
    }

    public enum TypeSelectedControlMethod
    {
        /// <summary>
        /// Собственниками выбран способ управления
        /// </summary>
        [Display("Собственниками выбран способ управления")]
        SelectedByOwner= 10,

        /// <summary>
        /// Органом мест.самоуправления выбрана УК
        /// </summary>
        [Display("Органом мест.самоуправления выбрана УК")]
        SelectedByLocalGov = 20,

        /// <summary>
        /// Способ управления не выбран
        /// </summary>
        [Display("Способ управления не выбран")]
        NotSelected = 30
    }
}