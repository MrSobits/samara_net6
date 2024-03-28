namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Entities;

    /// <summary>
    /// Представление для <see cref="DisclosureInfoRealityObj"/>
    /// </summary>
    public class DisclosureInfoRealityObjectViewModel : BaseViewModel<DisclosureInfoRealityObj>
    {
        /// <summary>
        /// Сервис для получения управляемых домов УО
        /// </summary>
        public IDiRealityObjectViewModelService DiRealityObjectViewModelService { get; set; }

        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<DisclosureInfoRealityObj> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
            var diRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
            var manOrgId = baseParams.Params.GetAs<long>("manOrgId");

            var serviceDisInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var diRealObjPercentDomain = this.Container.Resolve<IDomainService<DiRealObjPercent>>();
            var disclosureInfoRelationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var roRepo = this.Container.Resolve<IRepository<RealityObject>>();

            using (this.Container.Using(serviceDisInfo, diRealObjPercentDomain, disclosureInfoRelationDomain, roRepo))
            {
                var disInfoObj = serviceDisInfo.Get(disclosureInfoId);
                if (disInfoObj == null)
                {
                    return new ListDataResult();
                }

                var robjectFilter = this.DiRealityObjectViewModelService.GetManagedRealityObjects(disInfoObj, diRealityObjId);

                var diRoIds = disclosureInfoRelationDomain.GetAll()
                    .Where(x => x.DisclosureInfo == disInfoObj)
                    .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manOrgId)
                    .Select(x => x.DisclosureInfoRealityObj.Id)
                    .ToList();

                var diRealObjPercentsDict = diRealObjPercentDomain.GetAll()
                    .Where(y => y.Code == "DiRealObjPercent")
                    .Where(y => diRoIds.Contains(y.DiRealityObject.Id))
                    .Select(x => new { x.DiRealityObject.RealityObject.Id, x.Percent })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id, x => x.Percent);

                // Через IRepository так как нужны все дома независимо от условий фильтрации по контрагентам и МО оператора
                var data = roRepo.GetAll()
                    .Where(x => robjectFilter.Any(y => y == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.FiasAddress.AddressName,
                        x.AreaLiving,
                        x.DateLastOverhaul,
                        x.DateCommissioning,

                        HouseAccounting = !x.DateCommissioning.HasValue 
                                            || !disInfoObj.PeriodDi.DateAccounting.HasValue
                                            || x.DateCommissioning <= disInfoObj.PeriodDi.DateAccounting
                    })
                    .Filter(loadParams, this.Container)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.AddressName,
                        x.AreaLiving,
                        x.DateLastOverhaul,
                        x.DateCommissioning,
                        x.HouseAccounting,
                        percent = diRealObjPercentsDict.Get(x.Id, 0m)
                    });

                var totalCount = data.Count();

                if (loadParams.Order.Length == 0)
                {
                    data = data
                        .OrderByDescending(x => x.HouseAccounting)
                        .ThenBy(x => x.AddressName);
                }

                return new ListDataResult(data.AsQueryable().Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }
    }
}