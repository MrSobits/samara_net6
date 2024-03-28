namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction
{
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Действие акта проверки с типом "Инструментальное обследование"
    /// </summary>
    public class InstrExamAction : ActCheckAction
    {
        /// <summary>
        /// Территория
        /// </summary>
        public virtual string Territory { get; set; }

        /// <summary>
        /// Помещение
        /// </summary>
        public virtual string Premise { get; set; }

        /// <summary>
        /// Отказано в доступе на территорию
        /// </summary>
        public virtual bool TerritoryAccessDenied { get; set; }

        /// <summary>
        /// Нарушения выявлены?
        /// </summary>
        public virtual YesNoNotSet HasViolation { get; set; }

        /// <summary>
        /// Используемое оборудование
        /// </summary>
        public virtual string UsingEquipment { get; set; }

        /// <summary>
        /// Замечания?
        /// </summary>
        public virtual HasValuesNotSet HasRemark { get; set; }

        public InstrExamAction()
        {
            if (this.ActionType.IsDefault())
                this.ActionType = ActCheckActionType.InstrumentalExamination;
        }
        
        /// <inheritdoc />
        public InstrExamAction(ActCheckAction action)
            : base(action)
        {
            this.HasViolation = YesNoNotSet.NotSet;
            this.HasRemark = HasValuesNotSet.NotSet;
        }
    }
}