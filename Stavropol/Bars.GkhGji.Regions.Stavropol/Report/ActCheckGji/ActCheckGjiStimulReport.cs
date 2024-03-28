namespace Bars.GkhGji.Regions.Stavropol.Report.ActCheckGji
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Stavropol.Entities;
    using Bars.GkhGji.Report;

    using Bars.Gkh.Entities.Hcs;

    public class ActCheckGjiStimulReport : GjiBaseStimulReport
    {
        protected Disposal ParentDisposal;

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ActCheckGjiStimulReport"/> class.
        /// </summary>
        public ActCheckGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ActSurvey_all))
        {
        }
        #endregion

        /// <summary>
        /// Формат печатной формы.
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get
            {
                return "ActCheck";
            }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get
            {
                return "ActCheck";
            }
        }

        /// <summary>
        /// Наименование отчета.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Акт проверки";
            }
        }

        /// <summary>
        /// Описание отчета.
        /// </summary>
        public override string Description
        {
            get
            {
                return "Акт проверки";
            }
        }

        /// <summary>
        /// Код шаблона (файла).
        /// </summary>
        protected override string CodeTemplate { get; set; }

        private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры.
        /// </summary>
        /// <param name="userParamsValues">
        /// Значения пользовательских параметров.
        /// </param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
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
                                   Code = "BlockGJI_ActSurvey_all",
                                   Name = "ActSurvey",
                                   Description = "Акт проверки общий",
                                   Template = Properties.Resources.BlockGJI_ActSurvey_all
                               }
                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var actCheckDomain = Container.Resolve<IDomainService<ActCheck>>();
            var disposalDomain = Container.Resolve<IDomainService<Disposal>>();
            var typeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var inspectionRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var zonalInspectionMuDomain = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
            var containerInspectorDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var contragentContactDomain = Container.Resolve<IDomainService<ContragentContact>>();
            var actRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var typeSurveyGoalDomain = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>();
            var actAnnexDomain = Container.Resolve<IDomainService<ActCheckAnnex>>();
            var disposalExpertDomain = Container.Resolve<IDomainService<DisposalExpert>>();
            var disposalProvidedDocDomain = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var typeSurveyInspDomain = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>();
            var actCheckWitnessDomain = Container.Resolve<IDomainService<ActCheckWitness>>();
            var docChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var insappealCitsDomain = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var appealCitsRo = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var zonalInsInspectorDomain = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var actCheckTimeDomain = Container.Resolve<IDomainService<ActCheckTime>>();
            var baseProsClaimDomain = Container.Resolve<IDomainService<BaseProsClaim>>();

            var act = actCheckDomain.Get(this.DocumentId);
            if (act == null)
            {
                throw new ReportProviderException("Не удалось получить акт проверки");
            }

            var disposal = disposalDomain.Load(GetParentDocument(act, TypeDocumentGji.Disposal).Id);

            // зональную инспекцию получаем через муниципальное образование первого дома
            var firstRo = inspectionRoDomain.GetAll()
                .Where(x => x.Inspection.Id == act.Inspection.Id)
                .Select(x => x.RealityObject)
                .FirstOrDefault();

            if (firstRo != null)
            {
                var zonalInspection = zonalInspectionMuDomain.GetAll()
                    .Where(x => x.Municipality.Id == firstRo.Municipality.Id)
                    .Select(x => x.ZonalInspection)
                    .FirstOrDefault();

                if (zonalInspection != null)
                {
                    this.ReportParams["ЗональноеНаименование1ГосЯзык"] = zonalInspection.BlankName;
                }
            }

            var actCheckTime = actCheckTimeDomain.GetAll().FirstOrDefault(x => x.ActCheck.Id == act.Id);

            this.ReportParams["ДатаАкта"] = act.DocumentDate.HasValue
                                          ? act.DocumentDate.Value.ToString("d MMMM yyyy")
                                          : string.Empty;
            this.ReportParams["ВремяСоставления"] = actCheckTime != null
                                                  ? string.Format("{0:D2} час. {1:D2} мин.", actCheckTime.Hour, actCheckTime.Minute)
                                                  : string.Empty;
            this.ReportParams["НомерАкта"] = act.DocumentNumber;
            
            if (act.Inspection.Contragent != null)
            {
                var contragent = act.Inspection.Contragent;

                this.ReportParams["УправОрг"] = contragent.Name;
                this.ReportParams["ИННУправОрг"] = contragent.Inn;
                this.ReportParams["СокрУправОрг"] = contragent.ShortName;

                var headContragent = contragentContactDomain.GetAll()
                    .Where(x => x.Contragent.Id == contragent.Id)
                    .Where(x => x.DateStartWork.HasValue)
                    .Where(x => x.DateStartWork.Value <= DateTime.Now)
                    .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)
                    .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                if (headContragent != null)
                {
                    this.ReportParams["Руководитель"] = string.Format(
                        "{0} {1} {2} {3}",
                        headContragent.Position != null ? headContragent.Position.Name : string.Empty,
                        headContragent.Surname,
                        headContragent.Name,
                        headContragent.Patronymic);
                }

                this.ReportParams["АдресКонтрагента"] = contragent.FiasFactAddress.Return(x => x.AddressName);
            }

            this.ParentDisposal = disposal;

            // Дома акта проверки
            var actCheckRealtyObjects = actRoDomain.GetAll()
                    .Where(x => x.ActCheck.Id == this.DocumentId)
                    .Select(x => new
                                     {
                                         x.Id, 
                                         x.RealityObject, 
                                         x.HaveViolation, 
                                         x.Description, 
                                         x.NotRevealedViolations
                                     })
                    .ToArray();

            this.GetCodeTemplate();

            if (actCheckRealtyObjects.Length > 0)
            {
                var realtyObject = actCheckRealtyObjects.Select(x => x.RealityObject).FirstOrDefault();
                if (realtyObject != null)
                {
                    if (realtyObject.FiasAddress != null)
                    {
                        this.ReportParams["НаселенныйПункт"] = realtyObject.FiasAddress.PlaceName;
                    }
                }

                if (actCheckRealtyObjects.Count(x => x.RealityObject != null) == 1)
                {
                    this.ReportParams["МатериалСтен"] = realtyObject.WallMaterial != null
                                                      ? realtyObject.WallMaterial.Name
                                                      : string.Empty;
                    this.ReportParams["ОбщаяПлощадь"] = realtyObject.AreaMkd.HasValue
                                                      ? realtyObject.AreaMkd.Value.ToStr()
                                                      : string.Empty;
                    this.ReportParams["Этажей"] = realtyObject.Floors.HasValue
                                                ? realtyObject.Floors.Value.ToStr()
                                                : string.Empty;
                    this.ReportParams["ГодСдачи"] = realtyObject.DateCommissioning.HasValue
                                                  ? realtyObject.DateCommissioning.Value.ToShortDateString()
                                                  : string.Empty;
                    this.ReportParams["ФизИзнос"] = realtyObject.PhysicalWear.HasValue
                                                  ? realtyObject.PhysicalWear.Value.ToStr()
                                                  : string.Empty;
                    this.ReportParams["Подъезд"] = realtyObject.NumberEntrances.HasValue
                                                  ? realtyObject.NumberEntrances.Value.ToStr()
                                                  : string.Empty;
                    this.ReportParams["Кровля"] = realtyObject.RoofingMaterial != null
                                                ? realtyObject.RoofingMaterial.Name
                                                : string.Empty;
                    this.ReportParams["ЧислоКвартир"] = realtyObject.NumberApartments.HasValue
                                                  ? realtyObject.NumberApartments.Value.ToStr()
                                                  : string.Empty;
                    this.ReportParams["ДомАдрес"] = realtyObject.FiasAddress.AddressName;
                    this.ReportParams["Описание"] = actCheckRealtyObjects[0].Description;

                    var countPrivateRoom = this.Container.ResolveDomain<Room>().GetAll()
                        .Where(x => x.RealityObject.Id == realtyObject.Id)
                        .Count(x => x.OwnershipType == RoomOwnershipType.Private);
                    this.ReportParams["КвартирЧастнСобств"] = countPrivateRoom.ToStr();

                    var countHouseAccount = this.Container.ResolveDomain<HouseAccount>().GetAll()
                        .Count(x => x.RealityObject.Id == realtyObject.Id);
                    this.ReportParams["ЛицевыеСчета"] = countHouseAccount.ToStr();

                    var queryTehPassValue = this.Container.ResolveDomain<TehPassportValue>().GetAll();

                    var tehPassValue = queryTehPassValue
                        .Where(x => x.TehPassport.RealityObject.Id == realtyObject.Id)
                        .FirstOrDefault(x => x.FormCode == "Form_3_2CW_2" && x.CellCode == "1:4");
                    this.ReportParams["ОбщедомовыхХВС"] = tehPassValue == null ? string.Empty : tehPassValue.Value;

                    tehPassValue = queryTehPassValue
                        .Where(x => x.TehPassport.RealityObject.Id == realtyObject.Id)
                        .FirstOrDefault(x => x.FormCode == "Form_3_2_2" && x.CellCode == "1:4");
                    this.ReportParams["ОбщедомовыхГВС"] = tehPassValue == null ? string.Empty : tehPassValue.Value;
                }

                var actCheckRoId = actCheckRealtyObjects.Select(x => x.Id).FirstOrDefault();
                var violationsNames = this.Container.ResolveDomain<ActCheckViolation>()
                        .GetAll()
                        .Where(x => x.ActObject.Id == actCheckRoId)
                        .Select(viol => viol.InspectionViolation.Violation.Name)
                        .ToList();

                if (violationsNames.Count > 0)
                {
                    this.ReportParams["ТекстНарушения"] = string.Join(". ", violationsNames);
                }
            }

            if (disposal != null)
            {
                this.ReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                                                      ? disposal.DocumentDate.Value.ToString("d MMMM yyyy")
                                                      : string.Empty;
                this.ReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;

                var kindCheckName = disposal.KindCheck != null ? disposal.KindCheck.Name : string.Empty;
                this.ReportParams["ВидОбследования"] = kindCheckName;
                this.ReportParams["НачалоПериода"] = disposal.DateStart.HasValue
                                                   ? disposal.DateStart.Value.ToString("d MMMM yyyy")
                                                   : string.Empty;
                this.ReportParams["ОкончаниеПериода"] = disposal.DateEnd.HasValue
                                                      ? disposal.DateEnd.Value.ToString("d MMMM yyyy")
                                                      : string.Empty;
                this.ReportParams["СогласСПрокурат"] = disposal.TypeAgreementProsecutor.GetEnumMeta().Display;
            }

            var inspectors = containerInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == this.DocumentId)
                .Select(x => new
                {
                    x.Inspector.Id,
                    x.Inspector.Fio,
                    x.Inspector.Position,
                    x.Inspector.ShortFio,
                    x.Inspector.FioAblative
                })
                .ToArray();

            this.ReportParams["ИнспекторыИКоды"] = inspectors.Aggregate(
                string.Empty,
                (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Fio + " - " + y.Position : y.Fio + " - " + y.Position));

            var firstInspector = inspectors.FirstOrDefault();
            if (firstInspector != null && !string.IsNullOrEmpty(firstInspector.ShortFio))
            {
                this.ReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
            }

            this.FillCommonFields(act);
            
            this.FillCheckPeriods(reportParams, act);

            var actRealityObjectService = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var actCheckRo = actRealityObjectService.GetAll().FirstOrDefault(x => x.ActCheck.Id == act.Id);
            if (actCheckRo != null)
            {
                this.ReportParams["Описание"] = actCheckRo.Description;
            }

            if (act.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
            {
                var baseProsClaim = baseProsClaimDomain.Get(act.Inspection.Id);
                if (baseProsClaim != null)
                {
                    this.ReportParams["ДлВынесшееТребование"] = baseProsClaim.IssuedClaim;
                }
            }

            Container.Release(actCheckDomain);
            Container.Release(disposalDomain);
            Container.Release(typeSurveyDomain);
            Container.Release(typeSurveyDomain);
            Container.Release(inspectionRoDomain);
            Container.Release(zonalInspectionMuDomain);
            Container.Release(containerInspectorDomain);
            Container.Release(contragentContactDomain);
            Container.Release(actRoDomain);
            Container.Release(typeSurveyGoalDomain);
            Container.Release(actAnnexDomain);
            Container.Release(disposalExpertDomain);
            Container.Release(disposalProvidedDocDomain);
            Container.Release(typeSurveyInspDomain);
            Container.Release(actCheckWitnessDomain);
            Container.Release(docChildrenDomain);
            Container.Release(insappealCitsDomain);
            Container.Release(appealCitsRo);
            Container.Release(zonalInsInspectorDomain);
            Container.Release(baseProsClaimDomain);
        }

        protected virtual void FillCheckPeriods(ReportParams reportParams, DocumentGji doc)
        {
            var actPeriodDomain = Container.Resolve<IDomainService<ActCheckPeriod>>();

            var periodsCheck = actPeriodDomain.GetAll()
                .Where(x => x.ActCheck.Id == doc.Id && x.DateStart.HasValue && x.DateEnd.HasValue)
                .ToArray();

            var dc1 = new StringBuilder();
            
            foreach (var actCheckGjiPeriod in periodsCheck)
            {
                if (actCheckGjiPeriod != null && actCheckGjiPeriod.DateStart.HasValue && actCheckGjiPeriod.DateEnd.HasValue)
                {
                    if (dc1.Length > 0)
                    {
                        dc1.Append(" ## ");
                    }

                    var st = actCheckGjiPeriod.DateStart.Value.TimeOfDay;
                    var et = actCheckGjiPeriod.DateEnd.Value.TimeOfDay;

                    var checkLength = et - st;

                    var date1 = actCheckGjiPeriod.DateCheck.HasValue
                                    ? actCheckGjiPeriod.DateCheck.Value.ToString("D", new CultureInfo("ru-RU"))
                                    : string.Empty;

                    dc1.AppendFormat(
                        "{0} с {1} час. {2} мин. до {3} час. {4} мин. Продолжительность: {5} час. {6} мин.",
                        date1,
                        st.Hours,
                        st.Minutes,
                        et.Hours,
                        et.Minutes,
                        checkLength.Hours,
                        checkLength.Minutes);
                }
            }

            this.ReportParams["ДатаВремяПроверки"] = dc1.ToString();
            
            Container.Release(actPeriodDomain);
        }

        protected virtual void GetCodeTemplate()
        {
            this.CodeTemplate = "BlockGJI_ActSurvey_all";
        }

        protected virtual void FillSectionViolations(ReportParams reportParams, ActCheck act, long disposalId = 0)
        {
            var actCheckViolationDomain = Container.Resolve<IDomainService<ActCheckViolation>>();

            var serviceActCheckViolation = actCheckViolationDomain.GetAll();
            var queryActViols = serviceActCheckViolation.Where(x => x.Document.Id == act.Id);
            var actViols = queryActViols.Select(x => x.InspectionViolation.Violation)
                             .ToArray();

            #region Секция нарушений
            // На Ставрополе нет секций, если появятся, то расскомментировать и переделать под стимул
            /*
            if (actViols.Length > 0)
            {
                var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушений");

                var addedViols = new HashSet<long>();

                int i = 1;
                foreach (var viol in actViols.Where(viol => !addedViols.Contains(viol.Id)))
                {
                    addedViols.Add(viol.Id);

                    section.ДобавитьСтроку();
                    section["Номер1"] = i++;
                    section["Пункт"] = viol.CodePin;
                    section["ТекстНарушения"] = viol.Name;
                }
            }
            */
            #endregion

            Container.Release(actCheckViolationDomain);
        }
    }
}