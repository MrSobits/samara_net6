namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems.Intf
{
    using B4;
    using Ionic.Zip;

    public interface IValidateFile
    {
        IDataResult Validate();
        IDataResult ValidateHeader(ZipFile mainArchive, string fileName, bool checkDataSupplier);
    }
}
