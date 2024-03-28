namespace Bars.Gkh.Gasu.ImportExport
{
    /// <summary>
    ///     Описывает ответ сервиса ГАСУ при отправке xml.
    /// </summary>
    public sealed class GasuSendXmlResponse
    {
        /// <summary>
        ///     идентификатор записи об импорте файла данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     имя файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     дата/время обработки файла модулем импорта
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        ///     количество обработанных записей
        /// </summary>
        public int? ProcessedCount { get; set; }

        /// <summary>
        ///     пользователь системы
        /// </summary>
        public string User { get; set; }
    }
}