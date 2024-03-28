using Bars.Gkh.Utils.AddressPattern;

namespace Bars.Gkh.ClaimWork.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using Modules.ClaimWork.Entities;
    using Gkh.Utils;

    /// <summary>
    /// Интерцептор для сущности Учреждения в судебной практике
    /// </summary>
    public class JurInstitutionInterceptor : EmptyDomainInterceptor<JurInstitution>
    {
        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Учреждения в судебной практике"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<JurInstitution> service, JurInstitution entity)
        {
            SaveFiasAddress(entity);

            return Success();
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Учреждения в судебной практике"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<JurInstitution> service, JurInstitution entity)
        {
             SaveFiasAddress(entity);

             return Success();
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Учреждения в судебной практике"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<JurInstitution> service, JurInstitution entity)
        {
            var jurInstRealObjDomain = Container.ResolveDomain<JurInstitutionRealObj>();

            if (jurInstRealObjDomain.GetAll().Any(x => x.JurInstitution.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Территориальная подсудность;");
            }

            return Success();
        }

        private void SaveFiasAddress(JurInstitution entity)
        {
            var fiasAddress = entity.FiasAddress;

            Utils.SaveFiasAddress(Container, fiasAddress);

            entity.Address = null;
            if (entity.FiasAddress != null)
            {
                entity.Address = entity.Municipality != null
                    ? Container.Resolve<IAddressPattern>().FormatShortAddress(entity.Municipality, entity.FiasAddress)
                    : entity.FiasAddress.AddressName;
            }
        }
    }
}

