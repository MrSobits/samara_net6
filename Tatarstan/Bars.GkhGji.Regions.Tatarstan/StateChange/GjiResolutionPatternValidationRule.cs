using Bars.B4.Modules.States;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    class GjiResolutionPatternValidationRule : IRuleChangeStatus
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id
        {
            get { return "gji_resolution_pattern_validation_rule"; }
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get { return "Проверка актуальности шаблона ГИС ГМП"; }
        }

        /// <summary>
        /// Тип
        /// </summary>
        public string TypeId
        {
            get { return "gji_document_resol"; }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get { return "Данное правило проверяет актуальность выбранного шаблона ГИС ГМП"; }
        }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="statefulEntity">Сущность</param>
        /// <param name="oldState">Старый статус</param>
        /// <param name="newState">Новый статус</param>
        /// <returns>Результат валидации</returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var document = statefulEntity as Entities.Resolution.TatarstanResolution;
            string validationWarning = "Перевод на данный статус невозможен, так как выбран неактуальный шаблон";

            if (!document.PatternDict.Relevance)
            {
                return ValidateResult.No(validationWarning);
            }

            return ValidateResult.Yes();
        }
    }
}
