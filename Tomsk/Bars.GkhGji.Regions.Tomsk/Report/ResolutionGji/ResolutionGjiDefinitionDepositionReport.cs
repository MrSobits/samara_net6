namespace Bars.GkhGji.Regions.Tomsk.Report.ResolutionGji
{
    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    using ResolProsDefinition = Bars.GkhGji.Regions.Tomsk.Entities.ResolPros.ResolProsDefinition;

    public class ResolutionGjiDefinitionDepositionReport : GkhBaseReport
    {
        public ResolutionGjiDefinitionDepositionReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ResolutionGjiDefinitionDepositionReport_1))
        {
        }

        private long DefinitionId { get; set; }

        public override string Id
        {
            get { return "ResolutionGjiDefinitionDepositionReport"; }
        }

        public override string CodeForm
        {
            get { return "ResolutionDefinitionReturn"; }
        }

        public override string Name
        {
            get { return "Отложение"; }
        }

        public override string Description
        {
            get { return "Определение об отложении"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();
        }

        public override string ReportGeneratorName
        {
            get { return "DocIoGenerator"; }
        }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_ResolutionGjiDefinitionDepositionReport_1",
                                   Name = "ResolutionGjiDefinitionDepositionReport",
                                   Description =
                                       "Определение об отложении",
                                   Template = Properties.Resources.BlockGJI_ResolutionGjiDefinitionDepositionReport_1
                               }

                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            CodeTemplate = "BlockGJI_ResolutionGjiDefinitionDepositionReport_1";
            
            var definition = Container.Resolve<IDomainService<ResolProsDefinition>>().Get(DefinitionId);
            var definitionDate = definition.DocumentDate.ToDateTime().ToShortDateString();
            var inspectorPosition = definition.IssuedDefinition.Position;
            var inspectorCode = definition.IssuedDefinition.Code;
            var inspectorFio = definition.IssuedDefinition.Fio;
            var inspectorId = definition.IssuedDefinition.Id;
            var appointedDate = definition.ExecutionDate.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["ДатаОпределения"] = definitionDate;

            var inspectorsData = Container.Resolve<IDomainService<Inspector>>().GetAll()
                .Where(x => x.Code == inspectorCode)
                .Select(x => new
                {
                    x.Code
                })
                .FirstOrDefault();

            if (inspectorsData != null)
            {
                if (inspectorsData.Code == "021")
                {
                    reportParams.SimpleReportParams["Кабинет"] = "411";
                }
                if (inspectorsData.Code == "001")
                {
                    reportParams.SimpleReportParams["Кабинет"] = "410";
                }
                if (inspectorsData.Code == "019")
                {
                    reportParams.SimpleReportParams["Кабинет"] = "409";
                }
            }
            
            reportParams.SimpleReportParams["ДолжностьДЛ"] = inspectorPosition;

            reportParams.SimpleReportParams["ДЛВынесшееОпределение"] = inspectorFio;

            var resolProsArticleLaw = Container.Resolve<IDomainService<ResolProsArticleLaw>>().GetAll()
                .Where(x => x.ResolPros.Id == definition.ResolPros.Id)
                .Select(x => new
                {
                    ResolProsId = x.ResolPros.Id,
                    x.ArticleLaw.Name,
                    ArticleLawId = x.ArticleLaw.Id
                })
                .ToList();

            reportParams.SimpleReportParams["СтатьиЗакона"] = resolProsArticleLaw.AggregateWithSeparator(x => x.Name, ", ");

            var resolProsId = resolProsArticleLaw.Select(x => x.ResolProsId).FirstOrDefault();

            var resolProsData = Container.Resolve<IDomainService<ResolPros>>().GetAll()
                .Where(x => x.Id == resolProsId)
                .Select(x => new
                {
                    x.Id,
                    x.Executant.Code,
                    ContragentId = x.Contragent.Id,
                    ContragentName = x.Contragent.Name,
                    x.PhysicalPerson,
                    MunName = x.Municipality.Name,
                    x.DocumentDate
                })
                .FirstOrDefault();

            var executantCode = string.Empty;
            if (resolProsData != null)
            {
                executantCode = resolProsData.Code;
            }

            var contragentId = 0L;
            if (resolProsData != null)
            {
                contragentId = resolProsData.ContragentId;
            }

            var contragentData = Container.Resolve<IDomainService<Contragent>>().GetAll()
                .Where(x => x.Id == contragentId)
                .Select(x => new
                {
                    x.Name,
                    x.NameGenitive,
                    x.Inn,
                    x.Oktmo,
                    x.JuridicalAddress,
                    x.FactAddress
                })
                .FirstOrDefault();

            var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
            var secondExecutantCodeList = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
            var thirdExecutantCodeList = new List<string> { "6", "7", "14" };

            reportParams.SimpleReportParams["ДополнительныеДокументы"] = definition.AdditionalDocuments;

            if (contragentData != null)
            {
                reportParams.SimpleReportParams["ИнформацияОЮЛ"] = contragentData.Name + ", ИНН: " + contragentData.Inn + ", ОКТМО: " + contragentData.Oktmo + ", юр. адрес: "
                    + contragentData.JuridicalAddress + ", факт. адрес: " + contragentData.FactAddress;
            }

            if (resolProsData != null)
            {
                if (firstExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = "юридического лица " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                    if (contragentData != null)
                    {
                        var contr = contragentData.NameGenitive.IsEmpty() ? contragentData.Name : contragentData.NameGenitive;
                        reportParams.SimpleReportParams["Контрагент(РП)"] = "юридического лица - " + contr;
                    }
                    
                }
                if (secondExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = "должностного лица " + " " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                    reportParams.SimpleReportParams["Контрагент(РП)"] = "должностного лица - " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                }
                if (thirdExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = "физического лица " + resolProsData.PhysicalPerson;
                    reportParams.SimpleReportParams["Контрагент(РП)"] = "физического лица " + resolProsData.PhysicalPerson;
                }

                reportParams.SimpleReportParams["Дата"] = resolProsData.DocumentDate.ToDateTime().ToShortDateString();
                reportParams.SimpleReportParams["РайонПрокуратуры"] = resolProsData.MunName;

                reportParams.SimpleReportParams["Контрагент"] = resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
            }

            var resolProsDefinition = Container.Resolve<IDomainService<ResolProsDefinition>>().GetAll()
                .Where(x => x.ResolPros.Id == resolProsId)
                .Where(x => x.IssuedDefinition.Id == inspectorId)
                .Select(x => new
                {
                    x.AdditionalDocuments,
                    x.ResolutionInitAdminViolation,
                    x.RequestNeed
                })
                .FirstOrDefault();

            if (resolProsDefinition != null)
            {
                reportParams.SimpleReportParams["Установил"] = resolProsDefinition.ResolutionInitAdminViolation;
                reportParams.SimpleReportParams["ДополнительныеДокументы"] = resolProsDefinition.AdditionalDocuments;
                reportParams.SimpleReportParams["ЧтоНеобходимоЗапросить"] = resolProsDefinition.RequestNeed;
            }

            reportParams.SimpleReportParams["ДатаНазначения"] = appointedDate;

            if (resolProsData != null)
            {
                reportParams.SimpleReportParams["ДатаПостановления"] = resolProsData.DocumentDate.ToDateTime().ToShortDateString();
            }
        }
    }
}
