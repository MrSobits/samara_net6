namespace Bars.GkhEdoInteg.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сопоставление обращения граждан с интеграцией Эдо
    /// </summary>
    public class AppealCitsCompareEdo : BaseGkhEntity
    {
        /// <summary>
        /// Адрес из Электронного документооборота
        /// </summary>
        public virtual string AddressEdo { get; set; }

        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Создан интеграцией с Электронным документооборотом
        /// </summary>
        public virtual bool IsEdo { get; set; }

        /// <summary>
        /// Идентификатор ЕМСЭД
        /// </summary>
        public virtual int CodeEdo { get; set; }

        /// <summary>
        /// Дата актуальности ЕМСЭД
        /// </summary>
        public virtual DateTime? DateActual { get; set; }

        /// <summary>
        /// Дата загрузки документа
        /// </summary>
        public virtual DateTime? DateDocLoad { get; set; }

        /// <summary>
        /// Наличие документа
        /// </summary>
        public virtual bool IsDocEdo { get; set; }

        /// <summary>
        /// Количество загрузок документа
        /// </summary>
        public virtual int CountLoadDoc { get; set; }

        /// <summary>
        /// ответ сервера при загрузки документа
        /// </summary>
        public virtual string MsgLoadDoc { get; set; }
    }
}
