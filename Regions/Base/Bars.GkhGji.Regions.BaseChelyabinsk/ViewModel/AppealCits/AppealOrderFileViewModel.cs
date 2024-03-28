namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.AppealCits
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealOrderFileViewModel : BaseViewModel<AppealOrderFile>
    {
        public override IDataResult List(IDomainService<AppealOrderFile> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var appealOrderId = baseParams.Params.GetAs<long>("appealOrderId");

            var data = domainService.GetAll()
                                    .WhereIf(appealOrderId > 0, x => x.AppealOrder.Id == appealOrderId)
                                    .Select(x => new
                                    {
                                        x.Id,                                    
                                        x.FileInfo,
                                        x.ObjectCreateDate,
                                        x.Description
                                    })
                                    .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        
    }
}