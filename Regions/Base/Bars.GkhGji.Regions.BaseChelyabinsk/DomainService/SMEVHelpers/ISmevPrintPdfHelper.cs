namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers
{
    public interface ISmevPrintPdfHelper
    {
        byte[] GetPdfExtract(Bars.B4.Modules.FileStorage.FileInfo file, string resource);
    }
}