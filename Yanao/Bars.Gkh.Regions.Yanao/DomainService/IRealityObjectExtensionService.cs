namespace Bars.Gkh.Regions.Yanao.DomainService
{
    using Bars.B4;

    public interface IRealityObjectExtensionService
    {
        /// <summary>
        /// Сохранить документ технического паспорта
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult SaveTechPassportScanFile(BaseParams baseParams);
    }
}