namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Запрошенные сведения действия "Истребование документов"
    /// </summary>
    public class DocRequestActionRequestInfo : BaseEntity
    {
        /// <summary>
        /// Действие "Истребование документов"
        /// </summary>
        public virtual DocRequestAction DocRequestAction { get; set; }

        /// <summary>
        /// Тип сведений
        /// </summary>
        public virtual RequestInfoType RequestInfoType { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}