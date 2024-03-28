namespace Bars.GkhGji.Regions.Tomsk.Report.ResolutionGji
{
    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    
    using System.Collections.Generic;
    using System.Linq;

    using Bars.GkhGji.Entities;

    using ResolProsDefinition = Bars.GkhGji.Regions.Tomsk.Entities.ResolPros.ResolProsDefinition;

    public class ResolutionGjiDefinitionDateTimeReport : GkhBaseReport
    {
        public ResolutionGjiDefinitionDateTimeReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ResolutionGjiDefinitionDateTimeReport_1))
        {
        }

        private long DefinitionId { get; set; }

        public override string Id
        {
            get { return "ResolutionGjiDefinitionDateTimeReport"; }
        }

        public override string CodeForm
        {
            get { return "ResolutionDefinitionReturn"; }
        }

        public override string Name
        {
            get { return "Назначение даты"; }
        }

        public override string Description
        {
            get { return "Определение о назначении даты и времени"; }
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
                                   Code = "BlockGJI_ResolutionGjiDefinitionDateTimeReport_1",
                                   Name = "ResolutionGjiDefinitionDateTimeReport",
                                   Description =
                                       "Определение о назначении даты и времени",
                                   Template = Properties.Resources.BlockGJI_ResolutionGjiDefinitionDateTimeReport_1
                               }

                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            CodeTemplate = "BlockGJI_ResolutionGjiDefinitionDateTimeReport_1";
            
            var definition = Container.Resolve<IDomainService<ResolProsDefinition>>().Get(DefinitionId);
            var definitionDate = definition.DocumentDate.ToDateTime().ToShortDateString();
            var inspectorPosition = definition.IssuedDefinition.Position;
            var inspectorFio = definition.IssuedDefinition.Fio;
            var appointedDate = definition.ExecutionDate.ToDateTime().ToShortDateString();
            var inspectorCode = definition.IssuedDefinition.Code;
            var resolProsArticleLaw = Container.Resolve<IDomainService<ResolProsArticleLaw>>().GetAll()
                .Where(x => x.ResolPros.Id == definition.ResolPros.Id)
                .Select(x => new
                {
                    ResolProsId = x.ResolPros.Id,
                    x.ArticleLaw.Name,
                    ArticleLawId = x.ArticleLaw.Id
                })
                .ToList();

            var articleLawId = resolProsArticleLaw.Select(x => x.ArticleLawId).ToList();

            reportParams.SimpleReportParams["ДатаОпределения"] = definitionDate;
            reportParams.SimpleReportParams["ДолжностьДЛ"] = inspectorPosition;
            reportParams.SimpleReportParams["ДЛВынесшееОпределение"] = inspectorFio;
            reportParams.SimpleReportParams["СтатьиЗакона"] = resolProsArticleLaw.Aggregate("", (x, y) => x + ", " + y.Name);
            
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

            var contragentId = 0L;
            if (resolProsData != null)
            {
                contragentId = resolProsData.ContragentId;
            }

            var contragentContact = Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                .Where(x => x.Contragent.Id == contragentId)
                .Select(x => new
                {
                    x.FullName,
                    x.Position.Name
                })
                .FirstOrDefault();

            var firstExecutantCodeList = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
            var secondExecutantCodeList = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
            var thirdExecutantCodeList = new List<string> { "6", "7", "14" };

            var executantCode = string.Empty;
            if (resolProsData != null)
            {
                executantCode = resolProsData.Code;
            }
            var contragentData = Container.Resolve<IDomainService<Contragent>>().GetAll()
                .Where(x => x.Id == contragentId)
                .Select(x => new
                {
                    x.Name,
                    x.Inn,
                    x.Oktmo,
                    x.JuridicalAddress,
                    x.FactAddress
                })
                .FirstOrDefault();

            if (contragentData != null)
            {
                reportParams.SimpleReportParams["ИнформацияОЮЛ"] = contragentData.Name + ", ИНН: " + contragentData.Inn + ", ОКТМО: " + contragentData.Oktmo + ", юр. адрес: "
                    + contragentData.JuridicalAddress + ", факт. адрес: " + contragentData.FactAddress;
            }

            if (resolProsData != null)
            {
                if (firstExecutantCodeList.Contains(executantCode))
                {
                    if (contragentContact != null)
                    {
                        if (contragentContact.Name == "Руководитель")
                        {
                            reportParams.SimpleReportParams["ВызовЛица"] = contragentContact.FullName;
                        }
                    }

                    reportParams.SimpleReportParams["ТипИсполнителя"] = "юридического лица " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                }
                if (secondExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = "должностного лица " + " " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                    reportParams.SimpleReportParams["ВызовЛица"] = resolProsData.PhysicalPerson;
                }
                if (thirdExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = "физического лица " + resolProsData.PhysicalPerson;
                    reportParams.SimpleReportParams["ВызовЛица"] = resolProsData.PhysicalPerson;
                }
                reportParams.SimpleReportParams["Дата"] = resolProsData.DocumentDate.ToDateTime().ToShortDateString();

            }
            
            var articleLawGjiDiscription = Container.Resolve<IDomainService<ArticleLawGji>>().GetAll()
                .Where(x => articleLawId.Contains(x.Id))
                .Select(x => new
                {
                    x.Description
                })
                .ToList();

            if (articleLawGjiDiscription.Count >= 1)
            {
                reportParams.SimpleReportParams["ОписаниеСтатьи"] = articleLawGjiDiscription.Aggregate("", (x, y) => x + (string.IsNullOrEmpty(x) ? y.Description + "/" : y.Description));
            }
            
            reportParams.SimpleReportParams["ДатаНазначения"] = appointedDate;

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

            if (resolProsData != null)
            {
                reportParams.SimpleReportParams["ДатаПостановления"] = resolProsData.DocumentDate.ToDateTime().ToShortDateString();
            }
        }
    }
}
