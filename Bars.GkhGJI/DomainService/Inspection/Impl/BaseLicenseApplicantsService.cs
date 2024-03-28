namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Domain;

    public class BaseLicenseApplicantsService : IBaseLicenseApplicantsService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListByLicenseReq(BaseParams baseParams)
        {
            var disposalDomain = Container.ResolveDomain<Disposal>();
            var baseLicAppDomain = Container.ResolveDomain<BaseLicenseApplicants>();
            var viewBaseLicAppDomain = Container.ResolveDomain<ViewBaseLicApplicants>();

            var loadParam = baseParams.GetLoadParam();
            var requestId = loadParam.Filter.GetAsId("requestId");

            try
            {
                var inspQuery = baseLicAppDomain.GetAll()
                    .Where(x => x.ManOrgLicenseRequest.Id == requestId);

                var disposalsInfo = disposalDomain.GetAll()
                    .Where(x => inspQuery.Any(y => y.Id == x.Inspection.Id))
                    .Where(x => x.TypeDisposal == TypeDisposalGji.Base)
                    .Select(x =>
                        new
                        {
                            x.Inspection.Id,
                            x.DocumentDate,
                            x.DocumentNumber
                        })
                    .ToArray()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var data = viewBaseLicAppDomain.GetAll()
                    .Where(x => inspQuery.Any(y => y.Id == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionNumber,
                        x.RealObjAddresses,
                    })
                    .ToArray()
                    .Select(x =>
                    {
                        var dispInfo = disposalsInfo.Get(x.Id);

                        return new
                        {
                            x.Id,
                            x.InspectionNumber,
                            x.RealObjAddresses,
                            DisposalDate = dispInfo != null ? dispInfo.DocumentDate : null,
                            DisposalNumber = dispInfo != null ? dispInfo.DocumentNumber : null
                        };
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);

            }
            finally
            {
                Container.Release(disposalDomain);
                Container.Release(baseLicAppDomain);
            }
        }
    }
}