namespace Bars.Gkh.Regions.Tatarstan.Interceptors.GasEquipmentOrg
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg;

    public class GasEquipmentOrgInterceptor : EmptyDomainInterceptor<GasEquipmentOrg>
    {
        public override IDataResult BeforeCreateAction(IDomainService<GasEquipmentOrg> service, GasEquipmentOrg entity)
        {
            if (entity.Contragent == null)
            {
                return this.Failure("Не заполнены обязательные поля: Контрагент");
            }

            return service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id)
                ? this.Failure("ВДГО с таким контрагентом уже создана")
                : this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<GasEquipmentOrg> service, GasEquipmentOrg entity)
        {
            if (entity.Contragent == null)
            {
                return this.Failure("Не заполнены обязательные поля: Контрагент");
            }

            return service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id)
                ? this.Failure("ВДГО с таким контрагентом уже создана")
                : this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<GasEquipmentOrg> service, GasEquipmentOrg entity)
        {
            var gasEquipmentOrgRealityObjService = this.Container.Resolve<IDomainService<GasEquipmentOrgRealityObj>>();

            try
            {
                var gasEquipmentOrgRealityObjList =
                    gasEquipmentOrgRealityObjService.GetAll().Where(x => x.GasEquipmentOrg.Id == entity.Id).Select(x => x.Id).ToArray();

                foreach (var id in gasEquipmentOrgRealityObjList)
                {
                    gasEquipmentOrgRealityObjService.Delete(id);
                }

                return this.Success();
            }
            catch (Exception)
            {
                return this.Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                this.Container.Release(gasEquipmentOrgRealityObjService);
            }
        }
    }
}
