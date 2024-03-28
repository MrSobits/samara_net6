namespace Bars.Gkh.RegOperator.Regions.Tatarstan.ViewModels
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.GkhRf.Entities;

    using DataResult;
    using Entities;

    public class TransferHireViewModel : BaseViewModel<TransferHire>
    {
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }
        public IDomainService<TransferRfRecord> TransferRfRecordDomain { get; set; }

        public override IDataResult List(IDomainService<TransferHire> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            
            var transferRfRecordId = baseParams.Params.GetAs("transferRfRecordId", 0L);

            var record = this.TransferRfRecordDomain.FirstOrDefault(x => x.Id == transferRfRecordId);
            var dateTransfer = record.TransferDate ?? DateTime.MinValue;
            
            var query = domainService.GetAll()
                .Where(x => x.TransferRecord.Id == transferRfRecordId)
                .Where(x => x.Account.ServiceType == PersAccServiceType.Recruitment || x.Account.ServiceType == PersAccServiceType.OverhaulRecruitment);

            var transferRecords = TransferRfRecordDomain.GetAll()
               .Where(x => x.TransferRf.Id == record.TransferRf.Id)
               .Where(x => x.Id != record.Id)
               .Select(x => x.Id);

            // записи в том жепериоде но для других перечислений
            var beforeRecords = domainService.GetAll()
                .Where(x => transferRecords.Any(y => x.TransferRecord.Id == y)
                    && x.TransferRecord.TransferDate == record.TransferDate && x.TransferRecord.Id != record.Id)
                .GroupBy(x => x.Account.Id)
                .ToDictionary(x => x.Key, y => y.Sum(z => z.TransferredSum));

            var accountPayments = this.PersonalAccountPeriodSummaryDomain.GetAll()
                .Where(y => query.Any(x => x.Account.Id == y.PersonalAccount.Id))
                .Where(x => x.Period.StartDate.Month == dateTransfer.Month)
                .Where(x => x.Period.StartDate.Year == dateTransfer.Year)
                .Select(x => new
                {
                    x.PersonalAccount.Id,
                    Sum = x.RecruitmentPayment
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.Sum));

            var data = query
                .Select(x => new
                {
                    x.Id,
                    AccountId = x.Account.Id,
                    Flat = x.Account.Room.RoomNum,
                    RoAddress = x.Account.Room.RealityObject.Address,
                    Municipality = x.Account.Room.RealityObject.Municipality.Name,
                    AccountNum = x.Account.PersonalAccountNum,
                    x.Transferred,
                    x.TransferredSum
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Address = x.RoAddress + ", кв. " + x.Flat,
                    x.AccountNum,
                    x.Municipality,
                    x.Transferred,
                    x.TransferredSum,
                    PaidTotal = accountPayments.Get(x.AccountId),
                    BeforeTransfer = beforeRecords.ContainsKey(x.AccountId) ? beforeRecords[x.AccountId] : 0m
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            var summary = new
            {
                PaidTotal = data.Sum(x => x.PaidTotal),
                TransferredSum = data.Sum(x => x.TransferredSum),
                BeforeTransfer = data.Sum(x => x.BeforeTransfer)
            };

            return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count(), summary);
        }
    }
}