namespace Bars.FIAS.Converter
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IDbWorker
    {
        /// <summary>
        /// Фоновый поток для отмены операции
        /// </summary>
        BackgroundWorker BackgroundWorker { get; set; }

        /// <summary>
        /// Проверка соединения с БД
        /// </summary>
        bool TestConnection();

        /// <summary>
        /// Вставка FiasRecord
        /// </summary>
        void InsertRecords(IList<FiasRecord> fiasRecords, IList<FiasHouseRecord> fiasHouseRecords);

        /// <summary>
        /// Замена FiasRecord
        /// </summary>
        void UpdateFiasRecords(IList<FiasRecord> fiasRecords, IList<FiasHouseRecord> fiasHouseRecords);

        /// <summary>
        /// Удаление FiasRecord
        /// </summary>
        void DeleteRecords();

        /// <summary>
        /// Получить текущие записи ФИАС
        /// </summary>
        Dictionary<string, FiasRecord> GetCurrentFiasRecords();

        /// <summary>
        /// Получить текущие записи домов ФИАС
        /// </summary>
        Dictionary<string, FiasHouseRecord> GetCurrentFiasHouseRecords();
    }
}