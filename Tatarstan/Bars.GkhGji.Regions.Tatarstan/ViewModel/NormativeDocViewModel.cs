using System;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Utils;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities.Dicts;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    public class NormativeDocViewModel : BaseViewModel<NormativeDoc>
    {
        public override IDataResult List(IDomainService<NormativeDoc> domainService, BaseParams baseParams)
        {
            var documentDate = baseParams.Params.GetAs<DateTime?>("documentDate");

            return domainService.GetAll()
                    .WhereIf(documentDate != null, x => documentDate >= x.DateFrom && documentDate <= x.DateTo)
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}