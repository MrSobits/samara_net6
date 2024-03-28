namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Globalization;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalGjiStateToProsecReport : GjiBaseReport
    {
        private long DocumentId { get; set; }

        private Disposal disposal;

        public Disposal Disposal
        {
            get
            {
                return disposal;
            }
            set
            {
                disposal = value;
            }
        }
         
        protected override string CodeTemplate { get; set; }

        public DisposalGjiStateToProsecReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_InstructionStateToProsec))
        {
        }

        public override string ReportGeneratorName
        {
            get { return "XlsIoGenerator"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_InstructionStateToProsec",
                            Name = "InstructionStateToProsec",
                            Description = "Заявление в прокуратуру",
                            Template = Properties.Resources.BlockGJI_InstructionStateToProsec
                        }
                };
        }

        public override string Id
        {
            get { return "DisposalStateToProsec"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        public override string Name
        {
            get { return "Заявление в прокуратуру"; }
        }

        public override string Description
        {
            get { return "Заявление в прокуратуру"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalService = Container.Resolve<IDomainService<Disposal>>();
            var disposalTextService = Container.Resolve<IDisposalText>();

            try
            {
                Disposal = disposalService.Load(DocumentId);

                if (Disposal == null)
                {
                    var disposalText = disposalTextService.SubjectiveCase;
                    throw new ReportProviderException(string.Format("Не удалось получить распоряжение {0}", disposalText.ToLower()));
                }

                CodeTemplate = "BlockGJI_InstructionStateToProsec";

                // Заполняем общие поля
                FillCommonFields(reportParams, disposal);

                reportParams.SimpleReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;
                reportParams.SimpleReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                                                                          ? disposal.DocumentDate.Value.ToShortDateString()
                                                                          : string.Empty;

                reportParams.SimpleReportParams["ВидПроверки"] = GetKindCheck(disposal.KindCheck.Code);

                reportParams.SimpleReportParams["НачалоПериода"] = disposal.DateStart.ToDateTime().ToString("«dd» MMMM yyyy", CultureInfo.CurrentCulture);
                reportParams.SimpleReportParams["ОкончаниеПериода"] = disposal.DateEnd.ToDateTime().ToString("«dd» MMMM yyyy", CultureInfo.CurrentCulture);

                var contragent = disposal.Inspection.Contragent;
                if (contragent != null)
                {
                    reportParams.SimpleReportParams["УправОрг"] = contragent.Name;
                    reportParams.SimpleReportParams["ОГРН"] = contragent.Ogrn;
                    reportParams.SimpleReportParams["ИНН"] = contragent.Inn;
                    if (contragent.FiasJuridicalAddress != null)
                    {
                        reportParams.SimpleReportParams["Адрес"] = contragent.FiasJuridicalAddress.AddressName;
                    }
                    else
                    {
                        reportParams.SimpleReportParams["Адрес"] = contragent.AddressOutsideSubject;
                    }
                }
            }
            finally 
            {
                Container.Release(disposalService);
                Container.Release(disposalTextService);
            }
        }

        private string GetKindCheck(TypeCheck typeCheck)
        {
            switch (typeCheck)
            {
                case TypeCheck.PlannedExit:
                    {
                        return "плановой выездной";
                    }
                case TypeCheck.NotPlannedExit:
                    {
                        return "внеплановой выездной";
                    }
                case TypeCheck.PlannedDocumentation:
                    {
                        return "плановой документарной";
                    }
                case TypeCheck.NotPlannedDocumentation:
                    {
                        return "внеплановой документарной";
                    }
                case TypeCheck.InspectionSurvey:
                    {
                        return "внеплановой выездной";
                    }
                case TypeCheck.Monitoring:
                    {
                        return "мониторинг";
                    }
                case TypeCheck.PlannedDocumentationExit:
                    {
                        return "плановой документарной и выездной";
                    }
                case TypeCheck.VisualSurvey:
                    {
                        return "о внеплановой проверке технического состояния жилого помещения";
                    }
                case TypeCheck.NotPlannedDocumentationExit:
                    {
                        return "внеплановой документарной и выездной";
                    }
                    
                default:
                    return string.Empty;
            }
        }
    }
}