namespace Bars.Gkh.Import.FundRealtyObjects
{
    using System.Collections.Generic;
    using System.IO;

    public interface IExtraDataImport
    {
        void Import(MemoryStream memoryStream, Dictionary<string, ExtraDataProxy> extraDataDict);

        string Code { get; } 
    }
}