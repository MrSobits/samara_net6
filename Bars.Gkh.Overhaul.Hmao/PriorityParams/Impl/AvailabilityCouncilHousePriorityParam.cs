namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Entities;

    public class AvailabilityCouncilHousePriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "AvailabilityCouncilHouse"; }
        }

        public string Name
        {
            get { return "Наличие совета МКД"; }
        }

        public Type EnumType
        {
            get
            {
                return typeof(TypeAvailabilityCouncilHouse);
            }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Qualit; }
        }

        private HashSet<long> RealObjHasCouncill { get; set; }

        private HashSet<long> RealObjHasChairman { get; set; }

        public object GetValue(IStage3Entity obj)
        {
            if (RealObjHasCouncill == null || RealObjHasChairman == null)
            {
                GetInfo();
            }

            var hasCouncill = RealObjHasCouncill.Contains(obj.RealityObject.Id);
            var hasChairman = RealObjHasChairman.Contains(obj.RealityObject.Id);

            return hasCouncill && hasChairman
                ? TypeAvailabilityCouncilHouse.SelectedCouncilAndChairman
                : hasCouncill ? TypeAvailabilityCouncilHouse.SelectedCouncil : 0;
        }

        private void GetInfo()
        {
            var moContractRealObjDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            var realObjProtocolDomain = Container.ResolveDomain<RealityObjectProtocol>();
            var realObjCouncillorsDomain = Container.ResolveDomain<RealityObjectCouncillors>();

            var currentDate = DateTime.Now;
            var moContractRoQuery = moContractRealObjDomain.GetAll()
			            .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.JskTsj)
                        .Where(x => (!x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= currentDate)
                            && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= currentDate));

            RealObjHasCouncill = realObjProtocolDomain.GetAll()
                .Where(x => x.RealityObject.NumberApartments > 4 && x.CouncilResult == CouncilResult.CouncilCreated)
                .Where(x => moContractRoQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                .Select(x => x.RealityObject.Id)
                .ToHashSet();

            RealObjHasChairman = realObjCouncillorsDomain.GetAll()
                .Where(x => x.Post == TypeCouncillors.Сhairman)
                .Where(x => moContractRoQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                .Select(x => x.RealityObject.Id)
                .ToHashSet();
        }
    }

    public enum TypeAvailabilityCouncilHouse
    {
        [Display("Собственниками выбран совет МКД")]
        SelectedCouncil = 10,

        [Display("Собственниками выбран совет МКД и председатель совета")]
        SelectedCouncilAndChairman = 20
    }
}