namespace Bars.Gkh1468.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.Entities;

    public class PublicServOrgServiceInterceptor : EmptyDomainInterceptor<PublicServiceOrg>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PublicServiceOrg> service, PublicServiceOrg entity)
        {
            return this.CheckContragent(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PublicServiceOrg> service, PublicServiceOrg entity)
        {
            return this.CheckContragent(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PublicServiceOrg> service, PublicServiceOrg entity)
        {
            var publServOrgMunicServ = this.Container.Resolve<IDomainService<PublicServiceOrgMunicipality>>();
            var publServOrgRoServ = this.Container.Resolve<IDomainService<PublicServiceOrgRealtyObject>>();
            var roPulbServOrgServ = this.Container.Resolve<IDomainService<PublicServiceOrgContract>>();

            try
            {
                var publServOrgMunicList =
                    publServOrgMunicServ.GetAll().Where(x => x.PublicServiceOrg.Id == entity.Id).Select(x => x.Id).ToArray();
                var publServOrgRoList =
                    publServOrgRoServ.GetAll().Where(x => x.PublicServiceOrg.Id == entity.Id).Select(x => x.Id).ToArray();
                var roPulbServOrgList =
                    roPulbServOrgServ.GetAll().Where(x => x.PublicServiceOrg.Id == entity.Id).Select(x => x.Id).ToArray();
                foreach (var id in publServOrgMunicList)
                {
                    publServOrgMunicServ.Delete(id);
                }
                foreach (var id in publServOrgRoList)
                {
                    publServOrgRoServ.Delete(id);
                }
                foreach (var id in roPulbServOrgList)
                {
                    roPulbServOrgServ.Delete(id);
                }

                return this.Success();
            }
            catch (Exception)
            {
                return this.Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                this.Container.Release(publServOrgMunicServ);
                this.Container.Release(publServOrgRoServ);
                this.Container.Release(roPulbServOrgServ);
            }
        }

        private IDataResult CheckContragent(IDomainService<PublicServiceOrg> service, PublicServiceOrg entity)
        {
            if (service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id))
            {
                return this.Failure("Для указанного контрагента уже существует поставщик");
            }

            return this.Success();
        }
    }
}