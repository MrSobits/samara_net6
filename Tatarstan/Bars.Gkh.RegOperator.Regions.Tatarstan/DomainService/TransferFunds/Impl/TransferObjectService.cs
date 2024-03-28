namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;
    using Gkh.Domain;
    using Gkh.Entities;
    using GkhRf.Entities;

    /// <summary>
    /// Сервис перечислений средств
    /// </summary>
    public class TransferObjectService : ITransferObjectService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }
        public IDomainService<TransferObject> TransferObjectDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<TransferRfRecord> TransferRfRecordDomain { get; set; }
        public IDomainService<ContractRfObject> ContractRfObjectDomain { get; set; }
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }
        public IDomainService<RentPaymentIn> RentPaymentInDomain { get; set; }
        public IDomainService<AccumulatedFunds> AccumulatedFundsDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Расчитать начисления
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Пустой результат расчёта</returns>
        public IDataResult Calc(BaseParams baseParams)
        {
            var transferRecordId = baseParams.Params.GetAsId("transferRecordId");

            var record = TransferRfRecordDomain.Get(transferRecordId);

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
            };
        }

        private IDataResult CalcInternal(TransferRfRecord record)
        {
            var transferRecords = TransferRfRecordDomain.GetAll()
                .Where(x => x.TransferRf.Id == record.TransferRf.Id)
                .Where(
                    x =>
                        x.TransferDate.HasValue && record.TransferDate.HasValue &&
                        x.TransferDate.Value.Date == record.TransferDate.Value.Date)
                .Where(x => x != record)
                .Select(x => x.Id);

            // Не берем те дома, которые на дату перечисления были исключены, либо еще не включены
            var filterInlcuded = ContractRfObjectDomain.GetAll()
                .Where(x => x.ContractRf.Id == record.TransferRf.ContractRf.Id)
                .Where(x => x.IncludeDate <= record.TransferDate)
                .Where(x => x.ExcludeDate >= record.TransferDate || x.ExcludeDate == null)
                .Select(x => x.RealityObject.Id);

            //var overhaulTypeService = PersAccServiceTypeDomain.FirstOrDefault(x => x.Code == "269");
            //var overhaulTypePersAccRoIds =
            //    BasePersonalAccountDomain.GetAll()
            //        .Where(x => filterInlcuded.Any(y => y == x.Room.RealityObject.Id))
            //        .WhereIf(overhaulTypeService != null, x => x.ServiceType.Id == overhaulTypeService.Id)
            //        .Where(x => x.Room != null && x.Room.RealityObject != null)
            //        .Select(x => x.Room.RealityObject.Id);

            var overhaulTypePersAccRoIds =
                BasePersonalAccountDomain.GetAll()
                    .Where(x => filterInlcuded.Any(y => y == x.Room.RealityObject.Id))
                    .Where(
                        x =>
                            x.ServiceType == PersAccServiceType.Overhaul ||
                            x.ServiceType == PersAccServiceType.OverhaulRecruitment)
                    .Where(x => x.Room != null && x.Room.RealityObject != null)
                    .Select(x => x.Room.RealityObject.Id);

            // Запрос на исключение домов, которые в текущем периоде уже перечисляли
            var excludePaidQuery = TransferObjectDomain.GetAll()
                .Where(x => transferRecords.Contains(x.TransferRecord.Id))
                .Where(x => x.Transferred && x.TransferredSum != 0)
                .Select(x => x.RealityObject.Id);

            // Собираем для фильтрации
            var chargeAccountRoIds = Container.ResolveDomain<RealityObjectChargeAccountOperation>().GetAll()
                .Where(y => !excludePaidQuery.Contains(y.Account.RealityObject.Id))
                .Where(y => overhaulTypePersAccRoIds.Any(x => x == y.Account.RealityObject.Id))
                .Where(x => x.Date.Month == record.TransferDate.Value.Month)
                .Where(x => x.Date.Year == record.TransferDate.Value.Year)
                .Select(x => x.Account.RealityObject.Id)
                .Distinct()
                .ToList();

            var period = ChargePeriodDomain.GetAll()
                .Where(x => x.StartDate.Month == record.TransferDate.Value.Month)
                .FirstOrDefault(x => x.StartDate.Year == record.TransferDate.Value.Year);

            // Так как в базе хранятся суммы по всем типам собственности, придется расчитать суммы самим
            var chargeAccountPaids = this.CalcChargeAccountPaids(period, chargeAccountRoIds);

            var existRecords = TransferObjectDomain.GetAll()
                .Where(x => x.TransferRecord.Id == record.Id)
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.First());

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var account in chargeAccountPaids)
                    {
                        if (existRecords.ContainsKey(account.Key))
                        {
                            var rec = existRecords[account.Key];

                            existRecords.Remove(account.Key);

                            rec.TransferredSum = account.Value;
                            rec.Transferred = account.Value != 0;

                            TransferObjectDomain.Update(rec);
                        }
                        else
                        {
                            var transferObject = new TransferObject
                            {
                                RealityObject = RealityObjectDomain.Load(account.Key),
                                TransferRecord = record,
                                Transferred = account.Value != 0,
                                TransferredSum = account.Value
                            };

                            TransferObjectDomain.Save(transferObject);
                        }
                    }

                    foreach (var transferObject in existRecords)
                    {
                        TransferObjectDomain.Delete(transferObject.Value.Id);
                    }

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Посчитать начисления по домам за период, исключая начисления по помещениям с типом собственности "Муниципальная"
        /// </summary>
        /// <param name="date"> Период начислений </param>
        /// <param name="chargeAccountRoIds"> Список идентификаторов домов </param>
        /// <returns> Словарь: Ключ - ИД дома; Значение - начисления </returns>
        public Dictionary<long, decimal> GetPaids(DateTime date, IQueryable<long> chargeAccountRoIds)
        {
            var chargePeriodDomain = this.Container.ResolveDomain<ChargePeriod>();

            try
            {
                var period = chargePeriodDomain.GetAll().FirstOrDefault(x => x.StartDate.Month == date.Month && x.StartDate.Year == date.Year);
                return this.GetChargeAccountPaids(period, chargeAccountRoIds);
            }
            finally
            {
                this.Container.Release(chargePeriodDomain);
            }
        }

        /// <summary>
        /// Посчитать начисления по домам за период, исключая начисления по помещениям с типом собственности "Муниципальная"
        /// </summary>
        /// <param name="period"> Период начислений </param>
        /// <param name="chargeAccountRoIds"> Список идентификаторов домов </param>
        /// <returns> Словарь: Ключ - ИД дома; Значение - начисления </returns>
        private Dictionary<long, decimal> CalcChargeAccountPaids(ChargePeriod period, List<long> chargeAccountRoIds)
        {
            if (period == null)
            {
                return new Dictionary<long, decimal>();
            }

            var persAccPerQuery = PersonalAccountPeriodSummaryDomain.GetAll()
                .Where(x => x.Period.Id == period.Id)
                .Where(x => chargeAccountRoIds.Contains(x.PersonalAccount.Room.RealityObject.Id))
                .GroupBy(x => x.PersonalAccount.Room.RealityObject.Id)
                .Select(x => new
                {
                    x.Key,
                    PaidSum = x.Sum(y => y.OverhaulPayment),
                    AccountId = x.Max(y => y.PersonalAccount.Id)
                })
                .ToList();

            var rentPaymentQuery = this.RentPaymentInDomain.GetAll().GroupBy(x => x.Account.Id).ToDictionary(x => x.Key);
            var accumFundsQuery = this.AccumulatedFundsDomain.GetAll().GroupBy(x => x.Account.Id).ToDictionary(x => x.Key);

            var result = new Dictionary<long, decimal>();

            foreach (var chargeAcc in persAccPerQuery)
            {
                var paidTotal = chargeAcc.PaidSum;

                if (rentPaymentQuery.ContainsKey(chargeAcc.AccountId))
                {
                    paidTotal += rentPaymentQuery[chargeAcc.AccountId]
                        .Where(x => x.OperationDate >= period.StartDate)
                        .Where(x => !period.EndDate.HasValue || x.OperationDate <= period.EndDate)
                        .Sum(x => x.Sum);
                }

                if (accumFundsQuery.ContainsKey(chargeAcc.AccountId))
                {
                    paidTotal += accumFundsQuery[chargeAcc.AccountId]
                        .Where(x => x.ObjectCreateDate >= period.StartDate)
                        .Where(x => x.ObjectCreateDate <= period.EndDate)
                        .Sum(x => x.Sum);
                }

                result[chargeAcc.Key] = paidTotal;
            }

            return result;
        }

        /// <summary>
        /// Получить начисления по домам за период, исключая начисления по помещениям с типом собственности "Муниципальная"
        /// </summary>
        /// <param name="period"> Период начислений </param>
        /// <param name="chargeAccountRoIds"> Список идентификаторов домов </param>
        /// <returns> Словарь: Ключ - ИД дома; Значение - начисления </returns>
        private Dictionary<long, decimal> GetChargeAccountPaids(ChargePeriod period, IQueryable<long> chargeAccountRoIds)
        {
            if (period == null)
            {
                return new Dictionary<long, decimal>();
            }
            
            return PersonalAccountPeriodSummaryDomain.GetAll()
                .Where(x => x.Period.Id == period.Id)
                .Where(x => chargeAccountRoIds.Contains(x.PersonalAccount.Room.RealityObject.Id))
                .GroupBy(x => x.PersonalAccount.Room.RealityObject.Id)
                .Select(x => new
                {
                    x.Key,
                    PaidSum = x.Sum(y => y.OverhaulPayment)
                })
                .ToDictionary(x => x.Key, x => x.PaidSum);
        }
    }
}