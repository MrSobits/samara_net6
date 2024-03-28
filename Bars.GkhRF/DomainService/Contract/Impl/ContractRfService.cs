namespace Bars.GkhRf.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class ContractRfService : IContractRfService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ActualRealityObjectList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            //// получаем id домов, которые указаны в договорах, где даты договоров не пересекаются с текущей датой
            //var busyRealityObj = Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
            //    .Where(x =>
            //        (x.ContractRf.DateEnd != null && x.ContractRf.DateEnd >= DateTime.Now && x.ContractRf.DateBegin <= DateTime.Now)
            //        || (x.ContractRf.DateEnd == null && x.ContractRf.DateBegin <= DateTime.Now))
            //    .Where(x => x.TypeCondition == TypeCondition.Include)
            //    .Select(x => x.RealityObject.Id)
            //    .ToList();

            // получаем все дома, которые не содержатся в с списке busyRealityObject
            var data = Container.Resolve<IDomainService<ViewRealityObject>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Municipality,
                    x.Address,
                    x.ManOrgNames,
                    x.GkhCode,
                })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            data = data.Order(loadParam).Paging(loadParam);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult ListByManOrg(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var manorgId = baseParams.Params.ContainsKey("manorgId")
                    ? baseParams.Params["manorgId"].ToLong()
                    : 0;

            var data = Container.Resolve<IDomainService<ViewContractRf>>().GetAll()
                 .Where(x => x.ManagingOrganizationId == manorgId)
                 .Select(x => new
                 {
                     x.Id,
                     x.DocumentNum,
                     x.DocumentDate,
                     x.MunicipalityName,
                     x.ManagingOrganizationName,
                     x.ContractRfObjectsCount,
                 })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult CheckAvailableRealObj(BaseParams baseParams)
        {
            var recordsIds = baseParams.Params["recordIds"].ToString().Split(',').Select(x => x.ToLong()).ToList();

            var noAvailableRealObjs = Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                .Where(x =>
                    (x.ContractRf.DateEnd != null && x.ContractRf.DateEnd >= DateTime.Now && x.ContractRf.DateBegin <= DateTime.Now)
                    || (x.ContractRf.DateEnd == null && x.ContractRf.DateBegin <= DateTime.Now))
                .Where(x => x.TypeCondition == TypeCondition.Include)
                .Where(x => recordsIds.Contains(x.RealityObject.Id))
                .ToList();

            string message = string.Empty;
            if (noAvailableRealObjs.Any())
            {
	            message = noAvailableRealObjs.Aggregate(message,
		            (current, item) =>
			            current + string.Format(" - {0} (№ договора {1});<br>", item.RealityObject.Address, item.ContractRf.DocumentNum));

                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format("Следующие дома используются в текущих договорах:<br>{0}", message);
                    return new BaseDataResult { Success = false, Message = message };
                }
            }

            return new BaseDataResult { Success = true };
        }

        public IDataResult ListByManOrgAndContractDate(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var manOrgId = baseParams.Params.GetAs<long>("manOrgId");
            // выбираем дома у которых договор заключен позже этой даты
            var date = baseParams.Params.GetAs<DateTime>("date");

            var contractRfDomain = Container.Resolve<IDomainService<ContractRfObject>>();

            var data = contractRfDomain.GetAll()
                  .Where(x => x.ContractRf.ManagingOrganization.Id == manOrgId && x.ContractRf.DocumentDate >= date)
                  .Select(x => new
                  {
                      x.RealityObject.Id,
                      x.RealityObject.Address,
                      Municipality = x.RealityObject.Municipality.Name
                  })
                  .OrderIf(loadParams.Order.Length == 0, true, x => x.Address)
                  .Filter(loadParams, Container)
                  .Order(loadParams);

            Container.Release(contractRfDomain);

            return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
        }
    }
}