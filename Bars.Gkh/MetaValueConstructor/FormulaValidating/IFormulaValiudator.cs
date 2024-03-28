namespace Bars.Gkh.MetaValueConstructor.FormulaValidating
{
    using Bars.B4;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Интерфейс валидатора формулы
    /// </summary>
    public interface IFormulaValiudator
    {
        /// <summary>
        /// Метод проверяет, что валидатор подходит к текущей сущности
        /// </summary>
        /// <param name="metaInfo">Мета-информация</param>
        /// <returns>Результат проверки</returns>
        bool CanValidate(DataMetaInfo metaInfo);

        /// <summary>
        /// Проверить валидность формулы
        /// </summary>
        /// <param name="metaInfo">Мета-описание</param>
        /// <returns>Результат валидации</returns>
        IDataResult Validate(DataMetaInfo metaInfo);
    }
}