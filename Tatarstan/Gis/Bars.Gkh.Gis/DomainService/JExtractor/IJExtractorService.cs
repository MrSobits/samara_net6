namespace Bars.Gkh.Gis.DomainService.JExtractor
{
    using B4;

    public interface IJExtractorService
    {
        IDataResult ExtractToDirectory(string archive, string output);
    }
}
