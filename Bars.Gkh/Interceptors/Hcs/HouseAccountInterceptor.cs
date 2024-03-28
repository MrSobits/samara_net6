namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Hcs;
    using Utils;

    /// <summary>
    /// Интерцептор для сущности Лицевой счет дома
    /// </summary>
    public class HouseAccountInterceptor : EmptyDomainInterceptor<HouseAccount>
    {
        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Лицевой счет дома"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<HouseAccount> service, HouseAccount entity)
        {
            Utils.SaveFiasAddress(Container, entity.FiasFullAddress);
            Utils.SaveFiasAddress(Container, entity.FiasMailingAddress);

            entity.OwnerNumber = GetOwnerNumber(service, entity);
            entity.PersonalAccountNum = GetAccountNumber(entity);

            return Success();
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Лицевой счет дома"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<HouseAccount> service, HouseAccount entity)
        {
            Utils.SaveFiasAddress(Container, entity.FiasFullAddress);
            Utils.SaveFiasAddress(Container, entity.FiasMailingAddress);

            entity.OwnerNumber = GetOwnerNumber(service, entity);
            entity.PersonalAccountNum = GetAccountNumber(entity);

            return Success();
        }

        private string GetAccountNumber(HouseAccount entity)
        {
            var fiasReposity = Container.Resolve<IRepository<Fias>>();

            var roFiasAddress = entity.RealityObject.FiasAddress;

            var street = fiasReposity.GetAll().First(x => x.AOGuid == roFiasAddress.StreetGuidId);

            var accountNumber =
                FixLength(street.CodeArea, 2)
                + FixLength(street.CodeCity, 2)
                + FixLength(street.CodeStreet, 2)
                + FixLength(roFiasAddress.House, 3)
                + FixLength(roFiasAddress.Housing, 1)
                + FixLength(entity.Apartment.ToString(), 3)
                + entity.OwnerNumber;

            return accountNumber;
        }

        private int GetOwnerNumber(IDomainService<HouseAccount> service, HouseAccount entity)
        {
            return entity.OwnerNumber > 0
                ? entity.OwnerNumber
                : service.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                    .Where(x => x.Id != entity.Id)
                    .Where(x => x.Apartment == entity.Apartment)
                    .Select(x => x.OwnerNumber)
                    .AsEnumerable()
                    .DefaultIfEmpty(0)
                    .Max() + 1;
        }

        private string FixLength(string fiasCode, int length)
        {
            fiasCode = fiasCode ?? string.Empty;

            return fiasCode.TrimStart('0').Length > length ? fiasCode : fiasCode.ToInt().ToString().PadLeft(length, '0');
        }
    }
}
