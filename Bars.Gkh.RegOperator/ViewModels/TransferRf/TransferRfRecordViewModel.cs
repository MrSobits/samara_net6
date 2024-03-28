namespace Bars.Gkh.RegOperator.DomainService.TransferRf
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhRf.Entities;

    using Entities;
    using Gkh.Domain;

    public class TransferRfRecordViewModel : BaseViewModel<TransferRfRecord>
    {
        public override IDataResult List(IDomainService<TransferRfRecord> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var transferRfId = baseParams.Params.GetAsId("transferRfId");

            var transferRecordDomain = Container.ResolveDomain<TransferRfRecord>();
            var transferObjDomain = Container.ResolveDomain<TransferObject>();

            using (Container.Using(transferRecordDomain, transferObjDomain))
            {
                var transferRecords = transferRecordDomain.GetAll()
                    .Where(x => x.TransferRf.Id == transferRfId)
                    .Select(x => x.Id);
                var transferRecordDict = transferObjDomain.GetAll()
                    .Where(x => transferRecords.Contains(x.TransferRecord.Id))
                    .Where(x => x.Transferred
                                && x.TransferredSum.HasValue
                                && x.TransferredSum > 0)
                    .GroupBy(x => x.TransferRecord.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.TransferredSum));

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
                        SumRecords = transferRecordDict.ContainsKey(x.Id)
                            ? transferRecordDict[x.Id]
                            : 0M
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}