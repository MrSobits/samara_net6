namespace Bars.Gkh.Domain.ArchieveReader
{
    public class ArchiveReaderFactory
    {
        public static IArchiveReader Create(string fileExtension)
        {
            switch (fileExtension.ToUpperInvariant())
            {
                case "ZIP":
                    return new ZipArchiveReader();
                case "RAR":
                    return new RarArchiveReader();
                default:
                    return new SimpleTextFileArchieveReaderAdapter();
            }
        } 
    }
}