namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Interceptors.TransferFunds
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    using Bars.GkhRf.Entities;

    public class TransferRfRecordInterceptor : EmptyDomainInterceptor<TransferRfRecord>
    {
        public IDomainService<TransferHire> TransferHireDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<TransferRfRecord> service, TransferRfRecord entity)
        {
            TransferHireDomain.GetAll()
                .Where(x => x.TransferRecord.Id == entity.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => TransferHireDomain.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}