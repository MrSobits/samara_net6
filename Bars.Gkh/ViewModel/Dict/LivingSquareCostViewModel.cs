using Bars.B4;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Dicts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.ViewModel.Dict
{
    class LivingSquareCostViewModel : BaseViewModel<LivingSquareCost>
    {
        public override IDataResult List(IDomainService<LivingSquareCost> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    x.Cost,
                    x.Year
                }
                

                ).Filter(loadParams,Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
           
        }
        
    }
}
