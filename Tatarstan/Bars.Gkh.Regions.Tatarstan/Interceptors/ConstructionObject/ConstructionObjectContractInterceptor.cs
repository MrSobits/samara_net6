namespace Bars.Gkh.Regions.Tatarstan.Interceptors.ConstructionObject
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectContractInterceptor : EmptyDomainInterceptor<ConstructionObjectContract>
    {
        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<ConstructionObjectContract> service, ConstructionObjectContract entity)
        {
            var state = this.Container.Resolve<IStateProvider>();
            try
            {
                state.SetDefaultState(entity);
                return base.BeforeCreateAction(service, entity);
            }
            finally
            {
                this.Container.Release(state);
            }
        }
    }
}