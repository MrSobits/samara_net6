namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.Egso;
    using Bars.Gkh.Utils;

    public class EgsoIntegrationViewModel : BaseViewModel<EgsoIntegration>
    {
        public override IDataResult List(IDomainService<EgsoIntegration> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.TaskType,
                    x.StateType,
                    User = x.User.Name,
                    x.ObjectCreateDate,
                    x.EndDate,
                    x.Year,
                    LogId = x.Log != null ? x.Log.Id : (long?)null
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}