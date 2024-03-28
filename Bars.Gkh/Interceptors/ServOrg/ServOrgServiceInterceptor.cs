namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using Entities;

    public class ServOrgServiceInterceptor : EmptyDomainInterceptor<ServiceOrganization>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ServiceOrganization> service, ServiceOrganization entity)
        {
            if (service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id))
            {
                return new BaseDataResult(false, "Поставщик жилищных услуг с таким контрагентом уже создан");
            }

            return new BaseDataResult();
        }
        
        public override IDataResult BeforeUpdateAction(IDomainService<ServiceOrganization> service, ServiceOrganization entity)
        {
            if (service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id))
            {
                return new BaseDataResult(false, "Поставщик жилищных услуг с таким контрагентом уже создан");                
            }

            return new BaseDataResult();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ServiceOrganization> service, ServiceOrganization entity)
        {
            var servOrgContractService = Container.Resolve<IDomainService<ServiceOrgContract>>();
            var servOrgDocService = Container.Resolve<IDomainService<ServiceOrgDocumentation>>();
            var servOrgMunicService = Container.Resolve<IDomainService<ServiceOrgMunicipality>>();
            var servOrgRoService = Container.Resolve<IDomainService<ServiceOrgRealityObject>>();
            var servOrgServService = Container.Resolve<IDomainService<ServiceOrgService>>();

            try
            {
                var servOrgContractList =
                    servOrgContractService.GetAll().Where(x => x.ServOrg.Id == entity.Id).Select(x => x.Id).ToArray();
                var servOrgDocList =
                    servOrgDocService.GetAll()
                        .Where(x => x.ServiceOrganization.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var servOrgMunicList =
                    servOrgMunicService.GetAll()
                        .Where(x => x.ServOrg.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var servOrgRoList =
                    servOrgRoService.GetAll()
                        .Where(x => x.ServiceOrg.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var servOrgServList =
                    servOrgServService.GetAll()
                        .Where(x => x.ServiceOrganization.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                foreach (var value in servOrgContractList)
                {
                    servOrgContractService.Delete(value);
                }
                foreach (var value in servOrgDocList)
                {
                    servOrgDocService.Delete(value);
                }
                foreach (var value in servOrgMunicList)
                {
                    servOrgMunicService.Delete(value);
                }
                foreach (var value in servOrgRoList)
                {
                    servOrgRoService.Delete(value);
                }
                foreach (var value in servOrgServList)
                {
                    servOrgServService.Delete(value);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(servOrgContractService);
                Container.Release(servOrgDocService);
                Container.Release(servOrgMunicService);
                Container.Release(servOrgRoService);
                Container.Release(servOrgServService);
            }
        }
    }
}
