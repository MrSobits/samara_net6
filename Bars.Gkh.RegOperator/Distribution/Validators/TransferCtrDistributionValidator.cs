namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Абстрактная проверка на перечисление средств подрядчикам
    /// </summary>
    public abstract class TransferCtrDistributionValidator : IDistributionValidator
    {
        /// <summary>
        /// Код распределения
        /// </summary>
        public abstract string Code { get; }

        /// <inheritdoc />
        public virtual bool IsMandatory => true;

        public bool IsApply => false;

        /// <summary>
        /// Домен-сервис <see cref="TransferCtrPaymentDetail"/>
        /// </summary>
        public IDomainService<TransferCtrPaymentDetail> TransferCtrPaymentDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="TransferCtr"/>
        /// </summary>
        public IDomainService<TransferCtr> TransferCtrDomain { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод выполняет проверку
        /// </summary>
        public virtual IDataResult Validate(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            var transferCtrs = this.TransferCtrDomain.GetAll().Where(x => ids.Contains(x.Id));

            var paymentSumDict = this.TransferCtrPaymentDetailDomain.GetAll()
                .Where(x => transferCtrs.Any(y => y.Id == x.TransferCtr.Id))
                .Select(
                    x => new
                    {
                        x.TransferCtr.Id,
                        x.Amount
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Amount));

            var listError = new List<TransferCtr>();
            foreach (var transferCtr in transferCtrs.ToArray())
            {
                if (transferCtr.Sum != paymentSumDict.Get(transferCtr.Id))
                {
                    listError.Add(transferCtr);
                }
            }

            const string message = "По указанным заявкам распределение невозможно, т.к. не указаны источники оплат: ";

            if (listError.IsEmpty())
            {
                return new BaseDataResult();
            }

            return BaseDataResult.Error(message + listError.Select(x => $"{x.DocumentNum} от {x.DateFrom:dd.MM.yyyy}").AggregateWithSeparator("; "));
        }
    }
}