namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.LoadFromOtherSystems.Intf
{
    using B4;
    using Ionic.Zip;

    public interface ILoadFileFromOtherSystem
    {
        IDataResult LoadData(ZipFile mainArchive);
        IDataResult TrancateTables();
        IDataResult TransferData(ref bool deletePartition);
        IDataResult TransferHouseData(ref bool deletePartition);
    }
}
