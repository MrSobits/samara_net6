namespace Bars.Gkh.Regions.Tatarstan.Interceptors.GasEquipmentOrg
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg;

    public class GasEquipmentOrgRealityObjInterceptor : EmptyDomainInterceptor<GasEquipmentOrgRealityObj>
    {
        public override IDataResult BeforeCreateAction(IDomainService<GasEquipmentOrgRealityObj> service, GasEquipmentOrgRealityObj entity)
        {
            var validation = this.Validate(service, entity);

            if (!validation.Success)
            {
                return validation;
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<GasEquipmentOrgRealityObj> service, GasEquipmentOrgRealityObj entity)
        {
            var validation = this.Validate(service, entity);

            if (!validation.Success)
            {
                return validation;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult Validate(IDomainService<GasEquipmentOrgRealityObj> service, GasEquipmentOrgRealityObj entity)
        {
            if (entity.EndDate != null && entity.StartDate >= entity.EndDate)
            {
                return this.Failure("Дата окончания должна быть позже даты начала.");
            }

            var contractList = service.GetAll()
                .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                .Where(x => x.GasEquipmentOrg.Id != entity.GasEquipmentOrg.Id)
                .ToArray();

            var contractBefore = contractList
                .Where(x => x.StartDate <= entity.StartDate)
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault();

            var contractAfter = contractList
                .Where(x => x.StartDate > entity.StartDate)
                .OrderBy(x => x.StartDate)
                .FirstOrDefault();

            var currentContract = contractBefore ?? contractAfter;

            if (!this.CheckInterval(entity, contractBefore, contractAfter))
            {
                return this.Failure(
                    $"Сохранение невозможно. Данный дом находится в обслуживании другого контрагента. Данный дом обслуживается у {currentContract.GasEquipmentOrg.Contragent.ShortName}");
            }

            return this.Success();
        }

        private bool CheckInterval(GasEquipmentOrgRealityObj entity, GasEquipmentOrgRealityObj contractBefore, GasEquipmentOrgRealityObj contractAfter)
        {
            if (contractBefore != null)
            {
                if ((contractBefore.StartDate >= entity.StartDate.AddDays(-1))
                    || (contractBefore.EndDate != null && contractBefore.EndDate >= entity.StartDate))
                {
                    return false;
                }
            }

            if (contractAfter != null)
            {
                if (entity.EndDate == null
                    || (entity.EndDate != null && entity.EndDate >= contractAfter.StartDate))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
