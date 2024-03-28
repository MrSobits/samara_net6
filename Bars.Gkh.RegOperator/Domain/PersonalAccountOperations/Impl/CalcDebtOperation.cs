namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    /// <summary>
    /// Операция - Расчет долга
    /// </summary>
    public class CalcDebtOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Ключ для регистрации в контейнере
        /// </summary>
        public static string Key => nameof(CalcDebtOperation);

        /// <summary>
        /// Код операции
        /// </summary>
        public override string Code => CalcDebtOperation.Key;

        /// <summary>
        /// Наименование операции
        /// </summary>
        public override string Name => "Расчет долга";

        /// <summary>
        /// Ключ прав доступа
        /// </summary>
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.CalcDebtOperation";

        public IPersonalAccountCalcDebtService PersonalAccountCalcDebtService { get; set; }

        /// <summary>
        /// Метод выполнения операции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            try
            {
                return this.PersonalAccountCalcDebtService.SaveDebtTransfer(baseParams);
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
            return this.PersonalAccountCalcDebtService.CalcDebtTransfer(baseParams);
        }
    }
}