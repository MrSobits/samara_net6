namespace Bars.Gkh.RegOperator.Entities.Dto
{
    using System;

    using AutoMapper;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Хранимый DTO для <see cref="BasePersonalAccount"/>
    /// </summary>
    public class BasePersonalAccountDto : PersistentObject, IStatefulEntity
    {
        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        public virtual long RoomId { get; set; }

        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public virtual long RoId { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public virtual long OwnerId { get; set; }

        /// <summary>
        /// Категория льгот
        /// </summary>
        public virtual long? PrivilegedCategoryId { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Мунинципальный район (id)
        /// </summary>
        public virtual long MuId { get; set; }

        /// <summary>
        /// Муниципальное образование (id)
        /// </summary>
        public virtual long? SettleId { get; set; }

        /// <summary>
        /// Мунинципальный район
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Адрес помещения
        /// </summary>
        public virtual string RoomAddress { get; set; }

        /// <summary>
        /// Номер помещения
        /// </summary>
        public virtual string RoomNum { get; set; }

        /// <summary>
        /// Номер комнаты
        /// </summary>
        public virtual string ChamberNum { get; set; }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public virtual string AccountOwner { get; set; }

        /// <summary>
        /// Тип владельца
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Площадь помещенния
        /// </summary>
        public virtual decimal Area { get; set; }

        /// <summary>
        /// Площадь МКД
        /// </summary>
        public virtual decimal? AreaMkd { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual string PersonalAccountNum { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual string UnifiedAccountNumber { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public virtual decimal AreaShare { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public virtual DateTime OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public virtual DateTime? CloseDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер ЛС во внешней системе
        /// </summary>
        public virtual string PersAccNumExternalSystems { get; set; }

        /// <summary>
        /// Имеет только одно помещение со статусом Открыто
        /// </summary>
        public virtual bool HasOnlyOneRoomWithOpenState { get; set; }

        /// <summary>
        /// Способ формирования фонда кр на текущий момент
        /// </summary>
        public virtual CrFundFormationType AccountFormationVariant { get; set; }

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

        /// <summary>
        /// Обнвить dto, согласно изменений ЛС
        /// </summary>
        /// <param name="account">Лицевой счёт</param>
        /// <returns>Объект</returns>
        public virtual BasePersonalAccountDto UpdateMe(BasePersonalAccount account, IMapper mapper)
        {
            this.MergeData(account, mapper);
            return this;
        }

        /// <summary>
        /// Создать dto, согласно ЛС
        /// </summary>
        /// <param name="account">Лицевой счёт</param>
        /// <returns>Объект</returns>
        public static BasePersonalAccountDto FromAccount(BasePersonalAccount account, IMapper mapper)
        {
            var accountDto = new BasePersonalAccountDto { Id = account.Id };
            return accountDto.MergeData(account, mapper);
        }

        private BasePersonalAccountDto MergeData(BasePersonalAccount account, IMapper mapper)
        {
            return mapper.Map(account, this);
        }
    }
}