namespace Bars.GkhGji.Regions.Tomsk.Report.ResolutionGji
{
    using System;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.GkhGji.Entities;

    using ResolProsDefinition = Bars.GkhGji.Regions.Tomsk.Entities.ResolPros.ResolProsDefinition;

    public class ResolutionGjiDefinitionReturnProsecutorReport : GkhBaseReport
    {
        public ResolutionGjiDefinitionReturnProsecutorReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ResolutionGjiDefinitionReturnProsecutor_1))
        {
        }

        private long DefinitionId { get; set; }

        public override string Id
        {
            get { return "ResolutionGjiDefinitionReturnProsecutor"; }
        }

        public override string CodeForm
        {
            get { return "ResolutionDefinitionReturn"; }
        }

        public override string Name
        {
            get { return "Возврат в прок-ру"; }
        }

        public override string Description
        {
            get { return "Определение о возврате в прокуратуру"; }
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
                               }

                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            CodeTemplate = "BlockGJI_ResolutionGjiDefinitionReturnProsecutor_1";

            var definition = Container.Resolve<IDomainService<ResolProsDefinition>>().Get(DefinitionId);
            var definitionDate = definition.DocumentDate.ToDateTime().ToShortDateString();
            
            var inspectorPosition = definition.IssuedDefinition.Position;
            var inspectorFio = definition.IssuedDefinition.Fio;
            var inspectorId = definition.IssuedDefinition.Id;
            var resolProsArticleLaw = Container.Resolve<IDomainService<ResolProsArticleLaw>>().GetAll()
                .Where(x => x.ResolPros.Id == definition.ResolPros.Id)
                .Select(x => new
                {
                    ResolProsId = x.ResolPros.Id,
                    x.ArticleLaw.Name
                })
                .ToList();

            var resolProsId = resolProsArticleLaw.Select(x => x.ResolProsId).FirstOrDefault();
            reportParams.SimpleReportParams["ДатаОпределения"] = definitionDate;
            
            reportParams.SimpleReportParams["ДолжностьДЛ"] = inspectorPosition;
            reportParams.SimpleReportParams["ДЛВынесшееОпределение"] = inspectorFio;
            reportParams.SimpleReportParams["СтатьиЗакона"] = resolProsArticleLaw.Aggregate("", (x, y) => x + y.Name + ", ");

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


            var resolProsDefinition = Container.Resolve<IDomainService<ResolProsDefinition>>().GetAll()
                .Where(x => x.ResolPros.Id == resolProsId)
                .Where(x => x.IssuedDefinition.Id == inspectorId)
                .Select(x => new
                {
                    x.ResolutionInitAdminViolation,
                    x.ReturnReason,
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

            if (contragentData != null)
            {
                reportParams.SimpleReportParams["ИнформацияОЮЛ"] = contragentData.Name + ", ИНН: " + contragentData.Inn + ", ОКТМО: " + contragentData.Oktmo + ", юр. адрес: "
                    + contragentData.JuridicalAddress + ", факт. адрес: " + contragentData.FactAddress;
            }

            if (resolProsData != null)
            {
                if (firstExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = string.Format( "юридического лица {0} {1}", resolProsData.ContragentName, resolProsData.PhysicalPerson);
                }
                if (secondExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = string.Format("должностного лица {0} {1}", resolProsData.ContragentName, resolProsData.PhysicalPerson);
                }
                if (thirdExecutantCodeList.Contains(executantCode))
                {
                    reportParams.SimpleReportParams["ТипИсполнителя"] = string.Format("физического лица {0}", resolProsData.PhysicalPerson);
                }

                reportParams.SimpleReportParams["Дата"] = resolProsData.DocumentDate.ToDateTime().ToShortDateString();
                reportParams.SimpleReportParams["РайонПрокуратуры"] = resolProsData.MunName;
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
            
        }
    }
}
