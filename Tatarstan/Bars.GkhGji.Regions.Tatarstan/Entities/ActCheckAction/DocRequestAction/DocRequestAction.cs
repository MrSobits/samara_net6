namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction
{
    using System;

    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Действие акта проверки с типом "Истребование документов"
    /// </summary>
    public class DocRequestAction : ActCheckAction
    {
        /// <summary>
        /// Тип контролируемого лица
        /// </summary>
        public virtual TypeDocObject? ContrPersType { get; set; }

        /// <summary>
        /// Контрагент контролируемого лица
        /// </summary>
        public virtual Contragent ContrPersContragent { get; set; }

        /// <summary>
        /// Срок предоставления документов (сутки)
        /// </summary>
        public virtual long DocProvidingPeriod { get; set; }

        /// <summary>
        /// Адрес предоставления документов
        /// </summary>
        public virtual FiasAddress DocProvidingAddress { get; set; }

        /// <summary>
        /// Адрес эл. почты контролируемого лица
        /// </summary>
        public virtual string ContrPersEmailAddress { get; set; }

        /// <summary>
        /// Номер почтового отделения
        /// </summary>
        public virtual string PostalOfficeNumber { get; set; }

        /// <summary>
        /// Адрес эл. почты
        /// </summary>
        public virtual string EmailAddress { get; set; }

        /// <summary>
        /// Дата направления копии определения
        /// </summary>
        public virtual DateTime? CopyDeterminationDate { get; set; }

        /// <summary>
        /// Номер письма
        /// </summary>
        public virtual string LetterNumber { get; set; }

        public DocRequestAction()
        {
            if (this.ActionType.IsDefault())
                this.ActionType = ActCheckActionType.RequestingDocuments;
        }

        /// <inheritdoc />
        public DocRequestAction(ActCheckAction action)
            : base(action)
        {
        }
    }
}