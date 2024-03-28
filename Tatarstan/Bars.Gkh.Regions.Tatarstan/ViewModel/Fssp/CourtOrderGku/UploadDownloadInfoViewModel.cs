namespace Bars.Gkh.Regions.Tatarstan.ViewModel.Fssp.CourtOrderGku
{
    using System.Linq;
    using System.Linq.Dynamic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Utils;

    public class UploadDownloadInfoViewModel : BaseViewModel<UploadDownloadInfo>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<UploadDownloadInfo> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var userRoleDomain = this.Container.ResolveDomain<UserRole>();
            bool viewAll;
            using (this.Container.Using(userIdentity, userRoleDomain))
            {
                viewAll = userRoleDomain.GetAll()
                    .Any(x => x.User.Id == userIdentity.UserId
                        && x.Role.Permissions.Any(y =>
                            y.PermissionId == "Clw.Fssp.CourtOrderGku.UploadDownloadInfo.View"));
            }

            return domainService.GetAll()
                .WhereIf(!viewAll, x=> x.User.Id == userIdentity.UserId)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    DownloadFile = x.DownloadFile != null ? $"{x.DownloadFile.Id}|{x.DownloadFile.Name}.{x.DownloadFile.Extention}" : null,
                    x.DateDownloadFile,
                    User = x.User.Name,
                    x.Status,
                    LogFile = x.LogFile != null ? $"{x.LogFile.Id}|{x.LogFile.Name}.{x.LogFile.Extention}" : null,
                    UploadFile = x.UploadFile != null ? $"{x.UploadFile.Id}|{x.UploadFile.Name}.{x.UploadFile.Extention}" : null
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}