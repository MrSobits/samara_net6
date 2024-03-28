namespace Bars.Gkh.Domain.ArchieveReader
{
    public class ArchivePart
    {
        public ArchivePart(IStreamProvider streamProvider, string fileName)
        {
            StreamProvider = streamProvider;
            FileName = fileName;
        }

        public IStreamProvider StreamProvider { get; private set; }

        public string FileName { get; private set; }
    }
}