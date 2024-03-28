namespace Bars.Gkh.Services.Impl
{
    using System.IO;
    using System.Linq;
    using B4;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.B4.IoC;

    using DataContracts;
    using DataContracts.GetDocument;

    public partial class Service
    {
        public GetDocumentResponse GetDocument(string fileId)
        {
            var idFile = fileId.ToLong();

            if (idFile != 0)
            {
                var domain = this.Container.Resolve<IDomainService<B4.Modules.FileStorage.FileInfo>>();

                using (this.Container.Using(domain))
                {
                    var fileInfo = domain.Get(idFile);
                    if (fileInfo != null)
                    {
                        try
                        {
                            var base64String = this.Container.Resolve<IFileManager>().GetBase64String(fileInfo);

                            return new GetDocumentResponse
                            {
                                Document = new Document
                                {
                                    Id = fileInfo.Id,
                                    Name = fileInfo.FullName,
                                    Extension = fileInfo.Extention,
                                    File = base64String
                                }
                            };
                        }
                        catch (FileNotFoundException)
                        {
                        }
                    }
                }
                
            }

            return new GetDocumentResponse { Result = Result.DataNotFound };
        }
    }
}