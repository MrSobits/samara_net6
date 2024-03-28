namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PrescriptionAnnexViewModel : BaseViewModel<PrescriptionAnnex>
    {
        public override IDataResult List(IDomainService<PrescriptionAnnex> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.Prescription.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.Name,
                    x.Description,
                    x.TypePrescriptionAnnex,
                    x.Number,
                    x.TypeAnnex,
                    x.SignedFile,
                    x.MessageCheck,
                    x.DocumentSend,
                    x.DocumentDelivered,
                    x.File,
                    x.SendFileToErknm
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}