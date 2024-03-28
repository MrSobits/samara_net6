namespace Bars.Gkh.RegOperator.Tasks.Period
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Decisions;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using Castle.Windsor;

    /// <summary>
    ///     Блокировка таблиц для операции закрытия периода
    /// </summary>
    public static class PeriodCloseTableLocker
    {
        private static readonly string[] AllActions = { "INSERT", "UPDATE", "DELETE" };

        private static readonly Dictionary<Type, string[]> LockEntities =
            new Dictionary<Type, string[]> 
                {
                    { typeof(BasePersonalAccount), new[] { "INSERT", "DELETE" } },
                    { typeof(CashPaymentCenterPersAcc), AllActions },
                    { typeof(CoreDecision), AllActions },
                    { typeof(GkhConfigParam), AllActions },
                    { typeof(GovDecision), AllActions },
                    { typeof(IndividualAccountOwner), new[] { "INSERT", "DELETE" } },
                    { typeof(LegalAccountOwner), new[] { "INSERT", "DELETE" } },
                    { typeof(PaymentPenalties), AllActions },
                    { typeof(PaymentPenaltiesExcludePersAcc), AllActions },
                    { typeof(Paysize), AllActions },
                    { typeof(PaysizeRealEstateType), AllActions },
                    { typeof(PaysizeRecord), AllActions },
                    { typeof(PersonalAccountOwner), new[] { "INSERT", "DELETE" } },
                    { typeof(RealityObject), AllActions },
                    { typeof(RealityObjectDecisionProtocol), AllActions },
                    { typeof(Room), AllActions },
                    { typeof(UltimateDecision), AllActions },
                    { typeof(UnacceptedCharge), new []{ "INSERT" } },
                    { typeof(UnacceptedChargePacket), new [] { "INSERT" } },
                    { typeof(UnacceptedPayment), AllActions },
                    { typeof(UnacceptedPaymentPacket), AllActions },
                    { typeof(PersonalAccountCharge), new [] { "INSERT", "DELETE" } }
                };

        /// <summary>
        ///     Блокировать таблицы
        /// </summary>
        /// <param name="container"></param>
        public static void Lock(IWindsorContainer container)
        {
            var locker = container.Resolve<IBatchTableLocker>();
            try
            {
                locker.ThrowOnAlreadyLocked(false);
                foreach (var rec in LockEntities)
                {
                    foreach (var action in rec.Value)
                    {
                        locker.With(rec.Key, action);
                    }
                }

                locker.Lock();
            }
            finally
            {
                container.Release(locker);
            }
        }

        /// <summary>
        ///     Разблокировать
        /// </summary>
        /// <param name="container"></param>
        public static void Unlock(IWindsorContainer container)
        {
            var locker = container.Resolve<IBatchTableLocker>();
            try
            {
                foreach (var rec in LockEntities)
                {
                    foreach (var action in rec.Value)
                    {
                        locker.With(rec.Key, action);
                    }
                }

                locker.Unlock();
            }
            finally
            {
                container.Release(locker);
            }
        }
    }
}