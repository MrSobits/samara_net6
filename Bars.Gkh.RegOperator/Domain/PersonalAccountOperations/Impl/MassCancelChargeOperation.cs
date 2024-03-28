namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using B4;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Castle.Windsor;

    /// <summary>
    /// Отмена начислений
    /// </summary>
    public class MassCancelChargeOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Код
        /// </summary>
        public static string Key => "MassCancelChargeOperation";

        /// <summary>
        /// Код
        /// </summary>
        public override string Code => MassCancelChargeOperation.Key;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Отмена начислений и корректировок за периоды";

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис корректировки оплат ЛС
        /// </summary>
        public IPersonalAccountCancelChargeService PersonalAccountCancelChargeService { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public MassCancelChargeOperation(
            IWindsorContainer container,
            IPersonalAccountCancelChargeService personalAccountCancelChargeService)
        {     
            this.Container = container;
            this.PersonalAccountCancelChargeService = personalAccountCancelChargeService;
        }

        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            try
            {
                return this.PersonalAccountCancelChargeService.CancelCharges(baseParams);
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDataResult GetDataForUI(BaseParams baseParams)
        {
            try
            {
                return this.PersonalAccountCancelChargeService.GetDataForUI(baseParams);
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
        }
    }
}
