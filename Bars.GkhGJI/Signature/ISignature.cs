namespace Bars.GkhGji.DomainService
{
    using System.IO;
    using Entities;

    public interface ISignature<T>// where T : SpecialAccountReport
    {
        MemoryStream GetXmlStream(long id);
    }
}