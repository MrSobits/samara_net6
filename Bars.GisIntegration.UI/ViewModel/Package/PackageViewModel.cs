namespace Bars.GisIntegration.UI.ViewModel.Package
{
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Package.Impl;

    /// <summary>
    /// View - модель пакетов
    /// </summary>
    public class PackageViewModel: IPackageViewModel
    {
        public PackageView GetPackageView(TempPackageInfo packageInfo)
        {
            return new PackageView
            {
                Id = packageInfo.PackageId.ToString(),
                Name = packageInfo.Name,
                Type = "temp",
                Signed = packageInfo.Signed
            };
        }

        public PackageView GetPackageView(RisPackage packageInfo)
        {
            return new PackageView
            {
                Id = packageInfo.Id.ToString(),
                Name = packageInfo.Name,
                Type = "storable",
                Signed = packageInfo.Signed
            };
        }
    }
}
