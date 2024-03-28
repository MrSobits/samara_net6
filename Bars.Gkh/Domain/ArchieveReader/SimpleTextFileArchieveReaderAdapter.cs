namespace Bars.Gkh.Domain.ArchieveReader
{
    using System.Collections.Generic;
    using System.IO;

    public class SimpleTextFileArchieveReaderAdapter : IArchiveReader
    {
        public IEnumerable<ArchivePart> GetArchiveParts(Stream archive, string containerName)
        {
            return new []
            {
                new ArchivePart(null/*archive*/, containerName)
            };
        }
    }
}