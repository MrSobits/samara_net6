namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    /// <summary>
    /// Операция "Перераспределение оплаты"
    /// </summary>
    public class RepaymentOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Ключ регистрации
        /// </summary>
        public static string Key => nameof(RepaymentOperation);

        /// <inheritdoc />
        public override string Code => RepaymentOperation.Key;

        /// <inheritdoc />
        public override string Name => "Перераспределение оплаты";

        /// <inheritdoc />
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.RepaymentOperation";


        /// <summary>
        /// Интерфейс сервиса перераспределения оплат
        /// </summary>
        public IPersonalAccountRepaymentService PersonalAccountRepaymentService { get; set; }

        /// <inheritdoc />
        public override IDataResult Execute(BaseParams baseParams)
        {
            return this.PersonalAccountRepaymentService.Execute(baseParams);
        }

        /// <inheritdoc />
        public override IDataResult GetDataForUI(BaseParams baseParams)
        {
            return this.PersonalAccountRepaymentService.GetDataForUI(baseParams);
        }
    }
}