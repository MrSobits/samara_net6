namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Entities;

    public class AppealCitsEmergencyHouseViewModel : BaseViewModel<AppealCitsEmergencyHouse>
    {
        public override IDataResult List(IDomainService<AppealCitsEmergencyHouse> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            var isFiltered = baseParams.Params.GetAs<bool>("isFiltered");

            if (!isFiltered)
            {
                var data = domainService.GetAll()
            .Where(x => x.AppealCits.Id == appealCitizensId)
            .Select(x => new
            {
                x.Id,
                x.DocumentName,
                x.OMSDate,
                x.DocumentDate,
                Contragent = x.Contragent.Name,
                x.File,
                Inspector = x.Inspector.Fio,
                x.DocumentNumber
            })
            .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else
            {

                var dateStart2 = loadParams.Filter.GetAs("dateStart", new DateTime());
                var dateEnd2 = loadParams.Filter.GetAs("dateEnd", new DateTime());

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");

                var data = domainService.GetAll()
                    .Select(x => new
                        {
                            x.Id,
                            x.DocumentName,
                            x.OMSDate,
                            Contragent = x.Contragent.Name,
                            x.File,
                            Inspector = x.Inspector.Fio,
                            x.DocumentNumber,
                            x.DocumentDate
                        })
                    .Where(x => x.DocumentDate.HasValue
                            ? x.DocumentDate.Value >= dateStart && x.DocumentDate.Value <= dateEnd
                            : 1 == 1)
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}