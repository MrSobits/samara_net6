namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;
    using Castle.Core.Internal;
    using Entities;
    using B4;
    using Bars.B4.Utils;
    using B4.Modules.FIAS;

    public class MunicipalityServiceInterceptor : EmptyDomainInterceptor<Municipality>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Municipality> service, Municipality entity)
        {
            return CheckFias(service, entity, ServiceOperationType.Save);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Municipality> service, Municipality entity)
        {
            return CheckFias(service, entity, ServiceOperationType.Update);
        }

        private IDataResult CheckFias(IDomainService<Municipality> service, Municipality entity, ServiceOperationType operationType)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Group.IsNotEmpty() && entity.Group.Length > 30)
            {
                return Failure("Количество знаков в поле Группа не должно превышать 30 символов");
            }

            return Success();
        }

        private bool CheckParentRecords(string aoguid, IDomainService<Fias> domainService, IDomainService<Municipality> service)
        {
            var parentGuid = domainService.GetAll().Where(x => x.AOGuid == aoguid && x.ActStatus == FiasActualStatusEnum.Actual).Select(x => x.ParentGuid).FirstOrDefault();

            if (parentGuid == null)
            {
                return true;
            }

            if (service.GetAll().Any(x => x.FiasId == parentGuid))
            {
                return false;
            }

            return CheckParentRecords(parentGuid, domainService, service);
        }

        private bool CheckChildRecords(string aoguid, IDomainService<Fias> domainService, IDomainService<Municipality> service)
        {
            var childGuids =
                domainService.GetAll()
                    .Where(x => x.ParentGuid == aoguid)
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => x.AOLevel == FiasLevelEnum.City || x.AOLevel == FiasLevelEnum.Raion)
                    .Select(x => x.AOGuid)
                    .AsEnumerable();

            foreach (var childGuid in childGuids)
            {
                if (childGuid == null)
                    continue;

                if (service.GetAll().Any(x => x.FiasId == childGuid))
                {
                    return false;
                }

                var result = CheckChildRecords(childGuid, domainService, service);

                if (!result)
                {
                    return false;
                }
            }

            return true;
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Municipality> service, Municipality entity)
        {
            if (Container.Resolve<IDomainService<PoliticAuthorityMunicipality>>().GetAll().Any(x => x.Municipality.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Органы государственной власти мун образований;");
            }

            if (Container.Resolve<IDomainService<LocalGovernmentMunicipality>>().GetAll().Any(x => x.Municipality.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Органы местного самоуправления мун образований;");
            }

            if (Container.Resolve<IDomainService<RealityObject>>().GetAll().Any(x => x.Municipality.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Жилой дом. Муниципальный район;");
            }

            if (Container.Resolve<IDomainService<RealityObject>>().GetAll().Any(x => x.MoSettlement.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Жилой дом. Муниципальное образование;");
            }

            return Success();
        }

        protected enum ActionType
        {
            Create,
            Update,
            Insert,
            Delete
        }
    }
}
