namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    /// <summary>
    /// Отчет по проверкам
    /// </summary>
    public class DisposalGjiReport : GjiBaseReport
    {   /// <summary>
        /// Конструктор
        /// </summary>
        public DisposalGjiReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Inspection_1))
        {
        }
        /// <summary>
        /// ID
        /// </summary>
        public override string Id
        {
            get { return "Disposal"; }
        }
        /// <summary>
        /// Код
        /// </summary>
        public override string CodeForm
        {
            get { return "Disposal"; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }
        /// <summary>
        /// Название отчета
        /// </summary>
        public override string ReportGeneratorName
        {
            get { return "XlsIoGenerator"; }
        }
        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }

        private long DocumentId { get; set; }
        /// <summary>
        /// Информация о шаблонах
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Inspection_1",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 1 (Плановая выездная)",
                            Template = Properties.Resources.BlockGJI_Inspection_1
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Inspection_3",
                            Name = "DisposalGJI",
                            Description = "Тип основания проверки - Требование прокуратуры и Код вида проверки 2 (Внеплановая выездная)",
                            Template = Properties.Resources.BlockGJI_Inspection_3
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Inspection_7",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 3 (Плановая документарная)",
                            Template = Properties.Resources.BlockGJI_Inspection_7
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_1",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и тип обследования с кодом 1, или Код вида проверки 9 (внеплановая документарная и выездная) и тип обследования с кодом 1",
                            Template = Properties.Resources.BlockGJI_Instruction_1
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_1_1",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 4 (Внеплановая документарная) и тип обследования с кодом 1",
                            Template = Properties.Resources.BlockGJI_Instruction_1
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_2",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и Тип основания - Обращение граждан",
                            Template = Properties.Resources.BlockGJI_Instruction_2
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_5",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 4 (Внеплановая документарная)",
                            Template = Properties.Resources.BlockGJI_Instruction_5
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_5_1",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 4 (Внеплановая документарная) и Тип основания - поручение руководитства",
                            Template = Properties.Resources.BlockGJI_Instruction_5
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_5_2",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 4 (Внеплановая документарная) и Тип основания - требование прокуратуры",
                            Template = Properties.Resources.BlockGJI_Instruction_5
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_5_3",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 4 (Внеплановая документарная) и Тип основания - Обращение граждан",
                            Template = Properties.Resources.BlockGJI_Instruction_2
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_6",
                            Name = "DisposalGJI",
                            Description = "Основание проверки - Деятельность ТСЖ или Код вида проверки 5 (Инспекционное обследование) или (Код вида проверки 2 (Внеплановая выездная) && Тип обследования с кодом 18)",
                            Template = Properties.Resources.BlockGJI_Instruction_6
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_8",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 5 (Инспекционное обследование) и Тип обследования с кодом 22",
                            Template = Properties.Resources.BlockGJI_Instruction_8
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_9",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 5 (Инспекционное обследование) и Тип основания - Предписание и Тип обследования с кодом 22",
                            Template = Properties.Resources.BlockGJI_Instruction_9
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_11",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 5 (Инспекционное обследование) и Тип обследования с кодом 4 или 8 или 20",
                            Template = Properties.Resources.BlockGJI_Instruction_11
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890101",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 7 (Плановая документарная и выездная) и Тип обследования с кодом 890001",
                            Template = Properties.Resources.BlockGJI_Instruction_890101
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890102",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 7 (Плановая документарная и выездная) и Тип обследования с кодом 21 или 890002",
                            Template = Properties.Resources.BlockGJI_Instruction_890102
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890103",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 7 (Плановая документарная и выездная) и Тип обследования с кодом 890003",
                            Template = Properties.Resources.BlockGJI_Instruction_890103
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890201",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 9 (Внеплановая документарная и выездная) и Тип обследования с кодом 890001",
                            Template = Properties.Resources.BlockGJI_Instruction_890201
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890202",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 9 (Внеплановая документарная и выездная) и Тип обследования с кодом 21 или 890002",
                            Template = Properties.Resources.BlockGJI_Instruction_890202
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890203",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 9 (Внеплановая документарная и выездная) и Тип обследования с кодом 890003",
                            Template = Properties.Resources.BlockGJI_Instruction_890203
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890204",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 9 (Внеплановая документарная и выездная) и Тип обследования с кодом 890004",
                            Template = Properties.Resources.BlockGJI_Instruction_890204
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890205",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 9 (Внеплановая документарная и выездная) и Тип обследования с кодом 890005",
                            Template = Properties.Resources.BlockGJI_Instruction_890205
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890404",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 4 (Внеплановая документарная) и Тип обследования с кодом 890004",
                            Template = Properties.Resources.BlockGJI_Instruction_890404
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890506",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 8 (Визуальное обследование) и Тип обследования с кодом (13 или 890006 или 890007)",
                            Template = Properties.Resources.BlockGJI_Instruction_890506
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890301",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и Тип обследования с кодом 890001",
                            Template = Properties.Resources.BlockGJI_Instruction_890301
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890302",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и Тип обследования с кодом 21 или 890002",
                            Template = Properties.Resources.BlockGJI_Instruction_890302
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890303",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и Тип обследования с кодом 890003",
                            Template = Properties.Resources.BlockGJI_Instruction_890303
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890304",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и Тип обследования с кодом 890004",
                            Template = Properties.Resources.BlockGJI_Instruction_890304
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_890305",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и Тип обследования с кодом 890005",
                            Template = Properties.Resources.BlockGJI_Instruction_890305
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Instruction_1_13",
                            Name = "DisposalGJI",
                            Description = "Код вида проверки 2 (Внеплановая выездная) и тип основания - поручение руководства, и все Типы обследования кроме типа с кодом 890007 или 1",
                            Template = Properties.Resources.BlockGJI_Instruction_1_13
                        }
                };
        }
        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }
        /// <summary>
        /// Подготовка данных для отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            CodeTemplate = "BlockGJI_Inspection_1";

            var disposal = Container.Resolve<IDomainService<Disposal>>().Get(DocumentId);

            if (disposal == null)
            {
                var dispText = Container.Resolve<IDisposalText>();
                throw new ReportProviderException(string.Format("Не удалось получить {0}", dispText.SubjectiveCase.ToLower()));
            }
            
            var typeSurveys = Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll()
                                       .Where(x => x.Disposal.Id == disposal.Id)
                                       .Select(x => x.TypeSurvey)
                                       .ToArray();

            if (disposal.KindCheck != null)
            {
                GetCodeTemplate(disposal, typeSurveys);

                var kindCheck = string.Empty;

                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.PlannedExit:
                        kindCheck = "плановой выездной";
                        break;
                    case TypeCheck.NotPlannedExit:
                    case TypeCheck.InspectionSurvey:
                        kindCheck = "внеплановой выездной";
                        break;
                    case TypeCheck.PlannedDocumentation:
                        kindCheck = "плановой документарной";
                        break;
                    case TypeCheck.NotPlannedDocumentation:
                        kindCheck = "внеплановой документарной";
                        break;
                    case TypeCheck.PlannedDocumentationExit:
                        kindCheck = "плановой документарной и выездной";
                        break;
                    case TypeCheck.VisualSurvey:
                        kindCheck = "о внеплановой проверке технического состояния жилого помещения";
                        break;
                    case TypeCheck.NotPlannedDocumentationExit:
                        kindCheck = "внеплановой документарной и выездной";
                        break;
                }

                reportParams.SimpleReportParams["ВидПроверки"] = kindCheck;
                reportParams.SimpleReportParams["ВидОбследования"] = disposal.KindCheck.Name.ToLower();
            }

            var cultureInfo = new CultureInfo("ru-RU");
            var dateFormat = "«dd» MMMM yyyy";

            // заполняем общие поля
            FillCommonFields(reportParams, disposal);

            if (disposal.Inspection.Contragent != null)
            {
                reportParams.SimpleReportParams["УправОрг"] = disposal.Inspection.Contragent.Name;
                reportParams.SimpleReportParams["ОГРН"] = disposal.Inspection.Contragent.Ogrn;
                reportParams.SimpleReportParams["УправОргРП"] = disposal.Inspection.Contragent.NameGenitive;
                reportParams.SimpleReportParams["УправОргСокр"] = disposal.Inspection.Contragent.ShortName;
                reportParams.SimpleReportParams["ИНН"] = disposal.Inspection.Contragent.Inn;
                
                if (disposal.Inspection.Contragent.FiasJuridicalAddress != null)
                {
                    var subStr = disposal.Inspection.Contragent.FiasJuridicalAddress.AddressName.Split(',');

                    var newAddr = new StringBuilder();

                    foreach (var rec in subStr)
                    {
                        if (newAddr.Length > 0)
                        {
                            newAddr.Append(',');
                        }

                        if (rec.Contains("р-н."))
                        {
                            var mu = rec.Replace("р-н.", string.Empty) + " район";
                            newAddr.Append(mu);
                            continue;
                        }

                        newAddr.Append(rec);
                    }

                    reportParams.SimpleReportParams["АдресКонтрагента"] = newAddr;

                    var fiasAddr = disposal.Inspection.Contragent.FiasJuridicalAddress;

                    var addrExceptMu = new StringBuilder();

                    var addSeparator = false;

                    if (!string.IsNullOrEmpty(fiasAddr.PlaceName))
                    {
                        addrExceptMu.Append(fiasAddr.PlaceName);
                        addSeparator = true;
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.StreetName))
                    {
                        addrExceptMu.Append(addSeparator ? ", " + fiasAddr.StreetName : fiasAddr.StreetName);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.House))
                    {
                        addrExceptMu.Append(addSeparator ? ", д. " + fiasAddr.House : "д. " + fiasAddr.House);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Housing))
                    {
                        addrExceptMu.Append(addSeparator ? ", корп. " + fiasAddr.Housing : "корп. " + fiasAddr.Housing);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Building))
                    {
                        addrExceptMu.Append(addSeparator ? ", секц. " + fiasAddr.Building : "секц. " + fiasAddr.Building);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Flat))
                    {
                        addrExceptMu.Append(addSeparator ? ", кв." + fiasAddr.Flat : "кв." + fiasAddr.Flat);
                    }

                    reportParams.SimpleReportParams["АдресОтносительноМО"] = addrExceptMu.ToString();
                }
                else
                {
                    reportParams.SimpleReportParams["АдресКонтрагента"] = disposal.Inspection.Contragent.AddressOutsideSubject;                    
                }

                reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = disposal.Inspection.Contragent.FactAddress;
            }

            reportParams.SimpleReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                            ? disposal.DocumentDate.Value.ToString("D", cultureInfo)
                            : string.Empty;

            reportParams.SimpleReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;

            reportParams.SimpleReportParams["НачалоПериодаВыезд"] = disposal.ObjectVisitStart.HasValue
                                                                        ? disposal.ObjectVisitStart.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;

            reportParams.SimpleReportParams["ОкончаниеПериодаВыезд"] = disposal.ObjectVisitEnd.HasValue
                                                                        ? disposal.ObjectVisitEnd.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;

            reportParams.SimpleReportParams["НачалоПериода"] = disposal.DateStart.HasValue
                                                                        ? disposal.DateStart.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;

            reportParams.SimpleReportParams["ОкончаниеПериода"] = disposal.DateEnd.HasValue
                                                                        ? disposal.DateEnd.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;

            var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                      .Where(x => x.DocumentGji.Id == disposal.Id)
                                      .Select(x => x.Inspector)
                                      .ToList();

            if (inspectors.Any())
            {
                var inspectorsAndCodes = inspectors.Any() ? inspectors
                    .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                      ? string.Format("{0} - {1}", y.Fio, y.Position)
                                                      : string.Format(", {0} - {1}", y.Fio, y.Position)))
                                                      : string.Empty;

                var inspectorsAndCodesDative = inspectors.Any() ?  inspectors
                    .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                      ? string.Format("{0} - {1}", y.FioDative, y.PositionDative)
                                                      : string.Format(", {0} - {1}", y.FioDative, y.PositionDative)))
                                                      : string.Empty;

                var inspectorsAndCodesAccusative = inspectors.Any() ? inspectors
                    .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                      ? string.Format("{0} - {1}", y.FioAccusative, y.PositionAccusative)
                                                      : string.Format(", {0} - {1}", y.FioAccusative, y.PositionAccusative)))
                                                      : string.Empty;

                var inspectorsAndCodesGenitive = inspectors.Any() ? inspectors
                    .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                      ? string.Format("{0} - {1}", y.FioGenitive, y.PositionGenitive)
                                                      : string.Format(", {0} - {1}", y.FioGenitive, y.PositionGenitive)))
                                                      : string.Empty;

                reportParams.SimpleReportParams["ИнспекторыИКоды"] = inspectorsAndCodes;
                reportParams.SimpleReportParams["ИнспекторыИКодыДатП"] = inspectorsAndCodesDative;
                reportParams.SimpleReportParams["ИнспекторыИКодыВинП"] = inspectorsAndCodesAccusative;
                reportParams.SimpleReportParams["ИнспекторыИКодыРодП"] = inspectorsAndCodesGenitive;

                var firstInspector = inspectors.FirstOrDefault();
                if (firstInspector != null)
                {
                    reportParams.SimpleReportParams["ТелефонИнспектора"] = firstInspector.Phone;
                }

                // формируем следующую строку: дефис пробел должность в рп фио в рп
                var strBuilder = new StringBuilder();

                var currRow = new StringBuilder();

                foreach (var insp in inspectors)
                {
                    currRow.Append("- ");

                    if (!string.IsNullOrEmpty(insp.PositionGenitive))
                    {
                        currRow.Append(insp.PositionGenitive);
                    }

                    if (!string.IsNullOrEmpty(insp.FioGenitive))
                    {
                        currRow.Append(" ");
                        currRow.Append(insp.FioGenitive);
                    }

                    // чтобы не выводить строки типа "- " проверяем длину
                    if (currRow.ToString().Length > 2)
                    {
                        if (strBuilder.Length > 0)
                        {
                            strBuilder.Append(";\n");
                        }

                        strBuilder.Append(currRow);
                    }

                    currRow.Clear();
                }

                if (strBuilder.Length > 0)
                {
                    strBuilder.Append(".");
                }

                reportParams.SimpleReportParams["ДолжностьФИОРП"] = strBuilder.ToString();

                var listInspectorId = inspectors.Select(x => x.Id).ToList();

                var listEmails =
                    Container.Resolve<IDomainService<Operator>>()
                        .GetAll()
                        .Where(x => listInspectorId.Contains(x.Inspector.Id))
                        .ToList()
                        .Select(x => x.Return(z => z.User).Return(y => y.Email));

                reportParams.SimpleReportParams["E-mailИнспектора"] = string.Join("; ", listEmails);
            }

            if (disposal.IssuedDisposal != null)
            {
                reportParams.SimpleReportParams["КодРуководителя"] = disposal.IssuedDisposal.Position;

                reportParams.SimpleReportParams["КодРуководителяФИО(сИнициалами)"] = string.Format(
                    "{0} {1}",
                    disposal.IssuedDisposal.Position,
                    string.IsNullOrEmpty(disposal.IssuedDisposal.ShortFio) ? disposal.IssuedDisposal.Fio : disposal.IssuedDisposal.ShortFio);

                reportParams.SimpleReportParams["КодНачальника(ВинП)"] = disposal.IssuedDisposal.PositionAccusative; 
                reportParams.SimpleReportParams["Начальник(ВинП)"] = disposal.IssuedDisposal.FioAccusative;

                reportParams.SimpleReportParams["РуководительДолжность"] = disposal.IssuedDisposal.Position;
                reportParams.SimpleReportParams["РуководительФИОСокр"] = disposal.IssuedDisposal.ShortFio;
            }

            var realityObjs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                .Select(x => new
                    {
                        municipalityName = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address,
                        x.RealityObject.FiasAddress.PlaceName,
                        x.RealityObject.FiasAddress.StreetName,
                        x.RealityObject.FiasAddress.House,
                        x.RealityObject.FiasAddress.Housing,
                    })
                .ToList();
            
            if (realityObjs.Count == 1)
            {
                var firstObject = realityObjs.FirstOrDefault();
                
                reportParams.SimpleReportParams["ДомАдрес"] = firstObject.PlaceName + ", " + firstObject.StreetName;
                reportParams.SimpleReportParams["НомерДома"] = firstObject.House;
                reportParams.SimpleReportParams["АдресДома"] = string.Format(
                    "{0}, {1}, д.{2}",
                    firstObject.PlaceName,
                    firstObject.StreetName,
                    firstObject.House);
                reportParams.SimpleReportParams["ДомАдрес1"] = firstObject.StreetName + ", " + firstObject.PlaceName;

            }

            if (realityObjs.Count > 0)
            {
                var housesGroupedPyPlace = realityObjs
                    .GroupBy(x => x.PlaceName)
                    .Select(x =>
                        {
                            var houses = x.Select(y => new
                                {
                                    firstPart = string.Format("{0}, д.{1}", y.StreetName, y.House),
                                    housingPart = string.IsNullOrWhiteSpace(y.Housing) ? string.Empty : string.Format(", корп. {0}", y.Housing)
                                })
                                .Select(y => y.firstPart + y.housingPart);

                            return string.Format("{0}, {1}", x.Key, string.Join("; ", houses));
                        });

                reportParams.SimpleReportParams["ДомаИАдреса"] = string.Join("; ", housesGroupedPyPlace);

                var roAdrList = realityObjs.Select(x => string.Format("{0}, {1}", x.municipalityName, x.Address));

                reportParams.SimpleReportParams["Адрес"] = roAdrList.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? "; " + y : y));
            }

            if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                var servDocChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

                var firstPrescription = servDocChildren
                        .GetAll()
                        .Where(x => x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                if (firstPrescription != null)
                {
                    reportParams.SimpleReportParams["НомерПредписания"] = string.Format("№{0}", firstPrescription.DocumentNumber);
                    reportParams.SimpleReportParams["ДатаПредписания"] = firstPrescription.DocumentDate.HasValue
                                                                             ? firstPrescription.DocumentDate.Value.ToShortDateString()
                                                                             : string.Empty;
                }

                var prescriptions = servDocChildren.GetAll()
                             .Where(x => x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                             .Select(x => new { x.Parent.DocumentDate, x.Parent.DocumentNumber, x.Parent.DocumentSubNum })
                             .OrderBy(x => x.DocumentDate)
                             .AsEnumerable()
                             .GroupBy(x => x.DocumentDate)
                             .ToDictionary(
                                x => x.Key,
                                z => z.Select(x => new { x.DocumentNumber, x.DocumentSubNum })
                                    .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.DocumentNumber : y.DocumentNumber)));

                reportParams.SimpleReportParams["Предписания"] = prescriptions.Aggregate(
                    string.Empty,
                    (x, y) =>
                    {
                        if (!string.IsNullOrEmpty(x))
                        {
                            x += string.Format(", {0} от {1}", y.Value, y.Key.HasValue ? y.Key.Value.ToShortDateString() : string.Empty);
                        }
                        else
                        {
                            x += string.Format("{0}{1} от {2}", prescriptions.Count > 1 ? "предписаний №№" : "предписания №", y.Value, y.Key.HasValue ? y.Key.Value.ToShortDateString() : string.Empty);
                        }

                        return x;
                    });


                var prescriptionRealObjs = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
						.Where(x => x.Document.Id == firstPrescription.Id && x.InspectionViolation.RealityObject != null)
                        .Select(x => x.InspectionViolation.RealityObject)
                        .Distinct()
                        .ToList();
                
                if (prescriptionRealObjs.Count > 0)
                {
                    var prescripRealObjs = new StringBuilder();
                    var prescripRealObjNums = new StringBuilder();

                    foreach (var prescriptionRealObj in prescriptionRealObjs)
                    {
                        if (prescripRealObjNums.Length > 0)
                            prescripRealObjNums.Append("; ");

                        if (prescripRealObjs.Length > 0)
                            prescripRealObjs.Append("; ");

                        prescripRealObjs.AppendFormat(prescriptionRealObj.Address);
                        prescripRealObjNums.Append(prescriptionRealObj.FiasAddress.House);
                    }

                    reportParams.SimpleReportParams["ДомАдресПВП"] = prescripRealObjs.ToString();
                    reportParams.SimpleReportParams["НомерДомаПВП"] = prescripRealObjNums.ToString();
                }
            }
            
            if (disposal.ResponsibleExecution != null)
            {
                reportParams.SimpleReportParams["Ответственный"] = disposal.ResponsibleExecution.Fio;
                reportParams.SimpleReportParams["ДолжностьОтветственныйРп"] = disposal.ResponsibleExecution.PositionGenitive;

                // именно в винительном падеже
                reportParams.SimpleReportParams["ФИООтветственныйСокрРп"] = disposal.ResponsibleExecution.FioAccusative;
            }

            reportParams.SimpleReportParams["ФИООбр"] = "ФИО";

            reportParams.SimpleReportParams["АдресОбр"] = "Адрес";

            // если основание проверки - требование прокуратуры
            if (disposal.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
            {
                var prosClaim = Container.Resolve<IDomainService<BaseProsClaim>>().Load(disposal.Inspection.Id);

                reportParams.SimpleReportParams["№Требования"] = prosClaim.DocumentNumber;
                reportParams.SimpleReportParams["Дата"] = prosClaim.ProsClaimDateCheck.ToDateTime().ToShortDateString();
                reportParams.SimpleReportParams["ДЛТребование"] = prosClaim.IssuedClaim;

                reportParams.SimpleReportParams["Требование"] = string.Format("{0} от {1}", 
                                                                                prosClaim.DocumentNumber,
                                                                                prosClaim.DocumentDate.HasValue
                                                                                    ? prosClaim.DocumentDate.Value.ToShortDateString()
                                                                                    : string.Empty);
            }

            // если основание проверки - поручение руководства
            if (disposal.Inspection.TypeBase == TypeBase.DisposalHead)
            {
                reportParams.SimpleReportParams["ПравовоеОснованиеПроверки"] =
                    "на основании ст.20 Жилищного кодекса РФ, приказа начальника ГЖИ РТ № $НомерПриказа$ от $ДатаПриказа$";

                var dispHead = Container.Resolve<IDomainService<BaseDispHead>>().Load(disposal.Inspection.Id);

                reportParams.SimpleReportParams["НомерПриказа"] = dispHead.InspectionNum;
                reportParams.SimpleReportParams["ДатаПриказа"] = dispHead.DispHeadDate.HasValue
                                                                     ? dispHead.DispHeadDate.Value.ToString("D", cultureInfo)
                                                                     : string.Empty;
            }

            if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var citState = Container.Resolve<IDomainService<BaseStatement>>().Load(disposal.Inspection.Id);

                switch (citState.PersonInspection)
                {
                    case PersonInspection.Organization:
                        reportParams.SimpleReportParams["Лицо"] = "юридического лица";
                        break;
                    case PersonInspection.PhysPerson:
                        reportParams.SimpleReportParams["Лицо"] = "физического лица";
                        break;
                }

                var quryAppeals = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                        .Where(x => x.Inspection.Id == disposal.Inspection.Id);

                var appeals = quryAppeals.Select(x => new
                                        {
                                            x.Id,
                                            appealId = x.AppealCits.Id,
                                            x.AppealCits.Correspondent,
                                            x.AppealCits.DocumentNumber,
                                            x.AppealCits.CorrespondentAddress,
                                            x.AppealCits.NumberGji,
                                            x.AppealCits.DateFrom,
                                            x.AppealCits.TypeCorrespondent
                                        })
                                    .ToList();

                var quryAppealsId = quryAppeals.Select(x => x.AppealCits.Id);

                var appealsRo = Container.Resolve<IDomainService<AppealCitsRealityObject>>()
                                         .GetAll()
                                         .Where(x => quryAppealsId.Contains(x.AppealCits.Id))
                                         .Select(x => new
                                                         {
                                                             appId = x.AppealCits.Id,
                                                             muName = x.RealityObject.Municipality.Name,
                                                             address = x.RealityObject.Address
                                                         })
                                         .ToList();

                if (appeals.Count > 0)
                {
                    var firstCorrespondent = appeals.Select(x => x.TypeCorrespondent).FirstOrDefault();

                    switch (firstCorrespondent)
                    {
                        case TypeCorrespondent.CitizenHe:
                            reportParams.SimpleReportParams["СклонениеГражданин"] = "гражданина";
                            reportParams.SimpleReportParams["СклонениеОбращение"] = "обратившегося";
                            reportParams.SimpleReportParams["СклонениеПроживание"] = "проживающего";
                            break;
                        case TypeCorrespondent.CitizenShe:
                            reportParams.SimpleReportParams["СклонениеГражданин"] = "гражданки";
                            reportParams.SimpleReportParams["СклонениеОбращение"] = "обратившейся";
                            reportParams.SimpleReportParams["СклонениеПроживание"] = "проживающей";
                            break;
                        case TypeCorrespondent.CitizenThey:
                            reportParams.SimpleReportParams["СклонениеГражданин"] = "граждан";
                            reportParams.SimpleReportParams["СклонениеОбращение"] = "обратившихся";
                            reportParams.SimpleReportParams["СклонениеПроживание"] = "проживающих";
                            break;
                    }

                    var fioCorr = new StringBuilder();
                    var addrCorr = new StringBuilder();
                    var appealsNumDate = new StringBuilder();
                    var appealsDocNumDate = new StringBuilder();
                    var appealNumGji = new StringBuilder();
                    var appealAddress = new StringBuilder();

                    foreach (var appeal in appeals)
                    {
                        if (!string.IsNullOrEmpty(appeal.Correspondent))
                        {
                            if (fioCorr.Length > 0)
                                fioCorr.Append(", ");

                            fioCorr.Append(appeal.Correspondent);
                        }

                        if (!string.IsNullOrEmpty(appeal.CorrespondentAddress))
                        {
                            if (addrCorr.Length > 0)
                                addrCorr.Append(", ");

                            addrCorr.Append(appeal.CorrespondentAddress);
                        }

                        var appealsNumDateStr = string.Empty;
                        if (!string.IsNullOrEmpty(appeal.NumberGji))
                        {
                            if (appealsNumDate.Length > 0)
                                appealsNumDate.Append(", ");

                            appealsNumDateStr = string.Format(
                                "{0} от {1}",
                                appeal.NumberGji,
                                appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : string.Empty);


                            appealsNumDate.AppendFormat(appealsNumDateStr);

                            if (appealNumGji.Length > 0)
                                appealNumGji.Append(", ");

                            appealNumGji.Append(appeal.NumberGji);
                        }

                        var addressRo = string.Empty;
                        var appealRo = appealsRo.Where(x => x.appId == appeal.appealId).ToList();
                        if (appealRo.Count > 0)
                        {
                            var item = appealRo.First();
                            addressRo = string.Format("{0}, {1}", item.muName, item.address);
                        }

                        var appealsDocNumDateStr = string.Empty;
                        if (!string.IsNullOrEmpty(appeal.DocumentNumber))
                        {
                            if (appealsDocNumDate.Length > 0)
                                appealsNumDate.Append(", ");

                            appealsDocNumDateStr = string.Format(
                                "{0} от {1}",
                                appeal.DocumentNumber,
                                appeal.DateFrom.HasValue ? appeal.DateFrom.Value.ToShortDateString() : string.Empty);

                            appealsDocNumDate.AppendFormat(appealsDocNumDateStr);
                        }

                        if (!string.IsNullOrEmpty(appeal.Correspondent))
                        {
                            if (appealAddress.Length > 0) appealAddress.Append("; ");

                            appealAddress.AppendFormat("{0} (вх. {1}), проживающего по адресу {2}", appeal.Correspondent, appealsDocNumDateStr, addressRo);
                        }
                    }

                    reportParams.SimpleReportParams["ФИООбр"] = fioCorr.ToString();
                    reportParams.SimpleReportParams["АдресОбр"] = addrCorr.ToString();
                    reportParams.SimpleReportParams["Обращения"] = appealsNumDate.ToString();
                    reportParams.SimpleReportParams["НомерГЖИ"] = appealNumGji.ToString();
                    reportParams.SimpleReportParams["АдресЗаявителя"] = string.Join("; ", appealsRo.Select(x => string.Format("{0}, {1}", x.muName, x.address)));
                    reportParams.SimpleReportParams["РеквизитыОбращения"] = appealAddress.ToString();

                    var appealIds = appeals.Select(x => x.Id).ToList();

                    var sources = Container.Resolve<IDomainService<AppealCitsSource>>().GetAll()
                        .Where(x => appealIds.Contains(x.AppealCits.Id))
                        .Select(x => x.RevenueSource.Name)
                        .Distinct()
                        .ToList();

                    var appealSources = new StringBuilder();
                    foreach (var source in sources.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        if (appealSources.Length > 0)
                            appealSources.Append(", ");

                        appealSources.Append(source);
                    }

                    reportParams.SimpleReportParams["ИсточникОбращения"] = appealSources.ToString();
                }
            }

            reportParams.SimpleReportParams["Эксперты"] =
                Container.Resolve<IDomainService<DisposalExpert>>().GetAll()
                         .Where(x => x.Disposal.Id == disposal.Id)
                         .Select(x => x.Expert)
                         .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.Name : ", " + y.Name));

            var typeSurveyIds = typeSurveys.Select(x => x.Id).ToArray();

            if (typeSurveyIds.Any())
            {
                reportParams.SimpleReportParams["Цель"] =
                    Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>().GetAll()
                             .Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
                             .Where(x => x.SurveyPurpose != null)
                             .AsEnumerable()
                             .GroupBy(x => x.TypeSurvey.Id)
                             .SelectMany(x => x)
                             .AggregateWithSeparator(x => x.SurveyPurpose.Name, ", ");

                reportParams.SimpleReportParams["Задача"] =
                    Container.Resolve<IDomainService<TypeSurveyTaskInspGji>>().GetAll()
                             .Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
                             .Where(x => x.SurveyObjective != null)
                             .AsEnumerable()
                             .GroupBy(x => x.SurveyObjective.Id)
                             .SelectMany(x => x)
                             .AggregateWithSeparator(x => x.SurveyObjective.Name, ", ");

                var inspFounds =
                    Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>().GetAll()
                        .Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
                        .Select(x => new { x.NormativeDoc.Name, x.Code })
                        .OrderBy(x => x.Code)
                        .ToArray()
                        .Select(x => x.Name)
                        .Distinct()
                        .ToList();

                var inspFoundation = new StringBuilder();

                foreach (var foundation in inspFounds.Where(x => !string.IsNullOrEmpty(x)))
                {
                    if (inspFoundation.Length > 0)
                        inspFoundation.Append(";\n- ");

                    inspFoundation.Append(foundation);
                }

                if (inspFoundation.Length > 0)
                    inspFoundation.Append(".");

                reportParams.SimpleReportParams["ПравовоеОснование"] = inspFoundation.ToString();
            }

            var queryTypeSurveyIds = Container.Resolve<IDomainService<DisposalTypeSurvey>>()
                                                .GetAll()
                                                .Where(x => x.Disposal.Id == disposal.Id)
                                                .Select(x => x.TypeSurvey.Id);

            var strGoalInspGji = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>()
                         .GetAll()
                         .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                         .Select(x => x.SurveyPurpose.Name)
                         .AsEnumerable()
                         .Aggregate(string.Empty, (t, n) => t + n + ";");

            if (strGoalInspGji.Length > 0)
            {
                reportParams.SimpleReportParams["ЦельОбследования"] = strGoalInspGji.Substring(0, strGoalInspGji.Length - 1);
            }
            else
            {
                reportParams.SimpleReportParams["ЦельОбследования"] = string.Empty;
            }

            var listDisposalProvidedDoc = Container.Resolve<IDomainService<DisposalProvidedDoc>>()
                         .GetAll()
                         .Where(x => x.Disposal.Id == disposal.Id)
                         .Select(x => x.ProvidedDoc.Name)
                         .ToList();

            reportParams.SimpleReportParams["ПредоставляемыеДокументы"] = listDisposalProvidedDoc
                    .Aggregate(string.Empty, 
                        (total, next) => total == string.Empty ? next : total + ", " + next);

            FillRegionParams(reportParams, disposal);

            var strPurposeDetails = string.Empty;
            if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement && disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
            {
                var tmpstr = reportParams.SimpleReportParams["РеквизитыОбращения"];
                strPurposeDetails = tmpstr != null ? tmpstr.ToString() : string.Empty;
            }
            else
            {
                var addresses = reportParams.SimpleReportParams["ДомаИАдреса"];

                strPurposeDetails = addresses != null ? addresses.ToString() : string.Empty;
            }

            reportParams.SimpleReportParams["ЦельРеквизиты"] = strPurposeDetails;

        }
        /// <summary>
        /// Получение кода шаблона
        /// </summary>
        /// <param name="disposal"></param>
        /// <param name="typeSurveys"></param>
        protected virtual void GetCodeTemplate(Disposal disposal, TypeSurveyGji[] typeSurveys)
        {
            //основание проверки - по поручению руководства
            if (disposal.Inspection.TypeBase == TypeBase.DisposalHead)
            {
                if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                {
                    CodeTemplate = "BlockGJI_Instruction_5_1";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890007"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890506";
                    return;
                }
            }

            // если код вида проверки 5, инспекционное обследование
            if (disposal.KindCheck.Code == TypeCheck.InspectionSurvey)
            {
                if (typeSurveys.Any(x => x.Code == "4" || x.Code == "8" || x.Code == "20"))
                {
                    CodeTemplate = "BlockGJI_Instruction_11";
                    return;
                }

                // если распоряжение на проверку предписания и типы обследования содержат тип с кодом 22
                if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji && typeSurveys.Any(x => x.Code == "22"))
                {
                    CodeTemplate = "BlockGJI_Instruction_9";
                    return;
                }

                // если типы обследования содержат тип с кодом 22
                if (typeSurveys.Any(x => x.Code == "22"))
                {
                    CodeTemplate = "BlockGJI_Instruction_8";
                    return;
                }

                CodeTemplate = "BlockGJI_Instruction_6";
                return;
            }

            // если код вида проверки 8, визуальное обследование
            if (disposal.KindCheck.Code == TypeCheck.VisualSurvey)
            {
                // если типы обследования содержат записи с кодом 13 или 890006 или 890007
                if (typeSurveys.Any(x => x.Code == "13" || x.Code == "890006" || x.Code == "890007"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890506";
                    return;
                }
            }

            // если код вида проверки 2, внеплановая выездная
            if (disposal.KindCheck.Code == TypeCheck.NotPlannedExit)
            {
                if (typeSurveys.Any() && typeSurveys.All(x => x.Code == "1"))
                {
                    CodeTemplate = "BlockGJI_Instruction_1";
                    return;
                }

                // если 
                if (typeSurveys.Any(x => x.Code == "890001"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890301";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "21" || x.Code == "890002"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890302";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890003"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890303";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890004"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890304";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890005"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890305";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "18"))
                {
                    CodeTemplate = "BlockGJI_Instruction_6";
                    return;
                }

                // если основание проверки обращение граждан
                if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    CodeTemplate = "BlockGJI_Instruction_2";
                    return;
                }

                // Тип основания - требование прокуратуры
                if (disposal.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
                {
                    CodeTemplate = "BlockGJI_Inspection_3";
                    return;
                }

                // Тип основания - по поручению руководства
                if (disposal.Inspection.TypeBase == TypeBase.DisposalHead)
                {
                    if ((disposal.KindCheck.Code == TypeCheck.InspectionSurvey
                        || disposal.KindCheck.Code == TypeCheck.NotPlannedExit) && typeSurveys.All(x => x.Code != "890007" && x.Code != "1"))
                    {
                        CodeTemplate = "BlockGJI_Instruction_1_13";
                        return;
                    }
                }
            }

            // Тип основания - Деятельность ТСЖ
            if (disposal.Inspection.TypeBase == TypeBase.ActivityTsj)
            {
                CodeTemplate = "BlockGJI_Instruction_6";
                return;
            }

            // если код проверки 4, внеплановая документарная
            if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
            {
                if (typeSurveys.Any() && typeSurveys.All(x => x.Code == "1"))
                {
                    CodeTemplate = "BlockGJI_Instruction_1_1";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890004"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890404";
                    return;
                }

                // Тип основания - обращение граждан
                if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    CodeTemplate = "BlockGJI_Instruction_5_3";
                    return;
                }

                // Тип основания - требование прокуратуры
                if (disposal.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
                {
                    CodeTemplate = "BlockGJI_Instruction_5_2";
                    return;
                }

                CodeTemplate = "BlockGJI_Instruction_5";
                return;
            }

            // если код проверки 1, плановая выездная
            if (disposal.KindCheck.Code == TypeCheck.PlannedExit)
            {
                CodeTemplate = "BlockGJI_Inspection_1";
                return;
            }

            // если код проверки 3, плановая документарная
            if (disposal.KindCheck.Code == TypeCheck.PlannedDocumentation)
            {
                CodeTemplate = "BlockGJI_Inspection_7";
                return;
            }

            // если код проверки 7, плановая документарная и выездная
            if (disposal.KindCheck.Code == TypeCheck.PlannedDocumentationExit)
            {
                if (typeSurveys.Any(x => x.Code == "890001"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890101";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "21" || x.Code == "890002"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890102";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890003"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890103";
                    return;
                }
            }

            // если код проверки 9, внеплановая документарная и выездная
            if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit)
            {
                if (typeSurveys.Any() && typeSurveys.All(x => x.Code == "1"))
                {
                    CodeTemplate = "BlockGJI_Instruction_1";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890001"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890201";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "21" || x.Code == "890002"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890202";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890003"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890203";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890004"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890204";
                    return;
                }

                if (typeSurveys.Any(x => x.Code == "890005"))
                {
                    CodeTemplate = "BlockGJI_Instruction_890205";
                }
            }
        }
    }
}