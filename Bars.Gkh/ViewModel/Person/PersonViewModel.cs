using System;
using Bars.Gkh.DomainService;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class PersonViewModel : BaseViewModel<Person>
    {

        public IPersonService PersonService { get; set; }

        public override IDataResult List(IDomainService<Person> domain, BaseParams baseParams)
        {
            var totalCount = 0;

            var result = PersonService.GetList(baseParams, true, ref totalCount);

            return new ListDataResult(result, totalCount);
        }
    }
}