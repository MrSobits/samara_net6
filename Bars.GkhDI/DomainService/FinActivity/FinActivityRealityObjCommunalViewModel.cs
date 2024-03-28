namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.GkhDi.Enums;

    using Entities;

    public class FinActivityRealityObjCommunalViewModel : BaseViewModel<FinActivityRealityObjCommunalService>
    {
        public override IDataResult List(IDomainService<FinActivityRealityObjCommunalService> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var dataFinActivityRealityObjCommunalService = domainService
                .GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                .ToList();

            var dataNewFinActivityRealityObjCommunalService = new List<FinActivityRealityObjCommunalService>();

            foreach (TypeServiceDi type in Enum.GetValues(typeof(TypeServiceDi)))
            {
                var record = dataFinActivityRealityObjCommunalService.FirstOrDefault(x => x.TypeServiceDi == type);

                var finActivityRealityObjCommunalService = new FinActivityRealityObjCommunalService { Id = (int)type, TypeServiceDi = type };
                if (record != null)
                {
                    finActivityRealityObjCommunalService.DisclosureInfoRealityObj = record.DisclosureInfoRealityObj;
                    finActivityRealityObjCommunalService.DebtOwner = record.DebtOwner;
                    finActivityRealityObjCommunalService.PaidOwner = record.PaidOwner;
                    finActivityRealityObjCommunalService.PaidByIndicator = record.PaidByIndicator;
                    finActivityRealityObjCommunalService.PaidByAccount = record.PaidByAccount;
                }

                dataNewFinActivityRealityObjCommunalService.Add(finActivityRealityObjCommunalService);
            }

            var totalCount = dataNewFinActivityRealityObjCommunalService.AsQueryable().Count();
            var data = dataNewFinActivityRealityObjCommunalService.AsQueryable().Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}