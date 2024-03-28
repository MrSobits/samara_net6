namespace Bars.Gkh.RegOperator.Domain.ImportExport.IR
{
    using System.IO;

    public class DbfIRTranslatorException : IOException
    {
        public DbfIRTranslatorException(string message)
            : base(message)
        {
        }
    }
}