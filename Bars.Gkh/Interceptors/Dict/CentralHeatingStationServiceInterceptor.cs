namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;

    public class InstitutionsServiceInterceptor : EmptyDomainInterceptor<Institutions>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Institutions> service, Institutions entity)
        {
            // Перед добавлением формируем адрес
            if (entity.Address != null)
            {
                var addressRepository = Container.Resolve<IRepository<FiasAddress>>();
                addressRepository.Save(entity.Address);
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Institutions> service, Institutions entity)
        {
            // Перед обновлением обновляем и адрес
            if (entity.Address != null)
            {
                var addressRepository = Container.Resolve<IRepository<FiasAddress>>();
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

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Institutions> service, Institutions entity)
        {
            if (Container.Resolve<IDomainService<BuilderWorkforce>>().GetAll().Any(x => x.Institutions.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Состав трудовых ресурсов подрядной организации;");
            }

            return Success();
        }
    }
}
