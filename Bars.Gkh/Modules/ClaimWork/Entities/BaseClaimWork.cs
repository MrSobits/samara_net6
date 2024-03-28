namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System;
    using B4.DataAccess;
    using B4.Modules.States;

    using Enums;
    using Gkh.Entities;

    /// <summary>
    /// Основание претензионно исковой работы
    /// </summary>
    public class BaseClaimWork : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Тип основания 
        /// </summary>
        public virtual ClaimWorkTypeBase ClaimWorkTypeBase { get; set; }

        /// <summary>
        /// Информация из основания в зависимости от типа
        /// </summary>
        public virtual string BaseInfo { get; set; }

        /// <summary>
        /// Дата начала отсчета
        /// </summary>
        public virtual DateTime? StartingDate { get; set; }

        /// <summary>
        /// Количество дней просрочки
        /// </summary>
        public virtual int? CountDaysDelay { get; set; }

        /// <summary>
        /// Задолженность погашена
        /// </summary>
        public virtual bool IsDebtPaid { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дата погашения задолженности
        /// </summary>
        public virtual DateTime? DebtPaidDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
        
        // Для безумного Воронежа (Возможность создания дублей и сохранения истории движения по делу)
    }
}