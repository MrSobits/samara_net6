namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Селектор для Оплата
    /// </summary>
    public class OplataSelectorService : BaseProxySelectorService<OplataProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, OplataProxy> GetCache()
        {
            var epdCache = this.ProxySelectorFactory.GetSelector<EpdProxy>()
                .GetProxyList()
                .Select(x => x.Value)
                .ToDictionary(x => x.AccountId);

            var transferQuery = this.FilterService.GetFiltredQuery<PersonalAccountPaymentTransfer>();
            var bankDocumentImportQuery = this.FilterService.GetFiltredQuery<BankDocumentImport>();
            var bankAccountStatementQuery = this.FilterService.GetFiltredQuery<BankAccountStatement>();

            var oplataPackData = OplataPackSelectorService.GetProxies(bankDocumentImportQuery, bankAccountStatementQuery)
                .ToDictionary(x => x.TransferGuid);

            var exportByDate = this.FilterService.ExportDate;

            var cashPaymentCenterPersAccRepository = this.Container.ResolveRepository<CashPaymentCenterPersAcc>();

            using (this.Container.Using(cashPaymentCenterPersAccRepository))
            {
                var cashPaymentCenterQuery = cashPaymentCenterPersAccRepository.GetAll()
                    .Where(x => x.DateStart <= exportByDate && (!x.DateEnd.HasValue || exportByDate <= x.DateEnd))
                    .Where(x => x.CashPaymentCenter.ConductsAccrual);

                var transferList = transferQuery
                    .Where(x => !cashPaymentCenterQuery.Any(y => y.PersonalAccount.Id == x.Owner.Id))
                    .Where(x => bankDocumentImportQuery.Any(y => y.TransferGuid == x.Operation.OriginatorGuid)
                        || bankAccountStatementQuery.Any(y => y.TransferGuid == x.Operation.OriginatorGuid))
                    .Select(x => new
                    {
                        x.Id,
                        AccountId = x.Owner.Id,
                        OperationType = x.Reason.StartsWith("Оплата") ? 1 : 2,
                        x.PaymentDate,
                        x.OperationDate,
                        Amount = x.Reason.StartsWith("Оплата") ? x.Amount : -1 * x.Amount,
                        RoId = x.Owner.Room.RealityObject.Id,
                        OwnerId = x.Owner.AccountOwner.Id,
                        x.Operation.OriginatorGuid
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var oplataPack = oplataPackData.Get(x.OriginatorGuid);
                        var epd = epdCache.Get(x.AccountId);
                        var paymentDate = x.PaymentDate;
                        return new OplataProxy
                        {
                            Id = x.Id,
                            KvarId = x.AccountId,
                            OperationType = x.OperationType,
                            OplataPackNumber = oplataPack?.Number,
                            PaymentDate = paymentDate,
                            OperationDate = oplataPack?.OperationDate,
                            Amount = x.Amount > 0 ? x.Amount : 0,
                            Month = new DateTime(paymentDate.Year, paymentDate.Month, 1),
                            OplataPackId = oplataPack?.Id,
                            ContragentRschetId = epd?.ContragentRschetId,
                            EpdId = epd?.Id,
                            PayerIndId = x.OwnerId,
                            PayerName = oplataPack?.PayerName,
                            Destination = oplataPack?.Destination,

                            KvisolResult = epd?.KvisolResult,
                            KvisolSum = epd?.KvisolSum,

                            EpdCapitalId = epd?.SnapshotIdCapital
                        };
                    });

                var transferListByPaymentCenterList = transferQuery
                    .Where(x => cashPaymentCenterQuery.Any(y => y.PersonalAccount.Id == x.Owner.Id))
                    .Where(x => bankDocumentImportQuery.Any(y => y.TransferGuid == x.Operation.OriginatorGuid)
                        || bankAccountStatementQuery.Any(y => y.TransferGuid == x.Operation.OriginatorGuid))
                    .Select(x => new
                    {
                        x.Id,
                        OwnerId = x.Owner.Id,
                        OperationType = x.Reason.StartsWith("Оплата") ? 1 : 2,
                        x.OperationDate,
                        x.PaymentDate,
                        Amount = x.Reason.StartsWith("Оплата") ? x.Amount : -1 * x.Amount,
                        x.Operation.OriginatorGuid
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var oplataPack = oplataPackData.Get(x.OriginatorGuid);
                        var epd = epdCache.Get(x.OwnerId);
                        var paymentDate = x.PaymentDate;
                        return new OplataProxy
                        {
                            Id = x.Id,
                            KvarId = epd?.Id,
                            OperationType = x.OperationType,
                            OplataPackNumber = oplataPack?.Number,
                            PaymentDate = paymentDate,
                            OperationDate = oplataPack?.OperationDate,
                            Amount = x.Amount > 0 ? x.Amount : 0,
                            Month = new DateTime(paymentDate.Year, paymentDate.Month, 1),
                            OplataPackId = oplataPack?.Id,
                            ContragentRschetId = epd?.ContragentRschetId,
                            EpdId = epd?.Id,
                            PayerIndId = x.OwnerId,
                            PayerName = oplataPack?.PayerName,
                            Destination = oplataPack?.Destination,

                            KvisolResult = 2,
                            KvisolSum = epd?.SaldoOut,

                            EpdCapitalId = epd?.SnapshotIdCapital
                        };
                    });

                return transferList.Union(transferListByPaymentCenterList).ToList().ToDictionary(x => x.Id);
            }
        }
    }
}