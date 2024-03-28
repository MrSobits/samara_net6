namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    /// <summary>
    /// Операция "Разделение лицевого счета"
    /// </summary>
    public class PersonalAccountSplitOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Код регистрации
        /// </summary>
        public static string Key => nameof(PersonalAccountSplitOperation);

        /// <inheritdoc />
        public override string Code => PersonalAccountSplitOperation.Key;

        /// <inheritdoc />
        public override string Name => "Разделение лицевого счета";

        /// <inheritdoc />
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.PersonalAccountSplitOperation";

        /// <summary>
        /// Интефейс сервиса разедения ЛС
        /// </summary>
        public IPersonalAccountSplitService PersonalAccountSplitService { get; set; }

        /// <inheritdoc />
        public override IDataResult Execute(BaseParams baseParams)
        {
            return this.PersonalAccountSplitService.ApplyDistribution(baseParams);
        }

        /// <inheritdoc />
        public override IDataResult GetDataForUI(BaseParams baseParams)
        {
            var operation = baseParams.Params.GetAs<string>("operationType");

            switch (operation)
            {
                case "GetDistributableDebts":
                    return this.PersonalAccountSplitService.GetDistributableDebts(baseParams);

                case "DistributeDebts":
                    return this.PersonalAccountSplitService.DistributeDebts(baseParams);
            }

            return BaseDataResult.Error("Missing parameter operationType");
        }
    }
}