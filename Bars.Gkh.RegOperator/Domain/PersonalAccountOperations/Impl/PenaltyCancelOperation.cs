namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Dto;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using NHibernate.Linq;

    /// <summary>
    /// Отмена начисления пени
    /// </summary>
    public class PenaltyCancelOperation : PersonalAccountOperationBase
    {
        private readonly IDomainService<PersonalAccountCharge> chargeDomain;
        private readonly IDomainService<ChargePeriod> periodDomain;
        private readonly IDomainService<Transfer> trDomain;
        private readonly IDomainService<MoneyOperation> mopDomain;
        private readonly IWindsorContainer container;
        private readonly IRealtyObjectPaymentSession roSession;
        private readonly IChargePeriodRepository periodrepo;
        private Dictionary<long, BasePersonalAccount> accounts;
        private Dictionary<long, PersonalAccountCharge> charges;

        /// <summary>
        /// Код
        /// </summary>
        public static string Key { get { return "PenaltyCancelOperation"; } }

        /// <summary>
        /// Конструктор
        /// </summary>
        public PenaltyCancelOperation(
            IDomainService<PersonalAccountCharge> chargeDomain,
            IDomainService<ChargePeriod> periodDomain,
            IDomainService<Transfer> trDomain,
            IDomainService<MoneyOperation> mopDomain,
            IWindsorContainer container,
            IRealtyObjectPaymentSession roSession,
            IChargePeriodRepository periodrepo)
        {

            this.chargeDomain = chargeDomain;
            this.periodDomain = periodDomain;
            this.trDomain = trDomain;
            this.mopDomain = mopDomain;
            this.container = container;
            this.roSession = roSession;
            this.periodrepo = periodrepo;
        }

        #region Implementation of IPersAccountOperation

        /// <summary>
        /// Код
        /// </summary>
        public override string Code { get { return PenaltyCancelOperation.Key; } }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name { get { return "Отмена начисления пени"; } }
        
        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            try
            {
                this.container.InTransaction(() =>
                    {
                        using (new DatabaseMutexContext("Cancel_penalty_charges", "Отмена начислений пени"))
                        {
                            var period = this.periodDomain.Load(baseParams.Params.GetAsId("periodId"));
                            var records = baseParams.Params.GetAs<List<ModifiedRecord>>("records");

                            this.PrepareData(records, period);

                            var validRecords = records.Where(this.RecordIsValid).ToList();

                            if (validRecords.IsNotEmpty())
                            {
                                var changeInfo = PersonalAccountChangeInfo.FromParams(baseParams);

                                using (var tr = this.container.Resolve<IDataTransaction>())
                                {
                                    foreach (var record in validRecords)
                                    {
                                        this.CancelCharge(record, changeInfo);
                                    }

                                    try
                                    {
                                        this.roSession.Complete();
                                        tr.Commit();
                                    }
                                    catch
                                    {
                                        this.roSession.Rollback();
                                        tr.Rollback();
                                        throw;
                                    }
                                }
                            }
                        }
                    });

                return new BaseDataResult();
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Отмена начислений пени уже запущена");
            }
        }

        public override IDataResult GetDataForUI(BaseParams @params)
        {
            var loadParam = @params.GetLoadParam();
            var periodId = @params.Params.GetAsId("periodId");
            var ids = @params.Params.GetAs<string>("ids").ToLongArray();

            var chargePeriod = this.periodDomain.Get(periodId);
            var periodStart = chargePeriod.StartDate;
            var periodEnd = chargePeriod.GetEndDate();

            var persAccCharges = this.chargeDomain.GetAll()
                .Where(x => ids.Contains(x.BasePersonalAccount.Id))
                .Where(x => x.IsFixed)
                .Where(x => x.ChargeDate >= periodStart && x.ChargeDate <= periodEnd)
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.BasePersonalAccount.Room.RealityObject.Municipality.Name,
                        x.BasePersonalAccount.Room.RealityObject.Address,
                        x.BasePersonalAccount.PersonalAccountNum,
                        Penalty = x.Penalty + x.RecalcPenalty,
                        CancellationSum = x.Penalty + x.RecalcPenalty
                    });

            var result = persAccCharges
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                .Order(loadParam)
                .ToList();

            return new ListDataResult(result, persAccCharges.Count());
        }

        #endregion

        private void CancelCharge(ModifiedRecord record, PersonalAccountChangeInfo changeInfo)
        {
            var account = this.accounts.Get(record.Id);
            var charge = this.charges.Get(record.Id);

            var moneyOperation = charge.CreateOperation(this.periodrepo.GetCurrentPeriod());
            moneyOperation.Document = changeInfo.Document;

            var transfer = account.UndoPenalty(charge, moneyOperation, record.CancellationSum, changeInfo);

            if (transfer != null)
            {
                this.mopDomain.Save(moneyOperation);
                this.trDomain.Save(transfer);
            }
        }

        private bool RecordIsValid(ModifiedRecord record)
        {
            if (this.charges.ContainsKey(record.Id))
            {
                var rec = this.charges.Get(record.Id);
                return record.CancellationSum > 0
                    && (rec.Penalty + rec.RecalcPenalty) >= record.CancellationSum;
            }
            return false;
        }

        private void PrepareData(List<ModifiedRecord> records, ChargePeriod period)
        {
            var chargesId = records.Where(x => x.Id > 0).Select(x => x.Id).ToList();

            var charges = this.chargeDomain.GetAll()
                .Where(x => chargesId.Contains(x.Id))
                .Where(x => x.ChargeDate >= period.StartDate && x.ChargeDate <= period.EndDate)
                .Fetch(x => x.BasePersonalAccount)
                .Select(
                    x => new
                    {
                        x.Id,
                        Charge = x
                    });
            this.charges = charges.ToDictionary(x => x.Id, x => x.Charge);

            this.accounts = this.charges.Values.ToDictionary(x => x.Id, x => x.BasePersonalAccount);
        }
    }
}