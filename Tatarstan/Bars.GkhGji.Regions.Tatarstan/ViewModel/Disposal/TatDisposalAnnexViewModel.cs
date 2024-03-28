namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using System.Linq;

    public class TatDisposalAnnexViewModel : BaseViewModel<TatDisposalAnnex>
    {
        public override IDataResult List(IDomainService<TatDisposalAnnex> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.GetAsId("documentId");

            return domain
                .GetAll()
                .Where(x => x.Disposal.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.ErknmTypeDocument,
                    x.DocumentDate,
                    x.Name,
                    x.Description,
                    x.File
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}