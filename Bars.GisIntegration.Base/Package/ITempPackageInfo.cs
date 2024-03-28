namespace Bars.GisIntegration.Base.Package
{
    using System;

    /// <summary>
    /// Интерфейс описания временного пакета
    /// </summary>
    public interface ITempPackageInfo: IPackageInfo
    {
        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        Guid PackageId { get; set; }

        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        string SessionId { get; set; }
    }
}
