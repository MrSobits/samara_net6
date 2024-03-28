namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class InspectionBaseContragentViewModel : BaseViewModel<InspectionBaseContragent>
    {
        public override IDataResult List(IDomainService<InspectionBaseContragent> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var inspectionId = baseParams.Params.GetAs<long>("inspectionId");

            var data = domainService.GetAll()
                .Where(x => x.InspectionGji.Id == inspectionId)
                .Select(
                    x => new
                    {
                        x.Id,
                        MunicipalityName = x.Contragent.Municipality.Name,
                        x.Contragent.JuridicalAddress,
                        x.Contragent.Name,
                        x.Contragent.Inn
                    })
                .Filter(loadParam, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}