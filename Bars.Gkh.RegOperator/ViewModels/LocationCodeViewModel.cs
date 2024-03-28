namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class LocationCodeViewModel : BaseViewModel<LocationCode>
    {
        public override IDataResult List(IDomainService<LocationCode> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    FiasLevel1 = x.FiasLevel1.Name,
                    FiasLevel2 = x.FiasLevel2.Name,
                    x.FiasLevel3
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}