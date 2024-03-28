namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class ProgramVersionViewModel : BaseViewModel<ProgramVersion>
    {
        public override IDataResult List(IDomainService<ProgramVersion> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = domainService.GetAll();

            var muId = baseParams.Params.GetAs<long>("muId");
            if(muId != 0)
                data = data.Where(x => x.Municipality.Id == muId);

            //отображать неосновные?
            var isChecked = baseParams.Params.GetAs("isChecked", true);
            if (!isChecked)
                data = data.Where(x => x.IsMain == true);

            data = data.Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).AsEnumerable(), data.Count());

        }
    }
}
