namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction
{
    using System;
    using System.ComponentModel;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Действие акта проверки с типом "Опрос"
    /// </summary>
    public class SurveyAction : ActCheckAction
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
        /// Протокол прочитан?
        /// </summary>
        [DefaultValue(YesNoNotSet.NotSet)]
        public virtual YesNoNotSet ProtocolReaded { get; set; }

        /// <summary>
        /// Замечания?
        /// </summary>
        [DefaultValue(HasValuesNotSet.NotSet)]
        public virtual HasValuesNotSet HasRemark { get; set; }

        public SurveyAction()
        {
            if (this.ActionType.IsDefault())
                this.ActionType = ActCheckActionType.Survey;
        }
        
        public SurveyAction(ActCheckAction action)
            : base(action)
        {
        }
    }
}