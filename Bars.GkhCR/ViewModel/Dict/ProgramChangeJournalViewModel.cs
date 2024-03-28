using Bars.B4.Utils;

namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class ProgramChangeJournalViewModel : BaseViewModel<ProgramCrChangeJournal>
    {
        public override IDataResult List(IDomainService<ProgramCrChangeJournal> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var programCrId = loadParams.Filter.Get("programCrId", 0);

            var data = domain
                .GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
