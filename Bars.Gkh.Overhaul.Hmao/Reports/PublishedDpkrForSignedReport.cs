namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.IO;
    using System.Linq;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;

    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.StimulReport;
    using Castle.Windsor;

    public class PublishedDpkrForSignedReport : StimulReport, IBaseReport
    {
        public IWindsorContainer Container { get; set; }

        public string Name
        {
            get { return "Записи для подписания опубликованной программы"; }
        }
        
        public Stream GetTemplate()
        {
            return new ReportTemplateBinary(Properties.Resources.PublishedDpkrForSigned).GetTemplate();
        }
#warning Данный отчет делался для НСО поэтому для ХМАО надо переделывать, деления по МО нет
        public void PrepareReport(ReportParams reportParams)
        {

            var publishProgramService = Container.Resolve<IPublishProgramService>();

            var publishProgram = Container.Resolve<IDomainService<PublishedProgram>>().GetAll().FirstOrDefault(x => x.ProgramVersion.IsMain);
            
            this.ReportParams["ЗаголовокОтчета"] = "Опубликованная программа";

            var data = publishProgramService.GetPublishedProgramRecords(publishProgram)
                                    .Select(
                                         x =>
                                         new ReportSignedRecord
                                         {
                                             Номер = x.IndexNumber,
                                             ГодОпубликования = x.PublishedYear,
                                             Адрес = x.Address,
                                             Стоимость = x.Sum.RoundDecimal(2),
                                             ООИ = x.CommonEstateobject,
                                             ГодВводаВЭксплуатацию = x.CommissioningYear
                                         })
                                         .OrderBy(x => x.Номер)
                                         .ToArray();

            this.DataSources.Add(new MetaData
            {
                SourceName = "ЗаписиПрограммы",
                MetaType = nameof(ReportSignedRecord),
                Data = data
            });
        }

        protected class ReportSignedRecord
        {
            public int Номер { get; set; }

            public int ГодОпубликования { get; set; }

            public string Адрес { get; set; }

            public decimal Стоимость { get; set; }

            public string ООИ { get; set; }

            public int ГодВводаВЭксплуатацию { get; set; }
        }
    }
}