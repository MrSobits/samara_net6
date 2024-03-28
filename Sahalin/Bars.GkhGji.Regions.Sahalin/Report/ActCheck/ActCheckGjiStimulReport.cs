using System;

namespace Bars.GkhGji.Regions.Sahalin.Report.ActCheck
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Regions.Sahalin.Properties;
    using GkhGji.Report;

    /// <summary>
    /// Печать Акта проверки Сахалина
    /// </summary>
    public class ActCheckGjiStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ActCheckGjiStimulReport()
            : base(new ReportTemplateBinary(Resources.ActCheck))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Код формы, на которой находится кнопка печати.
        /// </summary>
        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        /// <summary>
        /// Описание отчета.
        /// </summary>
        public override string Description
        {
            get { return "Акт проверки"; }
        }

        /// <summary>
        /// Формат печатной формы.
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        /// <summary>
        /// Идентификатор отчета.
        /// </summary>
        public override string Id
        {
            get { return "ActCheck"; }
        }

        /// <summary>
        /// Наименование отчета.
        /// </summary>
        public override string Name
        {
            get { return "Акт проверки"; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Код шаблона (файла).
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор акта.
        /// </summary>
        private long DocumentId { get; set; }

        #endregion

        /// <summary>
        /// Установить пользовательские параметры.
        /// </summary>
        /// <param name="userParamsValues">
        /// Значения пользовательских параметров.
        /// </param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <summary>
        /// Получить список шаблонов.
        /// </summary>
        /// <returns>
        /// Список шаблонов.
        /// </returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NsoActSurvey",
                    Name = "TomskActSurvey",
                    Description = "Акт проверки НСО",
                    Template = Resources.ActCheck
                }
            };
        }

        /// <summary>
        /// Подготовить параметры отчета.
        /// </summary>
        /// <param name="reportParams">
        /// Параметры отчета.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var actCheckDomain = Container.Resolve<IDomainService<NsoActCheck>>();
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var actRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var actCheckWitnessDomain = Container.Resolve<IDomainService<ActCheckWitness>>();
            var actCheckProvidedDocDomain = Container.Resolve<IDomainService<ActCheckProvidedDoc>>();
            var actCheckViolationDomain = Container.Resolve<IDomainService<ActCheckViolation>>();
            var actCheckPeriodDomain = Container.Resolve<IDomainService<ActCheckPeriod>>();
            var docInspDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var actCheckLongDescDomain = Container.Resolve<IDomainService<ActCheckRoLongDescription>>();
            
            try
            {
                var act = actCheckDomain.FirstOrDefault(x => x.Id == DocumentId);
                if (act == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки");
                }
                FillCommonFields(act);

                var actCheckLongQuery = actCheckLongDescDomain.GetAll().Where(x => x.ActCheckRo.ActCheck.Id == DocumentId);

                var descriptions = actCheckLongQuery
                    .Where(x => x.Description != null)
                    .Select(x => Encoding.UTF8.GetString(x.Description))
                    .ToArray();

                var notRevealedViolations = actCheckLongQuery
                    .Where(x => x.NotRevealedViolations != null)
                    .Select(x => Encoding.UTF8.GetString(x.NotRevealedViolations))
                    .ToArray();

                var actCheckInfo = new
                {
                    Дата = act.DocumentDate.ToDateString(),
                    Номер = act.DocumentNumber,
                    Описание = descriptions.AggregateWithSeparator(", "),
                    НевыявленныеНарушения = notRevealedViolations.AggregateWithSeparator(", "),
                    СКопиейПриказаОзнакомлен = act.AcquaintedWithDisposalCopy
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "АктПроверки",
                    MetaType = nameof(Object),
                    Data = actCheckInfo
                });

                var inspectors = docInspDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        ФИО = x.Inspector.Fio,
                        Должность = x.Inspector.Position
                    })
                    .ToArray();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "Инспекторы",
                    MetaType = nameof(Object),
                    Data = inspectors
                });

                if (act.Inspection.Contragent != null)
                {
                    var contragent = act.Inspection.Contragent;

                    var contragents = new
                    {
                        Наименование = contragent.Name,
                        ЮрАдрес = contragent.JuridicalAddress,
                        ФактАдрес = contragent.FactAddress,
                        ИНН = contragent.Inn,
                        ОГРН = contragent.Ogrn,
                        НаименованиеСокр = contragent.ShortName
                    };

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "Контрагенты",
                        MetaType = nameof(Object),
                        Data = contragents
                    });
                }

                Disposal disposal = null;

                var parentDisposal = GetParentDocument(act, TypeDocumentGji.Disposal);

                if (parentDisposal != null)
                {
                    disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == parentDisposal.Id);
                }

                if (disposal != null)
                {
                    var disposalInfo = new
                    {
                        Номер = disposal.DocumentNumber,
                        Дата = disposal.DocumentDate.ToDateString(),
                        ВидПроверки = disposal.KindCheck.Return(x => x.Name),
                        ДолжностьРуководителяРП = disposal.IssuedDisposal.Return(x => x.PositionGenitive),
                        РуководительРП = disposal.IssuedDisposal.Return(x => x.FioGenitive),
                    };

                    this.DataSources.Add(new MetaData
                    {
                        SourceName = "Приказ",
                        MetaType = nameof(Object),
                        Data = disposalInfo
                    });
                }

                // получаем адрес дома и место составления
                var realityObjAddress = string.Empty;
                var realityObjPlaceName = string.Empty;
                var actCheckRealityObjs = actRoDomain.GetAll().Where(x => x.ActCheck.Id == DocumentId).ToArray();
                
                if (actCheckRealityObjs.Length == 1)
                {
                    var actCheckRealityObj = actCheckRealityObjs.FirstOrDefault();
                    
                    if (actCheckRealityObj != null)
                    {
                        realityObjPlaceName = actCheckRealityObj.RealityObject.Return(x => x.FiasAddress).Return(x => x.PlaceName) ?? "";
                        realityObjAddress = actCheckRealityObj.RealityObject.Return(x => x.Address) ?? "";
                    }
                }

                var roInfos = new
                {
                    //костыль, но щито поделать?!
                    Адрес =
                        realityObjAddress.Contains(realityObjPlaceName)
                            ? realityObjAddress
                            : "{0}, {1}".FormatUsing(realityObjPlaceName, realityObjAddress),
                    МестоСоставления = realityObjPlaceName
                };

                this.DataSources.Add(new MetaData
                {
                    SourceName = "АдресДома",
                    MetaType = nameof(Object),
                    Data = roInfos
                });

                var witness = actCheckWitnessDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .Select(x => new
                    {
                        ФИО = x.Fio,
                        Должность = x.Position,
                        Ознакомлен = x.IsFamiliar
                    })
                    .ToArray();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ЛицаПрисутствующиеНаПроверке",
                    MetaType = nameof(Object),
                    Data = witness
                });

                var actProvidedDocs = actCheckProvidedDocDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new
                    {
                        Наименование = x.ProvidedDoc.Name,
                        ДатаПредоставления = x.DateProvided
                    })
                    .ToArray();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПредоставленныеДокументы",
                    MetaType = nameof(Object),
                    Data = actProvidedDocs
                });

                var actCheckPeriods = actCheckPeriodDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.DateCheck,
                        x.DateEnd,
                        x.DateStart
                    })
                    .ToArray();

				var actCheckTime = actCheckPeriods
					.OrderBy(x => x.DateCheck)
					.ThenBy(x => x.DateEnd)
					.LastOrDefault();

	            if (actCheckTime != null)
	            {
		            this.ReportParams["ВремяСоставленияАкта"] = actCheckTime.DateEnd != null
			            ? actCheckTime.DateEnd.Value.ToShortTimeString()
			            : "";
	            }

				var actCheckPeriodsWithDuration = actCheckPeriods
		            .Select(x => new
		            {
			            Дата = x.DateCheck,
			            Продолжительность = (x.DateEnd.HasValue && x.DateStart.HasValue)
				            ? x.DateEnd - x.DateStart
				            : TimeSpan.MinValue
		            }).ToArray();

				var totalTime = actCheckPeriodsWithDuration.Select(x => x.Продолжительность.Value).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
				this.ReportParams["ОбщаяПродолжительностьПроверки"] = ConvertDuration(totalTime);

				var actCheckPeriodsForReport = actCheckPeriodsWithDuration.Select(x => new
		            {
			            x.Дата,
			            Продолжительность = ConvertDuration(x.Продолжительность)
		            }).ToArray();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ДатаИВремяПроведенияПроверки",
                    MetaType = nameof(Object),
                    Data = actCheckPeriodsForReport
                });
            }
            finally
            {
                Container.Release(actCheckDomain);
                Container.Release(disposalDomain);
                Container.Release(actCheckProvidedDocDomain);
                Container.Release(actRoDomain);
                Container.Release(actCheckWitnessDomain);
                Container.Release(actCheckViolationDomain);
                Container.Release(actCheckPeriodDomain);
                Container.Release(docInspDomain);
                Container.Release(actCheckLongDescDomain);
            }
        }

	    private string ConvertDuration(TimeSpan? source)
	    {
		    return source.HasValue && source != TimeSpan.MinValue
			    ? (
				    (source.Value.Days > 0
					    ? string.Format("{0} д. ", source.Value.Days)
					    : string.Empty)
				    +
				    (source.Value.Hours > 0
					    ? string.Format("{0} ч. ", source.Value.Hours)
					    : string.Empty)
				    +
				    (source.Value.Minutes > 0
					    ? string.Format("{0} мин. ", source.Value.Minutes)
					    : string.Empty)
				    )
			    : string.Empty;
	    }
    }
}