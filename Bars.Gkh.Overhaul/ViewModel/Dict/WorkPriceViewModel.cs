namespace Bars.Gkh.Overhaul.ViewModel
{
    using B4;

    using Bars.Gkh.Overhaul.DomainService;

    using Entities;

    // Пустышка на случай если от класс наследвоались
    public class WorkPriceViewModel : WorkPriceViewModel<WorkPrice>
    {
        // Внимание расширять нужно Generic а не данный класс
    }

    // GEneric для того чтобы расширять в регионах без дублирвоания функционала
    public class WorkPriceViewModel<T> : BaseViewModel<T>
        where T : WorkPrice
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<T>>();
            try
            {
                int totalCount = 0;
                var list = service.GetListView(baseParams, true, ref totalCount);
                return new ListDataResult(list, totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}