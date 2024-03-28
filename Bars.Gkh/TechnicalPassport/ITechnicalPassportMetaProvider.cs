namespace Bars.Gkh.TechnicalPassport
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Entities.TechnicalPassport;

    /// <summary>
    /// Поставщик мета описания технического паспорта
    /// </summary>
    public interface ITechnicalPassportMetaProvider
    {
        /// <summary>
        /// Получить меню
        /// </summary>
        /// <returns>Список элементов меню</returns>
        IList<MenuItem> GetMenu();

        /// <summary>
        /// Получить формы для секции
        /// </summary>
        /// <param name="section"></param>
        /// <returns>Список форм</returns>
        IList<Form> GetForms(Section section);

        /// <summary>
        /// Получить редакторы для формы
        /// </summary>
        /// <param name="form"></param>
        /// <returns>Список редакторов</returns>
        IList<Editor> GetEditors(Form form);
    }
}