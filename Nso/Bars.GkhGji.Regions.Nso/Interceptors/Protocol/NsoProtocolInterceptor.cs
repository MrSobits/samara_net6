using System.Linq;

namespace Bars.GkhGji.Regions.Nso.Interceptors
{
    using Bars.GkhGji.Interceptors;

    using Entities;
    using B4;

    public class NsoProtocolInterceptor : ProtocolServiceInterceptor<NsoProtocol>
    {
        public override IDataResult BeforeCreateAction(IDomainService<NsoProtocol> service, NsoProtocol entity)
        {
            if (entity.Contragent != null)
                entity.Contragent = entity.Inspection.Contragent;

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<NsoProtocol> service, NsoProtocol entity)
        {
            var longTextService = this.Container.Resolve<IDomainService<ProtocolLongText>>();

            try
            {
                var longIds = longTextService.GetAll().Where(x => x.Protocol.Id == entity.Id).ToList();

                foreach (var id in longIds)
                {
                    longTextService.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(longTextService);
            }
        }

    }
}
