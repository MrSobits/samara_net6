namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using System.IO;
    using Entities;

    public interface IAnswerSignature<T>// where T : SpecialAccountReport
    {
        MemoryStream GetXmlStream(long id);
    }
}