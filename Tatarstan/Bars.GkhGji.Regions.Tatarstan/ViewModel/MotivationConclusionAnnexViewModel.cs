namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <inheritdoc />
    public class MotivationConclusionAnnexViewModel : BaseViewModel<MotivationConclusionAnnex>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<MotivationConclusionAnnex> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService.GetAll()
                .Where(x => x.MotivationConclusion.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DocumentDate,
                    x.Description,
                    x.File
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}