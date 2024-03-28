namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using GkhRf.Entities;

    public class TransferHireService : ITransferHireService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<TransferRfRecord> TransferRfRecordDomain { get; set; }
        public IDomainService<ContractRfObject> ContractRfObjectDomain { get; set; }
        public IDomainService<TransferHire> TransferHireDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        //public IDomainService<PersAccServiceType> PersAccServiceTypeDomain { get; set; }

        public IDataResult Calc(BaseParams baseParams)
        {
            var transferRecordId = baseParams.Params.GetAsId("transferRecordId");
            //var recruitTypeService = PersAccServiceTypeDomain.FirstOrDefault(x => x.Code == "15");

            var record = TransferRfRecordDomain.FirstOrDefault(x => x.Id == transferRecordId);
            if (record.IsCalculating)
            {
                return BaseDataResult.Error("По данному договору в данном периоде уже ведётся расчёт!");
            }

            try
            {
                record.IsCalculation = true;
                TransferRfRecordDomain.Update(record);

                return CalcInternal(record);
            }
            catch (Exception ex)
            {
                return BaseDataResult.Error(ex.Message);
            }
            finally
            {
                var sessionProvider = Container.Resolve<ISessionProvider>();
                using (Container.Using(sessionProvider))
                using (sessionProvider.GetCurrentSession())
                {
                    record.IsCalculation = false;
                    TransferRfRecordDomain.Update(record);
                }
            }
        }

        protected virtual IDataResult CalcInternal(TransferRfRecord record)
        {
            var dateTransfer = record.TransferDate.HasValue ? record.TransferDate.Value : DateTime.MinValue;

            // текущие записи
            var existRecords = TransferHireDomain.GetAll()
                .Where(x => x.TransferRecord.Id == record.Id)
                .Where(
                    x =>
                        x.Account.ServiceType == PersAccServiceType.Recruitment ||
                        x.Account.ServiceType == PersAccServiceType.OverhaulRecruitment)
                .GroupBy(x => x.Account.Id)
                .ToDictionary(x => x.Key, y => y.First());

            var transferRecords = TransferRfRecordDomain.GetAll()
                .Where(x => x.TransferRf.Id == record.TransferRf.Id)
                .Where(x => x.Id != record.Id)
                .Select(x => x.Id);

            // записи в том жепериоде но для других перечислений
            var beforeRecords = TransferHireDomain.GetAll()
                .Where(x => transferRecords.Any(y => x.TransferRecord.Id == y))
                .GroupBy(x => x.Account.Id)
                .ToDictionary(x => x.Key, y => y.Sum(z => z.TransferredSum));

            // не берем те дома, которые на дату перечисления были исключены, либо еще не включены
            var filterInlcuded = ContractRfObjectDomain.GetAll()
                .Where(x => x.ContractRf.Id == record.TransferRf.ContractRf.Id)
                .Where(x => x.IncludeDate <= dateTransfer)
                .Where(x => x.ExcludeDate >= dateTransfer || x.ExcludeDate == null)
                .Select(x => x.RealityObject.Id);

            var accountsQuery = BasePersonalAccountDomain.GetAll()
                .Where(x => filterInlcuded.Contains(x.Room.RealityObject.Id))
                .Where(
                    x =>
                        x.ServiceType == PersAccServiceType.Recruitment ||
                        x.ServiceType == PersAccServiceType.OverhaulRecruitment);

            var accountPayments = PersonalAccountPeriodSummaryDomain.GetAll()
                .Where(y => accountsQuery.Any(x => x.Id == y.PersonalAccount.Id))
                .Where(x => x.Period.StartDate.Month == dateTransfer.Month)
                .Where(x => x.Period.StartDate.Year == dateTransfer.Year)
                .Select(x => new
                {
                    x.PersonalAccount.Id,
                    x.RecruitmentPayment
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Sum(x => x.RecruitmentPayment));

            var accountsList = accountsQuery.Select(x => x.Id).ToList();

            var listToSave = new List<TransferHire>();

            // Проходим по счетам
            foreach (var accId in accountsList)
            {
                // Всего оплачено в этом периоде
                var paidTotal = 0m;

                // Всего перечислено по данному счету но по другим записям в том же периоде
                var beforeTransfer = 0m;

                // всего требуется перечислить
                var transferSum = 0m;

                if (accountPayments.ContainsKey(accId))
                {
                    paidTotal = accountPayments[accId];
                }

                if (beforeRecords.ContainsKey(accId))
                {
                    beforeTransfer = beforeRecords[accId];
                }
                TransferHire transferHire = null;

                if (existRecords.ContainsKey(accId))
                {
                    transferHire = existRecords[accId];
                    transferSum = transferHire.TransferredSum;
                }

                if (transferHire == null)
                {
                    if (paidTotal > 0)
                    {
                        transferHire = new TransferHire
                        {
                            Account = BasePersonalAccountDomain.Load(accId),
                            TransferRecord = record,
                            TransferredSum = paidTotal,
                            Transferred = paidTotal != 0
                        };

                        listToSave.Add(transferHire);
                    }
                }
                else if (paidTotal > 0 && paidTotal >= beforeTransfer + transferSum)
                {
                    /* Если сумма оплат больше, чем все перечисления по данному счету
                     * то надо обновить существующую запись
                     */
                    transferHire.TransferredSum = paidTotal - beforeTransfer;
                    transferHire.Transferred = transferHire.TransferredSum != 0;

                    listToSave.Add(transferHire);
                }
                else if (transferSum != paidTotal)
                {
                    transferHire.TransferredSum = paidTotal;
                    transferHire.Transferred = transferHire.TransferredSum != 0;
                }
            }

            foreach (var rec in listToSave)
            {
                if (rec.Id > 0)
                {
                    TransferHireDomain.Update(rec);
                }
                else
                {
                    TransferHireDomain.Save(rec);
                }
            }

            return new BaseDataResult();
        }
    }
}