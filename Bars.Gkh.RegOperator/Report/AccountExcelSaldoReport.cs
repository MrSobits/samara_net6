namespace Bars.Gkh.RegOperator.Report
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Properties;

    /// <summary>
    /// Выгрузка Сальдо ЛС в Excel
    /// </summary>
    public class AccountExcelSaldoReport : BaseCodedReport
    {
        /// <summary>
        /// Шаблон
        /// </summary>
        protected override byte[] Template => Resources.AccountExcelSaldoReport;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Выгрузка сальдо";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => this.Name;

        private readonly IList<PersonalAccountSaldoInfo> personalAccountSaldoInfos;

        /// <summary>
        /// .ctor
        /// </summary>
        public AccountExcelSaldoReport(IList<PersonalAccountSaldoInfo> personalAccountSaldoInfos)
        {
            this.personalAccountSaldoInfos = personalAccountSaldoInfos;
        }

        /// <summary>
        /// Источники данных
        /// </summary>
        /// <returns>Список источников</returns>
        public override IEnumerable<IDataSource> GetDataSources()
        {
            var provider = new AccountExcelSaldoDataProvider(ApplicationContext.Current.Container, this.personalAccountSaldoInfos);

            return new Collection<IDataSource>
            {
                new CodedDataSource("AccountExcelSaldo", provider)
            };
        }
    }
}