namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class MkdChangeNotificationFileViewModel : BaseViewModel<MkdChangeNotificationFile>
    {
		public override IDataResult List(IDomainService<MkdChangeNotificationFile> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
			var notificationId = baseParams.Params.GetAs<long>("notificationId");

            var data = domainService.GetAll()
				.Where(x => x.MkdChangeNotification.Id == notificationId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Number,
					x.Date,
					x.Desc,
					x.File
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}