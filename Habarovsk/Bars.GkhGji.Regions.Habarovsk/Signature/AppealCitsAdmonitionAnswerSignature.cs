﻿namespace Bars.GkhGji.Regions.Habarovsk.DomainService.Impl
{
    using System.IO;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Entities;
    using Castle.Windsor;

    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class AppealCitsAdmonitionAnswerSignature : IAnswerSignature<AppealCitsAdmonition>
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;

        public AppealCitsAdmonitionAnswerSignature(IFileManager fileManager, IDomainService<FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }
        public IWindsorContainer Container { get; set; }

        public IDomainService<AppealCitsAdmonition> DomainService { get; set; }

        //public B4.Modules.FileStorage.IFileManager _fileManager { get; set; }

        //private IDomainService<FileInfo> _fileDomain { get; set; }

        public MemoryStream GetXmlStream(long id)
        {
            var pdfId = DomainService.Get(id).AnswerFile.Id;
            byte[] data;
            if (pdfId == 0)
            {
                data = new byte[0];
            }
            else
            {
                using (var file = _fileManager.GetFile(_fileDomain.Get(pdfId)))
                {
                    using (var tmpStream = new MemoryStream())
                    {
                        file.CopyTo(tmpStream);
                        data = tmpStream.ToArray();
                    }
                }
            }
            return new MemoryStream(data);
        }
    }
}