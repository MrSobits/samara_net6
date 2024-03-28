namespace Bars.Gkh.Overhaul.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Регистрация изменений полей сущности <see cref="ContragentBankCreditOrg"/>
    /// </summary>
    /// <remarks>
    /// Этот файл сгенерирован автоматичеески
    /// 21.08.2017 10:37:48
    /// УКАЖИТЕ ПОЛЯ ОТОБРАЖЕНИЯ ДЛЯ ССЫЛОЧНЫХ ТИПОВ
    /// </remarks>
    public class ContragentBankCreditOrgLogMap : AuditLogMap<ContragentBankCreditOrg>
    {
        public ContragentBankCreditOrgLogMap()
        {
            this.Name("Расчетные счета");
            this.Description(x => x.Name);

            // CreditOrg
            this.MapProperty(x => x.CreditOrg, "CreditOrg", "Кредитная организация", x => x?.Name);

            // string
            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.Bik, "Bik", "БИК");
            this.MapProperty(x => x.Okonh, "Okonh", "ОКОНХ");
            this.MapProperty(x => x.Okpo, "Okpo", "ОКПО");
            this.MapProperty(x => x.CorrAccount, "CorrAccount", "Корреспондентский счет");
            this.MapProperty(x => x.SettlementAccount, "SettlementAccount", "Расчетный счет");
            this.MapProperty(x => x.Description, "Description", "Описание");

            this.MapProperty(x => x.File, "File", "Справка из банка", x => x?.FullName);
        }
    }
}