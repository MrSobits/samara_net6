namespace Bars.GkhCr.DomainService
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;
    using System;
    using System.Linq;

    using Bars.GkhCr.Enums;
    using Gkh.Domain;
    public class EstimateCalculationService : IEstimateCalculationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListEstimateRegisterDetail(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var objectCrId = baseParams.Params.ContainsKey("objectCrId")
                           ? baseParams.Params["objectCrId"].ToLong()
                           : 0;

            if (objectCrId == 0)
            {
                objectCrId = loadParam.Filter.GetAsId("objectCrId");
            }

            var domainService = Container.Resolve<IDomainService<ViewObjCrEstimateCalcDetail>>();

            var data = domainService.GetAll()
                .Where(x => x.ObjectCrId == objectCrId)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    TypeWorkCrName = x.WorkName,
                    FinanceSourceName = x.FinSourceName,
                    x.TotalEstimate,
                    TotalEstimateSum = x.SumEstimate,
                    TotalResourceSum = x.SumResource,
                    x.EstimationType
                })
                .Filter(loadParam, Container);

            var summaryTotalEstimate = data.Where(x => x.EstimationType != EstimationType.Customer).Sum(x => x.TotalEstimate);
            var summaryEstimate = data.Where(x => x.EstimationType != EstimationType.Customer).Sum(x => x.TotalEstimateSum);
            var summaryResource = data.Where(x => x.EstimationType != EstimationType.Customer).Sum(x => x.TotalResourceSum);

            int totalCount = data.Count();

            data = data.Order(loadParam).Paging(loadParam);

            return new ListSummaryResult(data.ToList(), totalCount, new { TotalEstimate = summaryTotalEstimate, TotalEstimateSum = summaryEstimate, TotalResourceSum = summaryResource });
        }

        public IQueryable<ViewObjCrEstimateCalc> GetFilteredByOperator()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            var serviceManOrgContractRobject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            return Container.Resolve<IDomainService<ViewObjCrEstimateCalc>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.MunicipalityId))
                .WhereIf(contragentIds.Count > 0, y => serviceManOrgContractRobject.GetAll()
                .Any(x => x.RealityObject.Id == y.RealityObjectId
                    && contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                    && x.ManOrgContract.StartDate <= DateTime.Now.Date
                    && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now.Date)));
        }
    }
}