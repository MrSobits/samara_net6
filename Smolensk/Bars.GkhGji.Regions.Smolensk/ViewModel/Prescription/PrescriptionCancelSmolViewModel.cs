namespace Bars.GkhGji.Regions.Smolensk.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.GkhGji.ViewModel;

    using Entities;
    using Gkh.Domain;

    public class PrescriptionCancelSmolViewModel : PrescriptionCancelViewModel<PrescriptionCancelSmol>
    {
        public override IDataResult List(IDomainService<PrescriptionCancelSmol> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.Prescription.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.SmolTypeCancel,
                    x.DocumentNum,
                    x.DocumentDate,
                    IssuedCancel = x.IssuedCancel.Fio
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}