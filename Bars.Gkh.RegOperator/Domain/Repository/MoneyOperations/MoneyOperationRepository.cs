namespace Bars.Gkh.RegOperator.Domain.Repository.MoneyOperations
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Entities;
    using Entities.ValueObjects;
    using DomainModelServices;

    public class MoneyOperationRepository : IMoneyOperationRepository
    {
        private readonly IDomainService<MoneyOperation> _domain;

        public MoneyOperationRepository(IDomainService<MoneyOperation> domain)
        {
            _domain = domain;
        }

        public MoneyOperation GetCurrentMoneyOperationFor(SuspenseAccount suspenseAccount)
        {
            return GetNotCancelled()
                .SingleOrDefault(x => x.OriginatorGuid == suspenseAccount.TransferGuid);
        }

        public IQueryable<MoneyOperation> GetOperationsByOriginator(IEnumerable<ITransferParty> source)
        {
            var filterGuids = source.Select(x => x.TransferGuid).ToArray();
            return GetNotCancelled().Where(x => filterGuids.Contains(x.OriginatorGuid));
        }

        public IQueryable<MoneyOperation> GetOperationsByOriginator(IQueryable<ITransferParty> source)
        {
            return GetNotCancelled().Where(z => source.Any(x => x.TransferGuid == z.OriginatorGuid));
        }

        private IQueryable<MoneyOperation> GetNotCancelled()
        {
            return _domain.GetAll()
                .Where(x => !x.IsCancelled);
        }
    }
}