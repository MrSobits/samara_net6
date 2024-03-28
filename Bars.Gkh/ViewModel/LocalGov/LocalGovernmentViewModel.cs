namespace Bars.Gkh.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    // Пустышка на тот случай если наследвоались в других модулях
    public class LocalGovernmentViewModel : LocalGovernmentViewModel<LocalGovernment>
    {
        // Внимание все override и новые метод писать в Generic класс
    }

    /// <summary>
    /// Реализация ViewModel
    /// </summary>
    public class LocalGovernmentViewModel<T> : BaseViewModel<T>
        where T : LocalGovernment
    {
        public override IDataResult List(IDomainService<T> domain, BaseParams baseParams)
        {
            var service = Container.Resolve<ILocalGovernmentService>();

            try
            {
                int totalCount;
                return new ListDataResult(service.GetViewModelList(baseParams, out totalCount, false), totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }        
    }
} 