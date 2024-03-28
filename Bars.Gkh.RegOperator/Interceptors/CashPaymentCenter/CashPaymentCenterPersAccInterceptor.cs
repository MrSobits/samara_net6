namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Обработчики, которые необходимо выполнять при записи/удалении сущности "Лицевой счет расчетно-кассового центра"
    /// </summary>
    /// <remarks>
    /// Аналог триггеров в базах данных
    /// </remarks>
    public class CashPaymentCenterPersAccInterceptor : EmptyDomainInterceptor<CashPaymentCenterPersAcc>
    {
        /// <summary>
        /// Обработчик, выполняемый перед добавлением сущности
        /// </summary>
        /// <param name="service">Доменный сервис сущности</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Успех/ошибка</returns>
        public override IDataResult BeforeCreateAction(IDomainService<CashPaymentCenterPersAcc> service, CashPaymentCenterPersAcc entity)
        {
            return CheckDates(service, entity, ServiceOperationType.Save);
        }

        /// <summary>
        /// Обработчик, выполняемый перед обновлением сущности
        /// </summary>
        /// <param name="service">Доменный сервис сущности</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Успех/ошибка</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<CashPaymentCenterPersAcc> service, CashPaymentCenterPersAcc entity)
        {
            return CheckDates(service, entity, ServiceOperationType.Update);
        }
        
        /// <summary>
        /// Проверить пересечение с другими договорами по дому
        /// </summary>        
        private IDataResult CheckDates(IDomainService<CashPaymentCenterPersAcc> service, CashPaymentCenterPersAcc entity, ServiceOperationType type)
        {
            var existPersAccInPeriod =
                service.GetAll()
                    .Where(x => x.PersonalAccount.Id == entity.PersonalAccount.Id)
                    .WhereIf(type == ServiceOperationType.Update, x => x.Id != entity.Id) // Исключить «саму себя»
                    .Any(
                        x => (x.DateStart <= entity.DateStart && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateStart))
                             || (!entity.DateEnd.HasValue && x.DateStart >= entity.DateStart)
                             || (entity.DateEnd.HasValue && x.DateStart <= entity.DateEnd && (!x.DateEnd.HasValue || x.DateEnd >= entity.DateEnd)));

            if (existPersAccInPeriod)
            {
                var errorText = type == ServiceOperationType.Save
                    ? "Для добавления нового договора необходимо закрыть прошлый"
                    : "Для сохранения указанного периода договора необходимо закрыть прошлый";

                return Failure("У данного дома есть действующий договор в этом периоде. " + errorText);
            }

            return Success();
        }
    }
}