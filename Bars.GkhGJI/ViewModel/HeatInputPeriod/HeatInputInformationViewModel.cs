namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class HeatInputInformationViewModel : BaseViewModel<HeatInputInformation>
    {
        public IDomainService<HeatInputPeriod> HeatInputPeriodDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public override IDataResult List(IDomainService<HeatInputInformation> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var periodId = loadParams.Filter.GetAs("hipId", 0L);
            var period = HeatInputPeriodDomain.Get(periodId);

            var realObjs = RealityObjectDomain.GetAll().Where(x => x.Municipality.Id == period.Municipality.Id);
            var totalRoCount = realObjs.Count();
            var centralCount = realObjs.Count(x => x.HeatingSystem == HeatingSystem.Centralized);
            var individCount = realObjs.Count(x => x.HeatingSystem == HeatingSystem.Individual);
            var percent = totalRoCount != 0
                    ? decimal.Round(((centralCount + individCount) / totalRoCount) * 100, 5)
                    : 0;
            var noHeatCount = totalRoCount - centralCount - individCount;

            var infoList = domainService.GetAll().Where(x => x.HeatInputPeriod.Id == periodId).ToList();

            var roInfo = infoList.FirstOrDefault(x => x.TypeHeatInputObject == TypeHeatInputObject.RealityObject);
            if (roInfo != null)
            {
                roInfo.Count = totalRoCount;
                roInfo.CentralHeating = centralCount;
                roInfo.IndividualHeating = individCount;
                roInfo.Percent = percent;
                roInfo.NoHeating = noHeatCount;
            }

            var data = infoList
                .AsQueryable()
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();

            var total = data.Count();
            return new ListDataResult(data, total);
        }
    }
}