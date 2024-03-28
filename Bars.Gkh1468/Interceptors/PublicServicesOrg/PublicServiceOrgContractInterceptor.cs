namespace Bars.Gkh1468.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.Entities;

    /// <summary>
    /// Интерцептор сущности "Договор поставщика ресурсов с домом
    /// </summary>
    public class PublicServiceOrgContractInterceptor : EmptyDomainInterceptor<PublicServiceOrgContract>
    {
        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<PublicServiceOrgContract> service, PublicServiceOrgContract entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
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
        public override IDataResult BeforeUpdateAction(IDomainService<PublicServiceOrgContract> service, PublicServiceOrgContract entity)
        {
            var validationResult = this.ValidateEntity(service, entity);

            if (!validationResult.Success)
            {
                return validationResult;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<PublicServiceOrgContract> service, PublicServiceOrgContract entity)
        {
            if (entity.ResOrgReason == 0)
            {
                return this.Failure("Не заполнено обязательное поле: \"Основание\"");
            }

            return this.Success();
        }
    }
}
