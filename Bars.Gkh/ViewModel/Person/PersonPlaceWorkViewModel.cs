using System;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class PersonPlaceWorkViewModel : BaseViewModel<PersonPlaceWork>
    {
        public override IDataResult List(IDomainService<PersonPlaceWork> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var personId = loadParams.Filter.GetAs("personId", 0L);

            var data = domain.GetAll()
                .Where(x => x.Person.Id == personId)
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent.ShortName != null ? x.Contragent.ShortName : x.Contragent.Name,
                    LicenzeState = "", // потом заполнить
                    x.StartDate,
                    x.FileInfo,
                    x.EndDate,
                    Position = x.Position.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}