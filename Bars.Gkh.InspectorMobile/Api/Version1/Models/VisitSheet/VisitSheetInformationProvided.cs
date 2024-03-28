namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.VisitSheet
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;

    /// <summary>
    /// Информация о предоставленных данных. Модель выборки
    /// </summary>
    public class VisitSheetInformationProvidedGet : BaseVisitSheetInformationProvided
    {
        /// <summary>
        /// Уникальный идентификатор записи "Предоставленная информация"
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Информация о предоставленных данных. Модель создания
    /// </summary>
    public class VisitSheetInformationProvidedCreate : BaseVisitSheetInformationProvided
    {
    }

    /// <summary>
    /// Информация о предоставленных данных. Модель обновления
    /// </summary>
    public class VisitSheetInformationProvidedUpdate : BaseVisitSheetInformationProvided, INestedEntityId
    {
        /// <inheritdoc />
        public long? Id { get; set; }
    }

    /// <summary>
    /// Информация о предоставленных данных. Базовая модель
    /// </summary>
    public abstract class BaseVisitSheetInformationProvided
    {
        /// <summary>
        /// Описание предоставленных данных
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Комментарий по предоставленным данным
        /// </summary>
        public string Comment { get; set; }
    }
}