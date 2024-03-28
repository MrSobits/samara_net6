namespace Bars.Gkh.Report
{
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Entities;
    using Properties;

    using Castle.Windsor;
    using System;

    /// <summary>
    /// Отчет для Экспорт лицензии
    /// </summary>
    public class ExportLicenseGISReport : BasePrintForm
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private DateTime dateFrom;
        private DateTime dateTo;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ExportLicenseGISReport()
            : base(new ReportTemplateBinary(Resources.ExportLicenseGIS))
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "ExportLicenseGIS"; }
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateFrom = baseParams.Params["dateFrom"].ToDateTime();
            this.dateTo = baseParams.Params["dateTo"].ToDateTime();
        }

        /// <summary>
        /// Генератор отчета
        /// </summary>
        public override string ReportGenerator { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Наименование группы
        /// </summary>
        public override string GroupName
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Клиентский контроллер
        /// </summary>
        public override string ParamsController
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Ограничение прав доступа
        /// </summary>
        public override string RequiredPermission
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Подготовить отчет
        /// </summary>
        public override void PrepareReport(ReportParams reportParams)
        {
            if (dateFrom > DateTime.MinValue && dateTo > DateTime.MinValue)
            {
                var manOrgLicense = this.Container.Resolve<IDomainService<ManOrgLicense>>().GetAll()
                .Where(x => x.DateIssued.HasValue && x.DateIssued >= this.dateFrom && x.DateIssued <= this.dateTo)
                .Select(x => new
                {
                    x.Id,
                    x.Contragent.Ogrn,
                    x.LicNumber,
                    DateIssued = x.DateIssued.Value.ToString("dd.MM.yyyy"),
                    x.DisposalNumber,
                    DateDisposal = x.DateDisposal.Value.ToString("dd.MM.yyyy"),
                    x.Contragent.FiasFactAddress.HouseGuid
                    });

                if (manOrgLicense == null)
                {
                    return;
                }

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
                foreach (var item in manOrgLicense)
                {
                    section.ДобавитьСтроку();

                    section["OGRN"] = item.Ogrn;
                    section["LicNumber"] = item.LicNumber;
                    section["LicDate"] = item.DateIssued;
                    section["Order"] = item.DisposalNumber;
                    section["OrderDate"] = item.DateDisposal;
                    section["Fias"] = item.HouseGuid;

                }
            }

        }
    }
}