namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.Modules.States;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;

    using Enums;

    /// <summary>
    /// Класс Dto для <see cref="BasePersonalAccount"/>
    /// </summary>
    public class PersonalAccountDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        public long RoomId { get; set; }

        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public long RoId { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// Категория льгот
        /// </summary>
        public long? PrivilegedCategoryId { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Мунинципальный район (id)
        /// </summary>
        public long MuId { get; set; }

        /// <summary>
        /// Муниципальное образование (id)
        /// </summary>
        public long? SettleId { get; set; }

        /// <summary>
        /// Мунинципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// Адрес помещения
        /// </summary>
        public string RoomAddress { get; set; }

        /// <summary>
        /// Номер помещения
        /// </summary>
        public string RoomNum { get; set; }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public string AccountOwner { get; set; }

        /// <summary>
        /// Тип владельца
        /// </summary>
        public PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Площадь помещенния
        /// </summary>
        public decimal Area { get; set; }

        /// <summary>
        /// Площадь МКД
        /// </summary>
        public decimal? AreaMkd { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public string PersonalAccountNum { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public string UnifiedAccountNumber { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public decimal AreaShare { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Имеются начисления в текущем периоде
        /// </summary>
        public bool HasCharges { get; set; }

        /// <summary>
        /// Номер ЛС во внешней системе
        /// </summary>
        public string PersAccNumExternalSystems { get; set; }

        /// <summary>
        /// Действительная площадь
        /// </summary>
        public decimal RealArea { get; set; }

        /// <summary>
        /// Имеет только одно помещение со статусом Открыто
        /// </summary>
        public bool HasOnlyOneRoomWithOpenState { get; set; }

        /// <summary>
        /// Способ формирования фонда кр на текущий момент
        /// </summary>
        public CrFundFormationType AccountFormationVariant { get; set; }

        /// <summary>
        /// Имеется категория льготы
        /// </summary>
        public bool PrivilegedCategory { get; set; }

        /// <summary>
        /// Вх. сальдо
        /// </summary>
        public decimal SaldoIn { get; set; }
        
        /// <summary>
        /// Исх. сальдо
        /// </summary>
        public decimal SaldoOut { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        public decimal CreditedWithPenalty { get; set; }
        
        /// <summary>
        /// Оплачено
        /// </summary>
        public decimal PaidWithPenalty { get; set; }
        
        /// <summary>
        /// Перерасчет
        /// </summary>
        public decimal RecalculationWithPenalty { get; set; }

        /// <summary>
        /// Идентификатор периода данных
        /// </summary>
        public long PeriodId { get; set; }

        /// <summary>
        /// Идентификатор периода данных
        /// </summary>
        public RoomOwnershipType OwnershipType { get; set; }

        /// <summary>
        /// Признак электронная квитанция
        /// </summary>
        public virtual YesNo DigitalReceipt { get; set; }

        /// <summary>
        /// Наличие эл.почты
        /// </summary>
        public virtual bool HaveEmail { get; set; }

        /// <summary>
        /// Не считать должником
        /// </summary>
        public virtual bool IsNotDebtor { get; set; }
    }
}