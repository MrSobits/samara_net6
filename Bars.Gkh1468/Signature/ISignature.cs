namespace Bars.Gkh1468.DomainService
{
    using System.IO;
    using Entities;

    public interface ISignature<T> where T : BaseProviderPassport
    {
        MemoryStream GetXmlStream(long id);

        MemoryStream GetPdfStream(long id);
    }
}