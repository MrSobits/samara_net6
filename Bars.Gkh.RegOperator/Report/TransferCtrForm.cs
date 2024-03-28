namespace Bars.Gkh.RegOperator.Report
{
    using System.Collections.Generic;
    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Report;

    /// <summary>
    /// Печатная форма Заявки на перечисление средств подрядчикам
    /// </summary>
    class TransferCtrForm : GkhBaseReport
    {
        private long transferCtrId { get; set; }

        /// <summary>
        /// Домен-сервис Заявки на перечисление средств подрядчикам
        /// </summary>
        public IDomainService<TransferCtr> domainService { get; set; }

        /// <summary>
        /// Конструктор Печатной формы
        /// </summary>
        public TransferCtrForm()   
            : base(new ReportTemplateBinary(Properties.Resources.TransferCtr))
        {
        }

        /// <summary>
        /// Подготовка данных для Заявки на перечисление средств подрядчикам
        /// </summary>
        /// <param name="reportParams">параметры печати</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var obj = Container.Resolve<IDomainService<TransferCtr>>().Load(transferCtrId);

            if (obj == null)
            {
                throw new ReportProviderException("Не удалось получить заявку");
            }

            reportParams.SimpleReportParams["НомерЗаявки"] = obj.DocumentNum;
            reportParams.SimpleReportParams["ДатаЗаявки"] = obj.DateFrom.HasValue ? obj.DateFrom.Value.ToShortDateString() : string.Empty;
            reportParams.SimpleReportParams["Район"] = obj.ObjectCr.RealityObject.Municipality.Name;
            reportParams.SimpleReportParams["НаселенныйПункт"] = obj.ObjectCr.RealityObject.FiasAddress.PlaceAddressName;
            reportParams.SimpleReportParams["НаименованиеПодрядчика"] = obj.Builder.Contragent.Name;
            reportParams.SimpleReportParams["ИНН"] = obj.Builder.Contragent.Inn;
            reportParams.SimpleReportParams["КПП"] = obj.Builder.Contragent.Kpp;
            reportParams.SimpleReportParams["НомерДог"] = obj.Contract.DocumentNum;
            reportParams.SimpleReportParams["ДатаДог"] = obj.Contract.DocumentDateFrom.HasValue ? obj.Contract.DocumentDateFrom.Value.ToShortDateString() : string.Empty;
            reportParams.SimpleReportParams["ТелефонПодрядчика"] = obj.Builder.Contragent.Phone;
            reportParams.SimpleReportParams["Программа"] = obj.ProgramCr.Name;
            reportParams.SimpleReportParams["ТипПрограммы"] = obj.TypeProgramRequest.GetEnumMeta().Display;
            reportParams.SimpleReportParams["Итого"] = obj.BudgetMu + obj.BudgetSubject + obj.FundResource + obj.OwnerResource;
            reportParams.SimpleReportParams["ИтогоПрописью"] = obj.BudgetMu + obj.BudgetSubject + obj.FundResource + obj.OwnerResource;
            reportParams.SimpleReportParams["Исполнитель"] = obj.Perfomer;

            var fundDict = new Dictionary<string, decimal?>
            {
                {"Средства собственника", obj.OwnerResource},
                {"Средства фонда", obj.FundResource},
                {"Бюджет МО", obj.BudgetMu},
                {"Бюджет субъекта", obj.BudgetSubject}
            };

            var sectionRow = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow");

            foreach (var fund in fundDict)
            {
                sectionRow.ДобавитьСтроку();

                sectionRow["Адрес"] = obj.ObjectCr.RealityObject.Address;
                sectionRow["КодМЖФ"] = obj.ObjectCr.RealityObject.GkhCode;
                sectionRow["ТипПлатежа"] = obj.PaymentType.GetEnumMeta().Display;
                sectionRow["РасчСч"] = obj.ContragentBank.SettlementAccount;
                sectionRow["Банк"] = obj.ContragentBank.Name;
                sectionRow["КорСчет"] = obj.ContragentBank.CorrAccount;
                sectionRow["БИК"] = obj.ContragentBank.Bik;
                sectionRow["ИсточникФинансирования"] = fund.Key;
                sectionRow["Сумма"] = fund.Value;
            }

        }

        /// <summary>
        /// Установка пользовательских параметров Идентификатора заявки
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            transferCtrId = userParamsValues.GetValue<long>("transferCtrId");
        }

        /// <summary>
        /// Получение описания шаблона
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Name = "TransferCtrForm",
                                   Code = "TransferCtrForm",
                                   Description = string.Empty,
                                   Template = Properties.Resources.TransferCtr
                               }
                       };
        }

        /// <summary>
        /// Наименование печатной формы
        /// </summary>
        public override string Name
        {
            get { return "Перечисление срредств подрядчику"; }
        }

        /// <summary>
        /// Код печатной формы
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор печатной формы
        /// </summary>
        public override string Id
        {
            get { return "TransferCtrForm"; }
        }

        /// <summary>
        /// Код шаблона печатной формы
        /// </summary>
        public override string CodeForm
        {
            get { return "TransferCtrForm"; }
        }

        /// <summary>
        /// Описание печатной формы
        /// </summary>
        public override string Description
        {
            get { return "Перечисление срредств подрядчику"; }
        }
    }
}
