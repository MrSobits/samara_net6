namespace Bars.Gkh1468.PassportStructExport
{
    using System.IO;
    using Entities;
    using B4;

    public interface IPassportStructExporter
    {
        string Version { get; }
        
        IDataResult Import(Stream stream);

        IDataResult Export(PassportStruct passportStruct);
    }
}
