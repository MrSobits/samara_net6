namespace Bars.GisIntegration.Base.Package
{
    using System;

    /// <summary>
    /// Информация о пакете
    /// </summary>
    public interface IPackageInfo
    {
        /// <summary>
        /// Наименование пакета
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Имя пользователя - автора пакета
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Дата создания пакета
        /// </summary>
        DateTime ObjectCreateDate { get; set; }

        /// <summary>
        /// Пакет подписан
        /// </summary>
        bool Signed { get; set; }
    }
}
