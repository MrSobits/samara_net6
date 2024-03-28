namespace Bars.GkhGji.ViewModel
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class AppealCitsViewModel : AppealCitsViewModel<AppealCits>
    {
    }

    /// <summary>
    /// Перекрыт в модуле Интеграция с ЭДО и BaseChelyabinsk
    /// </summary>
    public class AppealCitsViewModel<T> : BaseViewModel<T>
        where T : AppealCits
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();
            IList data;
            int totalCount;

            using (this.Container.Using(service))
            {
                data = service.GetViewModelList(baseParams, out totalCount, true);
            }

            return new ListDataResult(data, totalCount);
        }
    }
}