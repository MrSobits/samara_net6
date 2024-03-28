namespace Bars.GkhGji.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Данный интерфейс отвечает за формирование документа ГЖИ
    /// в случае, если инициатор - другой документ ГЖИ
    /// </summary>
    public interface IDocumentGjiRule
    {
        /// <summary>
        /// Код региона
        /// </summary>
        string CodeRegion { get; }

        /// <summary>
        /// Идентификатор реализации
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Краткое описание
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Наименование документа результата
        /// </summary>
        string ResultName { get; }

        /// <summary>
        /// Карточка, которую нужно открыть после создания документа
        /// </summary>
        string ActionUrl { get; }

        /// <summary>
        /// Тип документа-инициатора, того кто инициирует действие
        /// </summary>
        TypeDocumentGji TypeDocumentInitiator { get; }

        /// <summary>
        /// Тип документа результата, то есть того, который должен получиться в результате формирвоания
        /// </summary>
        TypeDocumentGji TypeDocumentResult { get; }

        /// <summary>
        /// Получение параметров, которые переданы от клиента
        /// </summary>
        void SetParams(BaseParams baseParams);

        /// <summary>
        /// Метод формирования документа сразу по объекту документа (если есть необходимость)
        /// </summary>
        IDataResult CreateDocument(DocumentGji document);

        /// <summary>
        /// Проверка валидности правила
        /// </summary>
        /// <remarks>
        /// Используется, если перед выполнением действия требуется проверить,
        /// можно ли формировать какой-то документ. Например, если по документу уже есть созданные,
        /// то можно не давать создавать новые (если требуется по процессу)
        /// </remarks>
        IDataResult ValidationRule(DocumentGji document);
    }
}
