namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>
    /// Интерцептор сущности <see cref="RegOpAccountDecision"/>
    /// </summary>
    public class RegOpAccountDecisionInterceptor : EmptyDomainInterceptor<RegOpAccountDecision>
    {
        /// <summary>
        /// Интерфейс сервиса для актуализации способа формирования фонда дома.
        /// <remarks>Устанавливает значение свойства AccountFormationVariant</remarks>
        /// </summary>
        public IRealtyObjectAccountFormationService RealtyObjectAccountFormationService { get; set; }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<RegOpAccountDecision> service, RegOpAccountDecision entity)
        {
            this.ActualizeAccountFormationType(entity.RealityObject.Id);
            this.UpdateAccount(entity);
            return base.AfterCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после обновления объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterUpdateAction(IDomainService<RegOpAccountDecision> service, RegOpAccountDecision entity)
        {
            this.ActualizeAccountFormationType(entity.RealityObject.Id);
            this.UpdateAccount(entity);
            return base.AfterUpdateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после удаления объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterDeleteAction(IDomainService<RegOpAccountDecision> service, RegOpAccountDecision entity)
        {
            this.ActualizeAccountFormationType(entity.RealityObject.Id);
            return base.AfterDeleteAction(service, entity);
        }

        /// <summary>
        /// Использует интерфейс для актуализации способа формирования фонда
        /// </summary>
        /// <param name="roId">Идентификатор дома</param>
        protected virtual void ActualizeAccountFormationType(long roId)
        {
            this.RealtyObjectAccountFormationService.ActualizeAccountFormationType(roId);
        }

        private void UpdateAccount(RegOpAccountDecision entity)
        {
            var service = this.Container.Resolve<ICalcAccountOwnerDecisionService>();
            using (this.Container.Using(service))
            {
                service.SaveRegopCalcAccount(entity);
            }
        }
    }
}
