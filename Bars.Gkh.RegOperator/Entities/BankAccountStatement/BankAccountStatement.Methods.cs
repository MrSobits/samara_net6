namespace Bars.Gkh.RegOperator.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Exceptions;
    using ValueObjects;

    public partial class BankAccountStatement
    {
        /// <summary>
        /// Получить операции
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<MoneyOperation> GetOperations()
        {
            return this.Operations.Where(x => !x.IsCancelled && x.CanceledOperation == null);
        }

        /// <summary>
        /// Отменить текущую активную операцию
        /// </summary>
        /// <returns></returns>
        public virtual MoneyOperation CancelOperation(MoneyOperation operation, ChargePeriod period)
        {
            if (operation == null)
            {
                throw new MoneyOperationException("Opened operation does not exists.");
            }

            var op = operation.Cancel(period);
            this.operations.Add(op);

            return op;
        }

        /// <summary>
        /// Создать операцию
        /// </summary>
        /// <returns></returns>
        public virtual MoneyOperation CreateOperation(DynamicDictionary dictionary, ChargePeriod period)
        {
            ArgumentChecker.NotNull(dictionary, nameof(dictionary));
            var code = dictionary.GetAs<DistributionCode>("distributionCode");
            ArgumentChecker.ValidEnumerationValue(code, "distributionCode");

            var newOper = dictionary.GetAs<MoneyOperation>("operation") 
                ?? new MoneyOperation(this.TransferGuid, period, this.File);

            this.operations.Add(newOper);

            var distrOp = new DistributionOperation
            {
                Code = code,
                Operation = newOper,
                BankAccountStatement = this
            };
            this.distributionOperations.Add(distrOp);
            return newOper;
        }

        /// <summary>
        /// Метод для создания неглубокой копии
        /// </summary>
        public virtual BankAccountStatement ShallowCopy()
        {
            return (BankAccountStatement)this.MemberwiseClone();
        }
    }
}
