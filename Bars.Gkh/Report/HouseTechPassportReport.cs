namespace Bars.Gkh.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using Bars.Gkh.Report.TechPassportSections;
    using Castle.Windsor;

    public class HouseTechPassportReport : BasePrintForm
    {
        private long realtyObjectId;
        public IWindsorContainer Container { get; set; }

        public HouseTechPassportReport()
            : base(new ReportTemplateBinary(Resources.HouseTechPassportReport))
        {
        }

        //Конструктор для тех кто наследуется от этого класса в другом регионе
        public HouseTechPassportReport(IReportTemplate reportTemplate)
            : base(reportTemplate)
        {
        }

        public override string Name
        {
            get
            {
                return "Отчет по техпаспорту";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по техпаспорту";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Техпаспорт";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.HouseTechPassportReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhTp.HouseTechPassportReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.realtyObjectId = baseParams.Params["house"].ToLong();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            // получаем провайдер реализующий необходимый паспорт
            var passport = Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт");
            if (passport == null)
            {
               throw new Exception("Не найден провайдер технического паспорта");
            }
            
            // получаем все значения техпаспорта для объекта недвижимости
            var techPassportValues = Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                .Where(x => x.TehPassport.RealityObject.Id == realtyObjectId)
                .Select(x => new
                {
                    x.FormCode,
                    x.CellCode,
                    x.Value
                })
                .AsEnumerable()
                .GroupBy(x => x.FormCode)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.CellCode, y => passport.GetTextForCellValue(x.Key, y.CellCode, y.Value)));

            var sections = Container.ResolveAll<ITechPassportSectionReport>();

            foreach (var techPassportSection in sections)
            {
                techPassportSection.PrepareSection(passport, reportParams, realtyObjectId, techPassportValues);
            }
        }
    }
}