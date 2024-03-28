namespace Bars.GkhGji.Regions.Smolensk.ViewModel
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ActRemovalSmolViewModel : Bars.GkhGji.ViewModel.ActRemovalViewModel<ActRemovalSmol>
    {
        public override IDataResult Get(IDomainService<ActRemovalSmol> domainService, BaseParams baseParams)
        {
            var data = base.Get(domainService, baseParams);

            var obj = (ActRemovalSmol)data.Data;

            return
                    new BaseDataResult(obj);
        }

    }
}
