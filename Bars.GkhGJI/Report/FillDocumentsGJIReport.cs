namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет "Заполнение данных в Реестре ГЖИ"
    /// </summary>
    public class FillDocumentsGjiReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private List<long> municipalityListId = new List<long>(); 

        public FillDocumentsGjiReport()
            : base(new ReportTemplateBinary(Properties.Resources.FillDocumentsGji))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.FillDocumentsGji";
            }
        }

        public override string  Desciption
        {
            get { return "Отчет \"Заполнение данных в Реестре ГЖИ\""; }
        }

        public override string  GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string  ParamsController
        {
            get { return "B4.controller.report.FillDocumentsGji"; }
        }

        public override string Name
        {
            get { return "Отчет \"Заполнение данных в Реестре ГЖИ\""; }
        }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id;
                    if (long.TryParse(id, out Id))
                    {
                        if (!list.Contains(Id))
                        {
                            list.Add(Id);
                        }
                    }
                }
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateStart = baseParams.Params["dateStart"].ToDateTime();
            this.dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            this.ParseIds(this.municipalityListId, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            // словарь мун.образований по зональным инспекциям
            var zonalInspMuDictionary =
                Container.Resolve<IDomainService<ZonalInspectionMunicipality>>()
                         .GetAll()
                         .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.Municipality.Id))
                         .Select(x => new
                                 {
                                     ZonalInspectionId = x.ZonalInspection.Id,
                                     MunicipalityId = x.Municipality.Id
                                 })
                         .AsEnumerable()
                         .GroupBy(x => x.ZonalInspectionId)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.MunicipalityId).ToList());

            // словарь наименований зональных инспекций по ид
            var zonalInspectionsNames =
                Container.Resolve<IDomainService<ZonalInspectionMunicipality>>()
                         .GetAll()
                         .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.Municipality.Id))
                         .Select(
                             x =>
                             new
                             {
                                 ZonalInspectionId = x.ZonalInspection.Id,
                                 ZonalInspectionName = x.ZonalInspection.Name,
                                 ZonalInspectionZoneName = x.ZonalInspection.ZoneName,
                             })
                         .AsEnumerable()
                         .Distinct()
                         .GroupBy(x => x.ZonalInspectionId)
                         .ToDictionary(
                             x => x.Key,
                             x => x.Select(y => new
                                     {
                                         y.ZonalInspectionName,
                                         y.ZonalInspectionZoneName
                                     })
                                   .FirstOrDefault());

            // словарь наименований мун. образований
            var municipalitysNames = Container.Resolve<IDomainService<Municipality>>()
                         .GetAll()
                         .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.Id))
                         .Select(
                             x =>
                             new
                             {
                                 MunicipalityId = x.Id,
                                 MunicipalityName = x.Name,
                             })
                         .AsEnumerable()
                         .GroupBy(x => x.MunicipalityId)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.MunicipalityName).FirstOrDefault());

            // Количество актов по МО
            var actsCount = Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.RealityObject.Municipality.Id) 
                    && x.ActCheck.DocumentDate >= this.dateStart
                    && x.ActCheck.DocumentDate <= this.dateEnd)
                .Where(x => x.RealityObject != null)
                .GroupBy(x => x.RealityObject.Municipality.Id) 
                .Select(x => new
                        {
                            x.Key,
                            count = x.Select(y => y.ActCheck.Id).Distinct().Count()
                        })
                .ToDictionary(x => x.Key, y => y.count);

            // Количество актов проверки предписаний по МО
            var actsRemovalCount = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.InspectionViolation.RealityObject.Municipality.Id)
                    && x.Document.DocumentDate >= this.dateStart
                    && x.Document.DocumentDate <= this.dateEnd
                    && x.Document.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                .Where(x => x.InspectionViolation.RealityObject != null)
                .GroupBy(x => x.InspectionViolation.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    count = x.Select(y => y.Document.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, y => y.count);

            // Объединяем акты проверок и акты проверки предписаний
            actsRemovalCount.ForEach(x => actsCount[x.Key] = x.Value + (actsCount.ContainsKey(x.Key) ? actsCount[x.Key] : 0));

            // Количество протоколов по МО
            var protocolCount = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.InspectionViolation.RealityObject.Municipality.Id)
                    && x.Document.DocumentDate >= this.dateStart
                    && x.Document.DocumentDate <= this.dateEnd)
                .Where(x => x.InspectionViolation.RealityObject != null)
                .GroupBy(x => x.InspectionViolation.RealityObject.Municipality.Id)
                .Select(x => new
                            {
                                x.Key,
                                Count = x.Select(y => y.Document.Id).Distinct().Count()
                            })
                .ToDictionary(x => x.Key, y => y.Count);

            // Количество предписаний по МО
            var prescriptionCount = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.InspectionViolation.RealityObject.Municipality.Id)
                    && x.Document.DocumentDate >= this.dateStart
                    && x.Document.DocumentDate <= this.dateEnd)
                .Where(x => x.InspectionViolation.RealityObject != null)
                .GroupBy(x => x.InspectionViolation.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    Count = x.Select(y => y.Document.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, y => y.Count);

            var inspectionsOfDocumentsAtPeriodQuery = Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                .Where(x => x.DocumentDate >= this.dateStart && x.DocumentDate <= this.dateEnd)
                .Select(x => x.Inspection.Id);

            // Получение dictionary(Inspection.Id : Municipality.Id)
            var inspectionToMunicipality = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                //.Where(x => this.municipalityListId.Contains(x.RealityObject.Municipality.Id)) чтобы всегда совпадал МО с вьюшкой, это условие надо убирать
                .Where(x => inspectionsOfDocumentsAtPeriodQuery.Contains(x.Inspection.Id))
                .Select(x => new
                {
                    InspectionId = x.Inspection.Id,
                    MunicipalityId = x.RealityObject.Municipality.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.InspectionId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.MunicipalityId).OrderBy(y => y).FirstOrDefault());

            // Количество распоряжений по МО
            var disposalCount = Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Where(x => x.DocumentDate >= this.dateStart && x.DocumentDate <= this.dateEnd)
                .Select(x => new
                        {
                            InspectionId = x.Inspection.Id,
                            DocumentId = x.Id
                        })
                .AsEnumerable()
                .Distinct()
                .Select(x => inspectionToMunicipality.ContainsKey(x.InspectionId) ? inspectionToMunicipality[x.InspectionId] : -1)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            // Подзапрос на постановления прокуратуры
            var resolutionProcParentQuery = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution
                        && x.Children.DocumentDate >= this.dateStart
                        && x.Children.DocumentDate <= this.dateEnd
                        && x.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                .Select(x => x.Parent.Id);

            // Подзапрос на постановления (кроме постановлений прокуратуры)
            var resolutionParentQuery = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution
                        && x.Children.DocumentDate >= this.dateStart
                        && x.Children.DocumentDate <= this.dateEnd
                        && x.Parent.TypeDocumentGji != TypeDocumentGji.ResolutionProsecutor)
                .Select(x => x.Parent.Id);

            // Определение МО постановления прокуратуры
            var resolutionProcMu = Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
               .Where(x => resolutionProcParentQuery.Contains(x.ResolPros.Id))
               .Select(x => new
               {
                   x.ResolPros.Id,
                   municipalityId = x.RealityObject.Municipality.Id
               })
               .AsEnumerable()
               .GroupBy(x => x.Id)
               .ToDictionary(x => x.Key, x => x.Select(y => y.municipalityId).OrderBy(y => y).FirstOrDefault());

            // Определение МО постановления через родителя (кроме постановлений прокуратуры)
            var resolutionParentMunicipalityDict = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                .Where(x => resolutionParentQuery.Contains(x.Document.Id))
                .Where(x => x.InspectionViolation.RealityObject != null)
                .Select(x => new
                {
                    x.Document.Id,
                    municipalityId = x.InspectionViolation.RealityObject.Municipality.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.municipalityId).OrderBy(y => y).FirstOrDefault());

            // Постановления
            var resolutionCount = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution
                        && x.Children.DocumentDate >= this.dateStart
                        && x.Children.DocumentDate <= this.dateEnd)
                .Select(x => new
                {
                    ResolutionId = x.Children.Id,
                    isResolutionPros = x.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor,
                    parentId = x.Parent.Id
                })
                .AsEnumerable()
                .Distinct()
                .Select(x =>
                    {
                        long muId = -1;
                        // Если постановление прокуратуры, то берем МО постановления прокуратуры, иначе МО определяем через родительский документ
                        if (x.isResolutionPros)
                        {
                            if (resolutionProcMu.ContainsKey(x.parentId))
                            {
                                muId = resolutionProcMu[x.parentId];
                            }
                        }
                        else
                        {
                            if (resolutionParentMunicipalityDict.ContainsKey(x.parentId))
                            {
                                muId = resolutionParentMunicipalityDict[x.parentId];
                            }
                        }

                        return new { muId, x.ResolutionId };
                    })
                .GroupBy(x => x.muId)
                .ToDictionary(x => x.Key, x => x.Count());
            
            reportParams.SimpleReportParams["DateEnd"] = this.dateEnd.ToShortDateString();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");

            var totals = new int[5];

            foreach (var keyZonalInsp in zonalInspMuDictionary.Keys)
            {
                var currentZonalInspMuIds = municipalityListId.Count > 0 ? 
                                                zonalInspMuDictionary[keyZonalInsp].Intersect(this.municipalityListId).ToList()
                                                : zonalInspMuDictionary[keyZonalInsp];

                if (currentZonalInspMuIds.Any())
                {
                    section.ДобавитьСтроку();

                    var zonalInspTotals = new int[5];

                    var innerSection = section.ДобавитьСекцию("InnerSection");
                    
                    section["ZonalInspection"] = (zonalInspectionsNames[keyZonalInsp].ZonalInspectionZoneName == null 
                                                  || zonalInspectionsNames[keyZonalInsp].ZonalInspectionZoneName.Equals(string.Empty))
                                                    ? zonalInspectionsNames[keyZonalInsp].ZonalInspectionName
                                                    : zonalInspectionsNames[keyZonalInsp].ZonalInspectionZoneName;

                    var num = 0;

                    foreach (var currentZonalInspMuId in currentZonalInspMuIds)
                    {
                        innerSection.ДобавитьСтроку();

                        if (municipalitysNames.ContainsKey(currentZonalInspMuId))
                        {
                            innerSection["Municipality"] = municipalitysNames[currentZonalInspMuId] ?? string.Empty;
                        }

                        innerSection["Number1"] = string.Format("{0:D2}", ++num);

                        if (actsCount.ContainsKey(currentZonalInspMuId))
                        {
                            innerSection["Act"] = actsCount[currentZonalInspMuId];
                            zonalInspTotals[0] += actsCount[currentZonalInspMuId];
                        }
                        else
                        {
                            innerSection["Act"] = 0;
                        }

                        if (protocolCount.ContainsKey(currentZonalInspMuId))
                        {
                            innerSection["Protocol"] = protocolCount[currentZonalInspMuId];
                            zonalInspTotals[1] += protocolCount[currentZonalInspMuId];
                        }
                        else
                        {
                            innerSection["Protocol"] = 0;
                        }

                        if (prescriptionCount.ContainsKey(currentZonalInspMuId))
                        {
                            innerSection["Prescription"] = prescriptionCount[currentZonalInspMuId];
                            zonalInspTotals[2] += prescriptionCount[currentZonalInspMuId];
                        }
                        else
                        {
                            innerSection["Prescription"] = 0;
                        }

                        if (disposalCount.ContainsKey(currentZonalInspMuId))
                        {
                            innerSection["Disposal"] = disposalCount[currentZonalInspMuId];
                            zonalInspTotals[3] += disposalCount[currentZonalInspMuId];
                        }
                        else
                        {
                            innerSection["Disposal"] = 0;
                        }

                        if (resolutionCount.ContainsKey(currentZonalInspMuId))
                        {
                            innerSection["Resolution"] = resolutionCount[currentZonalInspMuId];
                            zonalInspTotals[4] += resolutionCount[currentZonalInspMuId];
                        }
                        else
                        {
                            innerSection["Resolution"] = 0;
                        }
                    }

                    section["ActZonalTotal"] = zonalInspTotals[0];
                    totals[0] += zonalInspTotals[0];
                    section["ProtocolZonalTotal"] = zonalInspTotals[1];
                    totals[1] += zonalInspTotals[1];
                    section["PrescriptionZonalTotal"] = zonalInspTotals[2];
                    totals[2] += zonalInspTotals[2];
                    section["DisposalZonalTotal"] = zonalInspTotals[3];
                    totals[3] += zonalInspTotals[3];
                    section["ResolutionZonalTotal"] = zonalInspTotals[4];
                    totals[4] += zonalInspTotals[4];
                }
            }

            reportParams.SimpleReportParams["ActTotal"] = totals[0];
            reportParams.SimpleReportParams["ProtocolTotal"] = totals[1];
            reportParams.SimpleReportParams["PrescriptionTotal"] = totals[2];
            reportParams.SimpleReportParams["DisposalTotal"] = totals[3];
            reportParams.SimpleReportParams["ResolutionTotal"] = totals[4];
        }
    }
}