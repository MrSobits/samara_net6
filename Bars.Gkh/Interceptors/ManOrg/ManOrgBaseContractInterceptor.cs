namespace Bars.Gkh.Interceptors.ManOrg
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class ManOrgBaseContractInterceptor : EmptyDomainInterceptor<ManOrgBaseContract>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgBaseContract> service, ManOrgBaseContract entity)
        {
            var manOrgContrRoService = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var manOrgContrRelatService = Container.Resolve<IDomainService<ManOrgContractRelation>>();

            try
            {
                manOrgContrRoService.GetAll().Where(x => x.ManOrgContract.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgContrRoService.Delete(x));
                manOrgContrRelatService.GetAll().Where(x => x.Parent.Id == entity.Id || x.Children.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgContrRelatService.Delete(x));
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(manOrgContrRoService);
                Container.Release(manOrgContrRelatService);
            }
            
            return Success();
        }
    }
}