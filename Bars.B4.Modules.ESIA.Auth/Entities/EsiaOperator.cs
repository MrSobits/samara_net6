namespace Bars.B4.Modules.ESIA.Auth.Entities
{
    using B4.DataAccess;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Оператор с привязанной учеткой ЕСИА
    /// </summary>
    public class EsiaOperator : BaseEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Идентификатор пользователя в ЕСИА
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Полное имя пользователя (ФИО) 
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public virtual string Gender { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string PersonSnils { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public virtual string PersonEmail { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual string BirthDate { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string OrgPosition { get; set; }

        /// <summary>
        /// Полное наименование организации
        /// </summary>
        public virtual string OrgName { get; set; }

        /// <summary>
        /// Сокращенное наименование организации
        /// </summary>
        public virtual string OrgShortName { get; set; }

        /// <summary>
        /// Тип организации
        /// </summary>
        public virtual string OrgType { get; set; }

        /// <summary>
        /// ОГРН организации
        /// </summary>
        public virtual string OrgOgrn { get; set; }

        /// <summary>
        /// ИНН организации
        /// </summary>
        public virtual string OrgInn { get; set; }

        /// <summary>
        /// КПП организации
        /// </summary>
        public virtual string OrgKpp { get; set; }

        /// <summary>
        /// Адреса организации
        /// </summary>
        public virtual string OrgAddresses { get; set; }

        /// <summary>
        /// Организационно-правовая форма организации
        /// </summary>
        public virtual string OrgLegalForm { get; set; }

        /// <summary>
<<<<<<< HEAD
        /// Наименование организации
        /// </summary>
        public virtual string FullName { get; set; }
=======
        /// Активность пользователя (признак НЕ блокировки)
        /// </summary>
        public virtual bool IsActive { get; set; }
>>>>>>> net6
    }
}