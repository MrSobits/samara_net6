using Bars.B4.DataAccess;

namespace Bars.Gkh.DomainService
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class PaymentAgentService : IPaymentAgentService
    {
        public IWindsorContainer Container { get; set; }

        public IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var service = Container.Resolve<IDomainService<PaymentAgent>>();
            try
            {
                var loadParams = GetLoadParam(baseParams);

                var query = service.GetAll()
                .Select(x => new
                {
                    x.Id,
                    CtrMunicipality = x.Contragent.Municipality.ParentMo == null ? x.Contragent.Municipality.Name : x.Contragent.Municipality.ParentMo.Name,
                    CtrSettlement = x.Contragent.MoSettlement != null ? x.Contragent.MoSettlement.Name : (x.Contragent.Municipality.ParentMo != null ? x.Contragent.Municipality.Name : ""),
                    CtrName = x.Contragent.Name,
                    CtrInn = x.Contragent.Inn,
                    CtrOgrn = x.Contragent.Ogrn,
                    CtrKpp = x.Contragent.Kpp,
                    x.Code
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.CtrMunicipality)
                .Filter(loadParams, Container);

                if (usePaging)
                {
                    totalCount = query.Count();

                    return query.Order(loadParams).Paging(loadParams).ToList();
                }

                var data = query.Order(loadParams).ToList();

                totalCount = data.Count;

                return data;
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public IDataResult ListForExport(BaseParams baseParams)
        {
            var service = Container.ResolveDomain<PaymentAgent>();
            try
            {
                var loadParams = GetLoadParam(baseParams);

                var query = service.GetAll()
                .Where(x => x.SumContractId != null && x.SumContractId != "" 
                    && x.PenaltyContractId != null && x.PenaltyContractId != "")
                .Select(x => new
                {
                    x.Id,
                    CtrMunicipality = x.Contragent.Municipality.Name,
                    CtrName = x.Contragent.Name,
                    CtrInn = x.Contragent.Inn,
                    CtrOgrn = x.Contragent.Ogrn,
                    CtrKpp = x.Contragent.Kpp,
                    x.Code,
                    x.SumContractId,
                    x.PenaltyContractId
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.CtrMunicipality)
                .Filter(loadParams, Container);

                var totalCount = query.Count();

                return new ListDataResult(query.Order(loadParams).ToList(), totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        protected LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }
    }
}