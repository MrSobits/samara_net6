namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;

    using Entities;

    public class SupplyResourceOrgServiceInterceptor : EmptyDomainInterceptor<SupplyResourceOrg>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SupplyResourceOrg> service, SupplyResourceOrg entity)
        {
            if (service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id))
            {
                return new BaseDataResult(false, "Поставщик коммунальных услуг с таким контрагентом уже существует");
            }

            return new BaseDataResult();
        }
        
        public override IDataResult BeforeUpdateAction(IDomainService<SupplyResourceOrg> service, SupplyResourceOrg entity)
        {
            if (service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id))
            {
                return new BaseDataResult(false, "Поставщик коммунальных услуг с таким контрагентом уже существует");                
            }

            return new BaseDataResult();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SupplyResourceOrg> service, SupplyResourceOrg entity)
        {
            var roResOrgService = Container.Resolve<IDomainService<RealityObjectResOrg>>();
            var supResOrgDocService = Container.Resolve<IDomainService<SupplyResourceOrgDocumentation>>();
            var supResOrgMunicService = Container.Resolve<IDomainService<SupplyResourceOrgMunicipality>>();
            var supResOrgRoService = Container.Resolve<IDomainService<SupplyResourceOrgRealtyObject>>();
            var supResOrgServService = Container.Resolve<IDomainService<SupplyResourceOrgService>>();

            try
            {
                var roResOrgList =
                    roResOrgService.GetAll().Where(x => x.ResourceOrg.Id == entity.Id).Select(x => x.Id).ToArray();
                var supResOrgDocList =
                    supResOrgDocService.GetAll()
                        .Where(x => x.SupplyResourceOrg.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var supResOrgMunicList =
                    supResOrgMunicService.GetAll()
                        .Where(x => x.SupplyResourceOrg.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var supResOrgRoList =
                    supResOrgRoService.GetAll()
                        .Where(x => x.SupplyResourceOrg.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var supResOrgServList =
                    supResOrgServService.GetAll()
                        .Where(x => x.SupplyResourceOrg.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                foreach (var value in roResOrgList)
                {
                    roResOrgService.Delete(value);
                }
                foreach (var value in supResOrgDocList)
                {
                    supResOrgDocService.Delete(value);
                }
                foreach (var value in supResOrgMunicList)
                {
                    supResOrgMunicService.Delete(value);
                }
                foreach (var value in supResOrgRoList)
                {
                    supResOrgRoService.Delete(value);
                }
                foreach (var value in supResOrgServList)
                {
                    supResOrgServService.Delete(value);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(roResOrgService);
                Container.Release(supResOrgDocService);
                Container.Release(supResOrgMunicService);
                Container.Release(supResOrgRoService);
                Container.Release(supResOrgServService);
            }
        }
    }
}
