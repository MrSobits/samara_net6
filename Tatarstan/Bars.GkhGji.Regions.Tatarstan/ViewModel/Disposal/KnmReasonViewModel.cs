namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// View-Model для <see cref="KnmReason"/>
    /// </summary>
    public class KnmReasonViewModel : BaseViewModel<KnmReason>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<KnmReason> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");

            return domain.GetAll()
                .Where(x => x.Decision.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.ErknmTypeDocument.DocumentType,
                    x.File,
                })
                .ToListDataResult(loadParam);
        }
    }
}