namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    public class DpkrDocumentViewModel : BaseViewModel<DpkrDocument>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<DpkrDocument> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    DocumentKindName = x.DocumentKind.Name,
                    x.DocumentName,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.DocumentDepartment,
                    x.PublicationDate,
                    x.ObligationAfter2014,
                    x.ObligationBefore2014
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}