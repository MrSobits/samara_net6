namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Интерцептор для сущности Решение собственников помещений МКД
    /// </summary>
    public class SpecialAccountDecisionInterceptor : EmptyDomainInterceptor<SpecialAccountDecision>
    {
        /// <summary>
        /// Интерфейс сервиса для актуализации способа формирования фонда дома.
        /// <remarks>Устанавливает значение свойства AccountFormationVariant</remarks>
        /// </summary>
        public IRealtyObjectAccountFormationService RealtyObjectAccountFormationService { get; set; }

        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Решение собственников помещений МКД"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {
            var serv = this.Container.Resolve<IDomainService<SpecialAccountDecisionNotice>>();
            serv.Save(
                new SpecialAccountDecisionNotice
                {
                    SpecialAccountDecision = entity
                });

            if (entity.CreditOrg != null)
            {
                var result = this.ValidateInnOgrn(entity);
                if (!result.Success)
                {
                    return result;
                }
            }

            if (entity.MailingAddress.IsNotNull())
            {
                Utils.SaveFiasAddress(this.Container, entity.MailingAddress);
            }

            return this.Success();
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Решение собственников помещений МКД"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {
            if (entity.CreditOrg != null)
            {
                var result = this.ValidateInnOgrn(entity);
                if (!result.Success)
                {
                    return result;
                }
            }

            if (entity.MailingAddress.IsNotNull())
            {
                Utils.SaveFiasAddress(this.Container, entity.MailingAddress);
            }

            return this.Success();
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Решение собственников помещений МКД"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {
            var serv = this.Container.Resolve<IDomainService<SpecialAccountDecisionNotice>>();

            var noticeIds = serv.GetAll()
                .Where(x => x.SpecialAccountDecision.Id == entity.Id)
                .Select(x => x.Id)
                .ToList();

            foreach (var noticeId in noticeIds)
            {
                serv.Delete(noticeId);
            }

            return this.Success();
        }

        private IDataResult ValidateInnOgrn(SpecialAccountDecision entity)
        {
            if (!Utils.VerifyInn(entity.Inn, true))
            {
                return this.Failure("Введен некорректный ИНН");
            }

            if (!string.IsNullOrEmpty(entity.Ogrn))
            {
                if (!Utils.VerifyOgrn(entity.Ogrn, true))
                {
                    return this.Failure("Указаный ОГРН не корректен");
                }
            }
            return this.Success();
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {
            this.ActualizeAccountFormationType(entity.RealityObject.Id);
            this.UpdateAccount(entity);
            return base.AfterCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после обновления объекта
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterUpdateAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
        {
            this.ActualizeAccountFormationType(entity.RealityObject.Id);
            this.UpdateAccount(entity);
            return base.AfterUpdateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после удаления объекта
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterDeleteAction(IDomainService<SpecialAccountDecision> service, SpecialAccountDecision entity)
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

        private void UpdateAccount(SpecialAccountDecision entity)
        {
            var service = this.Container.Resolve<ICalcAccountOwnerDecisionService>();
            using (this.Container.Using(service))
            {
                service.SaveSpecialCalcAccount(entity);
            }
        }
    }
}