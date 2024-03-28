namespace Bars.Gkh.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    /// <summary>
    /// Interceptor нормативов потребления
    /// </summary>
    public class NormConsumptionInterceptor : EmptyDomainInterceptor<NormConsumption>
    {
        /// <summary>
        /// Действие перед созданием
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult BeforeCreateAction(IDomainService<NormConsumption> service, NormConsumption entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            using (this.Container.Using(stateProvider, service))
            {
                stateProvider.SetDefaultState(entity);

                if (entity.State == null)
                {
                    return this.Failure("Не найден начальный статус");
                }

                var normCons = service
                    .GetAll().FirstOrDefault(x => x.Type == entity.Type && x.Period.Id == entity.Period.Id && x.Municipality.Id == entity.Municipality.Id);

                if (normCons != null)
                {
                    return this.Failure("В данном периоде уже создан указанный норматив");
                }

                return this.Success();
            }
        }
    }
}