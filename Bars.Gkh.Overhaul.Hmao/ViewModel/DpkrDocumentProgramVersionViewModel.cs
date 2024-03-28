namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// ViewModel for <see cref="DpkrDocumentProgramVersion"/>
    /// </summary>
    public class DpkrDocumentProgramVersionViewModel : BaseViewModel<DpkrDocumentProgramVersion>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<DpkrDocumentProgramVersion> domainService, BaseParams baseParams)
        {
            var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");
            
            return domainService.GetAll()
                .Where(x => x.DpkrDocument.Id == dpkrDocumentId)
                .Select(x => new
                {
                    x.Id,
                    x.ProgramVersion.Name,
                    x.ProgramVersion.VersionDate
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}