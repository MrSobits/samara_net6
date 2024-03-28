namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System.Collections.Generic;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Изменение пени счета
    /// Сущность является источником для операции на кошельке ЛС по изменению пеней
    /// Операция может быть только одна и отмены данного вида операции не предполагается 
    /// </summary>
    public class PenaltyChange : BaseImportableEntity, IChargeOriginator
    {
        private readonly List<MoneyOperation> operations;
        
        public PenaltyChange()
        {
            this.TransferGuid = System.Guid.NewGuid().ToString();
            this.operations = new List<MoneyOperation>();
        }

        /// <summary>
        /// Счет
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Суммарные значения по периоду для ЛС
        /// </summary>
        public virtual PersonalAccountPeriodSummary AccountPeriodSummary { get; set; }

        /// <summary>
        /// Значение перед изменением
        /// </summary>
        public virtual decimal CurrentValue { get; set; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public virtual decimal NewValue { get; set; }

        /// <summary>
        /// Документ-основание для изменения
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Причина изменения
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// На этот гуид будут вязаться различные операции, происходящие при распределении.
        /// Например, возврат займа дома
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        /// <summary>
        /// Создание операции по переводу денег
        /// </summary>
        /// <returns></returns>
        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            var newOper = new MoneyOperation(this.TransferGuid, period);
            this.Operations.Add(newOper);
            newOper.Reason = "Установка/изменение пени";
            return newOper;
        }

        /// <summary>
        /// Операции изменения пени
        /// У этой сущности всегда будет только одна операция потому что пеналти ченж только создается новая каждый раз
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<MoneyOperation> Operations
        {
            get { return this.operations; }
            protected set
            {
                this.operations.Clear();
                if (value != null)
                {
                    this.operations.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Тип источника поступления начислений (не хранимое)
        /// </summary>
        public virtual TypeChargeSource ChargeSource => TypeChargeSource.PenaltyChange;

        /// <summary>
        /// Guid инициатора (не хранимое)
        /// </summary>
        public virtual string OriginatorGuid => this.Guid;
    }
}
