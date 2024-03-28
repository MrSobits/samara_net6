namespace Bars.Gkh.TechnicalPassport
{
    using Bars.B4;

    /// <summary>
    /// Конструктор технического паспорта 
    /// </summary>
    public interface ITechnicalPassportConstructor
    {
        /// <summary>
        /// Добавить форму
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult AddForm(BaseParams baseParams);

        /// <summary>
        /// Изменить форму
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult UpdateForm(BaseParams baseParams);

        /// <summary>
        /// Удалить форму
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult RemoveForm(BaseParams baseParams);

        /// <summary>
        /// Добавить редактор
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult AddEditor(BaseParams baseParams);

        /// <summary>
        /// Изменить редактор
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult UpdateEditor(BaseParams baseParams);

        /// <summary>
        /// Удалить редактор
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult RemoveEditor(BaseParams baseParams);

        /// <summary>
        /// Добавить аттрибут
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult AddAttribute(BaseParams baseParams);

        /// <summary>
        /// Изменить аттрибут
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult UpdateAttribute(BaseParams baseParams);

        /// <summary>
        /// Удалить аттрибут
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult RemoveAttribute(BaseParams baseParams);
    }
}
