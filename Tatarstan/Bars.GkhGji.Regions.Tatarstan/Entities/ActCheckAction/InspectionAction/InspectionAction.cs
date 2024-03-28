namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InspectionAction
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Действие акта проверки с типом "Осмотр"
    /// </summary>
    public class InspectionAction : ActCheckAction
    {
        /// <summary>
        /// Дата продолжения
        /// </summary>
        public virtual DateTime? ContinueDate { get; set; }

        /// <summary>
        /// Время начала продолжения
        /// </summary>
        public virtual DateTime? ContinueStartTime { get; set; }

        /// <summary>
        /// Время окончания продолжения
        /// </summary>
        public virtual DateTime? ContinueEndTime { get; set; }

        /// <summary>
        /// Нарушения выявлены?
        /// </summary>
        public virtual YesNoNotSet HasViolation { get; set; }

        /// <summary>
        /// Замечания?
        /// </summary>
        public virtual HasValuesNotSet HasRemark { get; set; }

        public InspectionAction()
        {
            if (this.ActionType.IsDefault())
                this.ActionType = ActCheckActionType.Inspection;
        }
        
        /// <inheritdoc />
        public InspectionAction(ActCheckAction action)
            : base(action)
        {
            this.HasViolation = YesNoNotSet.NotSet;
            this.HasRemark = HasValuesNotSet.NotSet;
        }
    }
}