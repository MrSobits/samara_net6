namespace Bars.Gkh.Gis.Interceptors.ManOrg
{
    using System.Linq;
    using B4;
    using Entities.ManOrg;

    /// <summary>
    /// Interceptor для ManOrgBilAdditionService
    /// </summary>
    public class ManOrgBilAdditionServiceInterceptor : EmptyDomainInterceptor<ManOrgBilAdditionService>
    {
        /// <summary>
        /// Событие до создания
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Новый объект</param>
        /// <returns>Результат проверки</returns>
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgBilAdditionService> service, ManOrgBilAdditionService entity)
        {
            return this.CheckBilService(service, entity);
        }

        /// <summary>
        /// Событие до обновления
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Редактируемый объект</param>
        /// <returns>Результат проверки</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgBilAdditionService> service, ManOrgBilAdditionService entity)
        {
            return this.CheckBilService(service, entity);
        }

        /// <summary>
        /// Проверка записи на дублирование сервиса
        /// </summary>
        /// <param name="service">Запись</param>
        /// <param name="entity">Домен</param>
        /// <returns>Результат проверки</returns>
        private IDataResult CheckBilService(IDomainService<ManOrgBilAdditionService> service, ManOrgBilAdditionService entity)
        {
            if (service.GetAll().Any(
                x => x.BilService == entity.BilService &&
                    x.ManagingOrganization == entity.ManagingOrganization &&
                    x.Id != entity.Id))
            {
                return this.Failure("Такая услуга биллинга уже добавлена");
            }

            return this.Success();
        }
    }
}
