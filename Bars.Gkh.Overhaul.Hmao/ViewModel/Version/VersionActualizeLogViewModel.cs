namespace Bars.Gkh.Overhaul.Hmao.ViewModel.Version
{
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Entities.Version;

    /// <summary>
    /// View-модель для <see cref="VersionActualizeLog"/>
    /// </summary>
    public class VersionActualizeLogViewModel : BaseViewModel<VersionActualizeLog>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<VersionActualizeLog> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var versionId = loadParams.Filter.GetAsId("versionId");

            return domainService.GetAll()
                .Where(x => x.ProgramVersion.Id == versionId)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.ActualizeType,
                    DateAction = x.DateAction.ToUniversalTime(),
                    x.CountActions,
                    x.ProgramCrName,
                    x.LogFile
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}