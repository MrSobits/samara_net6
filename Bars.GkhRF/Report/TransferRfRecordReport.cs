namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhRf.DomainService;
    using Bars.GkhRf.Entities;

    public class TransferRfRecordReport : GkhBaseReport
    {
        /// <summary>
        /// ИД записи перечисления средств рег. фонда.
        /// </summary>
        private long TransferRfRecordId { get; set; }

        /// <summary>
        /// Интерфейс для доступа к методу получения сумм столбца "Оплачено"
        /// </summary>
        public ITransferObjectDataService TransferObjectDataService { get; set; }

        public TransferRfRecordReport() : base(new ReportTemplateBinary(Properties.Resources.GisuTransferRecordForm))
        {
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "TransferRfRecord"; }
        }

        public override string CodeForm
        {
            get { return "TransferRfRecord"; }
        }

        public override string Name
        {
            get { return "Заявка в ГИСу"; }
        }

        public override string Description
        {
            get { return "Заявка в ГИСу"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            TransferRfRecordId = userParamsValues.GetValue<object>("transferRfRecordId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Name = "RequestTransferRf",
                                   Code = "GisuTransferRecordForm",
                                   Description = string.Empty,
                                   Template = Properties.Resources.GisuTransferRecordForm
                               }
                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var transferRfRecordDomain = this.Container.Resolve<IDomainService<TransferRfRecord>>();

            var definition = transferRfRecordDomain.Load(TransferRfRecordId);

            if (definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            reportParams.SimpleReportParams["Номер"] = definition.DocumentNum;
            reportParams.SimpleReportParams["Дата"] = definition.DateFrom.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["АдресУК"] =
                definition.TransferRf.ContractRf.ManagingOrganization.Contragent.FactAddress;
            reportParams.SimpleReportParams["НаименованиеУК"] =
                definition.TransferRf.ContractRf.ManagingOrganization.Contragent.Name;
            reportParams.SimpleReportParams["ТелефонУК"] =
                definition.TransferRf.ContractRf.ManagingOrganization.Contragent.Phone;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция1");

            var transferRfRecObjDomain = this.Container.Resolve<IDomainService<TransferRfRecObj>>();

            var transferRfRecObjs = transferRfRecObjDomain.GetAll()
                .Where(x => x.TransferRfRecord.Id == this.TransferRfRecordId)
                .Where(x => x.RealityObject != null)
                .OrderBy(x => x.RealityObject.Address);

            var chargeAccountPaids = new Dictionary<long, decimal>();

            if (this.TransferObjectDataService != null)
            {
                var transferDate = definition.TransferDate.HasValue ? definition.TransferDate.Value : DateTime.Now;

                chargeAccountPaids = this.TransferObjectDataService.GetPaids(
                    transferDate,
                    transferRfRecObjs.Select(x => x.RealityObject.Id));
            }

            var strCount = 0;
            var totalSum = 0m;

            foreach (var transferRfRecObj in transferRfRecObjs.ToList())
            {
                strCount++;
                section.ДобавитьСтроку();

                section["НомерПп"] = strCount;
                section["КодМЖФ"] = transferRfRecObj.RealityObject.GkhCode;
                section["Адрес"] = transferRfRecObj.RealityObject.Address;

                if (chargeAccountPaids.ContainsKey(transferRfRecObj.RealityObject.Id))
                {
                    section["Сумма"] = chargeAccountPaids[transferRfRecObj.RealityObject.Id];
                    totalSum += chargeAccountPaids[transferRfRecObj.RealityObject.Id];
                }
                else
                {
                    section["Сумма"] = 0m;
                }
            }

            reportParams.SimpleReportParams["ОбщаяСумма"] = totalSum;
            reportParams.SimpleReportParams["СуммаПрописью"] = totalSum;

            var positions = Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                .Where(x => x.Contragent.Id == definition.TransferRf.ContractRf.ManagingOrganization.Contragent.Id && 
                    x.DateStartWork < definition.TransferDate && 
                    (x.DateEndWork == null || x.DateEndWork > definition.TransferDate))
                .Select(x => new
                                {
                                    x.Id,
                                    x.Surname,
                                    x.Name, 
                                    x.Patronymic,
                                    x.Position
                                });

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

            this.Container.Release(transferRfRecordDomain);
            this.Container.Release(transferRfRecObjDomain);
        }
    }
}