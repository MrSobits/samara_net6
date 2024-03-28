namespace Bars.GisIntegration.Base.Package
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Интерфейс хранимого описания пакета
    /// </summary>
    public interface IStorablePackageInfo : IEntity, IPackageInfo
    {
        long PackageDataId { get; set; }
    }
}
