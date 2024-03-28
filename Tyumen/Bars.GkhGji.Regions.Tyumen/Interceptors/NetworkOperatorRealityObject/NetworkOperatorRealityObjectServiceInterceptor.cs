namespace Bars.GkhGji.Regions.Tyumen.Interceptors.NetworkOperatorRealityObject
{
    using System.Linq;

    using B4;
    using B4.DataAccess;

    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tyumen.Entities;

    public class NetworkOperatorRealityObjectServiceInterceptor : EmptyDomainInterceptor<NetworkOperatorRealityObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<NetworkOperatorRealityObject> service, NetworkOperatorRealityObject entity)
        {

            var nopRealityObjectTechDecision = Container.ResolveDomain<NetworkOperatorRealityObjectTechDecision>();

            try
            {
                var nopTechDecisionRecord = nopRealityObjectTechDecision.GetAll().FirstOrDefault(x => x.NetworkOperatorRealityObject.Id == entity.Id);

                if (nopTechDecisionRecord.IsNotNull())
                {
                    nopRealityObjectTechDecision.Delete(nopTechDecisionRecord.Id);
                }
            }

            finally
            {
                Container.Release(nopRealityObjectTechDecision);
            }

            return Success();
        }
    }
}