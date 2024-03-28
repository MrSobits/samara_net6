namespace Bars.Gkh.Gku.ViewModel
{
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Domain;
    using Entities;

    public class GkuTariffGjiViewModel : BaseViewModel<GkuTariffGji>
    {
        public override IDataResult List(IDomainService<GkuTariffGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.ResourceOrg.Contragent.Municipality.Name ?? x.ManOrg.Contragent.Municipality.Name,
                    Contragent = x.ResourceOrg.Contragent.Name,
                    x.ServiceKind,
                    Service = x.Service.Name,
                    x.TarifRso,
                    x.NormativeValue
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<GkuTariffGji> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId("id"));

            return new BaseDataResult(new
            {
                obj.Id,
                obj.DateEnd,
                obj.DateStart,
                ManOrg =
                    obj.ManOrg != null
                        ? new
                        {
                            obj.ManOrg.Id,
                            ContragentName = obj.ManOrg.Return(x => x.Contragent).Return(x => x.Name)
                        }
                        : null,
                ResourceOrg =
                    obj.ResourceOrg != null
                        ? new
                        {
                            obj.ResourceOrg.Id,
                            ContragentName = obj.ResourceOrg.Return(x => x.Contragent).Return(x => x.Name)
                        }
                        : null,
               obj.NormativeActInfo,
               obj.NormativeValue,
               obj.PurchasePrice,
               obj.PurchaseVolume,
               obj.Service,
               obj.ServiceKind,
               obj.TarifMo,
               obj.TarifRso
            });
        }
    }
}