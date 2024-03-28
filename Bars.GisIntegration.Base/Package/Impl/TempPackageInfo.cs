namespace Bars.GisIntegration.Base.Package.Impl
{
    using System;

    /// <summary>
    /// Описание временного пакета
    /// </summary>
    public class TempPackageInfo: ITempPackageInfo
    {
        /// <summary>
        /// Наименование пакета
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя пользователя - автора пакета
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Дата создания пакета
        /// </summary>
        public DateTime ObjectCreateDate { get; set; }

        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        public Guid PackageId { get; set; }

        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Пакет подписан
        /// </summary>
        public bool Signed { get; set; }
    }
}
