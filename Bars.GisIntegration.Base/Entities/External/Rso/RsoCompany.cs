namespace Bars.GisIntegration.Base.Entities.External.Rso
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Contragent;

    /// <summary>
    /// РСО Компания 
    /// </summary>
    public class RsoCompany : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        ///  Контрагент
        /// </summary>
        public virtual ExtContragent Contragent { get; set; }

        /// <summary>
        /// Номер документа о включении (Федеральный информационный реестр гарантирующих поставщиков и зон их деятельности)
        /// </summary>
        public virtual string FirgpDocNumber { get; set; }

        /// <summary>
        /// Дата включения в Федеральный информационный реестр гарантирующих поставщиков и зон их деятельности
        /// </summary>
        public virtual DateTime FirgpIncludedOn { get; set; }

        /// <summary>
        /// Номер документа о включении (Реестр субъектов естественных монополий)
        /// </summary>
        public virtual string RsemDocNumber { get; set; }

        /// <summary>
        /// Дата включения в Реестр субъектов естественных монополий
        /// </summary>
        public virtual DateTime RsemIncludedOn { get; set; }

        /// <summary>
        /// Телефон диспетчерской службы
        /// </summary>
        public virtual string DispatchPhone { get; set; }

        /// <summary>
        /// Адрес диспетчерской службы
        /// </summary>
        public virtual string DispatchAddress { get; set; }

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
