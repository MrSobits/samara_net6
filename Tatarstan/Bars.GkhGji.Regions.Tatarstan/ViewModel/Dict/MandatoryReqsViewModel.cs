using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    public class MandatoryReqsViewModel : BaseViewModel<MandatoryReqs>
    {
        public override IDataResult List(IDomainService<MandatoryReqs> domainService, BaseParams baseParams)
        {
            var normDocService = this.Container.ResolveDomain<MandatoryReqsNormativeDoc>();

            var norm = normDocService.GetAll()
                .Select(x => new { x.Npa.FullName, x.MandatoryReqs.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id).ToDictionary(x=> x.Key, x=> string.Join(", ", x.Select(s => s.FullName).ToArray()));


            var result = domainService.GetAll()               
               .AsEnumerable()
               .Select(x=> new 
               {
                   x.Id, 
                   x.MandratoryReqName, 
                   x.MandratoryReqContent, 
                   NpaFullName = norm.ContainsKey(x.Id) ? norm[x.Id] : "",
                   x.TorId,
                   x.StartDateMandatory,
                   x.EndDateMandatory
               });

            return result.ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
