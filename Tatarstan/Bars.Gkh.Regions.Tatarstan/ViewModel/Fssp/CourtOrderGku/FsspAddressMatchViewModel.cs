namespace Bars.Gkh.Regions.Tatarstan.ViewModel.Fssp.CourtOrderGku
{
    using System.Linq;
    using System.Linq.Dynamic;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;
    using Bars.Gkh.Utils;

    public class FsspAddressMatchViewModel : BaseViewModel<FsspAddressMatch>
    {
        public IDataResult ImportedAddressMatchingList(IDomainService<FsspAddressMatch> domainService, BaseParams baseParams)
        {
            var showAll = baseParams.Params.GetAs<bool>("showAll");
            var loadParams = baseParams.GetLoadParam();
            var pgmuAddressService = this.Container.Resolve<IPgmuAddressService>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();

            using (Container.Using(pgmuAddressService))
            {
                var data = domainService
                    .GetAll()
                    .WhereIf(!showAll, x => x.UploadDownloadInfo.User.Id == userIdentity.UserId)
                    .Select(x => new
                    {
                        FileName = $"{x.UploadDownloadInfo.DownloadFile.Name}.{x.UploadDownloadInfo.DownloadFile.Extention}",
                        x.FsspAddress
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.FsspAddress.Id,
                        x.FileName,
                        FileAddress = x.FsspAddress.Address,
                        IsMatched = x.FsspAddress.PgmuAddress != null,
                        SystemAddress = pgmuAddressService.CombinePgmuAddress(x.FsspAddress.PgmuAddress)
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);
                
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}