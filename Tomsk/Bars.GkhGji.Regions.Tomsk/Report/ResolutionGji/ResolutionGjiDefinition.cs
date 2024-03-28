namespace Bars.GkhGji.Regions.Tomsk.Report.ResolutionGji
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    using ResolProsDefinition = Bars.GkhGji.Regions.Tomsk.Entities.ResolPros.ResolProsDefinition;

    public class ResolutionGjiDefinition : GkhBaseReport
    {
        public IDomainService<ResolProsDefinition> ResolProsDefinition { get; set; }

        public IDomainService<ResolProsArticleLaw> ResolProsArtLawDomain { get; set; }

        public IDomainService<ResolPros> ResolProsDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<ResolProsDefinition> ResolProsDefinitionDomain { get; set; }

        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<ArticleLawGji> ArticleLawGjiDomain { get; set; }
        
        public ResolutionGjiDefinition()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ResolutionGjiDefinitionReturnProsecutor_1))
        {
        }

        private long DefinitionId { get; set; }

        public override string Id
        {
            get { return "DefinitionId"; }
        }

        public override string CodeForm
        {
            get { return "ResolutionDocument"; }
        }

        public override string Name
        {
            get { return "Док. постановления"; }
        }

        public override string Description
        {
            get { return "Документ постановления"; }
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
                                   Code = "BlockGJI_ResolutionGjiDefinitionReturnProsecutor_1",
                                   Name = "ResolutionGjiDefinitionReturnProsecutor",
                                   Description =
                                       "Определение о возврате в прокуратуру",
                                   Template = Properties.Resources.BlockGJI_ResolutionGjiDefinitionReturnProsecutor_1
                               },
                          new TemplateInfo
                               {
                                   Code = "BlockGJI_ResolutionGjiDefinitionDepositionReport_1",
                                   Name = "ResolutionGjiDefinitionDepositionReport",
                                   Description =
                                       "Определение об отложении",
                                   Template = Properties.Resources.BlockGJI_ResolutionGjiDefinitionDepositionReport_1
                               },
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
            CodeTemplate = "ResolutionDocument";

            var definition = ResolProsDefinition.GetAll().FirstOrDefault(x => x.Id == DefinitionId);
            var definitionDate = definition.DocumentDate.ToDateTime().ToShortDateString();

            var inspectorPosition = string.Empty;
            var inspectorAdditionalDocuments = string.Empty;
            var inspectorRequestNeed = string.Empty;
            var inspectorId = 0L;

            if (definition.IssuedDefinition != null)
            {
                inspectorPosition = definition.IssuedDefinition.Position;
                inspectorId = definition.IssuedDefinition.Id;
                inspectorAdditionalDocuments = definition.AdditionalDocuments;
                inspectorRequestNeed = definition.RequestNeed;
            }

            var resolProsArticleLaw = ResolProsArtLawDomain.GetAll()
                .Where(x => x.ResolPros.Id == definition.ResolPros.Id)
                .Select(x => new
                {
                    ResolProsId = x.ResolPros.Id,
                    x.ArticleLaw.Name,
                    ArticleLawId = x.ArticleLaw.Id,
                    x.ArticleLaw.Part,
                    x.ArticleLaw.Article
                })
                .ToList();

            reportParams.SimpleReportParams["ЧастьРаздельно"] = resolProsArticleLaw.Distinct(x => x.Part).AggregateWithSeparator(x => x.Part, ", ");
            reportParams.SimpleReportParams["СтатьяРаздельно"] = resolProsArticleLaw.Distinct(x => x.Article).AggregateWithSeparator(x => x.Article, ", "); 

            var resolProsId = resolProsArticleLaw.Select(x => x.ResolProsId).FirstOrDefault();
            reportParams.SimpleReportParams["ДатаОпределения"] = definitionDate;

            reportParams.SimpleReportParams["ДолжностьДЛ"] = inspectorPosition;

            reportParams.SimpleReportParams["ДополнительныеДокументы"] = inspectorAdditionalDocuments;

            reportParams.SimpleReportParams["ЧтоНеобходимоЗапросить"] = inspectorRequestNeed;

            reportParams.SimpleReportParams["СрокПредоставления"] = "";
            
            reportParams.SimpleReportParams["СтатьиЗакона"] = resolProsArticleLaw.Aggregate("", (x, y) => x + ", " + y.Name.Remove(y.Name.IndexOf("К")));

            var resolProsData = ResolProsDomain.GetAll()
                .Where(x => x.Id == resolProsId)
                .Select(x => new
                {
                    x.Id,
                    x.Executant.Code,
                    ContragentId = x.Contragent.Id,
                    ContragentName = x.Contragent.Name,
                    x.PhysicalPerson,
                    MunName = x.Municipality.Name,
                    x.DocumentDate,
                    x.DateSupply
                })
                .FirstOrDefault();

            var contragentId = 0L;
            if (resolProsData != null)
            {
                contragentId = resolProsData.ContragentId;
            }

            var contragentData = ContragentDomain.GetAll()
                .Where(x => x.Id == contragentId)
                .Select(x => new
                {
                    x.Name,
                    x.NameGenitive,
                    x.Inn,
                    x.Oktmo,
                    x.JuridicalAddress,
                    x.FactAddress,
                    x.Kpp,
                    x.Ogrn

                })
                .FirstOrDefault();


            var resolProsDefinition = ResolProsDefinitionDomain.GetAll()
                .Where(x => x.ResolPros.Id == resolProsId)
                .Where(x => x.IssuedDefinition.Id == inspectorId)
                .Select(x => new
                {
                    x.ResolutionInitAdminViolation,
                    x.ReturnReason
                })
                .FirstOrDefault();


            var executantCode = string.Empty;
            if (resolProsData != null)
            {
                executantCode = resolProsData.Code;
            }

            var firstExecutantCodeList = new List<string> { "1", "9", "11", "8", "15", "18", "4" };
            var secondExecutantCodeList = new List<string> { "0", "10", "12", "13", "16", "19", "5" };
            var thirdExecutantCodeList = new List<string> { "6", "7", "14" };

            var callPersonExecutantCodeList = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
            var callPersonExecutantCodeSecondList = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
            
            if (contragentData != null)
            {
                reportParams.SimpleReportParams["ИнформацияОЮЛ"] = contragentData.Name + ", ИНН: " + contragentData.Inn + ", ОКТМО: " + contragentData.Oktmo + ", юр. адрес: "
                    + contragentData.JuridicalAddress + ", факт. адрес: " + contragentData.FactAddress;

                reportParams.SimpleReportParams["ОГРН"] = contragentData.Ogrn;
                reportParams.SimpleReportParams["ИНН"] = contragentData.Inn;
                reportParams.SimpleReportParams["КПП"] = contragentData.Kpp;
            }

            var contragentContact = ContragentContactDomain.GetAll()
                .Where(x => x.Contragent.Id == contragentId)
                .Select(x => new
                {
                    x.Position.Name,
                    x.Contragent.NameGenitive
                })
                .FirstOrDefault();

            if (resolProsData != null)
            {
                if (callPersonExecutantCodeSecondList.Contains(executantCode))
                {
                    if (contragentContact != null)
                    {
                        if (contragentContact.Name == "Руководитель")
                        {
                            reportParams.SimpleReportParams["ВызовЛица"] = contragentContact.NameGenitive;
                        }
                    }
                }

                if (callPersonExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ВызовЛица"] = resolProsData.PhysicalPerson;
                }

                if (firstExecutantCodeList.Contains(executantCode))
                {
                    if (contragentData != null)
                    {
                        var contr = contragentData.NameGenitive == "" ? contragentData.Name : contragentData.NameGenitive;
                        reportParams.SimpleReportParams["Контрагент(РП)"] = "юридического лица - " + contr;
                    }

                    reportParams.SimpleReportParams["ТипИсполнителя"] = "юридического лица " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                }
                
                if (secondExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = "должностного лица " + " " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                    
                    reportParams.SimpleReportParams["Контрагент(РП)"] = "должностного лица - " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                }
                if (thirdExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = "физического лица " + resolProsData.PhysicalPerson;
                    reportParams.SimpleReportParams["ВызовЛица"] = resolProsData.PhysicalPerson;
                    reportParams.SimpleReportParams["Контрагент(РП)"] = "должностного лица - " + resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;
                }

                reportParams.SimpleReportParams["Дата"] = resolProsData.DateSupply.ToDateTime().ToShortDateString();
                reportParams.SimpleReportParams["РайонПрокуратуры"] = resolProsData.MunName;
                reportParams.SimpleReportParams["Контрагент"] = resolProsData.ContragentName + " " + resolProsData.PhysicalPerson;

            }

            if (resolProsDefinition != null)
            {
                reportParams.SimpleReportParams["Установил"] = resolProsDefinition.ResolutionInitAdminViolation;
                reportParams.SimpleReportParams["Доводы"] = resolProsDefinition.ReturnReason;
            }

            if (resolProsData != null)
            {
                reportParams.SimpleReportParams["ДатаПостановления"] = resolProsData.DocumentDate.ToDateTime().ToShortDateString();
            }

            var appointedDate = definition.ExecutionDate.ToDateTime().ToShortDateString();
            reportParams.SimpleReportParams["ДатаНазначения"] = appointedDate;

            var inspectorCode = definition.IssuedDefinition.Code;
            var inspectorsData = InspectorDomain.GetAll()
                .Where(x => x.Code == inspectorCode)
                .Select(x => new
                {
                    x.ShortFio,
                    x.Code
                })
                .FirstOrDefault();
            
            if (inspectorsData != null)
            {
                reportParams.SimpleReportParams["ДЛВынесшееОпределение"] = inspectorsData.ShortFio;

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

            var articleLawId = resolProsArticleLaw.Select(x => x.ArticleLawId).ToList();
            var articleLawGjiDiscription = ArticleLawGjiDomain.GetAll()
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
            
            this.GetCodeTemplate(definition);

        }

        private void GetCodeTemplate(ResolProsDefinition definition)
        {
            switch (definition.TypeResolProsDefinition)
            {
                case TypeResolProsDefinition.ProsecutorReturn:
                    {
                        CodeTemplate = "BlockGJI_ResolutionGjiDefinitionReturnProsecutor_1";
                    }
                    break;
                case TypeResolProsDefinition.AppointmentDateTime:
                    {
                        CodeTemplate = "BlockGJI_ResolutionGjiDefinitionDateTimeReport_1";
                    }
                    break;
                case TypeResolProsDefinition.Deposition:
                    {
                        CodeTemplate = "BlockGJI_ResolutionGjiDefinitionDepositionReport_1";
                    }
                    break;
            }
        }
    }
}
