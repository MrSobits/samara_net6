namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ActProtocolReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private long[] municipalityIds;

        public ActProtocolReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActResponseProtocol))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ActProtocol";
            }
        }

        public override string Desciption
        {
            get { return "Реестр протоколов"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Protocol"; }
        }

        public override string Name
        {
            get { return "Реестр протоколов"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            
             // Создаем список нарушений
             var protocolViolations = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                 .WhereIf(dateStart != DateTime.MinValue, x => x.Document.DocumentDate >= dateStart)
                 .WhereIf(dateEnd != DateTime.MinValue, x => x.Document.DocumentDate <= dateEnd)
                 .Where(x => x.InspectionViolation.RealityObject != null)
                 .Select(x => new
                     {
                         ProtocolId = x.Document.Id,
                         x.InspectionViolation.RealityObject.Address,
                         MunicipalityName = x.InspectionViolation.RealityObject.Municipality.Name,
                         MunicipalityId = x.InspectionViolation.RealityObject.Municipality.Id,
                         PiN = x.InspectionViolation.Violation.CodePin,
                         NamePiN = x.InspectionViolation.Violation.Name
                     })
                 .AsEnumerable()
                 .GroupBy(x => x.ProtocolId)
                 .Select(x => new
                     {
                         x.Key,
                         municipalityId = x.First().MunicipalityId,
                         municipalityName = x.First().MunicipalityName,
                         realtyObjects = x.GroupBy(y => y.Address).ToDictionary(y => y.Key, y => y.ToList())
                     })
                 .Where(x => !municipalityIds.Any() || municipalityIds.Contains(x.municipalityId))
                 .ToDictionary(x => x.Key);

            // Создаем список протоколов
            var protocols = Container.Resolve<IDomainService<Protocol>>().GetAll()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .Select(x => new
                    {
                        x.Id, 
                        contrName = x.Contragent.Name, 
                        executantCode = x.Executant.Code,
                        x.DocumentNumber,
                        x.DocumentDate,
                    })
                .AsEnumerable()
                .Select(x =>
                    {
                        var violationData = protocolViolations.ContainsKey(x.Id) ? protocolViolations[x.Id] : null;

                        return new
                            {
                                x.Id,
                                x.contrName,
                                x.executantCode,
                                x.DocumentDate,
                                x.DocumentNumber,
                                municipalityId = violationData != null ? violationData.municipalityId : 0,
                                municipalityName = violationData != null ? violationData.municipalityName : string.Empty,
                                realtyObjects = violationData != null ? violationData.realtyObjects : null
                            };
                    })
                .Where(x => !municipalityIds.Any() || municipalityIds.Contains(x.municipalityId))
                .ToList();
            
            // Список статей
            var dictArticleLaw = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll()
                .WhereIf(dateStart != DateTime.MinValue, x => x.Protocol.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.Protocol.DocumentDate <= dateEnd)
                .Select(x => new { x.Protocol.Id, x.ArticleLaw.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.Name).Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)));

            var i = 0;

            // Код исполнителя
            var contragentTyptes1 = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
            var contragentTyptes2 = new List<string> { "1", "10", "12", "13", "16", "19", "5" };

            // Бегаем по списку нарушений
            foreach (var protocol in protocols.OrderBy(x => x.municipalityName))
            {
                section.ДобавитьСтроку();

                section["Number1"] = ++i;
                
                section["DocNumber"] = protocol.DocumentNumber;
                section["DocDate"] = protocol.DocumentDate.HasValue ? protocol.DocumentDate.Value.ToString("dd.MM.yyyy") : string.Empty;
                section["ArticleLaw"] = dictArticleLaw.ContainsKey(protocol.Id)
                            ? dictArticleLaw[protocol.Id]
                            : string.Empty;
                
                if (contragentTyptes1.Contains(protocol.executantCode))
                {
                    section["Organization"] = protocol.contrName;
                }
                else if (contragentTyptes2.Contains(protocol.executantCode))
                {
                    section["Organization"] = string.Format("Руководителю {0}", protocol.contrName);
                }

                if (protocol.realtyObjects == null)
                {
                    continue;
                }
                
                var firstRow = true;

                foreach (var realtyObject in protocol.realtyObjects.OrderBy(y => y.Key))
                {
                    var firstRowInLoop = firstRow;
                    if (firstRow)
                    {
                        firstRow = false;
                    }
                    else
                    {
                        section.ДобавитьСтроку();
                        firstRowInLoop = true;
                    }

                    section["Municipality"] = realtyObject.Value.First().MunicipalityName;
                    section["Address"] = realtyObject.Key;

                    var countViol = 0;
                    
                    foreach (var violation in realtyObject.Value)
                    {
                        if (firstRowInLoop)
                        {
                            firstRowInLoop = false;
                        }
                        else
                        {
                            section.ДобавитьСтроку();
                        }

                        section["Violation"] = string.Format(
                            "{0}. {1}. {2}", ++countViol, violation.PiN, violation.NamePiN);
                    }
                }
            }
        }
    }
}