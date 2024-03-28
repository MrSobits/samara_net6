using Bars.B4;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    public class MaxSumByYearViewModel : BaseViewModel<MaxSumByYear>
    {
        public override IDataResult List(IDomainService<MaxSumByYear> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            //Сплющивание модельки и фильтрация
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality != null ? x.Municipality.Name : "Все",
                    Program = x.Program != null ? x.Program.Name : "Все",
                    x.Year,
                    x.Sum
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
