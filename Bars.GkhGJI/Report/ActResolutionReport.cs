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

    public class ActResolutionReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;

        List<long> municipalities = new List<long>();
        public ActResolutionReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActResponseResolution))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ActResolution";
            }
        }

        public override string Desciption
        {
            get { return "Реестр постановлений"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Resolution"; }
        }

        public override string Name
        {
            get { return "Реестр постановлений"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();

            var m = baseParams.Params["municipalityIds"].ToString();
            municipalities.AddRange(!string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()) : new long[0]);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var documents = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                 .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                 .WhereIf(dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= dateStart)
                 .WhereIf(dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= dateEnd)
                 .Select(x => new
                 {
                     ParentId = x.Parent.Id,
                     ResolutionId = x.Children.Id,
                 })
                 .AsEnumerable()
                 .Distinct()
                 .ToDictionary(x => x.ResolutionId, y => y.ParentId);

            var resolutions = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new ResolutionProxy
                {
                    Id = x.Id,
                    Sanction = x.Sanction,
                    PenaltyAmount = x.PenaltyAmount,
                    ContragentName = x.Contragent.Name,
                    ExecutantCode = x.Executant.Code,
                    DocumentNumber = x.DocumentNumber,
                    DocumentDate = x.DocumentDate
                })
                .ToList();
            
            var serviceProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll();
            var protocolIdsQuery = serviceDocumentGjiChildren.GetAll()
                 .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                 .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                 .WhereIf(dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= dateStart)
                 .WhereIf(dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= dateEnd)
                 .Select(x => x.Parent.Id);

            var protocolRoList = serviceProtocolViolation
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                .Where(x => protocolIdsQuery.Contains(x.Document.Id))
                .Select(x => new
                {
                    x.Document.Id,
                    MunicipalityName = x.InspectionViolation.RealityObject.Municipality.Name,
                    x.InspectionViolation.RealityObject.Address
                })
                .ToList();

            var serviceResolProsRealityObject = Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll();
            var resolProsIdsQuery = serviceDocumentGjiChildren.GetAll()
                 .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                 .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                 .WhereIf(dateStart != DateTime.MinValue, x => x.Children.DocumentDate >= dateStart)
                 .WhereIf(dateEnd != DateTime.MinValue, x => x.Children.DocumentDate <= dateEnd)
                 .Select(x => x.Parent.Id);

            var resolProsList = serviceResolProsRealityObject
                              .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                              .Where(x => resolProsIdsQuery.Contains(x.ResolPros.Id))
                              .Select(x => new
                              {
                                  x.ResolPros.Id,
                                  MunicipalityName = x.RealityObject.Municipality.Name,
                                  x.RealityObject.Address
                              })
                              .ToList();
            
            var protocolRo = protocolRoList
                .Distinct()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => new { y.MunicipalityName, y.Address }).Distinct().ToList());

            var resolProsRo = resolProsList
               .Distinct()
               .GroupBy(x => x.Id)
               .ToDictionary(x => x.Key, x => x.Select(y => new { y.MunicipalityName, y.Address }).Distinct().ToList());

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var counter = 0;
            var contragentTypes1 = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
            var contragentTypes2 = new List<string> { "1", "10", "12", "13", "16", "19", "5" };

            var municipalitiesNotSet = !municipalities.Any();

            var resolutionData = resolutions
                .Select(x =>
                    {
                        var parentId = documents.ContainsKey(x.Id) ? documents[x.Id] : 0;
                        var realtyObjectList = protocolRo.ContainsKey(parentId)
                                                   ? protocolRo[parentId]
                                                   : resolProsRo.ContainsKey(parentId) ? resolProsRo[parentId]: null;
                        return realtyObjectList != null 
                            ? realtyObjectList.Select(y => new { x , y }).ToList()
                            : municipalitiesNotSet ? Enumerable.Range(1, 1).Select(z => new { x, y = new { MunicipalityName = string.Empty, Address = string.Empty } }).ToList(): null;
                    })
                .Where(x => x != null)
                .SelectMany(x => x)
                .ToList();

            foreach (var resolution in resolutionData.OrderBy(x => x.y.MunicipalityName).ThenBy(x => x.y.Address))
            { 
                if (resolution.y.MunicipalityName == "")
                {
                    continue;
                }
                section.ДобавитьСтроку();
                section["Municipality"] = resolution.y.MunicipalityName;
                section["Address"] = resolution.y.Address;
                SetData(section, ++counter, resolution.x, contragentTypes1, contragentTypes2); 
                
            }
            
        }

        private void SetData(Section section, int counter, ResolutionProxy resolution, List<string> contragentTypes1, List<string> contragentTypes2)
        {
            section["Number1"] = counter;
            section["DocNumber"] = resolution.DocumentNumber;
            section["DocDate"] = resolution.DocumentDate.HasValue ? resolution.DocumentDate.Value.ToShortDateString() : string.Empty;

            if (resolution.ExecutantCode != null && resolution.ContragentName != null)
            {
                if (contragentTypes1.Contains(resolution.ExecutantCode))
                {
                    section["Organization"] = resolution.ContragentName;
                }

                if (contragentTypes2.Contains(resolution.ExecutantCode))
                {
                    section["Organization"] = string.Format("Руководителю {0}", resolution.ContragentName);
                }
            }

            if (resolution.Sanction != null)
            {
                section["MeasuresTaken"] = resolution.Sanction.Code == "1" ? resolution.PenaltyAmount.ToStr() : resolution.Sanction.Name;
            }
        }

        private class ResolutionProxy
        {
            public long Id;
            public SanctionGji Sanction;
            public decimal? PenaltyAmount;
            public string ContragentName;
            public string ExecutantCode;
            public string DocumentNumber;
            public DateTime? DocumentDate;
        }
    }
}