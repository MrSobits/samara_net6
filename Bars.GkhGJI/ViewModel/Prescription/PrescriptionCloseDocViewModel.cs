namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PrescriptionCloseDocViewModel : BaseViewModel<PrescriptionCloseDoc>
    {
        public override IDataResult List(IDomainService<PrescriptionCloseDoc> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domain.GetAll()
                .Where(x => x.Prescription.Id == documentId)
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}
