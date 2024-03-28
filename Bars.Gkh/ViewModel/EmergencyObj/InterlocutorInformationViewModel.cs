namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Entities.EmergencyObj;


    public class InterlocutorInformationViewModel : BaseViewModel<InterlocutorInformation>
    {
        public override IDataResult List(IDomainService<InterlocutorInformation> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.ApartmentNumber,
                    x.ApartmentArea,
                    x.FIO,
                    x.PropertyType,
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}