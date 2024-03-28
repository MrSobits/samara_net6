namespace Bars.Gkh.RegOperator.Distribution.Validators
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Distribution.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Валидатор Распределения по спец. счетам домов
    /// </summary>
    public class SpecialAccountDistributionValidator : IDistributionValidator
    {
        /// <summary>
        /// Домен-сервис <see cref="CalcAccountRealityObject"/>
        /// </summary>
        public IDomainService<CalcAccountRealityObject> CalcAccountRealityObjectDomain { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Code => nameof(SpecialAccountDistribution);

        /// <inheritdoc />
        public bool IsMandatory => true;

        public bool IsApply => false;

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams"></param>
        public IDataResult Validate(BaseParams baseParams)
        {
            var recipientAccountNumbers = DistributionHelper.ExtractManyDistributables(baseParams)
                .Cast<BankAccountStatement>()
                .Select(x => x.RecipientAccountNum)
                .Distinct()
                .ToList();

            var realSpecAccountCount = this.CalcAccountRealityObjectDomain.GetAll()
                .Where(x => recipientAccountNumbers.Contains(x.Account.AccountNumber) && x.Account.TypeAccount == TypeCalcAccount.Special && !x.Account.DateClose.HasValue)
                .Select(x => x.Account.Id)
                .Distinct()
                .Count();

            if (realSpecAccountCount != recipientAccountNumbers.Count)
            {
                return BaseDataResult.Error($"По {recipientAccountNumbers.Count - realSpecAccountCount} не найденным р/с распределение выполнено не будет!");
            }

            return new BaseDataResult();
        }
    }
}