namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    class FiasOktmoInterceptor : EmptyDomainInterceptor<FiasOktmo>
    {
        public IDomainService<RealityObject> RoService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<FiasOktmo> service, FiasOktmo entity)
        {
            if (!CheckUniqueGuid(service, entity))
            {
                return Failure("Привязка для данного населенного пунтка уже задана");
            }

            UpdateRoMunicipality(entity);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<FiasOktmo> service, FiasOktmo entity)
        {
            if (!CheckUniqueGuid(service, entity))
            {
                return Failure("Привязка для данного населенного пунтка уже задана");
            }

            UpdateRoMunicipality(entity);

            return base.BeforeUpdateAction(service, entity);
        }
        
        private void UpdateRoMunicipality(FiasOktmo entity)
        {
            var objects = RoService.GetAll().Where(x => x.FiasAddress.PlaceGuidId == entity.FiasGuid).ToList();

            var highLevelMo = entity.Municipality.ParentMo ?? entity.Municipality;
            
            using (var session = Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var ro in objects)
                        {
                            ro.Municipality = highLevelMo;
                            ro.MoSettlement = entity.Municipality;

                            session.Update(ro);
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        private bool CheckUniqueGuid(IDomainService<FiasOktmo> service, FiasOktmo entity)
        {
            return entity.Id == 0 ? 
                !service.GetAll().Any(x => x.FiasGuid == entity.FiasGuid) : 
                !service.GetAll().Any(x => x.FiasGuid == entity.FiasGuid && x.Id != entity.Id);
        }
    }
}
