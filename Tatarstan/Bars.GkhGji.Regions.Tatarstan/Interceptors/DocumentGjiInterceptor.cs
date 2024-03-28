using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    public class DocumentGjiInterceptor : GkhGji.Interceptors.DocumentGjiInterceptor<DocumentGji>
    {
        ///<inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<DocumentGji> service, DocumentGji entity)
        {
            var docPdfSignInfoDomain = this.Container.Resolve<IDomainService<DocumentGjiPdfSignInfo>>();
            using (this.Container.Using(docPdfSignInfoDomain))
            {
                docPdfSignInfoDomain.GetAll()
                    .Where(x => x.DocumentGji == entity)
                    .ForEach(x =>
                    {
                        docPdfSignInfoDomain.Delete(x.Id);
                    });
            }

            return this.Success();
        }
    }
}