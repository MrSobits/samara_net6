namespace Bars.Gkh.RegOperator.CodedReports
{
    using System;
    using System.Collections.Generic;
    using B4.Application;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Reports;
    using System.Collections.ObjectModel;
    using System.IO;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;

    using Castle.Windsor;
    using Newtonsoft.Json;

    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Отчет по лицевому счету
    /// </summary>
    public class PersonalAccountClaimworkReport : BaseCodedReport, IPersonalAccountCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Шаблон отчета
        /// </summary>
        protected override byte[] Template
        {
            get
            {
                return Properties.Resources.PersonalAccountReport;
            }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get
            {
                return "Отчет по лицевому счету (ПИР)";
            }
        }

        /// <summary>
        /// Результат печати
        /// </summary>
        public Stream ReportFileStream { get; set; }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description => "Отчет по лицевому счету (ПИР)";

        /// <summary>
        /// Id лс
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Название выходного файла
        /// </summary>
        public string OutputFileName { get; set; }

        /// <summary>
        /// Получение данных для отчета по лицевым счетам
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new Collection<IDataSource>
            {
                new CodedDataSource("Справка", new PersonalAccountClaimworkDataProvider(ApplicationContext.Current.Container)
                {
                    AccountId = this.AccountId
                })
            };
        }

        /// <summary>
        /// Генерация документа для выгрузки
        /// </summary>
        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();

            try
            {
                if (this.AccountId == 0)
                {
                    throw new Exception("Не найден лицевой счет!");
                }

                var account = personalAccountDomain.Get(this.AccountId);

                if (account == null)
                {
                    throw new Exception("Не найден лицевой счет!");
                }
                
                this.OutputFileName = string.Format("{0}.xls", account.PersonalAccountNum);

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.xls);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(personalAccountDomain);
            }
        }

        /// <summary>
        /// Генерация документа для выгрузки
        /// </summary>
        public void GenerateFromService()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var personalAccountDomain = this.Container.ResolveDomain<BasePersonalAccount>();

            try
            {
                if (this.AccountId == 0)
                {
                    throw new Exception("Не найден лицевой счет!");
                }

                var account = personalAccountDomain.Get(this.AccountId);

                if (account == null)
                {
                    throw new Exception("Не найден лицевой счет!");
                }

                this.OutputFileName = string.Format("{0}.xls", account.PersonalAccountNum);

                this.ReportFileStream = generator.GenerateReport(this, null, ReportPrintFormat.xls);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(personalAccountDomain);
            }
            this.Generate();
        }
    }
}