namespace Bars.Gkh.Domain.ArchieveReader
{
    using System.IO;
    using Ionic.Zip;

    using SharpCompress.Archives;
    using SharpCompress.Archives.Rar;

    public interface IStreamProvider
    {
        Stream OpenStream();
    }

    public class ZipStreamProvider : IStreamProvider
    {
        private readonly ZipEntry _entry;

        public ZipStreamProvider(ZipEntry entry)
        {
            _entry = entry;
        }

        public Stream OpenStream()
        {
            var ms = new MemoryStream();
            _entry.Extract(ms);

            return ms;
        }
    }


    public class RarStreamProvider : IStreamProvider
    {
        private readonly RarArchiveEntry _entry;

        public RarStreamProvider(RarArchiveEntry entry)
        {
            _entry = entry;
        }

        public Stream OpenStream()
        {
            var ms = new MemoryStream();

            _entry.WriteTo(ms);

            return ms;
        }
    }
}
