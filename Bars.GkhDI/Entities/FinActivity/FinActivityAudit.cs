namespace Bars.GkhDi.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Аудиторская проверка финансовой деятельности
    /// </summary>
    public class FinActivityAudit : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Тип состояния проверки
        /// </summary>
        public virtual TypeAuditStateDi TypeAuditStateDi { get; set; }

        /// <summary>
        /// Год проверки
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
