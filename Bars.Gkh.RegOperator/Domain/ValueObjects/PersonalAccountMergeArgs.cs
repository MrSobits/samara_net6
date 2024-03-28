namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.RegOperator.Domain.Repository.PersonalAccounts;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Exceptions;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Аргументы-параметры слияния аккаунтов
    /// </summary>
    public class PersonalAccountMergeArgs
    {
        private readonly MergeItem[] mergeItems;
        private readonly MergeItem[] closingItems;
        private readonly MergeItem[] recipientItems;
        private readonly Dictionary<MergeItem, decimal> mergeKoeffs;

        /// <summary>
        /// Информация для истории изменений ЛС.
        /// </summary>
        public PersonalAccountChangeInfo ChangeInfo { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="mergeInfos">Перечисление информации о слиянии счетов</param>
        public PersonalAccountMergeArgs(IEnumerable<MergeItem> mergeInfos)
        {
            this.mergeItems = mergeInfos.ToArray();
            this.closingItems = this.mergeItems.Where(x => x.NewShare == 0).ToArray();
            this.recipientItems = this.mergeItems.Where(x => x.NewShare > 0).ToArray();

            Validate();

            this.mergeKoeffs = new Dictionary<MergeItem, decimal>();
            foreach (var item in this.recipientItems)
            {
                var koeff = (item.NewShare - item.PersonalAccount.AreaShare) / this.closingItems.Sum(x => x.PersonalAccount.AreaShare);
                this.mergeKoeffs.Add(item, koeff);
            }

            // Если какая-то часть доли собственности потерялась при расчетах
            if (!this.closingItems.Sum(x => x.PersonalAccount.AreaShare)
                    .Equals(this.recipientItems.Sum(x => this.mergeKoeffs[x] * (x.NewShare - x.PersonalAccount.AreaShare))))
            {
                var closingAreaShare = closingItems.Sum(x => x.PersonalAccount.AreaShare);
                var accuracy = closingAreaShare -
                               this.recipientItems.Sum(x => this.mergeKoeffs[x] * closingAreaShare);
                this.mergeKoeffs[this.recipientItems.First()] += accuracy;
            }
        }

        /// <summary>
        /// Лицевые счета, которые будут закрыты.
        /// </summary>
        public IEnumerable<MergeItem> ClosingItems
        {
            get { return this.closingItems; }
        }

        /// <summary>
        /// Лицевые счета, на которые перейдут долги и переплаты закрываемых счетов.
        /// </summary>
        public IEnumerable<MergeItem> RecipientItems
        {
            get { return this.recipientItems; }
        }

        /// <summary>
        /// Возврат коэффициента слияния
        /// </summary>
        /// <param name="item">Лицевой счёт слияния</param>
        /// <returns>Коэффициент</returns>
        public decimal GetMergeKoeff(MergeItem item)
        {
            return this.mergeKoeffs.Get(item);
        }

        private void Validate()
        {
            if (this.mergeItems.Count() < 2)
            {
                throw new PersonalAccountOperationException(
                    "В слиянии должны участвовать не меньше двух счетов.");
            }

            if (!this.recipientItems.Any())
            {
                throw new PersonalAccountOperationException("При слиянии должен остаться хотя бы один лицевой счет.");
            }

            if (!this.closingItems.Any())
            {
                throw new PersonalAccountOperationException(
                    "При слиянии лицевых счетов, должен быть лицевой счет с нулевой долей собственности.");
            }

            if (this.mergeItems.Sum(x => x.PersonalAccount.AreaShare) == 0)
            {
                var persAccs = this.mergeItems
                    .Select(x => x.PersonalAccount.PersonalAccountNum)
                    .AggregateWithSeparator(", ");

                throw new PersonalAccountOperationException(
                    string.Format("У лицевых счетов {0} доля собственности равна 0. Для них слияние невозможно", persAccs));
            }

            if (this.mergeItems.Select(x => x.PersonalAccount.Room).Distinct().Count() != 1)
            {
                throw new PersonalAccountOperationException(
                    "Слияние может проводится для лицевых счетов только одного помещения.");
            }

            if (this.recipientItems.Any(x => x.NewShare < x.PersonalAccount.AreaShare))
            {
                throw new PersonalAccountOperationException(
                    "Доля собственности не может уменьшится");
            }

            //Сумма долей не должна быть больше 1
            var mergeSum = this.mergeItems.Sum(x => x.PersonalAccount.AreaShare);
            mergeSum = mergeSum > 1 ? 1 : mergeSum;

            if (Math.Abs(this.mergeItems.Sum(x => x.NewShare) - mergeSum) > 0.001M)
            {
                throw new PersonalAccountOperationException(
                    "При слиянии суммарная доля собственности не должна меняться.");
            }
        }

        /// <summary>
        /// Генерация <see cref="PersonalAccountMergeArgs"/> из базовых параметров
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Экземпляр <see cref="PersonalAccountMergeArgs"/></returns>
        public static PersonalAccountMergeArgs FromParams(BaseParams baseParams)
        {
            var proxies = baseParams.Params.GetAs<MergeItemProxy[]>("MergeInfos", ignoreCase: true);
            var accIds = proxies.Select(x => x.BasePersonalAccountId).ToArray();
            var accountRepo = ApplicationContext.Current.Container.Resolve<IPersonalAccountRepository>();
            using (ApplicationContext.Current.Container.Using(accountRepo))
            {
                var accounts = accountRepo.GetByIds(accIds).ToList();
                if (accIds.Length != accounts.Count)
                {
                    throw new PersonalAccountOperationException("Один из счетов, участвующих в слиянии не найден.");
                }

                var args =
                    new PersonalAccountMergeArgs(
                        proxies.Select(
                            x => new MergeItem(accounts.Single(y => y.Id == x.BasePersonalAccountId), x.NewShare)))
                    {
                        ChangeInfo = PersonalAccountChangeInfo.FromParams(baseParams)
                    };

                return args;
            }
        }

        /// <summary>
        /// Лицевой счёт при слиянии
        /// </summary>
        public sealed class MergeItem
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="account">Лицевой счёт</param>
            /// <param name="newShare">Новая доля собственности</param>
            public MergeItem(BasePersonalAccount account, decimal newShare)
            {
                ArgumentChecker.NotNull(account, "account");
                if (newShare < 0)
                {
                    throw new ArgumentException(@"Доля собственности должна быть неотрицательной.", "newShare");
                }
                this.NewShare = newShare;
                this.PersonalAccount = account;
            }

            /// <summary>
            /// Новая доля собственности
            /// </summary>
            public decimal NewShare { get; private set; }

            /// <summary>
            /// Лицевой счет
            /// </summary>
            public BasePersonalAccount PersonalAccount { get; private set; }
        }

        private class MergeItemProxy
        {
            public long BasePersonalAccountId { get; set; }

            public decimal NewShare { get; set; }
        }
    }
}
