namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Уведомление о начале предпринимательской деятельности
    /// </summary>
    public class BusinessActivity : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Вид деятельности
        /// </summary>
        public virtual TypeKindActivity TypeKindActivity { get; set; }        

        /// <summary>
        /// Входящий номер уведомления
        /// </summary>
        public virtual string IncomingNotificationNum { get; set; }

        /// <summary>
        /// Дата начала деятельности
        /// </summary>
        public virtual DateTime? DateBegin { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public virtual DateTime? DateRegistration { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime? DateNotification { get; set; }

        /// <summary>
        /// Не осуществляет предпринимательскую деятельность
        /// </summary>
        public virtual bool IsNotBuisnes { get; set; }

        /// <summary>
        /// Орган принявший уведомление
        /// </summary>
        public virtual string AcceptedOrganization { get; set; }

        /// <summary>
        /// Регистрационный номер(string)
        /// </summary>
        public virtual string RegNum { get; set; }

        /// <summary>
        /// Оригинал
        /// </summary>
        public virtual bool IsOriginal { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Регистрационный номер(int)
        /// </summary>
        public virtual int RegNumber { get; set; }

        /// <summary>
        /// Зарегистрировано
        /// </summary>
        public virtual bool Registered { get; set; }
    }
}