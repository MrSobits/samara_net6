namespace Bars.GisIntegration.UI.ViewModel
{
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Package.Impl;
    using Bars.GisIntegration.UI.ViewModel.Package;

    /// <summary>
    /// Интерфейс View - модели пакетов
    /// </summary>
    public interface IPackageViewModel
    {
        /// <summary>
        /// Получить представление пакета
        /// </summary>
        /// <param name="packageInfo">Описание временного пакета</param>
        /// <returns>Представление пакета</returns>
        PackageView GetPackageView(TempPackageInfo packageInfo);

        /// <summary>
        /// Получить представление пакета
        /// </summary>
        /// <param name="packageInfo">Описание хранимого пакета</param>
        /// <returns>Представление пакета</returns>
        PackageView GetPackageView(RisPackage packageInfo);
    }
}
