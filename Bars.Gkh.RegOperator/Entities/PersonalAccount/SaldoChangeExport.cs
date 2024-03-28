namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Сущность выгруженного файла для установки/изменения сальдо
    /// </summary>
    public class SaldoChangeExport : BaseImportableEntity, ISaldoChangeCreator
    {
        private IList<AccountExcelSaldoChange> accounts;

        /// <summary>
        /// NH
        /// </summary>
        public SaldoChangeExport()
        {
            this.FileName = SaldoChangeExport.GenerateFileName();
            this.accounts = new List<AccountExcelSaldoChange>();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public SaldoChangeExport(ChargePeriod period) : this()
        {
            this.Period = period;
        }

        /// <summary>
        /// Имя выгруженного ранее файла
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Период выгрузки
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Производился ли импорт указанного файла
        /// </summary>
        public virtual bool Imported { get; protected set; }

        /// <summary>
        /// Лицевые счета, связанные с текущим экспортом
        /// </summary>
        public virtual IEnumerable<AccountExcelSaldoChange> Accounts => this.accounts;

        /// <summary>
        /// Добавить лицевой счет
        /// </summary>
        public virtual void AddPersonalAccount(BasePersonalAccount account, decimal saldoByBase, decimal saldoByDecision, decimal saldoByPenalty)
        {
            this.accounts.Add(new AccountExcelSaldoChange
            {
                SaldoChangeExport = this,
                PersonalAccount = account,
                SaldoByBaseTariffBefore = saldoByBase,
                SaldoByDecisinTariffBefore = saldoByDecision,
                SaldoByPenaltyBefore = saldoByPenalty
            });
        }

        /// <summary>
        /// Метод проставляет статус импортированности файла
        /// </summary>
        public virtual void SetImported()
        {
            // чистим все импортированные данные, т.к. они создадутся в детализации
            this.accounts.Clear();
            this.Imported = true;
        }

        /// <summary>
        /// Метод инициализирует сущность-инициатор изменение сальдо
        /// </summary>
        public virtual SaldoChangeSource CreateSaldoChangeOperation(string userName)
        {
            return new SaldoChangeSource(TypeChargeSource.ImportSaldoChangeMass, this.Period)
            {
                SaldoChangeExport = this,
                User = userName,
                OperationDate = DateTime.Now
            };
        }

        private static string GenerateFileName() => $"{DateTime.Now:yyyyMMddHHmmss}";
    }
}