namespace Bars.Gkh.SystemDataTransfer.Meta.Serialization
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.FileManager;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;
    using Bars.Gkh.SystemDataTransfer.Utils;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сериализатор файлов
    /// </summary>
    public class FileInfoSerializer : AbstractDataSerializer<FileInfo>
    {
        private string storagePath;
        private readonly IDictionary<long, FileInfo> files = new Dictionary<long, FileInfo>();

        private IFileManager fileManager;

        private IFileManager FileManager => this.fileManager ?? (this.InitAndGetFileManager());

        private IFileManager InitAndGetFileManager()
        {
            this.fileManager = new StatelessFileManager(IntegrationSessionAccessor.GetSession());
            this.Container.BuildUp(this.fileManager);

            return this.fileManager;
        }

        /// <summary>
        /// Путь к хранилищу
        /// </summary>
        public string StoragePath => this.storagePath 
            ?? (this.storagePath = this.Container.Resolve<IConfigProvider>().GetConfig()
                .ModulesConfig["Bars.B4.Modules.FileSystemStorage"]
                .GetAs("FileDirectory", string.Empty) ?? string.Empty);

        /// <inheritdoc />
        public override object Serialize(FileInfo entity)
        {
            if (File.Exists(this.GetFilePath(entity)))
            {
                this.files.Add(entity.Id, entity);
                return entity;
            }

            return null;
        }

        /// <inheritdoc />
        public override void Flush(long id, Stream stream)
        {
            var fileInfo = this.files.Get(id);

            if (fileInfo.IsNotNull())
            {
                using (var fs = File.Open(this.GetFilePath(fileInfo), FileMode.Open))
                {
                    fs.CopyTo(stream);   
                }

                this.files.Remove(id);
            }
        }

        /// <inheritdoc />
        public override string GetFileName(long id)
        {
            return this.files.Get(id)?.FullName;
        }

        /// <inheritdoc />
        public override IEntity Deserializer(IEntity saveEntity, Item item, Stream stream)
        {
            return this.FileManager.SaveFile(stream, item["Name"].ToString());
        }

        private string GetFilePath(FileInfo fileInfo)
        {
            return Path.Combine(this.StoragePath,
                fileInfo.ObjectCreateDate.Year.ToString(),
                fileInfo.ObjectCreateDate.Month.ToString(),
                $"{fileInfo.Id}.{fileInfo.Extention}");
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            this.Container.Release(this.fileManager);
            base.Dispose();
        }
    }
}