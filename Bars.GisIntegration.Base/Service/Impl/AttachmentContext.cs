namespace Bars.GisIntegration.Base.Service.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Контекст прикрепления файлов
    /// </summary>
    public class AttachmentContext : IDisposable
    {
        [ThreadStatic]
        private static AttachmentContext current;

        private static readonly AttachmentContext DummyContext = new DummyAttachmentContext();

        private readonly List<Attachment> attachments;

        private readonly FileStorageName? fileStorageName;

        public AttachmentContext(FileStorageName? fileStorageName)
        {
            this.fileStorageName = fileStorageName;
            AttachmentContext.current = this;
            this.attachments = new List<Attachment>();
        }

        public virtual FileStorageName FileStorageName
        {
            get
            {
                if (!this.fileStorageName.HasValue)
                {
                    throw new InvalidOperationException("Не указано имя файлового хранилища ГИС");
                }

                return this.fileStorageName.Value;
            }
        }

        public static AttachmentContext Current => AttachmentContext.current ?? AttachmentContext.DummyContext;

        public virtual IReadOnlyList<Attachment> Attachments => this.attachments.AsReadOnly();

        public void Dispose()
        {
            AttachmentContext.current = null;
        }

        public virtual void RegisterAttachment(Attachment attachment)
        {
            this.attachments.Add(attachment);
        }

        private class DummyAttachmentContext : AttachmentContext
        {
            public DummyAttachmentContext()
                : base(default(FileStorageName))
            {
            }

            public override IReadOnlyList<Attachment> Attachments { get; } = new List<Attachment>().AsReadOnly();

            public override FileStorageName FileStorageName
            {
                get
                {
                    throw new InvalidOperationException("Операция выполняется все контекста прикрепления");
                }
            }

            public override void RegisterAttachment(Attachment attachment)
            {
                throw new InvalidOperationException("Прикрепление файла невозможно т.к. операция выполняется все контекста прикрепления");
            }
        }
    }
}