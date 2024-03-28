namespace Bars.Gkh.Gis.Interceptors.ManOrg
{
    using System.Linq;
    using B4;
    using Entities.ManOrg;

    /// <summary>
    /// Interceptor для ManOrgBilCommunalService
    /// </summary>
    public class ManOrgBilCommunalServiceInterceptor : EmptyDomainInterceptor<ManOrgBilCommunalService>
    {
        /// <summary>
        /// Событие до создания
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Новый объект</param>
        /// <returns>Результат проверки</returns>
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgBilCommunalService> service, ManOrgBilCommunalService entity)
        {
            return this.CheckBilService(service, entity);
        }

        /// <summary>
        /// Событие до обновления
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Редактируемый объект</param>
        /// <returns>Результат проверки</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ManOrgBilCommunalService> service, ManOrgBilCommunalService entity)
        {
            return this.CheckBilService(service, entity);
        }

        /// <summary>
        /// Проверка записи на дублирование сервиса
        /// </summary>
        /// <param name="service">Запись</param>
        /// <param name="entity">Домен</param>
        /// <returns>Результат проверки</returns>
        private IDataResult CheckBilService(IDomainService<ManOrgBilCommunalService> service, ManOrgBilCommunalService entity)
        {
            if (service.GetAll().Any(
                x => x.BilService == entity.BilService &&
                    x.ManagingOrganization == entity.ManagingOrganization &&
                    x.Id != entity.Id))
            {
                return this.Failure("Такая услуга уже добавлена");
            }

            return this.Success();
        }
    }
}
