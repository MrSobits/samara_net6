namespace Bars.GkhGji.Regions.Nnovgorod.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;

    using Bars.B4.Utils;

    public class ProtocolServiceInterceptor : GkhGji.Interceptors.ProtocolServiceInterceptor
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<Protocol> service, Protocol entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return result;
            }

            this.DocumentGjiPhysPersonInfoDomain.GetAll()
                .Where(x => x.Document.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => this.DocumentGjiPhysPersonInfoDomain.Delete(x));

            return this.Success();
        }
    }
}
