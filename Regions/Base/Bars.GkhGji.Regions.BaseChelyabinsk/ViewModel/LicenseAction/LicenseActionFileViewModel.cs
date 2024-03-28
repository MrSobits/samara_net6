namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class LicenseActionFileViewModel : BaseViewModel<LicenseActionFile>
    {
		public override IDataResult List(IDomainService<LicenseActionFile> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
			var licenseActionId = baseParams.Params.GetAs<long>("licenseActionId");

            var data = domainService.GetAll()
				.Where(x => x.LicenseAction.Id == licenseActionId)
                .Select(x => new
                {
                    x.Id,
                    x.FileName,
                    x.SignedInfo,
                    x.FileInfo
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}