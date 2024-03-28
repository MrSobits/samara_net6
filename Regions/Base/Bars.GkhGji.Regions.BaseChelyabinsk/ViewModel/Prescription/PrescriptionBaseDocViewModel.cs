namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Prescription
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    public class PrescriptionBaseDocViewModel : BaseViewModel<PrescriptionBaseDocument>
    {
        public override IDataResult List(IDomainService<PrescriptionBaseDocument> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = loadParam.Filter.GetAs("documentId", 0L);

            var data = domain
                .GetAll()
                .Where(x => x.Prescription.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    KindBaseDocument = x.KindBaseDocument.Name,
                    x.DateDoc,
                    x.NumDoc
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}