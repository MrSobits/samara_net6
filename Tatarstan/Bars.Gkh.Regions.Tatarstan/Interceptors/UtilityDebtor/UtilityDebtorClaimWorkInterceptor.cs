namespace Bars.Gkh.Regions.Tatarstan.Interceptors.UtilityDebtor
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Modules.ClaimWork.Interceptors;
    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;

    /// <summary>
    /// Интерсептор для сущности UtilityDebtorClaimWork
    /// </summary>
    public class UtilityDebtorClaimWorkInterceptor : BaseClaimWorkInterceptor<UtilityDebtorClaimWork>
    {
        /// <summary>Метод вызывается перед созданием объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeCreateAction(IDomainService<UtilityDebtorClaimWork> service, UtilityDebtorClaimWork entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();

            if (entity.State.IsNull())
            {
                stateProvider.SetDefaultState(entity);
            }

            entity.ClaimWorkTypeBase = ClaimWorkTypeBase.UtilityDebtor;
            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое после обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат работы</returns>
        public override IDataResult AfterUpdateAction(IDomainService<UtilityDebtorClaimWork> service, UtilityDebtorClaimWork entity)
        {
            return this.Success();
        }
    }
}
