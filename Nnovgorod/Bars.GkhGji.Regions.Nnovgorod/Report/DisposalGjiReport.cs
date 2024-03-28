namespace Bars.GkhGji.Regions.Nnovgorod.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalGjiReport : GkhGji.Report.DisposalGjiReport
    {
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>()
                       {
                           new TemplateInfo()
                               {
                                   Code = "BlockGJI_Instruction_5001",
                                   Name = "DisposalGJI",
                                   Description = "Коды видов проверок 1,2,3,4,7,9 и типы обследования 5001,5002, 5011, 5012, 5014,5015, 5019, 5020",
                                   Template = Properties.Resources.BlockGJI_Instruction_5001
                               },
                               new TemplateInfo()
                               {
                                   Code = "BlockGJI_Instruction_5002",
                                   Name = "DisposalGJI",
                                   Description = "Коды видов проверок 5,6 и типы обследования 5008, 5010, 5013",
                                   Template = Properties.Resources.BlockGJI_Instruction_5002
                               },
                               new TemplateInfo()
                               {
                                   Code = "BlockGJI_Instruction_5003",
                                   Name = "DisposalGJI",
                                   Description = "Тип основания проверки - обращение граждан и вид проверки 2, 4, 9 и типы обследования 5003, 5004, 5005, 5006, 5007",
                                   Template = Properties.Resources.BlockGJI_Instruction_5003
                               },
                               new TemplateInfo()
                               {
                                   Code = "BlockGJI_Instruction_5004_PP",
                                   Name = "DisposalGJI",
                                   Description = "Тип основания - предписание и вид проверки 2,4 и типы обследования 5009, 5016",
                                   Template = Properties.Resources.BlockGJI_Instruction_5004_PP
                               },
                               new TemplateInfo()
                               {
                                   Code = "BlockGJI_Instruction_5005_PP",
                                   Name = "DisposalGJI",
                                   Description = "Тип основания - предписание и вид проверки  5 и тип обследования 5017",
                                   Template = Properties.Resources.BlockGJI_Instruction_5005_PP
                               }
                       };
        }

        protected override void GetCodeTemplate(Disposal disposal, TypeSurveyGji[] typeSurveys)
        {
            CodeTemplate = "BlockGJI_Instruction_5001";
            if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.NotPlannedExit:
                    case TypeCheck.NotPlannedDocumentation:
                    case TypeCheck.NotPlannedDocumentationExit:
                        var listInspectionTypeCheck = new List<string> { "5003", "5004", "5005", "5006", "5007" };
                        if (typeSurveys.Any(x => listInspectionTypeCheck.Contains(x.Code)))
                        {
                            CodeTemplate = "BlockGJI_Instruction_5003";                            
                            return;
                        }
                        break;
                }
            }

            if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.NotPlannedExit:
                    case TypeCheck.NotPlannedDocumentation:
                        if (typeSurveys.Any(x => x.Code == "5009" || x.Code == "5016"))
                        {
                            CodeTemplate = "BlockGJI_Instruction_5004_PP";
                            return;
                        }
                        break;
                    case TypeCheck.InspectionSurvey:
                        if (typeSurveys.Any(x => x.Code == "5017"))
                        {
                            CodeTemplate = "BlockGJI_Instruction_5005_PP";
                            return;
                        }
                        break;

                }
            }
            
            // Коды видов проверок 5,6 и типы обследования все кроме "5008", "5010", "5013"
            var listTypeSurvTypeCheck8_13 = new List<string> { "5008", "5010", "5013" };

            if (typeSurveys.Any(x => listTypeSurvTypeCheck8_13.Contains(x.Code)))
            {
                if (disposal.KindCheck.Code == TypeCheck.InspectionSurvey || disposal.KindCheck.Code == TypeCheck.Monitoring)
                {
                    CodeTemplate = "BlockGJI_Instruction_5002";
                    return;
                }                
            }

            // Коды видов проверок 1,2,3,4,7,9 и типы обследования все кроме "5001", "5002", "5011", "5012", "5014", "5015"
            var listTypeSurvTypeCheck1_15 = new List<string> { "5001", "5002", "5011", "5012", "5014", "5015", "5019", "5020" };
            if (typeSurveys.Any(x => listTypeSurvTypeCheck1_15.Contains(x.Code)))
            {
                if (disposal.KindCheck.Code == TypeCheck.PlannedExit 
                    || disposal.KindCheck.Code == TypeCheck.NotPlannedExit 
                    || disposal.KindCheck.Code == TypeCheck.PlannedDocumentation 
                    || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation 
                    || disposal.KindCheck.Code == TypeCheck.PlannedDocumentationExit 
                    || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit)
                {
                    CodeTemplate = "BlockGJI_Instruction_5001";
                    return;
                }
            }
        }

        protected override void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            var disposal = (Disposal)doc;

            if ((disposal.Inspection.TypeBase == TypeBase.PlanJuridicalPerson) && (disposal.TypeDisposal != TypeDisposalGji.DocumentGji))
            {
                var countDays = this.Container.Resolve<IDomainService<BaseJurPerson>>().GetAll()
                    .Where(x => x.Id == disposal.Inspection.Id)
                    .Select(x => x.CountDays)
                    .FirstOrDefault();
                reportParams.SimpleReportParams["СрокПроверки"] = countDays != null ? countDays.ToString() : string.Empty;
            }
            else
            {
                reportParams.SimpleReportParams["СрокПроверки"] = "1 рабочий день";
            }

        }
    }
}