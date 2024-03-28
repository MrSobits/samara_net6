using System.Linq;

namespace Bars.GkhGji.Regions.Tyumen.ViewModel
{

    using B4;
    using B4.Utils;
    using Entities;
    using System;

    public class LicensePrescriptionViewModel : BaseViewModel<LicensePrescription>
    {
        public override IDataResult List(IDomainService<LicensePrescription> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("parentId", 0L);
        
            var data = domain.GetAll()
             .Where(x => x.MorgContractRO.Id == id)
            .Select(x => new
            {
                x.Id,
                x.DocumentDate,
                x.DocumentNumber,
                x.FileInfo,
                x.Penalty,
                SanctionGji = x.SanctionGji.Name,
                x.YesNoNotSet,
                x.ActualDate,
                ArticleLawGji = x.ArticleLawGji.Name
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());

        }
    }
}