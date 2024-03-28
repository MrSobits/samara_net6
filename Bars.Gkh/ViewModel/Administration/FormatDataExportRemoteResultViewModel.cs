namespace Bars.Gkh.ViewModel.Administration
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Utils;

    public class FormatDataExportRemoteResultViewModel : BaseViewModel<FormatDataExportRemoteResult>
    {
        public IGkhUserManager GkhUserManager { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<FormatDataExportRemoteResult> domainService, BaseParams baseParams)
        {
            var activeUser = this.GkhUserManager.GetActiveUser()?.Id ?? 0;

            var isAdministrator = this.GkhUserManager.GetActiveUser().Roles.Any(x => x.Role.Name == "Администратор");

            return domainService.GetAll()
                .WhereIf(!isAdministrator, x => x.TaskResult.Task.User.Id == activeUser)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}