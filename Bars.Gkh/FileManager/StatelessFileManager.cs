namespace Bars.Gkh.FileManager
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;

    using NHibernate;

    public class StatelessFileManager : FileSystemFileManager
    {
        private readonly IRepository<FileInfo> repository;

        public StatelessFileManager(IStatelessSession session)
        {
            this.repository = new StatelessNhRepository<FileInfo>(session);
        }

        /// <inheritdoc />
        protected override FileInfo CreateEntity(string name, string extension, string checkSum, long size)
        {
            var value = new FileInfo
            {
                CheckSum = checkSum,
                Extention = extension,
                Name = name,
                Size = size
            };

            this.repository.Save(value);

            return value;
        }
    }
}