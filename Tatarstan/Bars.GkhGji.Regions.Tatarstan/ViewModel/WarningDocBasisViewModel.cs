namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <inheritdoc />
    public class WarningDocBasisViewModel : BaseViewModel<WarningDocBasis>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<WarningDocBasis> domainService, BaseParams baseParams)
        {
            var warningDocId = baseParams.Params.GetAsId("documentId");

            return domainService.GetAll()
                .Where(x => x.WarningDoc.Id == warningDocId)
                .Select(x => new
                {
                    x.Id,
                    x.WarningBasis.Code,
                    x.WarningBasis.Name
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}