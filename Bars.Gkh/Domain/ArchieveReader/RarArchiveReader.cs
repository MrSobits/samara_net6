namespace Bars.Gkh.Domain.ArchieveReader
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using SharpCompress.Archives.Rar;

    public class RarArchiveReader: IArchiveReader
    {
        #region IArchiveReader Members

        public IEnumerable<ArchivePart> GetArchiveParts(Stream archive, string containerName)
        {
            var rar = RarArchive.Open(archive);

            return rar.Entries.Select(x => new ArchivePart(new RarStreamProvider(x), x.Key));
        }

        #endregion
    }
}
