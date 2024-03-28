namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
     using Bars.B4.Modules.Analytics.Reports.Enums;
    using Utils;
    using Gkh.Report;
    using System;
    using B4;
    using Bars.Gkh.Entities;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Извещение
    /// </summary>
    public class PersonOnlineTestReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public PersonOnlineTestReport()
            : base(new ReportTemplateBinary(Properties.Resources.PersonOnlineTestReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _onlineTestId;

        #endregion Private fields

        #region Protected properties

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        #endregion Protected properties

        #region Public properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Уведомление о результатах квалифиакционного экзамена"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Печатная форма уведомления о результатах квалифиакционного экзамена"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "PersonOnlineTest"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "PersonOnlineTest"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var onlineTestDomain = Container.ResolveDomain<PersonRequestToExam>();

            var data = onlineTestDomain.GetAll()
                .Where(x => x.Id == _onlineTestId).FirstOrDefault();

            try
            {
             
                this.ReportParams["Id"] = data.Id;
                this.ReportParams["ФИО"] = data.Person.FullName;               
                this.ReportParams["ДатаЭкзамена"] = $"{ data.ExamDate.Value.ToString("dd.MM.yyyy")}";
                this.ReportParams["КоличествоВопросов"] = "100";
                this.ReportParams["ПравильныхОтветов"] = data.CorrectAnswersPercent.HasValue ? Decimal.Round(data.CorrectAnswersPercent.Value, 0).ToString() : "0";
                this.ReportParams["Балл"] = "172";
                this.ReportParams["Сдал"] = GetResultKE(data.CorrectAnswersPercent);
                this.ReportParams["Процент"] = GetPercent(data.CorrectAnswersPercent).ToString();


            }
            finally
            {

            }

        }

        private bool GetResultKE(decimal? correctAnswersPercent)
        {
            if (!correctAnswersPercent.HasValue)
            {
                return false;
            }
            if (correctAnswersPercent.Value >= 172)
            {
                return true;
            }
            return false;
        }

        private decimal GetPercent(decimal? correctAnswersPercent)
        {
            if (!correctAnswersPercent.HasValue)
            {
                return 0;
            }
            return Decimal.Round(correctAnswersPercent.Value / 2);
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _onlineTestId = userParamsValues.GetValue<long>("Id");
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Уведомление о результатах квалифиакционного экзамена",
                    Description = "Уведомление о результатах квалифиакционного экзамена",
                    Code = "Person",
                    Template = Properties.Resources.PersonOnlineTestReport
                }
            };
        }

        #endregion Public methods

        private string GetHashData(string fio, string reqnum, string examdate)
        {
            var token = $"{fio}, {reqnum}, {examdate}";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }
    }
}