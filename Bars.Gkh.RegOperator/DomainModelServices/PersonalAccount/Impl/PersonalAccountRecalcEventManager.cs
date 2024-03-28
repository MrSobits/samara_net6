namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Entities;
    using Entities.PersonalAccount;
    using Gkh.Domain;
    using PersonalAccount;

    public class PersonalAccountRecalcEventManager : IPersonalAccountRecalcEventManager
    {
        private readonly IDictionary<string, PersonalAccountRecalcEvent> eventBuffer;

        private readonly ISessionProvider sessions;
        private readonly IDomainService<PersonalAccountRecalcEvent> eventDomain;
        private readonly IChargePeriodRepository periodRepository;
        private readonly IDomainService<PersonalAccountPaymentTransfer> transferDomain;

        private IDictionary<long, DateTime> paymentDateCache;

        public PersonalAccountRecalcEventManager(ISessionProvider sessions, 
            IDomainService<PersonalAccountRecalcEvent> eventDomain,
            IChargePeriodRepository periodRepository,
            IDomainService<PersonalAccountPaymentTransfer> transferDomain)
        {
            this.sessions = sessions;
            this.eventDomain = eventDomain;
            this.periodRepository = periodRepository;

            this.eventBuffer = new Dictionary<string, PersonalAccountRecalcEvent>();
            this.paymentDateCache = new Dictionary<long, DateTime>();

            this.transferDomain = transferDomain;
        }

        #region Implementation of IPersonalAccountRecalcEventCreator

        /// <summary>
        /// Создание отсечки для перерасчета пеней
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <param name="eventDate">Реальная дата наступления события</param>
        /// <param name="eventType">Тип события перерасчета</param>
        /// <param name="description">Описание события</param>
        public void CreatePenaltyEvent(BasePersonalAccount account, DateTime eventDate, RecalcEventType eventType, string description = null)
        {
            var date = eventType == RecalcEventType.Payment ? this.GetEventDateInternal(account, eventDate) : eventDate;
            this.CreateRecalcEvent(account, date, PersonalAccountRecalcEvent.PenaltyType, description, eventType);
        }

        /// <summary>
        /// Создание отсечки для перерасчета начисления
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <param name="eventDate">Реальная дата наступления события</param>
        /// <param name="eventType">Тип события перерасчета</param>
        /// <param name="description">Описание события</param>
        public void CreateChargeEvent(BasePersonalAccount account, DateTime eventDate, RecalcEventType eventType, string description = null)
        {
            this.CreateRecalcEvent(account, eventDate, PersonalAccountRecalcEvent.ChargeType, description, eventType);
        }

        /// <inheritdoc />
        public void InitCache(IDictionary<DateTime, BasePersonalAccount[]> accountInfo)
        {
            this.paymentDateCache.Clear();

            foreach (var kvp in accountInfo)
            {
                var data = kvp.Value
                    .Select(x => new
                    {
                        x.Id,
                        WalletGuids = new[]
                        {
                            x.BaseTariffWallet.TransferGuid,
                            x.DecisionTariffWallet.TransferGuid
                        }
                    })
                    .Section(5000);

                foreach (var accountPortion in data)
                {
                    var periodId = this.periodRepository.GetPeriodByDate(kvp.Key).Id;

                    var cacheDict = accountPortion
                        .SelectMany(x => x.WalletGuids.Select(y => new { x.Id, TransferGuid = y }))
                        .ToDictionary(x => x.TransferGuid);

                    var accIds = accountPortion.Select(x => x.Id).ToArray();

                    var dataResult = this.transferDomain.GetAll()
                        .Where(x => x.ChargePeriod.Id <= periodId)
                        .Where(x => accIds.Contains(x.Owner.Id))
                        .Where(x => cacheDict.Keys.Contains(x.TargetGuid))
                        .Where(x => x.IsAffect && !x.IsInDirect)
                        .Where(x => x.PaymentDate < kvp.Key)
                        .Select(x => new
                        {
                            x.TargetGuid,
                            x.PaymentDate
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.TargetGuid, x => x.PaymentDate)
                        .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x).First());

                    foreach (var dataGroup in cacheDict.Values)
                    {
                        var paymentDate = dataResult.Get(dataGroup.TransferGuid);

                        if (!paymentDate.IsValid())
                        {
                            paymentDate = this.periodRepository.GetFirstPeriod().StartDate;
                        }

                        DateTime currentDate;
                        if (this.paymentDateCache.TryGetValue(dataGroup.Id, out currentDate))
                        {
                            if (paymentDate > currentDate)
                            {
                                currentDate = paymentDate;
                            }
                        }
                        else
                        {
                            currentDate = paymentDate;
                        }

                        this.paymentDateCache[dataGroup.Id] = currentDate;
                    }
                }
            }
        }

        /// <summary>
        /// Сохранение созданных изменений
        /// </summary>
        public void SaveEvents()
        {
            if (!this.eventBuffer.Any())
            {
                return;
            }

            var ids = this.eventBuffer.Values.Select(x => x.PersonalAccount.Id).ToList();

            var existing = new List<PersonalAccountRecalcEvent>();
            foreach (var idsPortion in ids.Section(2000))
            {
                this.eventDomain.GetAll().Where(x => idsPortion.Contains(x.PersonalAccount.Id)).AddTo(existing);
            }

            var toSave = new List<PersonalAccountRecalcEvent>();
            var curPeriod = this.periodRepository.GetCurrentPeriod();

            foreach (var exist in existing)
            {
                PersonalAccountRecalcEvent evt;
                var key = CreateKey(exist.PersonalAccount.Id, exist.RecalcType);
                if (this.eventBuffer.TryGetValue(key, out evt))
                {
                    if (evt.EventDate < exist.EventDate)
                    {
                        exist.EventDate = evt.EventDate;
                        exist.Period = curPeriod;
                        toSave.Add(exist);
                    }

                    this.eventBuffer.Remove(key);
                }
            }

            toSave.AddRange(this.eventBuffer.Values);
            this.eventBuffer.Clear();

            toSave.ForEach(x => this.eventDomain.SaveOrUpdate(x));
        }
        private DateTime GetEventDateInternal(BasePersonalAccount account, DateTime eventDate)
        {
            DateTime date;

            if (!this.paymentDateCache.TryGetValue(account.Id, out date))
            {
                var wallets = new[] { account.BaseTariffWallet.TransferGuid, account.DecisionTariffWallet.TransferGuid };

                var maxPeriodId = this.periodRepository.GetPeriodByDate(eventDate)?.Id;
                date = this.transferDomain.GetAll()
                    .WhereIf(maxPeriodId.HasValue, x => x.ChargePeriod.Id <= maxPeriodId.Value)
                    .Where(x => wallets.Contains(x.TargetGuid))
                    .Where(x => x.IsAffect)
                    .Where(x => x.PaymentDate < eventDate)
                    .Select(x => x.PaymentDate)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();

                if (!date.IsValid())
                {
                    date = this.periodRepository.GetFirstPeriod().StartDate;
                }

                this.paymentDateCache[account.Id] = date;
            }
           
            return date;
        }

        /// <summary>
        /// Удаление информации по событиям для всех ЛС.
        /// Полезно при закрытии периода
        /// </summary>
        public void ClearPersistedEvents()
        {
            var prevPeriodIds = this.periodRepository.GetAllClosedPeriods().OrderBy(x => x.StartDate).Select(x => x.Id);

            prevPeriodIds = prevPeriodIds.TakeIf(prevPeriodIds.Any(), prevPeriodIds.Count() - 1);

            using (var session = this.sessions.OpenStatelessSession())
            {
                using (var tr = session.BeginTransaction())
                {
                    session.CreateQuery("delete from PersonalAccountRecalcEvent e where e.Period.Id in (:periodId) or e.RecalcEventType != 40")
                        .SetParameterList("periodId", prevPeriodIds)
                        .ExecuteUpdate();

                    try
                    {
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        private void CreateRecalcEvent(BasePersonalAccount account, DateTime eventDate, string type, string provider, RecalcEventType eventType)
        {
            var key = CreateKey(account.Id, type);

            PersonalAccountRecalcEvent evt;
            if (this.eventBuffer.TryGetValue(key, out evt))
            {
                if (evt.EventDate > eventDate)
                {
                    evt.EventDate = eventDate;
                }
            }
            else
            {
                this.eventBuffer[key] = new PersonalAccountRecalcEvent
                {
                    EventDate = eventDate,
                    PersonalAccount = account,
                    RecalcType = type,
                    RecalcProvider = provider,
                    Period = this.periodRepository.GetCurrentPeriod(),
                    RecalcEventType = eventType

                };
            }
        }

        private static string CreateKey(long id, string type)
        {
            return "{0}#{1}".FormatUsing(id, type);
        }
        #endregion
    }
}