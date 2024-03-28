namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ReportOnCourseOfHeatingSeason : BasePrintForm
    {
        #region Параметры отчета
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private List<long> municipalityIds = new List<long>();
        #endregion

        #region Вспомогательные поля
        private Dictionary<long, List<DataOnResolution>> dictDataOnResolution = new Dictionary<long, List<DataOnResolution>>();
        private DataOnReport dataReport;
        private DataOnReport dataZji;
        #endregion

        public IWindsorContainer Container { get; set; }

        public ReportOnCourseOfHeatingSeason()
            : base(new ReportTemplateBinary(Properties.Resources.ReportOnCourseOfHeatingSeason))
        {
        }

        public override string Name
        {
            get { return "Отчет по ходу отопительного сезона"; }
        }

        public override string Desciption
        {
            get { return "Отчет по ходу отопительного сезона"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ReportOnCourseOfHeatingSeason"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GJI.ReportOnCourseOfHeatingSeason"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateStart = baseParams.Params["dateStart"].ToDateTime();
            this.dateEnd = baseParams.Params["dateEnd"].ToDateTime();

            var m = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds.AddRange(!string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()) : new long[0]);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var sectionZji = reportParams.ComplexReportParams.ДобавитьСекцию("SectionZji");
            var sectionData = sectionZji.ДобавитьСекцию("SectionData");

            var juneOne = new DateTime(this.dateEnd.Year, 6, 1);

            reportParams.SimpleReportParams["началоПериода"] = this.dateStart.ToString("d MMMM");
            reportParams.SimpleReportParams["конецПериода"] = this.dateEnd.ToString("d MMMM");
            reportParams.SimpleReportParams["год1"] = this.dateEnd.Date < juneOne ? this.dateEnd.Year - 1 : this.dateEnd.Year;
            reportParams.SimpleReportParams["год2"] = this.dateEnd.Date < juneOne ? this.dateEnd.Year : this.dateEnd.Year + 1;

            var dictZji = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                                    .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Municipality.Id))
                                    .Select(x => new
                                    {
                                        zonalInspection = new ZonalInspectionInfoProxy
                                        {
                                            zonalInspectionId = x.ZonalInspection.Id,
                                            zoneName = x.ZonalInspection.ZoneName,
                                        },
                                        municipalityId = x.Municipality.Id,
                                        municipalityName = x.Municipality.Name
                                    })
                                    .AsEnumerable()
                                    .GroupBy(x => x.zonalInspection)
                                    .ToDictionary(
                                    x => x.Key, 
                                    x => x.Select(y => new MunicipaltyInfoProxy { municipalityId = y.municipalityId, municipalityName = y.municipalityName }).ToList());

            this.dataReport = new DataOnReport();

            this.DataMunicipality();

            foreach (var zji in dictZji)
            {
                sectionZji.ДобавитьСтроку();
                sectionZji["ЗЖИ"] = zji.Key.zoneName;

                foreach (var municipality in zji.Value)
                {
                    sectionData.ДобавитьСтроку();
                    sectionData["Мо"] = municipality.municipalityName;

                    this.FillSection(sectionData, municipality.municipalityId);
                }

                this.SumZji(zji);
                this.FillSection(sectionZji, this.dataZji, zji.Key.zonalInspectionId);
            }

            var sumZjItotal = this.SumZjiTotal(dictZji);
            this.FillSectionTotal(reportParams, sumZjItotal);
        }

        private void DataMunicipality()
        {
            var serviceDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var col2 = this.Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                .WhereIf(this.dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= this.dateStart)
                .WhereIf(this.dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= this.dateEnd)
                .Where(x => x.InspectionViolation.DateFactRemoval <= this.dateEnd)
                .Where(x => x.InspectionViolation.RealityObject != null)
                .GroupBy(x => new
                {
                    municipalityId = x.InspectionViolation.RealityObject.Municipality.Id,
                    prescriptionId = x.Document.Id
                })
                .Select(x => x.Key)
                .AsEnumerable()
                .GroupBy(x => x.prescriptionId)
                .Select(x => new { prescriptionId = x.Key, municiplalityId = x.Select(y => y.municipalityId).OrderBy(y => y).First() })
                .GroupBy(x => x.municiplalityId)
                .ToDictionary(x => x.Key, x => (long)x.Select(z => z.prescriptionId).Count());

            // выдано
            this.dataReport.Column2 = col2;

            var col3 = this.Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                .Join(
                    serviceDocumentGjiChildren.GetAll(),
                    x => x.Document.Id,
                    y => y.Parent.Id,
                    (a, b) => new { PrescriptionViol = a, DocumentGjiChildren = b, })
                .Join(
                        this.Container.Resolve<IDomainService<ActRemoval>>().GetAll(),
                        x => x.DocumentGjiChildren.Children.Id,
                        y => y.Id,
                        (c, d) => new { c.DocumentGjiChildren, c.PrescriptionViol, ActRemoval = d })
                        .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                        .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .WhereIf(this.dateStart != DateTime.MinValue, x => x.PrescriptionViol.Document.DocumentDate >= this.dateStart)
                        .WhereIf(this.dateEnd != DateTime.MinValue, x => x.PrescriptionViol.Document.DocumentDate <= this.dateEnd)
                        .Where(x => x.PrescriptionViol.InspectionViolation.RealityObject != null)
                        .Where(x => x.ActRemoval.TypeRemoval == YesNoNotSet.Yes)
                        .GroupBy(x => new
                        {
                            municiplalityId = x.PrescriptionViol.InspectionViolation.RealityObject.Municipality.Id,
                            parentId = x.DocumentGjiChildren.Parent.Id
                        })
                        .Select(x => x.Key)
                        .AsEnumerable()
                        .GroupBy(x => x.parentId)
                        .Select(x => new { parentId = x.Key, municiplalityId = x.Select(y => y.municiplalityId).OrderBy(y => y).First() })
                        .GroupBy(x => x.municiplalityId)
                        .ToDictionary(x => x.Key, x => (long)x.Select(z => z.parentId).Count());

            // выполнено
            this.dataReport.Column3 = col3;

            var col4 = this.Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                .Join(
                    serviceDocumentGjiChildren.GetAll(),
                    x => x.Document.Id,
                    y => y.Children.Id,
                    (a, b) => new { PrescriptionViol = a, DocumentGjiChildren = b })
                    .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.PrescriptionViol.Document.DocumentDate >= this.dateStart)
                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.PrescriptionViol.Document.DocumentDate <= this.dateEnd)
                    .Where(x => !serviceDocumentGjiChildren.GetAll()
                            .Where(y => y.Parent.Id == x.PrescriptionViol.Document.Id)
                            .Where(y => y.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                            .Any(y => y.Children.TypeDocumentGji == TypeDocumentGji.Disposal))
                    .Where(x => !serviceDocumentGjiChildren.GetAll()
                            .Where(y => y.Children.Id == x.DocumentGjiChildren.Parent.Id)
                            .Where(y => y.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                            .Any(y => y.Parent.TypeDocumentGji == TypeDocumentGji.Prescription))
                    .GroupBy(x => new
                    {
                        municiplalityId = x.PrescriptionViol.InspectionViolation.RealityObject.Municipality.Id,
                        parentId = x.DocumentGjiChildren.Parent.Id
                    })
                    .Select(x => x.Key)
                    .AsEnumerable()
                    .GroupBy(x => x.parentId)
                    .Select(x => new { parentId = x.Key, municiplalityId = x.Select(y => y.municiplalityId).OrderBy(y => y).First() })
                    .GroupBy(x => x.municiplalityId)
                    .ToDictionary(x => x.Key, x => (long)x.Select(z => z.parentId).Count());

            this.dataReport.Column4 = col4;

            // Протоколы
            var listProtocolsMuId = this.Container.Resolve<IDomainService<ProtocolViolation>>()
                                         .GetAll()
                                         .Where(x => x.InspectionViolation.RealityObject != null)
                                         .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                                         .WhereIf(this.dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= this.dateStart)
                                         .WhereIf(this.dateEnd != DateTime.MaxValue, x => x.Document.DocumentDate <= this.dateEnd)
                                         .Select(x => new { docId = x.Document.Id, muId = x.InspectionViolation.RealityObject.Municipality.Id })
                                         .ToList();

            this.dataReport.Column5 = listProtocolsMuId.Distinct(x => x.docId).GroupBy(x => x.muId).ToDictionary(x => x.Key, v => (long)v.Count());

            // Постановления.
            // Для того чтобы получить постановления по Мо необходимо получить родительские нарушения и
            // через дом получить МО

            // Постановления прокуратуры
            var col6_1 = this.Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll(),
                    x => x.ResolPros.Id,
                    y => y.Parent.Id,
                    (a, b) => new { ResolPros = a, DocumentGjiChildren = b })
                .Join(
                    this.Container.Resolve<IDomainService<Resolution>>().GetAll(),
                    x => x.DocumentGjiChildren.Children.Id,
                    y => y.Id,
                    (c, d) => new { c.DocumentGjiChildren, c.ResolPros, Resolution = d })
                    .Where(x => x.ResolPros.RealityObject.Municipality != null)
                    .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ResolPros.RealityObject.Municipality.Id))
                    .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate >= this.dateStart)
                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate <= this.dateEnd)
                    .Where(x => x.Resolution.Executant != null)
                    .Where(x => x.Resolution.Sanction != null)
                    .Select(x => new ResolutionProxy
                    {
                        ResolId = x.Resolution.Id,
                        ExecutantCode = x.Resolution.Executant.Code,
                        SanctionCode = x.Resolution.Sanction.Code,
                        PenaltyAmount = x.Resolution.PenaltyAmount,
                        ContrgName = x.Resolution.Contragent.Name,
                        PhysicalPerson = x.Resolution.PhysicalPerson,
                        DateTransferSsp = x.Resolution.DateTransferSsp,
                        Address = x.ResolPros.RealityObject.Address,
                        MuId = x.ResolPros.RealityObject.Municipality.Id,
                        ChildId = x.DocumentGjiChildren.Children.Id,
                        ParentId = x.DocumentGjiChildren.Parent.Id,
                        TypeDocumentParent = x.DocumentGjiChildren.Parent.TypeDocumentGji
                    })
                    .AsEnumerable()
                    .Distinct()
                    .ToList();

            // Постановления через родит.нарушения
            var col6_2 = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll(),
                    x => x.Document.Id,
                    y => y.Parent.Id,
                    (a, b) => new { Doc = a, DocumentGjiChildren = b })
                .Join(
                    this.Container.Resolve<IDomainService<Resolution>>().GetAll(),
                    x => x.DocumentGjiChildren.Children.Id,
                    y => y.Id,
                    (c, d) => new { c.DocumentGjiChildren, c.Doc, Resolution = d })
                    .Where(x => x.Doc.InspectionViolation.RealityObject.Municipality != null)
                    .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Doc.InspectionViolation.RealityObject.Municipality.Id))
                    .Where(x => x.Doc.Document.TypeDocumentGji == TypeDocumentGji.Prescription || x.Doc.Document.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate >= this.dateStart)
                    .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate <= this.dateEnd)
                    .Where(x => x.Resolution.Executant != null)
                    .Where(x => x.Resolution.Sanction != null)
                    .Select(x => new ResolutionProxy
                    {
                        ResolId = x.Resolution.Id,
                        ExecutantCode = x.Resolution.Executant.Code,
                        SanctionCode = x.Resolution.Sanction.Code,
                        PenaltyAmount = x.Resolution.PenaltyAmount,
                        ContrgName = x.Resolution.Contragent.Name,
                        PhysicalPerson = x.Resolution.PhysicalPerson,
                        DateTransferSsp = x.Resolution.DateTransferSsp,
                        Address = x.Doc.InspectionViolation.RealityObject.Address,
                        MuId = x.Doc.InspectionViolation.RealityObject.Municipality.Id,
                        ChildId = x.DocumentGjiChildren.Children.Id,
                        ParentId = x.DocumentGjiChildren.Parent.Id,
                        TypeDocumentParent = x.DocumentGjiChildren.Parent.TypeDocumentGji
                    })
                    .AsEnumerable()
                    .Distinct()
                    .ToList();

            var resolutionMu = col6_1.Union(col6_2).ToList();

            var dictChildProtocolId = resolutionMu
                .Where(x => x.TypeDocumentParent == TypeDocumentGji.Protocol)
                .AsEnumerable()
                .GroupBy(x => x.ChildId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ParentId).FirstOrDefault());

            var col_6 = resolutionMu
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, x => (long)x.Select(y => y.ResolId).Distinct().Count());

            this.dataReport.Column6 = col_6;

            this.DataIntruder(resolutionMu, dictChildProtocolId);
        }

        private void DataIntruder(List<ResolutionProxy> resolutions, Dictionary<long, long> dictChildProtocolId)
        {
            if (this.municipalityIds.Count == 0)
            {
                this.municipalityIds = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .Select(x => x.Id)
                    .ToList();
            }

            foreach (var muId in this.municipalityIds)
            {
                this.dictDataOnResolution.Add(muId, new List<DataOnResolution>());
            }

            var col_12a = resolutions
                .Join(
                    this.Container.Resolve<IDomainService<Protocol>>().GetAll(),
                    x => x.ParentId,
                    y => y.Id,
                    (a, b) => new { Resol = a, Protoc = b })
                    .Where(x => x.Resol.TypeDocumentParent == TypeDocumentGji.Protocol)
                    .Select(x => new ProtocolData
                    {
                        docId = x.Protoc.Id,
                        number = x.Protoc.DocumentNumber,
                        dataDoc = x.Protoc.DocumentDate.HasValue
                                    ? (DateTime)x.Protoc.DocumentDate
                                    : DateTime.MinValue
                    })
                    .AsEnumerable()
                    .Distinct()
                    .GroupBy(x => x.docId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => new { y.number, y.dataDoc }).FirstOrDefault());

            var col_10_11 = resolutions
                .Join(
                    this.Container.Resolve<IDomainService<ResolutionPayFine>>().GetAll(),
                    x => x.ResolId,
                    y => y.Resolution.Id,
                    (a, b) => new { Resol = a, ResolPayFine = b })
                .Select(x => new ResolutionPayFineProxy
                {
                    docId = x.ResolPayFine.Resolution.Id,
                    Amount = x.ResolPayFine.Amount.ToDecimal()
                })
                .AsEnumerable()
                .Distinct()
                .GroupBy(x => x.docId)
                .ToDictionary(
                    x => x.Key, x => x.Sum(y => y.Amount).ToDecimal());


            var col_7a = resolutions
                .Join(
                    this.Container.Resolve<IDomainService<ResolutionDispute>>().GetAll(),
                    x => x.ResolId,
                    y => y.Resolution.Id,
                    (a, b) => new { Resol = a, ResolDispute = b })
                    .Where(x => x.ResolDispute.CourtVerdict != null)
                    .Where(x => x.ResolDispute.CourtVerdict.Code == "2")
                .Select(x => x.ResolDispute.Resolution.Id)
                .AsEnumerable()
                .Distinct()
                .ToList();

            var typeExecutantJur = new List<string> { "0", "2", "4", "8", "11", "18", "15", "9" };
            var typeExecutantPerson = new List<string> { "1", "3", "5", "6", "7", "17", "19", "16", "10", "12", "13", "14" };
            var typePosition = new List<string> { "13", "12", "16", "3", "5", "19", "10", "1" };

            foreach (var dataOnMu in resolutions.GroupBy(x => x.MuId))
            {
                var muid = dataOnMu.First().MuId;
                var listResolution = new List<DataOnResolution>();

                foreach (var resMu in dataOnMu)
                {
                    var item = new DataOnResolution();

                    item.Column7 = (resMu.SanctionCode == "1" && !col_7a.Contains(resMu.ResolId)) ? resMu.PenaltyAmount.ToDecimal() : 0M;

                    if (typeExecutantJur.Contains(resMu.ExecutantCode))
                    {
                        item.Column8 = resMu.ContrgName ?? string.Empty;
                    }
                    else
                    {
                        item.Column8 = typeExecutantPerson.Contains(resMu.ExecutantCode) ? resMu.PhysicalPerson : string.Empty;
                    }

                    item.Column9 = typePosition.Contains(resMu.ExecutantCode)
                                       ? (resMu.ContrgName ?? string.Empty)
                                       : string.Empty;

                    item.Column10 = col_10_11.ContainsKey(resMu.ResolId) ? col_10_11[resMu.ResolId] : 0M;

                    item.Column11 = resMu.DateTransferSsp.HasValue
                                        ? (col_10_11.ContainsKey(resMu.ResolId)
                                               ? col_10_11[resMu.ResolId]
                                               : 0M)
                                        : 0M;

                    if (dictChildProtocolId.ContainsKey(resMu.ResolId))
                    {
                        var dataProtocol = col_12a[dictChildProtocolId[resMu.ResolId]];

                        item.Column12 = string.Format("{0} от {1}", dataProtocol.number, dataProtocol.dataDoc.Date);
                    }
                    else
                    {
                        item.Column12 = string.Empty;
                    }

                    item.Column13 = resMu.Address;
                    listResolution.Add(item);
                }

                this.dictDataOnResolution[muid] = listResolution;
            }
        }

        private void FillSection(Section section, long muId)
        {
            section["Выдано"] = this.dataReport.Column2.ContainsKey(muId) ? this.dataReport.Column2[muId] : 0;
            section["Выполнено"] = this.dataReport.Column3.ContainsKey(muId) ? this.dataReport.Column3[muId] : 0;
            section["Контроль"] = this.dataReport.Column4.ContainsKey(muId) ? this.dataReport.Column4[muId] : 0;
            section["Протоколы"] = this.dataReport.Column5.ContainsKey(muId) ? this.dataReport.Column5[muId] : 0;
            section["Постановление"] = this.dataReport.Column6.ContainsKey(muId) ? this.dataReport.Column6[muId] : 0;

            this.FillSectionColmn7_13(section, muId);
        }

        private void FillSectionColmn7_13(Section section, long muId)
        {
            var listData = this.dictDataOnResolution.ContainsKey(muId)
                               ? this.dictDataOnResolution[muId]
                               : new List<DataOnResolution>();

            var count = 1;
            foreach (var data in listData)
            {
                if (count > 1)
                {
                    section.ДобавитьСтроку();
                    section["Выдано"] = string.Empty;
                    section["Выполнено"] = string.Empty;
                    section["Контроль"] = string.Empty;
                    section["Протоколы"] = string.Empty;
                    section["Постановление"] = string.Empty;
                }

                section["СуммаШтрафа"] = data.Column7;
                section["Нарушитель"] = data.Column8;
                section["Должность"] = data.Column9;
                section["ФактОплата"] = data.Column10;
                section["ПередачаВССП"] = data.Column11;
                section["НомерПротокола"] = data.Column12;
                section["Адрес"] = data.Column13;

                count++;
            }
        }

        private void FillSection(Section section, DataOnReport zji, long muId)
        {
            section["ИтогоЗЖИВыдано"] = zji.Column2[muId];
            section["ИтогоЗЖИВыполнено"] = zji.Column3[muId];
            section["ИтогоЗЖИКонтроль"] = zji.Column4[muId];
            section["ИтогоЗЖИПротоколы"] = zji.Column5[muId];
            section["ИтогоЗЖИПостановление"] = zji.Column6[muId];

            section["ИтогоЗЖИСуммаШтрафа"] = zji.Column7[muId];
            section["ИтогоЗЖИФактОплата"] = zji.Column10[muId];
            section["ИтогоЗЖИПередачаВССП"] = zji.Column11[muId];
        }

        private void FillSectionTotal(ReportParams report, DataOnReport zji)
        {
            report.SimpleReportParams["ИтогоВыдано"] = zji.Column2[0];
            report.SimpleReportParams["ИтогоВыполнено"] = zji.Column3[0];
            report.SimpleReportParams["ИтогоКонтроль"] = zji.Column4[0];
            report.SimpleReportParams["ИтогоПротоколы"] = zji.Column5[0];
            report.SimpleReportParams["ИтогоПостановление"] = zji.Column6[0];

            report.SimpleReportParams["ИтогоСуммаШтрафа"] = zji.Column7[0];
            report.SimpleReportParams["ИтогоФактОплата"] = zji.Column10[0];
            report.SimpleReportParams["ИтогоПередачаВССП"] = zji.Column11[0];
        }

        private void SumZji(KeyValuePair<ZonalInspectionInfoProxy, List<MunicipaltyInfoProxy>> zji)
        {
            if (dataZji == null)
            {
                dataZji = new DataOnReport();
            }

            foreach (var mu in zji.Value)
            {
                SumLong(ref dataZji.Column2, dataReport.Column2, mu.municipalityId, zji.Key.zonalInspectionId);
                SumLong(ref dataZji.Column3, dataReport.Column3, mu.municipalityId, zji.Key.zonalInspectionId);
                SumLong(ref dataZji.Column4, dataReport.Column4, mu.municipalityId, zji.Key.zonalInspectionId);
                SumLong(ref dataZji.Column5, dataReport.Column5, mu.municipalityId, zji.Key.zonalInspectionId);
                SumLong(ref dataZji.Column6, dataReport.Column6, mu.municipalityId, zji.Key.zonalInspectionId);

                if (dictDataOnResolution.ContainsKey(mu.municipalityId))
                {
                    SumDecimal(ref dataZji.Column7, dictDataOnResolution[mu.municipalityId], 7, zji.Key.zonalInspectionId);
                    SumDecimal(ref dataZji.Column10, dictDataOnResolution[mu.municipalityId], 11, zji.Key.zonalInspectionId);
                    SumDecimal(ref dataZji.Column11, dictDataOnResolution[mu.municipalityId], 12, zji.Key.zonalInspectionId);
                }
            }
        }

        private DataOnReport SumZjiTotal(Dictionary<ZonalInspectionInfoProxy, List<MunicipaltyInfoProxy>> dictZji)
        {
            var result = new DataOnReport();

            foreach (var zji in dictZji.Keys)
            {
                this.SumLong(ref result.Column2, dataZji.Column2, zji.zonalInspectionId);
                this.SumLong(ref result.Column3, dataZji.Column3, zji.zonalInspectionId);
                this.SumLong(ref result.Column4, dataZji.Column4, zji.zonalInspectionId);
                this.SumLong(ref result.Column5, dataZji.Column5, zji.zonalInspectionId);
                this.SumLong(ref result.Column6, dataZji.Column6, zji.zonalInspectionId);

                this.SumDecimal(ref result.Column7, dataZji.Column7, zji.zonalInspectionId);
                this.SumDecimal(ref result.Column10, dataZji.Column10, zji.zonalInspectionId);
                this.SumDecimal(ref result.Column11, dataZji.Column11, zji.zonalInspectionId);
            }

            return result;
        }

        private void SumLong(ref Dictionary<long, long> result, Dictionary<long, long> data, long Id = 0, long zjiId = 0)
        {
            if (result == null)
            {
                result = new Dictionary<long, long>();
            }

            if (data == null)
            {
                return;
            }

            if (result.ContainsKey(zjiId))
            {
                result[zjiId] += data.ContainsKey(Id) ? data[Id] : 0;
            }
            else
            {
                result.Add(zjiId, data.ContainsKey(Id) ? data[Id] : 0);
            }
        }

        private void SumDecimal(ref Dictionary<long, decimal> result, Dictionary<long, decimal> data, long Id = 0, long zjiId = 0)
        {
            if (result == null)
            {
                result = new Dictionary<long, decimal>();
            }

            if (data == null)
            {
                return;
            }

            if (result.ContainsKey(zjiId))
            {
                result[zjiId] += data.ContainsKey(Id) ? data[Id] : 0;
            }
            else
            {
                result.Add(zjiId, data.ContainsKey(Id) ? data[Id] : 0);
            }
        }

        private void SumDecimal(ref Dictionary<long, decimal> result, IEnumerable<DataOnResolution> listRes, long colNum, long zjiId = 0)
        {
            if (result == null)
            {
                result = new Dictionary<long, decimal>();
            }

            decimal sum = 0M;
            switch (colNum)
            {
                case 7:
                    sum = listRes.Sum(x => x.Column7).ToDecimal();
                    break;
                case 10:
                    sum = listRes.Sum(x => x.Column10).ToDecimal();
                    break;
                case 11:
                    sum = listRes.Sum(x => x.Column11).ToDecimal();
                    break;
            }

            if (result.ContainsKey(zjiId))
            {
                result[zjiId] += sum;
            }
            else
            {
                result.Add(zjiId, sum);
            }
        }

        private class DataOnReport
        {
            public Dictionary<long, long> Column2;
            public Dictionary<long, long> Column3;
            public Dictionary<long, long> Column4;
            public Dictionary<long, long> Column5;
            public Dictionary<long, long> Column6;
            public Dictionary<long, decimal> Column7;
            public Dictionary<long, long> Column8;
            public Dictionary<long, long> Column9;
            public Dictionary<long, decimal> Column10;
            public Dictionary<long, decimal> Column11;
            public Dictionary<long, long> Column12;
            public Dictionary<long, long> Column13;
        }

        /// <summary>
        /// Информация по нарушителю предписания.
        /// </summary>
        private class DataOnResolution
        {
            /// <summary>
            /// Сумма штрафов.
            /// </summary>
            public decimal Column7;

            /// <summary>
            /// Нарушитель.
            /// </summary>
            public string Column8;

            /// <summary>
            /// Должность.
            /// </summary>
            public string Column9;

            /// <summary>
            /// Фактическая оплата.
            /// </summary>
            public decimal Column10;

            /// <summary>
            /// напрвал. в ССП
            /// </summary>
            public decimal Column11;

            /// <summary>
            /// Номер протокола.
            /// </summary>
            public string Column12;

            /// <summary>
            /// Адрес.
            /// </summary>
            public string Column13;
        }

        private sealed class ResolutionProxy : IEquatable<ResolutionProxy>
        {
            public long ResolId { get; set; }
            public string ExecutantCode { get; set; }
            public string SanctionCode { get; set; }
            public Decimal? PenaltyAmount { get; set; }
            public string ContrgName { get; set; }
            public string PhysicalPerson { get; set; }
            public DateTime? DateTransferSsp { get; set; }
            public string Address { get; set; }
            public long MuId { get; set; }
            public long ChildId { get; set; }
            public long ParentId { get; set; }
            public TypeDocumentGji TypeDocumentParent { get; set; }

            public bool Equals(ResolutionProxy other)
            {
                if (Object.ReferenceEquals(other, null)) { return false; }
                if (Object.ReferenceEquals(this, other)) { return true; }

                var res = this.ResolId == other.ResolId
                       && this.ExecutantCode == other.ExecutantCode
                       && this.SanctionCode == other.SanctionCode
                       && this.PenaltyAmount == other.PenaltyAmount
                       && this.ContrgName == other.ContrgName
                       && this.PhysicalPerson == other.PhysicalPerson
                       && this.DateTransferSsp == other.DateTransferSsp
                       && this.Address == other.Address
                       && this.MuId == other.MuId
                       && this.ChildId == other.ChildId
                       && this.ParentId == other.ParentId
                       && this.TypeDocumentParent == other.TypeDocumentParent;

                return res;
            }

            public override int GetHashCode()
            {
                int hashResolId = this.ResolId.GetHashCode();
                int hashExecutantCode = this.ExecutantCode == null ? 0 : this.ExecutantCode.GetHashCode();
                int hashSanctionCode = this.SanctionCode == null ? 0 : this.SanctionCode.GetHashCode();
                int hashPenaltyAmount = this.PenaltyAmount == null ? 0 : this.PenaltyAmount.GetHashCode();
                int hashContrgName = this.ContrgName == null ? 0 : this.ContrgName.GetHashCode();
                int hashPhysicalPerson = this.PhysicalPerson == null ? 0 : this.PhysicalPerson.GetHashCode();
                int hashDateTransferSsp = this.DateTransferSsp == null ? 0 : this.DateTransferSsp.GetHashCode();
                int hashAddress = this.Address == null ? 0 : this.Address.GetHashCode();
                int hashMuId = this.MuId.GetHashCode();
                int hashChildId = this.ChildId.GetHashCode();
                int hashParentId = this.ParentId.GetHashCode();
                int hashTypeDocumentParent = this.TypeDocumentParent.GetHashCode();


                return hashResolId ^ hashExecutantCode ^ hashMuId ^ hashSanctionCode ^ hashPenaltyAmount
                       ^ hashContrgName ^ hashPhysicalPerson ^ hashDateTransferSsp ^ hashChildId
                       ^ hashParentId ^ hashAddress ^ hashTypeDocumentParent;
            }
        }

        private class ResolutionPayFineProxy
        {
            public long docId;

            public decimal Amount;
        }

        private class ProtocolData
        {
            public long docId;

            public string number;

            public DateTime dataDoc;
        }

        private class MunicipaltyInfoProxy : IEquatable<MunicipaltyInfoProxy>
        {
            public long municipalityId { get; set; }

            public string municipalityName { get; set; }

            public bool Equals(MunicipaltyInfoProxy other)
            {
                if (Object.ReferenceEquals(other, null)) { return false; }
                if (Object.ReferenceEquals(this, other)) { return true; }

                var res = this.municipalityId == other.municipalityId && this.municipalityName == other.municipalityName;

                return res;
            }

            public override int GetHashCode()
            {
                int hashMuId = this.municipalityId.GetHashCode();
                int hashMuName = this.municipalityName == null ? 0 : this.municipalityName.GetHashCode();

                return hashMuId ^ hashMuName;
            }
        }

        private class ZonalInspectionInfoProxy : IEquatable<ZonalInspectionInfoProxy>
        {
            public long zonalInspectionId { get; set; }

            public string zoneName { get; set; }

            public bool Equals(ZonalInspectionInfoProxy other)
            {
                if (Object.ReferenceEquals(other, null)) { return false; }
                if (Object.ReferenceEquals(this, other)) { return true; }

                var res = this.zonalInspectionId == other.zonalInspectionId && this.zoneName == other.zoneName;

                return res;
            }

            public override int GetHashCode()
            {
                int hashZjiId = this.zonalInspectionId.GetHashCode();
                int hashZoneName = this.zoneName == null ? 0 : this.zoneName.GetHashCode();

                return hashZjiId ^ hashZoneName;
            }
        }
    }
}