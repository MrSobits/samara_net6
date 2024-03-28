namespace Bars.Gkh.Entities.Hcs
{
    using System;
    
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Лицевой счет дома
    /// </summary>
    public class HouseAccount : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual string PersonalAccountNum { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string HouseAccountNumber { get; set; }

        /// <summary>
        /// Платежный код 
        /// </summary>
        public virtual string PaymentCode { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public virtual int Apartment { get; set; }

        /// <summary>
        /// Жилое помещение 
        /// </summary>
        public virtual bool Living { get; set; }

        /// <summary>
        /// Количество проживающих/прописанных
        /// </summary>
        public virtual int ResidentsCount { get; set; }

        /// <summary>
        /// Статус жилья 
        /// </summary>
        public virtual string HouseStatus { get; set; }

        /// <summary>
        /// Общая площадь квартиры 
        /// </summary>
        public virtual decimal ApartmentArea { get; set; }

        /// <summary>
        /// Жилая площадь 
        /// </summary>
        public virtual decimal LivingArea { get; set; }

        /// <summary>
        /// Количество комнат 
        /// </summary>
        public virtual int RoomsCount { get; set; }

        /// <summary>
        /// Состояние счета 
        /// </summary>
        public virtual string AccountState { get; set; }

        /// <summary>
        /// Приватизированная (да/нет) 
        /// </summary>
        public virtual bool Privatizied { get; set; }

        /// <summary>
        /// Количество временно убывших 
        /// </summary>
        public virtual int TemporaryGoneCount { get; set; }

        /// <summary>
        /// Номер собственника (1, 2, 3, ..)
        /// </summary>
        public virtual int OwnerNumber { get; set; }

        /// <summary>
        /// Дата открытия ЛС
        /// </summary>
        public virtual DateTime? OpenAccountDate { get; set; }

        /// <summary>
        /// Дата закрытия ЛС
        /// </summary>
        public virtual DateTime? CloseAccountDate { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public virtual Decimal? OwnershipPercentage { get; set; }


        /// <summary>
        /// Почтовый адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasMailingAddress { get; set; }

        /// <summary>
        /// Полный адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasFullAddress { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        public virtual DateTime? ContractDate { get; set; }

        /// <summary>
        /// Имя собственника (ФИО/Наименование организации)
        /// </summary>
        public virtual string OwnerName { get; set; }

        /// <summary>
        /// Тип собственника
        /// </summary>
        public virtual OwnerType OwnerType { get; set; }
    }
}
