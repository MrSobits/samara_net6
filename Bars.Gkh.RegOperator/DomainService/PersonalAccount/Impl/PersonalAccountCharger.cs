namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using Bars.B4;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.Interface;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.Charges.Providers;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Castle.Windsor;
    using System;

    /// <summary>
    /// Интерфейс для работы с начислениями ЛС
    /// </summary>
    public class PersonalAccountCharger : IPersonalAccountCharger
    {
        private static readonly object AcceptSyncRoot = new object();

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен неподтвержденных начислений
        /// </summary>
        public IDomainService<UnacceptedCharge> UnAccChargeDomain { get; }

        /// <summary>
        /// Домен подтвержденных начислений
        /// </summary>
        public IDomainService<PersonalAccountCharge> AccChargeDomain { get; }

        /// <summary>
        /// Домен пакета неподтвержденных начислений
        /// </summary>
        public IDomainService<UnacceptedChargePacket> UnAccChargePacketDomain { get; }

        private readonly IWindsorContainer container;

        /// <summary>
        /// Период начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepo { get; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager taskManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        public PersonalAccountCharger(
            IDomainService<UnacceptedCharge> unAccChargeDomain,
            IDomainService<PersonalAccountCharge> accChargeDomain,
            IDomainService<UnacceptedChargePacket> unAccChargePacketDomain,
            IWindsorContainer container,
            IChargePeriodRepository chargePeriodRepo,
            ITaskManager taskManager)
        {
            this.UnAccChargeDomain = unAccChargeDomain;
            this.AccChargeDomain = accChargeDomain;
            this.UnAccChargePacketDomain = unAccChargePacketDomain;
            this.container = container;
            this.ChargePeriodRepo = chargePeriodRepo;
            this.taskManager = taskManager;
        }

        /// <summary>
        /// Лог менеджер
        /// </summary>
        public ILogger LogManager { get; set; }

        #region Создание начислений
        /// <summary>
        /// Создание неподтвержденных начислений по ЛС
        /// </summary>
        public virtual IDataResult CreateUnacceptedCharges(BaseParams baseParams)
        {
            try
            {
                return this.taskManager.CreateTasks(new PersonalAccountChargeTaskProvider(this.container), baseParams);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

        #endregion Создание начислений

        /// <summary>
        /// Подтвердить начисления по пакетно.
        /// Подтвержаются только те неподтвержденные начисления:
        /// 1) дата создания которых больше даты боевого начисления
        /// 2) ранее неподтвержденные
        /// 3) принадлжеащие одному ЛС
        /// Также удалятся боевые начисления из текущего периода, такие что:
        /// 1) для боевого есть неподтвержденное начисление
        /// 2) Дата создания этого начисления больше даты боевого начисления
        /// 3) принадлежат одному ЛС
        /// </summary>
        public virtual IDataResult AcceptUnaccepted(BaseParams baseParams)
        {
            var packetid = baseParams.Params.GetAsId();
            IDataResult result = null;

            lock (PersonalAccountCharger.AcceptSyncRoot)
            {
                var packet = this.UnAccChargePacketDomain.Get(packetid);

                this.container.InTransaction(() =>
                {
                    if (packet.PacketState == PaymentOrChargePacketState.Accepted)
                    {
                        result = new BaseDataResult(false, "Запись уже подтверждена, невозможно подтвердить запись дважды!");
                    }
                    else if (packet.PacketState == PaymentOrChargePacketState.InProgress)
                    {
                        result = new BaseDataResult(false, "Запись обрабатывается, невозможно подтвердить запись дважды!");
                    }
                    else
                    {
                        packet.PacketState = PaymentOrChargePacketState.InProgress;
                        this.UnAccChargePacketDomain.Update(packet);
                    }
                });

                if (result == null)
                {
                    try
                    {
                        result = this.TaskManager.CreateTasks(new AcceptedChargesTaskProvider(), baseParams);

                        if (!result.Success)
                        {
                            this.container.InTransaction(() =>
                            {
                                packet.PacketState = PaymentOrChargePacketState.Pending;
                                this.UnAccChargePacketDomain.Update(packet);
                            });
                        }

                    }
                    catch
                    {
                        this.container.InTransaction(() =>
                        {
                            packet.PacketState = PaymentOrChargePacketState.Pending;
                            this.UnAccChargePacketDomain.Update(packet);
                        });

                        throw;
                    }
                }
            }

            return result ?? BaseDataResult.Error("Ошибка при подтверждении начислений");
        }
    }
}