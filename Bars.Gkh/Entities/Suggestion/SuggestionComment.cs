namespace Bars.Gkh.Entities.Suggestion
{
    using System;

    /// <summary>
    /// Комментарии
    /// </summary>
    public partial class SuggestionComment : BaseGkhEntity
    {
        public virtual CitizenSuggestion CitizenSuggestion { get; set; }

        public virtual DateTime? CreationDate { get; set; }

        public virtual string Question { get; set; }

        public virtual string Answer { get; set; }

        public virtual DateTime? AnswerDate { get; set; }

        public virtual bool IsFirst { get; set; }

        public virtual ManagingOrganization ExecutorManagingOrganization { get; set; }

        public virtual Municipality ExecutorMunicipality { get; set; }

        public virtual ZonalInspection ExecutorZonalInspection { get; set; }

        public virtual ContragentContact ExecutorCrFund { get; set; }

        public virtual ProblemPlace ProblemPlace { get; set; }

        public virtual string Description { get; set; }

        public virtual bool HasAnswer { get; set; }
    }
}