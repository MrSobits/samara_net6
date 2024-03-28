namespace Bars.Gkh.Entities.Suggestion
{
    using System;
    using B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Exceptions;

    /// <summary>
    /// История обработки обращения граждан. Сущность создается в рамках процесса автоматической обработки
    /// обращения граждан в <see cref="Bars.Gkh.Entities.Suggestion.Rubric"/>.
    /// Сущность хранит в себе информацию о смене исполнителя и крайнем сроке исполнения.
    /// </summary>
    public class CitizenSuggestionHistory : BaseImportableEntity
    {
        /// <summary>
        /// Создание нового экземпляра.
        /// </summary>
        protected CitizenSuggestionHistory()
        {

        }

        public CitizenSuggestionHistory(CitizenSuggestion suggestion, ExecutorType targetExecutorType, string executorEmail = null)
        {
            ArgumentChecker.NotNull(suggestion, "suggestion");

            this.CitizenSuggestion = suggestion;
            this.ExecutionDeadline = suggestion.Deadline.GetValueOrDefault();
            this.RecordDate = DateTime.Now;
            this.ExecutorEmail = executorEmail;
            this.TargetExecutorType = targetExecutorType;
            this.ExecutorManagingOrganization = suggestion.ExecutorManagingOrganization;
            this.ExecutorMunicipality = suggestion.ExecutorMunicipality;
            this.ExecutorZonalInspection = suggestion.ExecutorZonalInspection;
        }

        public CitizenSuggestionHistory(SuggestionComment comment, ExecutorType targetExecutorType, string executorEmail = null)
        {
            ArgumentChecker.NotNull(comment, "comment");

            this.CitizenSuggestion = comment.CitizenSuggestion;
            this.ExecutionDeadline = comment.CitizenSuggestion.Deadline;
            this.RecordDate = comment.CreationDate ?? DateTime.Today;
            this.ExecutorEmail = executorEmail;
            this.TargetExecutorType = targetExecutorType;
            this.ExecutorManagingOrganization = comment.ExecutorManagingOrganization;
            this.ExecutorMunicipality = comment.ExecutorMunicipality;
            this.ExecutorZonalInspection = comment.ExecutorZonalInspection;
        }


        /// <summary>
        /// Обращение граждан, для которого создана запись в истории
        /// </summary>
        public virtual CitizenSuggestion CitizenSuggestion { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ExecutorType TargetExecutorType { get; protected set; }

        /// <summary>
        /// Дата записи в истории
        /// </summary>
        public virtual DateTime RecordDate { get; protected set; }

        #region Поля для хранения ссылок на исполнителей.
        // В один момен времени может быть заполнено не более одного поля
        // Так сделано из-за legacy в CitizenSuggestion

        /// <summary>
        /// Исполнитель - управляющая организация
        /// </summary>
        public virtual ManagingOrganization ExecutorManagingOrganization { get; protected set; }

        /// <summary>
        /// Исполнитель - муниципальное образование
        /// </summary>
        public virtual Municipality ExecutorMunicipality { get; protected set; }

        /// <summary>
        /// Исполнитель - ГЖИ
        /// </summary>
        public virtual ZonalInspection ExecutorZonalInspection { get; protected set; }

        /// <summary>
        /// EMail департамента ЖКХ
        /// </summary>
        public virtual string ExecutorEmail { get; protected set; }

        #endregion

        /// <summary>
        /// Крайний срок исполнения
        /// </summary>
        public virtual DateTime? ExecutionDeadline { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string GetExecutorDisplayName()
        {
            switch (this.TargetExecutorType)
            {
                case ExecutorType.Gji:
                {
                    return this.ExecutorZonalInspection.Return(x => x.Name);
                }
                case ExecutorType.Mu:
                {
                    return this.ExecutorMunicipality.Return(x => x.Name);
                }
                case ExecutorType.Mo:
                {
                    return this.ExecutorManagingOrganization.Return(x => x.Contragent).Return(x => x.Name);
                }
                case ExecutorType.None:
                {
                    return "Оператор";
                }
            }

            if (!string.IsNullOrEmpty(this.ExecutorEmail))
            {
                return this.ExecutorEmail;
            }

            throw new CitizenSuggestionHistoryException("Неизвестный тип исполнителя");
        }
    }
}