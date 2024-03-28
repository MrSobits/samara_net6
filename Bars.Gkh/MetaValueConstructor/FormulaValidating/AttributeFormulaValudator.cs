namespace Bars.Gkh.MetaValueConstructor.FormulaValidating
{
    using Bars.B4;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Валидатор атрибута
    /// </summary>
    public class AttributeFormulaValudator : AbstractEfficiencyRatingFormulaValidator
    {
        /// <summary>
        /// Тип элемента
        /// </summary>
        protected override ElementLevel Level => ElementLevel.Attribute;

        /// <summary>
        /// Проверить валидность формулы
        /// </summary>
        /// <param name="metaInfo">Мета-описание</param>
        /// <returns>Результат валидации</returns>
        public override IDataResult Validate(DataMetaInfo metaInfo)
        {
            metaInfo.Weight = null;
            return new BaseDataResult();
        }
    }
}