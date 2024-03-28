namespace Bars.GisIntegration.Base.Entities.External.Contragent
{
    using System;
    using Bars.B4.DataAccess;
    using Administration.System;
    using Bars.B4.Modules.FIAS;

    /// <summary>
    /// Контрагент
    /// </summary>
    public class ExtContragent : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// Полное имя
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// Короткое имя
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public virtual string Kpp { get; set; }

        /// <summary>
        /// Орган регистрации 
        /// </summary>
        public virtual string RegisteredBy { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public virtual DateTime? RegisteredOn { get; set; }

        /// <summary>
        /// ОКОПФ
        /// </summary>
        public virtual string Okopf { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string GisGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? TerminatedOn { get; set; }
        /// <summary>
        /// Актуальность записи
        /// </summary>
        public virtual bool IsActive { get; set; }
        /// <summary>
        /// Сайт
        /// </summary>
        public virtual string Website { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int ChiefId { get; set; }
        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// Факс
        /// </summary>
        public virtual string Fax { get; set; }
        /// <summary>
        /// Емаил
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int BookerId { get; set; }
        /// <summary>
        /// Юридический адрес ФИАС 
        /// </summary>
        public virtual FiasAddress FiasJurAddress { get; set; }
        /// <summary>
        /// Фактический адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasFactAddress { get; set; }
        /// <summary>
        /// Адрес для отправки
        /// </summary>
        public virtual FiasAddress FiasMailAddress { get; set; }
        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
