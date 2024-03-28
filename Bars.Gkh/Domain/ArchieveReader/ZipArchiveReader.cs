namespace Bars.Gkh.Domain.ArchieveReader
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ionic.Zip;

    public class ZipArchiveReader : IArchiveReader
    {
        public IEnumerable<ArchivePart> GetArchiveParts(Stream archive, string containerName)
        {
            using (var zip = ZipFile.Read(archive))
            {
                return zip.Entries.Select(x => new ArchivePart(new ZipStreamProvider(x), x.FileName));
            }
        }
    }
}