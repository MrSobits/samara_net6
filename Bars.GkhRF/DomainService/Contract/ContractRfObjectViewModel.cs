namespace Bars.GkhRf.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Entities;
    using Gkh.Enums;
    using Entities;
    using Enums;

    /// <summary>
    /// Представление для Объекта договора рег.фонда
    /// </summary>
    public class ContractRfObjectViewModel : BaseViewModel<ContractRfObject>
    {
        /// <summary>
        /// Выдать список
        /// </summary>
        /// <param name="domain">Домен сервис</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Список Объектов договора рег.фонда</returns>
        public override IDataResult List(IDomainService<ContractRfObject> domain, BaseParams baseParams)
        {
#warning переделать

            var loadParams = this.GetLoadParam(baseParams);

            var typeCondition = baseParams.Params.ContainsKey("typeCondition")
                                ? baseParams.Params["typeCondition"].ToStr() == "In" && !string.IsNullOrEmpty(baseParams.Params["typeCondition"].ToStr()) ? 10 : 20
                                : 0;

            var contractRfId = baseParams.Params.GetAs<long>("contractRfId", 0);

            if (contractRfId == 0)
            {
                return new ListDataResult();
            }

            var contractRf = this.Container.Resolve<IDomainService<ContractRf>>().Get(contractRfId);

            var dictManOrg = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(y => domain.GetAll().Any(x => x.RealityObject.Id == y.RealityObject.Id && x.ContractRf.Id == contractRfId))
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Where(x => (
                    (contractRf.DateEnd != null && x.ManOrgContract.StartDate >= contractRf.DateBegin && contractRf.DateEnd >= x.ManOrgContract.StartDate)
                    ||
                    (x.ManOrgContract.EndDate != null && contractRf.DateBegin >= x.ManOrgContract.StartDate && x.ManOrgContract.EndDate >= contractRf.DateBegin)
                    ||
                    (x.ManOrgContract.EndDate == null && contractRf.DateEnd != null && x.ManOrgContract.StartDate <= contractRf.DateEnd)
                    ||
                    (x.ManOrgContract.EndDate != null && contractRf.DateEnd == null && contractRf.DateBegin <= x.ManOrgContract.EndDate)
                    || (x.ManOrgContract.EndDate == null && contractRf.DateEnd == null)))
                
                .Select(x => new
                    {
                        x.RealityObject.Id,
                        ManOrg = x.ManOrgContract.ManagingOrganization.Contragent.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Aggregate(string.Empty, (x, rec) => x + (!string.IsNullOrEmpty(x) ? ", " + rec.ManOrg : rec.ManOrg)));

            var data = domain.GetAll()
                .Where(x => x.TypeCondition == typeCondition.To<TypeCondition>() && x.ContractRf.Id == contractRfId)
                .Select(x => new
                    {
                        x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        RealityObjectName = x.RealityObject.Address,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        x.IncludeDate,
                        x.ExcludeDate,
                        x.TypeCondition,
                        x.RealityObject.GkhCode,
                        RealityObjectAreaMkd = x.RealityObject.AreaMkd,
                        RealityObjectAreaLivingOwned = x.RealityObject.AreaLivingOwned,
                        x.AreaLiving,
                        x.AreaLivingOwned,
                        x.AreaNotLiving,
                        x.TotalArea,
                        x.Note
                    })
                .AsEnumerable()
                .Select(x => new
                    {
                        x.Id,
                        x.RealityObjectId,
                        x.RealityObjectName,
                        x.MunicipalityName,
                        ManOrgName = dictManOrg.Get(x.RealityObjectId),
                        x.IncludeDate,
                        x.ExcludeDate,
                        x.TypeCondition,
                        x.GkhCode,
                        x.RealityObjectAreaMkd,
                        x.RealityObjectAreaLivingOwned,
                        x.AreaLiving,
                        x.AreaLivingOwned,
                        x.AreaNotLiving,
                        x.TotalArea,
                        x.Note
                })
                .AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjectName)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}