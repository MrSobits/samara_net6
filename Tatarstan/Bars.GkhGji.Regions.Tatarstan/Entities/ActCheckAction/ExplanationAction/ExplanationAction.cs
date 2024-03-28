namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.ExplanationAction
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Действие акта проверки с типом "Получение письменных объяснений"
    /// </summary>
    public class ExplanationAction : ActCheckAction
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
        /// Наименование приложения
        /// </summary>
        public virtual string AttachmentName { get; set; }

        /// <summary>
        /// Файл приложения
        /// </summary>
        public virtual FileInfo AttachmentFile { get; set; }

        /// <summary>
        /// Объяснение
        /// </summary>
        public virtual string Explanation { get; set; }

        public ExplanationAction()
        {
            if (this.ActionType.IsDefault())
                this.ActionType = ActCheckActionType.GettingWrittenExplanations;
        }
        
        /// <inheritdoc />
        public ExplanationAction(ActCheckAction action)
            : base(action)
        {
        }
    }
}