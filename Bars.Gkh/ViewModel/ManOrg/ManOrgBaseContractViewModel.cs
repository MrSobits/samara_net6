namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Utils;

    using Entities;
    using Enums;

    public class ManOrgBaseContractViewModel : BaseViewModel<ManOrgBaseContract>
    {
        /// <summary>
        /// Список договоров управления
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="baseParams">
        /// manorgId и fromManagOrg = true - если нужно получить договора по управляющей организации
        /// contragentId и fromContragent = true - если нужно получить договора по контрагенту управляющей организации
        /// realityObjectId и fromManagOrg = false - если нужно получить договора по жилому дому
        /// showNotValid = true - если нужно получить недействующие договора по управляющей организации
        /// </param>

        public override IDataResult List(IDomainService<ManOrgBaseContract> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var manorgId = baseParams.Params.GetAs<long>("manorgId");
            var contragentId = baseParams.Params.GetAs<long>("contragentId");
            var fromManagOrg = baseParams.Params.Get("fromManagOrg", false);
            var fromContragent = baseParams.Params.Get("fromContragent", false);
            var realityObjectId = loadParams.Filter.GetAs<long>("realityObjectId");
            var showClose = loadParams.Filter.GetAs<bool>("showClose");
            var showNotValid = baseParams.Params.GetAs<bool>("showNotValid");

            var moContractDirectManagServ = this.Container.Resolve<IDomainService<RealityObjectDirectManagContract>>();
            var manOrgContractRelation = this.Container.Resolve<IDomainService<ManOrgContractRelation>>();
            var manOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            try
            {
                if (contragentId > 0 && fromContragent)
                {
                    var addressDict = manOrgContractRealityObject.GetAll()
                        .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == contragentId)
                        .Select(
                            x => new
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

                    return domain.GetAll()
                        .WhereIf(showNotValid, x => x.EndDate <= DateTime.Now)
                        .Where(x => x.ManagingOrganization.Contragent.Id == contragentId)
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

                if (realityObjectId > 0 && !fromManagOrg)
                {
                    var data1 = manOrgContractRealityObject.GetAll()
                        .Where(x => x.RealityObject.Id == realityObjectId)
                        .Select(
                            x => new
                            {
                                x.ManOrgContract.Id,
                                ManagingOrganizationName = x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag
                                    ? moContractDirectManagServ.GetAll().Any(y => y.Id == x.ManOrgContract.Id && y.IsServiceContract)
                                        ? ManOrgBaseContract.DirectManagementWithContractText
                                        : ManOrgBaseContract.DirectManagementText
                                    : x.ManOrgContract.ManagingOrganization.Contragent.Name,
                                x.ManOrgContract.TypeContractManOrgRealObj,
                                ManagingOrganization = (long?)x.ManOrgContract.ManagingOrganization.Id,
                                DocumentName =
                                    x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj
                                        || x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners
                                        ? "Договор"
                                        : x.ManOrgContract.DocumentName,
                                x.ManOrgContract.StartDate,
                                x.ManOrgContract.EndDate,
                                x.ManOrgContract.FileInfo,
                                TypeContractString =
                                    x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj ? "Передача управления" : "Основной"
                            })
                        .Filter(loadParams, this.Container)
                        .Order(loadParams);

                    int totalCount1 = data1.Count();

                    return new ListDataResult(data1.ToList(), totalCount1);
                }

                var dictAddress = manOrgContractRealityObject.GetAll()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Id == manorgId)
                    .Select(
                        x => new
                        {
                            ContractId = x.ManOrgContract.Id,
                            x.RealityObject.Address,
                            x.RealityObject.AreaMkd,
                            x.RealityObject.AreaLivingNotLivingMkd
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var dictForJskTsj = manOrgContractRelation.GetAll()
                    .Select(
                        x => new
                        {
                            ContractId = x.Parent.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.ContractId)
                    .ToDictionary(x => x.Key);

                var servDates =
                    moContractDirectManagServ.GetAll()
                        .Where(y => y.ManagingOrganization.Id == manorgId)
                        .Select(x => new {x.Id, x.DateStartService, x.DateEndService})
                        .ToDictionary(x => x.Id);

                var data = domain.GetAll()
                    .WhereIf(showNotValid, x => x.EndDate <= DateTime.Now)
                    .Where(x => x.ManagingOrganization.Id == manorgId)
                     .WhereIf(!showClose, x => !x.EndDate.HasValue)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.StartDate,
                            x.EndDate,
                            x.PlannedEndDate,
                            x.Note,
                            x.TypeContractManOrgRealObj,
                            x.DocumentName,
                            x.FileInfo,

                            x.ContractStopReason,
                            x.TerminationDate,

                            (x as ManOrgContractOwners).DateLicenceRegister,
                            (x as ManOrgContractOwners).RegisterReason,
                            (x as ManOrgContractOwners).DateLicenceDelete,
                            (x as ManOrgContractOwners).DeleteReason
                        })
                    .AsEnumerable()
                    .Select(
                        x => new
                        {
                            x.Id,
                            StartDate =
                                x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag && servDates.ContainsKey(x.Id)
                                    ? servDates[x.Id].DateStartService
                                    : x.StartDate,
                            EndDate =
                                x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag && servDates.ContainsKey(x.Id)
                                    ? servDates[x.Id].DateEndService
                                    : x.EndDate,
                            x.PlannedEndDate,
                            x.Note,
                            x.TypeContractManOrgRealObj,
                            x.DocumentName,
                            Address = dictAddress.Get(x.Id)?.Address ?? string.Empty,
                            IsTransferredManagement = dictForJskTsj.ContainsKey(x.Id) ? YesNoNotSet.Yes : YesNoNotSet.No,
                            TypeContractString = x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj
                                ? "Передача управления"
                                : x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag ? "Оказание услуг" : "Основной",

                            dictAddress.Get(x.Id)?.AreaMkd,
                            dictAddress.Get(x.Id)?.AreaLivingNotLivingMkd,

                            x.ContractStopReason,
                            x.TerminationDate,

                            x.DateLicenceRegister,
                            x.RegisterReason,
                            x.DateLicenceDelete,
                            x.DeleteReason
                        })
                    .AsQueryable()
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Address)
                    .ToListDataResult(loadParams, this.Container);

                return data;
            }
            finally
            {
                this.Container.Release(moContractDirectManagServ);
                this.Container.Release(manOrgContractRelation);
                this.Container.Release(manOrgContractRealityObject);
            }
        }
    }
}