﻿namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Ранее накопленные средства
    /// </summary>
    public class AccumulatedFunds : BaseImportableEntity
    {
        /// <summary>
        /// Счет
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }
    }
}