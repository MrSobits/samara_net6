using System;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class PersonDisqualificationInfoViewModel : BaseViewModel<PersonDisqualificationInfo>
    {
        public override IDataResult List(IDomainService<PersonDisqualificationInfo> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var personId = loadParams.Filter.GetAs("personId", 0L);

            var data = domain.GetAll()
                .Where(x => x.Person.Id == personId)
                .Select(x => new
                {
                    x.Id,
                    x.TypeDisqualification,
                    x.DisqDate,
                    x.EndDisqDate,
                    x.PetitionNumber,
                    x.PetitionDate,
                    x.NameOfCourt
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}