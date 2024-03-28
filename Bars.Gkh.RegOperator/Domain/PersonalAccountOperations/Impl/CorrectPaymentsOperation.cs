namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    /// <summary>
    /// Операция - Корректировка оплат
    /// </summary>
    public class CorrectPaymentsOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Ключ для регистрации в контейнере
        /// </summary>
        public static string Key => nameof(CorrectPaymentsOperation);

        /// <summary>
        /// Код операции
        /// </summary>
        public override string Code => CorrectPaymentsOperation.Key;

        /// <summary>
        /// Наименование операции
        /// </summary>
        public override string Name => "Корректировка оплат";

        /// <summary>
        /// Ключ прав доступа
        /// </summary>
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.CorrectPaymentsOperation";

        /// <summary>
        /// Сервис корректировки оплат ЛС
        /// </summary>
        public IPersonalAccountCorrectPaymentsService PersonalAccountCorrectPaymentsService { get; set; }

        /// <summary>
        /// Метод выполнения операции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            try
            {
                return this.PersonalAccountCorrectPaymentsService.MovePayments(baseParams);
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
            
        }

        /// <summary>
        /// Метод получения данных пользовательского интерфейса
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult GetDataForUI(BaseParams baseParams)
        {
            return this.PersonalAccountCorrectPaymentsService.GetAccountPaymentInfo(baseParams);
        }
    }
}