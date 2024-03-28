namespace Bars.GkhGji.InspectionRules
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Данный интерфейс отвечает за формирование документа ГЖИ  
    /// в случае если Инициатор - Основание проверки ГЖИ
    /// </summary>
    public interface IInspectionGjiRule
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
        /// Карточка, которую нужно открыть после создания дкоумента
        /// </summary>
        string ActionUrl { get; }

        /// <summary>
        /// Наименование документ резултата
        /// </summary>
        string ResultName { get; }

        /// <summary>
        /// Тип основания проверки, инициирующий формирование дкоумента
        /// </summary>
        TypeBase TypeInspectionInitiator { get; }

        /// <summary>
        /// Тип документа результата, тоест ьсформирвоанный документ
        /// </summary>
        TypeDocumentGji TypeDocumentResult { get; }

        /// <summary>
        /// Получение параметров котоыре переданы с клиента
        /// </summary>
        void SetParams(BaseParams baseParams);

        /// <summary>
        /// Метод формирвоания документа ГЖИ
        /// по InspectionGji (если в этом ест ьнеобходимость)
        /// </summary>
        IDataResult CreateDocument(InspectionGji inspection);

        /// <summary>
        /// проверка валидности правила
        /// Например перед выполнением действия требуется проверить
        /// Можно ли формирвоать какойто документ, по Основанию проверки
        /// Например Если у Основания уже есть распоряжение, то нельзя больше еще одно распоряжение формировать
        /// </summary>
        IDataResult ValidationRule(InspectionGji inspection);
    }
}
