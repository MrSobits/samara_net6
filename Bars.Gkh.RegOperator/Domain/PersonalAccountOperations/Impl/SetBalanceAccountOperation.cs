namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using B4;
    using DomainModelServices;

    /// <summary>
    /// Операция установки/изменения сальдо
    /// </summary>
    public class SetBalanceAccountOperation : PersonalAccountOperationBase
    {
        private readonly IPersonalAccountChangeService accountChangeService;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="accountChangeService">Сервис изменений по ЛС</param>
        public SetBalanceAccountOperation(IPersonalAccountChangeService accountChangeService)
        {
            this.accountChangeService = accountChangeService;
        }

        /// <summary>
        /// Код операции
        /// </summary>
        public static string Key => "SetBalanceAccountOperation";

        /// <summary>
        /// Код операции
        /// </summary>
        public override string Code => SetBalanceAccountOperation.Key;

        /// <summary>
        /// Наименование операции
        /// </summary>
        public override string Name => "Изменение основной задолженности";

        /// <summary>
        /// Метод выполнения операции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            return this.accountChangeService.ChangePeriodBalance(baseParams);
        }
    }
}