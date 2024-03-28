namespace Bars.Gkh.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities.Dicts;

    public class CentralHeatingStationServiceInterceptor : EmptyDomainInterceptor<CentralHeatingStation>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CentralHeatingStation> service, CentralHeatingStation entity)
        {
            // Перед добавлением формируем адрес
            if (entity.Address != null)
            {
                var addressRepository = this.Container.Resolve<IRepository<FiasAddress>>();
                addressRepository.Save(entity.Address);
            }

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<CentralHeatingStation> service, CentralHeatingStation entity)
        {
            // Перед обновлением обновляем и адрес
            if (entity.Address != null)
            {
                var addressRepository = this.Container.Resolve<IRepository<FiasAddress>>();
                if (entity.Address.Id > 0)
                {
                    var currAddr = addressRepository.Load(entity.Address.Id);
                    if (currAddr.IsModified(entity.Address))
                    {
                        entity.Address.Id = 0;
                        addressRepository.Save(entity.Address);
                    }
                }
                else
                {
                    addressRepository.Save(entity.Address);
                }
            }

            return this.Success();
        }
    }
}