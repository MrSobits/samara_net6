namespace Bars.Gkh.ViewModel
{
    using Bars.B4;
    using System.Linq;

    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class MeteringDevicesChecksViewModel : BaseViewModel<MeteringDevicesChecks>
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public override IDataResult List(IDomainService<MeteringDevicesChecks> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
            .Where(x => x.RealityObject.Id == objectId)
            .Select(x => new
            {
                x.Id,
                x.MeteringDevice.PersonalAccountNum,
                x.MarkMeteringDevice,
                x.EndDateCheck, 
                x.NextDateCheck
            })
            .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}