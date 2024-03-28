namespace Bars.GkhRf.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhRf.Entities;

    public class GisuRequestTransferForm : GkhBaseReport
    {
        private long GisuRequestTransferId { get; set; }

        public GisuRequestTransferForm()
            : base(new ReportTemplateBinary(Properties.Resources.RequestTransferForm))
        {
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "GisuRequestTransferForm"; }
        }

        public override string CodeForm
        {
            get { return "GisuRequestTransferForm"; }
        }

        public override string Name
        {
            get { return "Заявка в ГИСу"; }
        }

        public override string Description
        {
            get { return "Заявка в ГИСу. Получение денежных средств"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            GisuRequestTransferId = userParamsValues.GetValue<long>("gisuRequestTransferId");
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Name = "GisuRequestTransferForm",
                                   Code = "RequestTransferForm",
                                   Description = string.Empty,
                                   Template = Properties.Resources.RequestTransferForm
                               }
                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var definition = Container.Resolve<IDomainService<RequestTransferRf>>().Load(GisuRequestTransferId);

            if (definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            reportParams.SimpleReportParams["НомерЗаявки"] = definition.DocumentNum;
            if (definition.DateFrom.HasValue)
            {
                reportParams.SimpleReportParams["ДатаЗаявки"] = definition.DateFrom.Value.ToLongDateString();
            }

            if (!string.IsNullOrEmpty(definition.ManagingOrganization.Contragent.Municipality.Name))
            {
                reportParams.SimpleReportParams["Район"] = definition.ManagingOrganization.Contragent.Municipality.Name;
            }

            if (!string.IsNullOrEmpty(definition.ManagingOrganization.Contragent.FiasJuridicalAddress.PlaceName))
            {
                reportParams.SimpleReportParams["НаселенныйПункт"] =
                    definition.ManagingOrganization.Contragent.FiasJuridicalAddress.PlaceName;
            }

            if (!string.IsNullOrEmpty(definition.ManagingOrganization.Contragent.Name))
            {
                reportParams.SimpleReportParams["НаименованиеУК"] = definition.ManagingOrganization.Contragent.Name;
            }

            if (!string.IsNullOrEmpty(definition.ManagingOrganization.Contragent.Inn))
            {
                reportParams.SimpleReportParams["ИНН"] = definition.ManagingOrganization.Contragent.Inn;
            }

            if (!string.IsNullOrEmpty(definition.ManagingOrganization.Contragent.Kpp))
            {
                reportParams.SimpleReportParams["КПП"] = definition.ManagingOrganization.Contragent.Kpp;
            }

            reportParams.SimpleReportParams["НомерДог"] = definition.ContractRf.DocumentNum;
            if (definition.ContractRf.DocumentDate.HasValue)
            {
                reportParams.SimpleReportParams["ДатаДог"] = definition.ContractRf.DocumentDate.Value.ToLongDateString();
            }

            if (!string.IsNullOrEmpty(definition.ManagingOrganization.Contragent.Phone))
            {
                reportParams.SimpleReportParams["ТелефонУК"] = definition.ManagingOrganization.Contragent.Phone;
            }

            reportParams.SimpleReportParams["Программа"] = definition.ProgramCr.Name;
            reportParams.SimpleReportParams["ТипПрограммы"] = definition.TypeProgramRequest.GetEnumMeta().Display;

            var funds =
                Container.Resolve<IDomainService<TransferFundsRf>>()
                         .GetAll()
                         .Where(x => x.RequestTransferRf.Id == GisuRequestTransferId);
           
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция1");

            var strCount = 0;
            foreach (var fund in funds)
            {
                strCount++;
                section.ДобавитьСтроку();

                section["НомерПП"] = strCount;
                section["Адрес"] = fund.RealityObject.Address;
                section["КодМЖФ"] = fund.RealityObject.GkhCode;
                section["Назначение"] = fund.PayAllocate;
                section["ВидРабот"] = fund.WorkKind;
                section["ЛицевойСчет"] = fund.PersonalAccount;
                section["РасчСч"] = definition.ContragentBank.SettlementAccount;
                section["Банк"] = definition.ContragentBank.Name;
                section["КорСчет"] = definition.ContragentBank.CorrAccount;
                section["БИК"] = definition.ContragentBank.Bik;
                section["Сумма"] = fund.Sum.GetValueOrDefault(0);
            }

            var sum = funds.Sum(x => x.Sum);
            reportParams.SimpleReportParams["Итого"] = sum;
            reportParams.SimpleReportParams["ИтогоПрописью"] = sum;

            var positions =
                Container.Resolve<IDomainService<ContragentContact>>()
                         .GetAll()
                         .OrderBy(x => x.Position.Code)
                         .Where(
                             x =>
                             x.Contragent.Id == definition.ContractRf.ManagingOrganization.Contragent.Id
                             && (x.DateEndWork == null || x.DateEndWork > definition.DateFrom)
                             && x.DateStartWork < definition.DateFrom)
                         .Select(x => new { x.Id, x.Surname, x.Name, x.Patronymic, x.Position });

            if (positions.Any())
            {
                var headPosition = positions.FirstOrDefault(x => x.Position.Code == "1" || x.Position.Code == "4");
                if (headPosition != null)
                {
                    reportParams.SimpleReportParams["РуководительФИО"] = string.Format("{0} {1} {2}", headPosition.Surname, headPosition.Name, headPosition.Patronymic);
                }

                var bookerPosition = positions.FirstOrDefault(x => x.Position.Code == "2");
                if (bookerPosition != null)
                {
                    reportParams.SimpleReportParams["Гл.бух.ФИО"] = string.Format("{0} {1} {2}", bookerPosition.Surname, bookerPosition.Name, bookerPosition.Patronymic);
                }
            }

            reportParams.SimpleReportParams["Исполнитель"] = definition.Perfomer;
        }
    }
}