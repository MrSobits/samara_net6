namespace Bars.Gkh.Gis.DomainService.RealEstate.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities.RealEstate.GisRealEstateType;

    public class RealEstateTypeService : IRealEstateTypeService
    {
        protected IWindsorContainer Container;
        protected IRepository<GisRealEstateTypeGroup> RealEstateTypeGroupRepository;
        protected IRepository<GisRealEstateType> RealEstateTypeRepository;
        protected IDomainService<GisRealEstateTypeCommonParam> CommonParamService;
        protected ISessionProvider SessionProvider;
        protected IDomainService<GisRealEstateTypeIndicator> IndicatorService;

        public RealEstateTypeService(IWindsorContainer container, IRepository<GisRealEstateTypeGroup> realEstateTypeGroupRepository
            , IRepository<GisRealEstateType> realEstateTypeRepository, IDomainService<GisRealEstateTypeCommonParam> commonParamService
            , ISessionProvider sessionProvider, IDomainService<GisRealEstateTypeIndicator> indicatorService)
        {
            Container = container;
            RealEstateTypeGroupRepository = realEstateTypeGroupRepository;
            RealEstateTypeRepository = realEstateTypeRepository;
            CommonParamService = commonParamService;
            SessionProvider = sessionProvider;
            IndicatorService = indicatorService;
        }

        private class RealEstateTypeProxy
        {
            public string Id;
            public long EntityId;
            public string Entity;
            public string Name;
            public bool leaf;
            public int IndCount;
        }

        public IDataResult GroupedTypeList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var typesDict = RealEstateTypeRepository
                .GetAll()
                .Where(x => x.Group != null)
                .GroupBy(x => x.Group)
                .ToDictionary(x => x.Key.Id, x => x.Select(y => new RealEstateTypeProxy()
                {
                    Id = string.Format("{0}_{1}", "RealEstateType", y.Id),
                    EntityId = y.Id,
                    Entity = "RealEstateType",
                    Name = y.Name,
                    leaf = true,
                    IndCount = CommonParamService.GetAll().Count(z => z.RealEstateType.Id == y.Id)
                }));

            var data = RealEstateTypeGroupRepository
                .GetAll()
                .OrderBy(x => x.Name)
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList()
                .Select(x => new
                {
                    Id = string.Format("{0}_{1}", "RealEstateTypeGroup", x.Id),
                    EntityId = x.Id,
                    Entity = "RealEstateTypeGroup",
                    x.Name,
                    children = typesDict.ContainsKey(x.Id) ? typesDict[x.Id] : new List<RealEstateTypeProxy>()
                });

            return new ListDataResult(data, data.Count());
        }

        public IDataResult Delete(BaseParams baseParams)
        {
            var idList = baseParams.Params.GetAs<long[]>("records");
            var session = SessionProvider.OpenStatelessSession();
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    CommonParamService
                        .GetAll()
                        .Where(x => idList.Contains(x.RealEstateType.Id))
                        .ForEach(x => CommonParamService.Delete(x.Id));

                    IndicatorService
                        .GetAll()
                        .Where(x => idList.Contains(x.RealEstateType.Id))
                        .ForEach(x => IndicatorService.Delete(x.Id));

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }

            return new BaseDataResult();
        }
    }
}