namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    public class FormatDataExportManOrgBaseContractRepository : BaseFormatDataExportRepository<ManOrgBaseContract>
    {
        /// <inheritdoc />
        public override IQueryable<ManOrgBaseContract> GetQuery(IFormatDataExportFilterService filterService)
        {
            var moContractDirectManagServ = this.Container.Resolve<IDomainService<RealityObjectDirectManagContract>>();
            var manOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            using (this.Container.Using(moContractDirectManagServ, manOrgContractRealityObject))
            {
                var isSelectByIds = filterService.DuUstavIds.Any();

                var query = this.Repository.GetAll().WhereIfContainsBulked(isSelectByIds, x => x.Id, filterService.DuUstavIds);

                if (!isSelectByIds)
                {
                    var filtredManOrgRo =
                        filterService.FilterByContragent(manOrgContractRealityObject.GetAll(), x => x.ManOrgContract.ManagingOrganization.Contragent);

                    var filtredmoContractDirectManagServ =
                        filterService.FilterByContragent(moContractDirectManagServ.GetAll(), x => x.ManagingOrganization.Contragent);

                    var addressDict = filtredManOrgRo
                        .Select(x => new
                        {
                            ContractId = x.ManOrgContract.Id,
                            x.RealityObject.Address,
                            x.RealityObject.AreaMkd,
                            x.RealityObject.AreaLivingNotLivingMkd
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.ContractId)
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                    var servDatesDict = filtredmoContractDirectManagServ
                        .Select(x => new { x.Id, x.DateStartService, x.DateEndService })
                        .ToDictionary(x => x.Id);

                    var contractFiltredQuery = filterService.FilterByContragent(query, x => x.ManagingOrganization.Contragent);

                    query = query
                        .Select(x => new
                        {
                            x.Id,
                            x.StartDate,
                            x.EndDate,
                            x.TypeContractManOrgRealObj,
                            Contract = x
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Id,
                            StartDate =
                            x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag && servDatesDict.ContainsKey(x.Id)
                                ? servDatesDict[x.Id].DateStartService
                                : x.StartDate,
                            EndDate =
                            x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag && servDatesDict.ContainsKey(x.Id)
                                ? servDatesDict[x.Id].DateEndService
                                : x.EndDate,
                            Address = addressDict.Get(x.Id)?.Address ?? string.Empty,
                            TypeContractString = x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj
                                ? "Передача управления"
                                : x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                                    ? "Оказание услуг"
                                    : "Основной",
                            x.Contract
                        })
                        .AsQueryable()
                        .Filter(filterService.ObjectCrFilter, this.Container)
                        .Select(x => x.Contract);

                    return this.FilterByEditDate(contractFiltredQuery, filterService);
                }

                return query;
            }
        }

        public override IDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var contragentId = baseParams.Params.GetAs<long>("contragentId");
            var isDu = baseParams.Params.GetAs<bool>("isDu");
            var isUstav = baseParams.Params.GetAs<bool>("isUstav");
            var showValid = baseParams.Params.GetAs<bool>("showValid");

            var moContractDirectManagServ = this.Container.Resolve<IDomainService<RealityObjectDirectManagContract>>();
            var manOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            using (this.Container.Using(moContractDirectManagServ, manOrgContractRealityObject))
            {
                var addressDict = manOrgContractRealityObject.GetAll()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == contragentId)
                    .Select(x => new
                    {
                        ContractId = x.ManOrgContract.Id,
                        x.RealityObject.Address,
                        x.RealityObject.AreaMkd,
                        x.RealityObject.AreaLivingNotLivingMkd
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var servDatesDict =
                    moContractDirectManagServ.GetAll()
                        .Where(y => y.ManagingOrganization.Contragent.Id == contragentId)
                        .Select(x => new { x.Id, x.DateStartService, x.DateEndService })
                        .ToDictionary(x => x.Id);

                return this.Repository.GetAll()
                    .Where(x => x.ManagingOrganization.Contragent.Id == contragentId)
                    .WhereIf(isUstav,
                        x => x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag || x.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                    .WhereIf(isDu,
                        x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners ||
                            x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                    .WhereIf(showValid, x => x.StartDate.HasValue && x.StartDate <= DateTime.Now
                            && (!x.EndDate.HasValue || x.EndDate >= DateTime.Now))
                    .Select(x => new
                    {
                        x.Id,
                        x.StartDate,
                        x.EndDate,
                        x.TypeContractManOrgRealObj,
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        StartDate =
                        x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag && servDatesDict.ContainsKey(x.Id)
                            ? servDatesDict[x.Id].DateStartService
                            : x.StartDate,
                        EndDate =
                        x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag && servDatesDict.ContainsKey(x.Id)
                            ? servDatesDict[x.Id].DateEndService
                            : x.EndDate,
                        Address = addressDict.Get(x.Id)?.Address ?? string.Empty,
                        TypeContractString = x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj
                            ? "Передача управления"
                            : x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                                ? "Оказание услуг"
                                : "Основной",
                    })
                    .AsQueryable()
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Address)
                    .ToListDataResult(loadParams, this.Container);
            }
        }
    }
}