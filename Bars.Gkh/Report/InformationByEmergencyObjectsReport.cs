namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class InformationByEmergencyObjectsReport : BasePrintForm
    {
        public InformationByEmergencyObjectsReport() : base(new ReportTemplateBinary(Properties.Resources.InformationByEmergencyObjects))
        {
        }
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;

        public override string Name
        {
            get
            {
                return "Сведения по аварийным домам";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сведения по аварийным домам";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Аварийность";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationByEmergencyObjects";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.InformationByEmergencyObjects";
            }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceEmergencyObject =
                this.Container.Resolve<IDomainService<EmergencyObject>>().GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var emergencyObjectIdsQuery = serviceEmergencyObject.Select(x => x.Id);
            
            var emergencyObjects = serviceEmergencyObject
                                      .Select(x => new
                                      {
                                          x.Id,
                                          MuName = x.RealityObject.Municipality.Name,
                                          x.RealityObject.Address,
                                          WallMaterial = x.RealityObject.WallMaterial.Name,
                                          x.RealityObject.DateCommissioning,
                                          ReasonInexpedient = x.ReasonInexpedient.Name,
                                          x.DocumentNumber,
                                          x.DocumentDate,
                                          x.ResettlementFlatAmount,
                                          x.ResettlementFlatArea,
                                          x.LandArea,
                                          FurtherUse = x.FurtherUse.Name,
                                          x.DemolitionDate
                                      })
                                      .OrderBy(x => x.MuName)
                                      .ThenBy(x => x.Address)
                                      .ToList();

            var emerObjResettlementProgramInfo =
                this.Container.Resolve<IDomainService<EmerObjResettlementProgram>>()
                    .GetAll()
                    .Where(x => emergencyObjectIdsQuery.Contains(x.EmergencyObject.Id))
                    .GroupBy(x => new { x.EmergencyObject.Id, x.ResettlementProgramSource.Code })
                    .Select(
                        x =>
                        new
                            {
                                EmergencyObjectId = x.Key.Id,
                                ResettlementProgramSourceCode = x.Key.Code,
                                CountResidents = x.Sum(z => z.CountResidents),
                                Area = x.Sum(z => z.Area),
                                Cost = x.Sum(z => z.Cost)
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.EmergencyObjectId)
                    .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.ResettlementProgramSourceCode));

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var counter = 0;
            foreach (var emergencyObject in emergencyObjects)
            {
                section.ДобавитьСтроку();
                section["column1"] = ++counter;
                section["column2"] = string.Format("{0}, {1}", emergencyObject.MuName, emergencyObject.Address);
                section["column3"] = emergencyObject.DateCommissioning.HasValue ? emergencyObject.DateCommissioning.Value.ToShortDateString() : string.Empty; 
                section["column4"] = emergencyObject.WallMaterial;
                section["column5"] = emergencyObject.DateCommissioning.HasValue ? emergencyObject.DateCommissioning.Value.ToShortDateString() : string.Empty; 
                section["column6"] = emergencyObject.ReasonInexpedient;
                section["column7"] = emergencyObject.DocumentNumber;
                section["column8"] = emergencyObject.DocumentDate.HasValue ? emergencyObject.DocumentDate.Value.ToShortDateString() : string.Empty;
                section["column9"] = emergencyObject.ResettlementFlatAmount;
                section["column10"] = emergencyObject.ResettlementFlatArea;
                section["column20"] = emergencyObject.LandArea;
                section["column21"] = emergencyObject.FurtherUse;
                section["column22"] = emergencyObject.DemolitionDate.HasValue ? emergencyObject.DemolitionDate.Value.ToShortDateString() : string.Empty;

                if (emerObjResettlementProgramInfo.ContainsKey(emergencyObject.Id))
                {
                    var informationByPrograms = emerObjResettlementProgramInfo[emergencyObject.Id];
                    if (informationByPrograms.ContainsKey("0")) // По програмам с использоваием средств Фонда ЖКХ
                    {
                        var programInfo = informationByPrograms["0"];
                        section["column11"] = programInfo.CountResidents;
                        section["column12"] = programInfo.Area;
                        section["column13"] = programInfo.Cost;
                    }

                    if (informationByPrograms.ContainsKey("1")) // По программам, финансируемым за счет иных источников
                    {
                        var programInfo = informationByPrograms["1"];
                        section["column14"] = programInfo.CountResidents;
                        section["column15"] = programInfo.Area;
                        section["column16"] = programInfo.Cost;
                    }

                    if (informationByPrograms.ContainsKey("2")) // Не предусмотрено действующими программами
                    {
                        var programInfo = informationByPrograms["2"];
                        section["column17"] = programInfo.CountResidents;
                        section["column18"] = programInfo.Area;
                        section["column19"] = programInfo.Cost;
                    }
                }
            }
        }
    }
}