namespace Bars.Gkh.ViewModel.Administration
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Utils;

    public class FormatDataExportResultViewModel : BaseViewModel<FormatDataExportResult>
    {
        public IGkhUserManager GkhUserManager { get; set; }
        public IDomainService<FormatDataExportRemoteResult> FormatDataExportRemoteResultDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<FormatDataExportResult> domainService, BaseParams baseParams)
        {
            var activeUser = this.GkhUserManager.GetActiveUser()?.Id ?? 0;
            var isAdministrator = this.GkhUserManager.GetActiveUser().Roles.Any(x => x.Role.Name == "Администратор");
            var loadParam = baseParams.GetLoadParam();

            return domainService.GetAll()
                .WhereIf(!isAdministrator, x => x.Task.User.Id == activeUser)
                .Join(this.FormatDataExportRemoteResultDomain.GetAll(),
                    x => x,
                    x => x.TaskResult,
                    (x, r) => new
                    {
                        x.Id,
                        x.Task.User.Login,
                        x.Status,
                        x.StartDate,
                        x.EndDate,
                        x.Progress,
                        x.LogOperation.LogFile,
                        x.EntityCodeList,
                        r.TaskId,
                        r.FileId,
                        r.LogId,
                        RemoteStatus = r.Status,
                        r.UploadResult,
                    })
                .ToListDataResult(loadParam);
        }
    }
}