namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;
    using Entities;
    using Bars.B4.Utils;
    using Gkh.Utils;
    using B4.Modules.FIAS;
    using Gkh.Entities;
    using System.Linq;

    /// <summary>
    /// Интерцептор для сущности "Абонент - физ. лицо"
    /// </summary>
    public class IndividualAccountOwnerInterceptor : PersonalAccountOwnerInterceptor<IndividualAccountOwner>
    {
        /// <summary>
        /// Домен-сервис для <see cref="FiasAddress"/>
        /// </summary>
        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="RealityObject"/>
        /// </summary>
        public IDomainService<RealityObject> RealityObjectObjDomain { get; set; }

        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Абонент физ.лицо"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<IndividualAccountOwner> service, IndividualAccountOwner entity)
        {
            var result = base.BeforeCreateAction(service, entity);

            if (result.Success)
            {
                result = this.OnBeforeCreateOrUpdateAction(entity);
            }

            return result;
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Абонент - физ.лицо"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<IndividualAccountOwner> service, IndividualAccountOwner entity)
        {
            var result = this.OnBeforeCreateOrUpdateAction(entity);

            if (result.Success)
            {
                result = base.BeforeUpdateAction(service, entity);
            }

            return result;
        }
        
        private IDataResult OnBeforeCreateOrUpdateAction(IndividualAccountOwner entity)
        {
            if (entity.FiasFactAddress != null)
            {
                Utils.SaveFiasAddress(this.Container, entity.FiasFactAddress);
            }

            if (entity.FirstName.IsEmpty())
            {
                return this.Failure("Не заполнено обязательное поле: Имя");
            }

            if (entity.Surname.IsEmpty())
            {
                return this.Failure("Не заполнено обязательное поле: Фамилия");
            }

            if (entity.FirstName.Length > 100)
            {
                return this.Failure("Количество знаков в поле Имя не должно превышать 100 символов");
            }

            if (entity.Surname.Length > 100)
            {
                return this.Failure("Количество знаков в поле Фамилия не должно превышать 100 символов");
            }

            if (entity.SecondName.IsNotEmpty() && entity.SecondName.Length > 100)
            {
                return this.Failure("Количество знаков в поле Отчество не должно превышать 100 символов");
            }

            if (entity.IdentityNumber.IsNotEmpty() && entity.IdentityNumber.Length > 200)
            {
                return this.Failure("Количество знаков в поле Номер документа не должно превышать 200 символов");
            }

            if (entity.IdentitySerial.IsNotEmpty() && entity.IdentitySerial.Length > 200)
            {
                return this.Failure("Количество знаков в поле Серия документа не должно превышать 200 символов");
            }

            // Получение и запись айди дома фактического адреса
            if (entity.FiasFactAddress != null)
                switch (this.Container.GetGkhConfig<Bars.Gkh.ConfigSections.General.GeneralConfig>().UseFiasHouseIdentification)
                {
                    case Gkh.Enums.UseFiasHouseIdentification.NotUse:

                        var houseParam = this.FiasAddressDomain.Get(entity.FiasFactAddress.Id);
                        if (houseParam.PlaceName.IsEmpty() || houseParam.StreetName.IsEmpty())
                        {
                            //без этих параметров берется первый найденный дом с другой улицей и другим населенным пунктом
                            entity.RealityObject = null;
                            break;
                        }

                        entity.RealityObject = this.RealityObjectObjDomain.GetAll()
                            .Where(x => x.FiasAddress.PlaceName == houseParam.PlaceName)
                            .Where(x => x.FiasAddress.StreetName == houseParam.StreetName)
                            .WhereIf(houseParam.Letter.IsNotEmpty(), x => x.FiasAddress.Letter == houseParam.Letter)
                            .WhereIf(houseParam.Building.IsNotEmpty(), x => x.FiasAddress.Building == houseParam.Building)
                            .WhereIf(houseParam.House.IsNotEmpty(), x => x.FiasAddress.House == houseParam.House)
                            .FirstOrDefault();

                        break;
                    case Gkh.Enums.UseFiasHouseIdentification.Use:

                        entity.RealityObject = this.RealityObjectObjDomain.GetAll()
                            .FirstOrDefault(x => x.FiasAddress.AddressGuid == this.FiasAddressDomain.Get(entity.FiasFactAddress.Id).AddressGuid);

                        break;
                    default:
                        break;
                }

            return this.Success();
        }
    }
}