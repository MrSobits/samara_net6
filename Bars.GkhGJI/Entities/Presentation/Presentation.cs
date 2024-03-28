namespace Bars.GkhGji.Entities
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Представление
    /// </summary>
    public class Presentation : DocumentGji
    {
        /// <summary>
        /// тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Должность 
        /// </summary>
        public virtual string ExecutantPost { get; set; }

        /// <summary>
        /// Текст требования
        /// </summary>
        public virtual string DescriptionSet { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Inspector Official { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Тип инициативного органа
        /// </summary>
        public virtual TypeInitiativeOrgGji TypeInitiativeOrg { get; set; }

        /// <summary>
        /// Не хранимое поле: Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }
    }
}