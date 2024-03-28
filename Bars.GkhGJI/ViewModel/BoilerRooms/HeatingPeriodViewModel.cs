namespace Bars.GkhGji.ViewModel.BoilerRooms
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Entities.BoilerRooms;

    public class HeatingPeriodViewModel: BaseViewModel<HeatingPeriod>
    {
        public override IDataResult List(IDomainService<HeatingPeriod> domainService, BaseParams baseParams)
        {
            var boilerRoomId = baseParams.Params.GetAs<long>("boilerRoomId", ignoreCase:true);

            var loadParams = GetLoadParam(baseParams);
            var data = domainService.GetAll()
                .Where(x => x.BoilerRoom.Id == boilerRoomId)
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}
