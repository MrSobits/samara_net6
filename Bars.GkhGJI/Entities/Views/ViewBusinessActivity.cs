namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха прденазначена для реестра Уведомление оначале предприм. деят.
     * чтобы получать количественные показатели:
     * количество услуг, 
     */
    public class ViewBusinessActivity : PersistentObject
    {
        /// <summary>
        /// Наименование юр. лица
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Вид деятельности
        /// </summary>
        public virtual TypeKindActivity TypeKindActivity { get; set; }

        /// <summary>
        /// Входящий номер уведомления
        /// </summary>
        public virtual string IncomingNotificationNum { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public virtual DateTime? DateRegistration { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime? DateNotification { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public virtual string RegNum { get; set; }

        /// <summary>
        /// Оригинал
        /// </summary>
        public virtual bool IsOriginal { get; set; }

        /// <summary>
        /// id Файла
        /// </summary>
        public virtual long? FileInfoId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string ContragentInn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string ContragentOgrn { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public virtual string ContragentMailingAddress { get; set; }

        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        public virtual string OrgFormName { get; set; }

        /// <summary>
        /// Муниципальное образование(string)
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Количество услуг
        /// </summary>
        public virtual int ServiceCount { get; set; }

        /// <summary>
        /// Зарегистрировано
        /// </summary>
        public virtual bool Registered { get; set; }

        /// <summary>
        /// Муниципальное образование(id)
        /// </summary>
        public virtual long MunicipalityId { get; set; }

        /// <summary>
        /// Контрагент(id)
        /// </summary>
        public virtual long ContragentId { get; set; }
    }
}