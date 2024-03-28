namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class GISGMPPaymentsViewModel : BaseViewModel<GISGMPPayments>
    {
        public override IDataResult List(IDomainService<GISGMPPayments> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("GisGmp", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.GisGmp.Id == id)
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.Amount,
                x.Kbk,
                x.OKTMO,
                x.PaymentDate,
                x.Purpose,
                x.SupplierBillID,
                x.PaymentId,
                x.Reconcile
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}