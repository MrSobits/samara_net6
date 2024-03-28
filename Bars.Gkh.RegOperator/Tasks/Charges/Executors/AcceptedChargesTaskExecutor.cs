namespace Bars.Gkh.RegOperator.Tasks.Charges.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Исполнитель задач подтвержденных начислений
    /// </summary>
    public class AcceptedChargesTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType?.FullName;

        private static readonly object AcceptSyncRoot = new object();

        private readonly IDomainService<UnacceptedCharge> unAccChargeDomain;

        private readonly IDomainService<PersonalAccountCharge> accChargeDomain;

        private readonly IDomainService<UnacceptedChargePacket> unAccChargePacketDomain;

        private readonly IWindsorContainer container;

        private readonly IChargePeriodRepository chargePeriodRepo;

        /// <summary>
        /// Конструктор исполнителя задач подтвержденных начислений
        /// </summary>
        /// <param name="unAccChargeDomain">Домен-сервис <see cref="UnacceptedCharge"/></param>
        /// <param name="accChargeDomain">Домен-сервис <see cref="PersonalAccountCharge"/></param>
        /// <param name="unAccChargePacketDomain">Домен-сервис <see cref="UnacceptedChargePacket"/></param>
        /// <param name="container">Контейнер</param>
        /// <param name="chargePeriodRepo">Репозиторий периодов начисления</param>
        public AcceptedChargesTaskExecutor(
            IDomainService<UnacceptedCharge> unAccChargeDomain,
            IDomainService<PersonalAccountCharge> accChargeDomain,
            IDomainService<UnacceptedChargePacket> unAccChargePacketDomain,
            IWindsorContainer container,
            IChargePeriodRepository chargePeriodRepo)
        {
            this.unAccChargeDomain = unAccChargeDomain;
            this.accChargeDomain = accChargeDomain;
            this.unAccChargePacketDomain = unAccChargePacketDomain;
            this.container = container;
            this.chargePeriodRepo = chargePeriodRepo;
        }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Логгер менеджера логов
        /// </summary>
        public ILogger Logger { get; set; }


        /// <summary>
        /// Код исполнителя
        /// </summary>
        public string ExecutorCode { get; private set; }

        #region Implementation of ITaskExecutor

        /// <summary>
        /// Функция исполнения
        /// </summary>
        /// <param name="params">Параметры задачи</param>
        /// <param name="ctx">Контекст выполнения</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns></returns>
        [Obsolete("Подтверждение начислений упразднено")]
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var packetId = @params.Params.GetAsId();

            lock (AcceptedChargesTaskExecutor.AcceptSyncRoot)
            {
                var period = this.chargePeriodRepo.GetCurrentPeriod();
                var packet = this.unAccChargePacketDomain.Get(packetId);

                if (period == null)
                {
                    this.SetPendingStatus(packet);
                    return new BaseDataResult(false, "Не найден текущий период!");
                }

                var unAccCharges = this.unAccChargeDomain.GetAll().WhereIf(packetId != 0, x => x.Packet.Id == packetId);

                if (this.unAccChargeDomain.GetAll().Any(x => x.Packet.Id == packetId && x.PersonalAccount == null))
                {
                    this.SetPendingStatus(packet);
                    return new BaseDataResult(false, "Присутствуют начисления без определенного лицевого счета");
                }

                var sessionProvider = this.container.Resolve<ISessionProvider>();
                var session = sessionProvider.GetCurrentSession();

                var acceptedList = new List<PersonalAccountCharge>();

                try
                {
                    this.container.InTransaction(() =>
                    {
                        packet.PacketState = PaymentOrChargePacketState.InProgress;
                        this.unAccChargePacketDomain.Update(packet);
                    });

                    this.container.InTransaction(() =>
                    {
                        session.CreateSQLQuery(@"DELETE FROM REGOP_PERS_ACC_CHARGE
                        WHERE ID IN (

                            (SELECT
                                c1.ID
                            FROM regop_pers_acc_charge c1
                                join REGOP_UNACCEPT_CHARGE c2 on c1.pers_acc_id = c2.ACC_ID
                            WHERE c1.CHARGE_DATE >= :start_date
                                {0})

                            union

                            (SELECT
                                c1.ID
                            FROM regop_pers_acc_charge c1
                                join REGOP_UNACCEPT_CHARGE c2 on c2.cguid = c1.guid
                            WHERE c1.CHARGE_DATE >= :start_date
                                {0})
                        )".FormatUsing(packetId > 0 ? "and c2.packet_id = {0}".FormatUsing(packetId) : string.Empty))
                            .SetDateTime("start_date", period.StartDate)
                            .ExecuteUpdate();

                        foreach (var unAcc in unAccCharges)
                        {
                            var chargeDate = unAcc.ObjectCreateDate;
                            var periodEnd = period.GetEndDate();
                            if (chargeDate <= period.StartDate || chargeDate >= periodEnd)
                            {
                                chargeDate = periodEnd;
                            }

                                    var accepted = new PersonalAccountCharge
                                    {
                                        BasePersonalAccount = unAcc.PersonalAccount,
                                        Charge = unAcc.Charge,
                                        ChargeDate = chargeDate,
                                        ChargeTariff = unAcc.ChargeTariff,
                                        Penalty = unAcc.Penalty,
                                        Guid = unAcc.Guid,
                                        OverPlus = unAcc.TariffOverplus,
                                        RecalcByBaseTariff = unAcc.RecalcByBaseTariff,
                                        RecalcByDecisionTariff = unAcc.RecalcByDecision,
                                        RecalcPenalty = unAcc.RecalcPenalty,
                                        ChargePeriod = period
                                    };

                            acceptedList.Add(accepted);
                        }

                        TransactionHelper.InsertInManyTransactions(this.container, acceptedList, 1000, true, true);

                        packet.PacketState = PaymentOrChargePacketState.Accepted;
                        this.unAccChargePacketDomain.Update(packet);
                    });
                }
                catch
                {
                    this.SetPendingStatus(packet);

                    acceptedList.Where(x => x.Id > 0).ForEach(x => this.accChargeDomain.Delete(x.Id));

                    return BaseDataResult.Error("Возникла ошибка при подтверждении");
                }
                finally
                {
                    this.container.Release(sessionProvider);
                    acceptedList.Clear();
                    GC.Collect();
                }
            }

            return new BaseDataResult(true, "Начисления применены");
        }

        private void SetPendingStatus(UnacceptedChargePacket packet)
        {
            this.container.InTransaction(() =>
            {
                packet.PacketState = PaymentOrChargePacketState.Pending;
                this.unAccChargePacketDomain.Update(packet);
            });
        }

        #endregion
    }
}