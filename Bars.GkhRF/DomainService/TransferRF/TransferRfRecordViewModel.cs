namespace Bars.GkhRf.DomainService
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Entities;
    using Gkh.Domain;
    

    public class TransferRfRecordViewModel : BaseViewModel<TransferRfRecord>
    {
        public override IDataResult List(IDomainService<TransferRfRecord> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var transferRfId = baseParams.Params.GetAsId("transferRfId");

            var transferRecordDomain = Container.ResolveDomain<TransferRfRecord>();
            
            using (Container.Using(transferRecordDomain))
            {
                var data = Container.Resolve<IDomainService<ViewTransferRfRecord>>().GetAll()
                    .Where(x => x.TransferRf.Id == transferRfId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Description,
                        x.DateFrom,
                        x.DocumentName,
                        x.DocumentNum,
                        x.State,
                        x.TransferDate,
                        x.File,
                        x.CountRecords,
                        SumRecords =  0M
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}