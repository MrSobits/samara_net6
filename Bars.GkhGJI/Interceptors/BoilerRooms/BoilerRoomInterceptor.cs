namespace Bars.GkhGji.Interceptors.BoilerRooms
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities.BoilerRooms;
    using Gkh.Utils;

    /// <summary>
    /// Интерцептор для сущности Котельная
    /// </summary>
    public class BoilerRoomInterceptor : EmptyDomainInterceptor<BoilerRoom>
    {
        private readonly IDomainService<Fias> _fiasDomain;
        private readonly IRepository<Municipality> _municipalityDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fiasDomain">Домен-сервис "ФИАС"</param>
        /// <param name="municipalityDomain">Домен-сервис "Муниципальное образование"</param>
        public BoilerRoomInterceptor(
            IDomainService<Fias> fiasDomain,
            IRepository<Municipality> municipalityDomain)
        {
            _fiasDomain = fiasDomain;
            _municipalityDomain = municipalityDomain;
        }

        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Котельная"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<BoilerRoom> service, BoilerRoom entity)
        {
            PreventAddressDuplication(entity);
            Utils.SaveFiasAddress(Container, entity.Address);
            entity.Municipality = this.GetMunicipality(entity.Address);
            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Котельная"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<BoilerRoom> service, BoilerRoom entity)
        {
            PreventAddressDuplication(entity);
            Utils.SaveFiasAddress(Container, entity.Address);
            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>
        /// Получение фиас-адреса из базы, который совпадает по полям с переданным адресом.
        /// </summary>
        /// <param name="phantom"></param>
        /// <returns></returns>
        private FiasAddress GetPersistentFiasAddress(FiasAddress phantom)
        {
            return null;
        }

        /// <summary>
        /// Получение МО
        /// </summary>
        /// <param name="fiasAddress"></param>
        /// <returns></returns>
        private Municipality GetMunicipality(FiasAddress fiasAddress)
        {
            var fias = _fiasDomain.GetAll()
                    .FirstOrDefault(x => x.ActStatus == FiasActualStatusEnum.Actual && fiasAddress.PlaceGuidId == x.AOGuid);
            return _municipalityDomain.FirstOrDefault(
                x =>
                    x.FiasId == fiasAddress.PlaceGuidId ||
                    (fias != null && fias.OKTMO == x.Oktmo.ToString()));
        }

        /// <summary>
        /// Если фиас-адрес котельной уже есть в системе, то изменяет ссылку на него
        /// </summary>
        /// <param name="entity"></param>
        private void PreventAddressDuplication(BoilerRoom entity)
        {
            if (entity.Address.Id != 0)
            {
                return;
            }

            var persistAddress = GetPersistentFiasAddress(entity.Address);
            if (persistAddress != null)
            {
                entity.Address = persistAddress;
            }
        }
    }
}