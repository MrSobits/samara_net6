namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class VprVprPrescriptionViewModel : BaseViewModel<VprPrescription>
    {
        public override IDataResult List(IDomainService<VprPrescription> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var parentId = loadParams.Filter.GetAs("parentId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ROMCategory.Id == parentId)
                .Select(x => new
                {
                    x.Id,
                    Prescription = x.Prescription.DocumentNumber,
                    PrescriptionDate = x.Prescription.DocumentDate,
                    x.StateName
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}