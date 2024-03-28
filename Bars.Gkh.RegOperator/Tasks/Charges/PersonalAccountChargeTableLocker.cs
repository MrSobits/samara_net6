namespace Bars.Gkh.RegOperator.Tasks.Charges
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
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;

    /// <summary>
    ///     Блокировка таблиц для операции начисления
    /// </summary>
    public static class PersonalAccountChargeTableLocker
    {
        private static readonly string[] AllActions = { "INSERT", "UPDATE", "DELETE" };

        private static readonly Dictionary<Type, string[]> LockEntities =
            new Dictionary<Type, string[]>
                {
                    { typeof(BasePersonalAccount), AllActions },
                    { typeof(CashPaymentCenterPersAcc), AllActions },
                    { typeof(CoreDecision), AllActions },
                    { typeof(EntityLogLight), AllActions },
                    { typeof(GkhConfigParam), AllActions },
                    { typeof(GovDecision), AllActions },
                    { typeof(IndividualAccountOwner), AllActions },
                    { typeof(LegalAccountOwner), AllActions },
                    { typeof(MoneyOperation), AllActions },
                    { typeof(PaymentPenalties), AllActions },
                    { typeof(PaymentPenaltiesExcludePersAcc), AllActions },
                    { typeof(Paysize), AllActions },
                    { typeof(PaysizeRealEstateType), AllActions },
                    { typeof(PaysizeRecord), AllActions },
                    { typeof(PersonalAccountOwner), AllActions },
                    { typeof(RealityObject), AllActions },
                    { typeof(RealityObjectDecisionProtocol), AllActions },
                    { typeof(Room), AllActions },
                    { typeof(Transfer), AllActions },
                    { typeof(UltimateDecision), AllActions },
                    { typeof(UnacceptedPayment), AllActions },
                    { typeof(UnacceptedPaymentPacket), AllActions }
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