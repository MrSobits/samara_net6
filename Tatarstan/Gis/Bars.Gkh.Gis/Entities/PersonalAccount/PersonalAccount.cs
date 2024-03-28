namespace Bars.Gkh.Gis.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;

    /// <summary>
    /// Лицевой счет
    /// </summary>
    public class GisPersonalAccount : PersistentObject
    {
        /// <summary>
        /// Код дома в системе МЖФ
        /// </summary>
        public virtual long RealityObjectId { get; set; }
        
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public virtual long HouseId { get; set; }

        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual long AccountId { get; set; }

        /// <summary>
        /// Тип лицевого счета
        /// 1-население, 2-муниципальное учреждение, 3-арендодатели
        /// </summary>
        public virtual int AccountType { get; set; }

        /// <summary>
        /// ПСС
        /// </summary>
        public virtual string PSS { get; set; }

        /// <summary>
        /// Платежный код
        /// </summary>
        public virtual decimal PaymentCode { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public virtual string ApartmentNumber { get; set; }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public virtual string RoomNumber { get; set; }

        /// <summary>
        /// Начало действия лицевого счета
        /// </summary>
        public virtual DateTime DateBegin { get; set; }

        /// <summary>
        /// Окончание действия лицевого счета
        /// </summary>
        public virtual DateTime DateEnd { get; set; }

        /// <summary>
        /// Префикс
        /// </summary>
        public virtual string Prefix { get; set; }

        /// <summary>
        /// Префикс
        /// </summary>
        public virtual string IsOpen { get; set; }

        /// <summary>
        /// Префикс
        /// </summary>
        public virtual string Tenant { get; set; }
    }
}
