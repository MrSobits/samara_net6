namespace Bars.GisIntegration.Base
{
    using System;

    /// <summary>
    /// Описание результата подготовки данных
    /// </summary>
    public class PrepareDataResultDescription
    {
        /// <summary>
        /// Имя пользователя, инициировавшего подготовку данных
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Время начала подготовки данных (дата актуальности)
        /// </summary>
        public DateTime StartPrepareTime { get; set; }

        /// <summary>
        /// Количество сформированных пакетов
        /// </summary>
        public int PackagesCount { get; set; }

        /// <summary>
        /// Количество сообщений валидации
        /// </summary>
        public int ValidationMessagesCount { get; set; }
    }
}
