namespace Bars.GkhRf.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.DataResult;
    using Gkh.Domain;
    using Entities;

    public class RequestTransferRfViewModel : BaseViewModel<RequestTransferRf>
    {
        public override IDataResult List(IDomainService<RequestTransferRf> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var municipalities = baseParams.Params.GetAs<string>("municipalities");

            List<long> checkedMunicipalities = null;

            if (!string.IsNullOrEmpty(municipalities))
            {
                checkedMunicipalities = municipalities.Split(';').Select(x => x.ToLong()).ToList();
            }

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);

            var data = Container.Resolve<IDomainService<ViewRequestTransferRf>>().GetAll()
                .WhereIf(checkedMunicipalities != null, x => x.MunicipalityId.HasValue && checkedMunicipalities.Contains(x.MunicipalityId.Value))
                .WhereIf(dateStart != DateTime.MinValue, x => x.DateFrom >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateFrom <= dateEnd)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.DocumentNum,
                    x.DateFrom,
                    x.TypeProgramRequest,
                    x.MunicipalityName,
                    x.ManagingOrganizationName,
                    x.TransferFundsCount,
                    x.TransferFundsSum
                })
                .Filter(loadParams, Container);

            //чуть чуть оптимизации
            var summary =
                data
                    .Where(x => x.TransferFundsSum.HasValue)
                    .Where(x => x.TransferFundsSum != 0)
                    .Sum(x => x.TransferFundsSum);

            return new ListSummaryResult(data.OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName).Order(loadParams).Paging(loadParams).ToList(), data.Count(), new { TransferFundsSum = summary });
        }

        public override IDataResult Get(IDomainService<RequestTransferRf> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId("id"));

            return new BaseDataResult(
                new
                {
                    obj.Id,
                    obj.ContractRf,
                    obj.ProgramCr,
                    obj.ContragentBank,
                    obj.TypeProgramRequest,
                    obj.Perfomer,
                    obj.State,
                    obj.DocumentName,
                    obj.DocumentNum,
                    obj.File,
                    obj.DateFrom,
                    ManagingOrganization = new
                    {
                        obj.ManagingOrganization.Id,
                        ContragentName = obj.ManagingOrganization.Contragent.Name,
                        ContragentInn = obj.ManagingOrganization.Contragent.Inn,
                        ContragentKpp = obj.ManagingOrganization.Contragent.Kpp,
                        ContragentPhone = obj.ManagingOrganization.Contragent.Phone,
                        ContragentId = obj.ManagingOrganization.Contragent.Id
                    }
                });
        }
    }
}