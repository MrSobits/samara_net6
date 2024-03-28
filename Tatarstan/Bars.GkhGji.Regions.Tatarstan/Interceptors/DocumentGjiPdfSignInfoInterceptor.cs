using Bars.B4;
using Bars.B4.IoC;
// TODO : Расскоментировать после реализации GisIntegration
//using Bars.Gkh.PrintForm.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    public class DocumentGjiPdfSignInfoInterceptor : EmptyDomainInterceptor<DocumentGjiPdfSignInfo>
    {
        ///<inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<DocumentGjiPdfSignInfo> service, DocumentGjiPdfSignInfo entity)
        {
           /* var pdfSignInfoDomain = Container.Resolve<IDomainService<PdfSignInfo>>();
            using (Container.Using(pdfSignInfoDomain))
            {
                pdfSignInfoDomain.Delete(entity.PdfSignInfo.Id);
            }*/

            return base.BeforeDeleteAction(service, entity);
        }
    }
}