namespace Bars.GisIntegration.Base.Service.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.File;

    using Castle.Windsor;

    using CryptoPro.Sharpei;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для работы с вложениями
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        /// <summary>
        /// IoC Container
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc/>
        public Attachment CreateAttachment(FileInfo file, string description)
        {
            var attachmentDomain = this.Container.ResolveDomain<Attachment>();
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                Attachment attachment = null;

                this.Container.InTransaction(
                    () =>
                        {
                            var fileStorageName = AttachmentContext.Current.FileStorageName;

                            attachment =
                                attachmentDomain.GetAll().FirstOrDefault(x => x.SourceFileInfoId == file.Id && x.FileStorageName == fileStorageName);

                            if (attachment == null)
                            {
                                var fileInfo = fileManager.SaveFile(fileManager.GetFile(file), file.FullName);
                                attachment = new Attachment
                                                 {
                                                     SourceFileInfoId = file.Id,
                                                     FileInfo = fileInfo,
                                                     Name = fileInfo.Name,
                                                     Description = description,
                                                     FileStorageName = fileStorageName
                                                 };
                            }

                            var fileHash = this.GetGostHash(attachment.FileInfo);

                            if (attachment.Hash != fileHash)
                            {
                                attachment.Hash = fileHash;
                                attachment.Guid = string.Empty;
                            }

                            attachmentDomain.SaveOrUpdate(attachment);

                            AttachmentContext.Current.RegisterAttachment(attachment);
                        });

                return attachment;
            }
            finally
            {
                this.Container.Release(attachmentDomain);
                this.Container.Release(fileManager);
            }
        }

        /// /// <inheritdoc/>
        public void UploadAttachment(Attachment attachment, string orgPpaGuid)
        {
            if (!string.IsNullOrEmpty(attachment.Guid))
            {
                return;
            }

            //5 МБ = 5242880 Б 
            var uploaderName = attachment.FileInfo.Size <= 5242880 ? "SimpleFileUploader" : "FileUploader";

            var uploader = this.Container.Resolve<IFileUploader>(uploaderName);

            var uploadResult = uploader.UploadFile(attachment.FileInfo, orgPpaGuid);

            if (uploadResult.Success)
            {
                this.SetAttachmentGuid(attachment, uploadResult.FileGuid);
            }
            else
            {
                throw new Exception(uploadResult.Message);
            }
        }

        private void SetAttachmentGuid(Attachment attachment, string fileGuid)
        {
            var attachmentDomain = this.Container.ResolveDomain<Attachment>();    

            try
            {
                attachment.Guid = fileGuid;
                attachmentDomain.Update(attachment);
            }
            finally
            {
                this.Container.Release(attachmentDomain);
            }
        }

        /// <summary>
        /// Получить хэш файла по алгоритму ГОСТ
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private string GetGostHash(FileInfo fileInfo)
        {
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                byte[] hash;

                using (var gost = Gost3411.Create())
                {
                    if (gost == null)
                    {
                        throw new ApplicationException("Не удалось получть хэш вложения по алгоритму ГОСТ-3411");
                    }

                    using (var stream = fileManager.GetFile(fileInfo))
                    {
                        hash = gost.ComputeHash(stream);
                    }
                }

                var hex = new StringBuilder(hash.Length * 2);

                foreach (var b in hash)
                {
                    hex.AppendFormat("{0:x2}", b);
                }

                return hex.ToString();
            }
            catch (FileNotFoundException)
            {
                return string.Empty;
            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }
    }
}