namespace Bars.Gkh.TechnicalPassport
{
    using Bars.Gkh.Entities.TechnicalPassport;

    /// <summary>
    /// Сервис изменения структуры технического паспорта в базе данных
    /// </summary>
    public interface ITechnicalPassportTransformer
    {
        /// <summary>
        /// Создать форму
        /// </summary>
        /// <param name="form">Форма</param>
        void CreateForm(Form form);

        /// <summary>
        /// Обновить форму
        /// </summary>
        /// <param name="form">Форма</param>
        void UpdateForm(Form form);

        /// <summary>
        /// Удалить форму
        /// </summary>
        /// <param name="form">Форма</param>
        void DeleteForm(Form form);
    }
}