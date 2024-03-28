namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System.IO;
    using Entities;

    public interface IAnswerSignature<T>// where T : SpecialAccountReport
    {
        MemoryStream GetXmlStream(long id);
    }
}