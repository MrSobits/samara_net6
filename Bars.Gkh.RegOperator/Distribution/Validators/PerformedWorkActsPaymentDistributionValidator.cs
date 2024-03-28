namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using System.Linq;
    using B4;

    using Bars.Gkh.RegOperator.Distribution.Impl;

    using Gkh.Domain;
    using GkhCr.Entities;
    using NHibernate.Linq;

    /// <summary>
    /// Проверка для <see cref="PerformedWorkActsDistribution"/>
    /// </summary>
    public class PerformedWorkActsPaymentDistributionValidator : IDistributionValidator
    {
        private readonly IDomainService<PerformedWorkActPayment> actPaymentDomain;

        /// <inheritdoc />
        public bool IsMandatory => true;

        public bool IsApply => false;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="actPaymentDomain">Домен-сервис <see cref="PerformedWorkActPayment"/></param>
        public PerformedWorkActsPaymentDistributionValidator(
            IDomainService<PerformedWorkActPayment> actPaymentDomain)
        {
            this.actPaymentDomain = actPaymentDomain;
        }

        /// <inheritdoc />
        public string Code => nameof(PerformedWorkActsDistribution);

        /// <inheritdoc />
        public IDataResult Validate(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
            var distr = DistributionHelper.ExtractDistributable(baseParams, 0);

            if (distr == null)
            {
                return new BaseDataResult(false, "Не удалось получить распределяемый объект");
            }

            var acts = this.actPaymentDomain.GetAll()
                .Fetch(x => x.PerformedWorkAct)
                .Where(x => ids.Contains(x.PerformedWorkAct.Id))
                .Where(x => x.Paid == 0)
                .ToList();

            if (distr.Sum != acts.Sum(x => x.Sum))
            {
                return new BaseDataResult(false, "Сумма распределения не соответствует распоряжениям об оплате.");
            }

            return new BaseDataResult();
        }
    }
}
