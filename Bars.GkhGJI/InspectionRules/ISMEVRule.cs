namespace Bars.GkhGji.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Данный интерфейс отвечает за отправку информации в СМЕВ3
    /// в случае если Инициатор - Основание проверки ГЖИ
    /// </summary>
    public interface ISMEVRule
    {
        /// <summary>
        /// Код региона
        /// </summary>
        string CodeRegion { get; }

        /// <summary>
        /// Идентификатр реализации
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Краткое описание
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Получение параметров котоыре переданы с клиента
        /// </summary>
        void SendRequests(InspectionGji inspection);

        /// <summary>
        /// Получение параметров котоыре переданы с клиента
        /// </summary>
        void GetResponce();
    }
}
