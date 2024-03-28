namespace Bars.Gkh.Modules.Gkh1468.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Интерцептор сущности <see cref="PublicServiceOrgContractService"/>
    /// </summary>
    public class PublicServiceOrgContractServiceInterceptor : EmptyDomainInterceptor<PublicServiceOrgContractService>
    {
        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<PublicServiceOrgContractService> service, PublicServiceOrgContractService entity)
        {
            var validateResult = this.ValidateEntity(service, entity);
            if (!validateResult.Success)
            {
                return validateResult;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<PublicServiceOrgContractService> service, PublicServiceOrgContractService entity)
        {
            var validateResult = this.ValidateEntity(service, entity);
            if (!validateResult.Success)
            {
                return validateResult;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<PublicServiceOrgContractService> service, PublicServiceOrgContractService entity)
        {
            // пока завязываемся на наименование,
            // когда аналитики сообразят, завяжемся на код
            var communalResources = new [] {"тепловая энергия", "горячая вода"};
            if (entity.CommunalResource.Name.In(communalResources))
            {
                if (!entity.HeatingSystemType.HasValue)
                {
                    return this.Failure("Не заполнено обязательное поле: \"Тип системы теплоснабжения\"");
                }

                if (!entity.SchemeConnectionType.HasValue)
                {
                    return this.Failure("Не заполнено обязательное поле: \"Схема присоединения\"");
                }
            }
            else
            {
                entity.HeatingSystemType = null;
                entity.SchemeConnectionType = null;
            }

            return this.Success();
        }
    }
}
