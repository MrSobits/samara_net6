namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.AppealCits
{
    using B4;
    using B4.IoC;

    using Entities.AppealCits;
    using GkhGji.DomainService;
    using GkhGji.Entities;

    public class AppealCitsBaseChelyabinskViewModel : GkhGji.ViewModel.AppealCitsViewModel
    {
        public override IDataResult List(IDomainService<AppealCits> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizensBaseChelyabinsk>>();

            using (this.Container.Using(service))
            {
                int totalCount;
                var data = service.GetViewModelList(baseParams, out totalCount, true);
                return new ListDataResult(data, totalCount);
            }
        }
    }
}
