namespace Bars.Gkh.MetaValueConstructor.FormulaValidating
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Валидатор формул для конструктора рейтинга эффективности
    /// </summary>
    public abstract class AbstractEfficiencyRatingFormulaValidator : IFormulaValiudator
    {
       /// <summary>
        /// Сервис для работы с формулами
        /// </summary>
        public IFormulaService FormulaService { get; set; }

        /// <summary>
        /// Метод проверяет, что валидатор подходит к текущей сущности
        /// </summary>
        /// <param name="metaInfo">Мета-информация</param>
        /// <returns>Результат проверки</returns>
        public virtual bool CanValidate(DataMetaInfo metaInfo)
        {
            return metaInfo.Group.ConstructorType == DataMetaObjectType.EfficientcyRating && (ElementLevel)metaInfo.Level == this.Level;
        }

        /// <summary>
        /// Проверить валидность формулы
        /// </summary>
        /// <param name="metaInfo">Мета-описание</param>
        /// <returns>Результат валидации</returns>
        public virtual IDataResult Validate(DataMetaInfo metaInfo)
        {
            if (metaInfo.Formula.IsEmpty())
            {
                return new BaseDataResult();
            }

            var encodedFormula = metaInfo.Formula.Translate();
            var result = this.FormulaService.CheckFormula(encodedFormula);
            if (!result.Success)
            {
                return result;
            }

            result = this.FormulaService.GetParamsList(encodedFormula);
            if (!result.Success)
            {
                return result;
            }

            var childCodes = metaInfo.GetChildrenCodes().Values.ToArray();
            var parameterList = result.Data as HashSet<string>;

            if (parameterList != null && parameterList.Except(childCodes).Any())
            {
                return BaseDataResult.Error("Формула расчета некорректна. В формуле расчета должны присутствовать только коды дочерних элементов");
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Тип элемента
        /// </summary>
        protected abstract ElementLevel Level { get; }

        /// <summary>
        /// Уровень элемента
        /// </summary>
        protected enum ElementLevel
        {
            /// <summary>
            /// Фактор
            /// </summary>
            Factor,

            /// <summary>
            /// Коэффициент
            /// </summary>
            Coefficient,

            /// <summary>
            /// Атрибут
            /// </summary>
            Attribute
        }
    }
}