namespace Bars.GkhGji.ViewModel
{
    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ProtocolMhcViewModel : ProtocolMhcViewModel<ProtocolMhc>
    {
    }

    public class ProtocolMhcViewModel<T> : BaseViewModel<T>
        where T : ProtocolMhc
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolMhcService>();

            try
            {
                var totalCount = 0;
                var list = service.GetViewModelList(baseParams, false, ref totalCount);
                return new ListDataResult(list, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
