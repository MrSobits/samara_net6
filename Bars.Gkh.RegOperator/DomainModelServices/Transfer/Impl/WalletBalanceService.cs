namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Extenstions;

    using Entities;
    using Entities.ValueObjects;
    using Gkh.Domain.CollectionExtensions;

    public class WalletBalanceService : IWalletBalanceService
    {
        public IRepository<Transfer> TransferRepo { get; set; }

        public Dictionary<string, WalletBalance> GetDebetBalance(
            IEnumerable<RealityObjectPaymentAccount> accounts,
            params Expression<Func<Transfer, bool>>[] filters)
        {
            var result = new Dictionary<string, WalletBalance>();

            var transferQuery = ApplyFilters(TransferRepo.GetAll(), filters);

            foreach (var section in accounts.Section(100))
            {
                var sectionGuids = section.SelectMany(x => x.GetWallets().Select(w => w.WalletGuid).ToList()).ToArray();

                var positiveTransfers = transferQuery
                    .Where(x => x.Operation.CanceledOperation == null)
                    .Where(x => sectionGuids.Contains(x.TargetGuid))
                    .Select(x => new
                    {
                        Guid = x.TargetGuid,
                        x.Amount
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Guid)
                    .ToDictionary(x => x.Key, z => z.Sum(x => x.Amount));

                var negativeTransfers = transferQuery
                    .Where(x => x.Operation.CanceledOperation != null)
                    .Where(x => sectionGuids.Contains(x.SourceGuid))
                    .Select(x => new
                    {
                        Guid = x.SourceGuid,
                        Amount = -1 * x.Amount
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Guid)
                    .ToDictionary(x => x.Key, z => z.Sum(x => x.Amount));

                foreach (var guid in sectionGuids)
                {
                    if (!result.ContainsKey(guid))
                    {
                        result[guid] = new WalletBalance(guid);
                    }

                    var wb = result[guid];
                    wb.Amount += positiveTransfers.Get(guid) + negativeTransfers.Get(guid);
                }
            }

            return result;
        }

        public Dictionary<string, WalletBalance> GetCreditBalance(
            IEnumerable<RealityObjectPaymentAccount> accounts,
            params Expression<Func<Transfer, bool>>[] filters)
        {
            var result = new Dictionary<string, WalletBalance>();

            var transferQuery = ApplyFilters(TransferRepo.GetAll(), filters);

            foreach (var section in accounts.Section(100))
            {
                var sectionGuids = section.SelectMany(x => x.GetWallets().Select(w => w.WalletGuid).ToList()).ToArray();

                var positiveTransfers = transferQuery
                    .Where(x => x.Operation.CanceledOperation == null)
                    .Where(x => sectionGuids.Contains(x.TargetGuid))
                    .Select(x => new
                    {
                        Guid = x.SourceGuid,
                        x.Amount
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Guid)
                    .ToDictionary(x => x.Key, z => z.Sum(x => x.Amount));

                var negativeTransfers = transferQuery
                    .Where(x => x.Operation.CanceledOperation != null)
                    .Where(x => sectionGuids.Contains(x.SourceGuid))
                    .Select(x => new
                    {
                        Guid = x.TargetGuid,
                        Amount = -1 * x.Amount
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Guid)
                    .ToDictionary(x => x.Key, z => z.Sum(x => x.Amount));

                foreach (var guid in sectionGuids)
                {
                    if (!result.ContainsKey(guid))
                    {
                        result[guid] = new WalletBalance(guid);
                    }

                    var wb = result[guid];
                    wb.Amount += positiveTransfers.Get(guid) + negativeTransfers.Get(guid);
                }
            }

            return result;
        }

        private IQueryable<Transfer> ApplyFilters(IQueryable<Transfer> query, params Expression<Func<Transfer, bool>>[] filters)
        {
            if (filters.IsNotEmpty())
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            return query;
        }
    }
}