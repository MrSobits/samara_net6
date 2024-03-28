namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class InformationOnApartments : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private int resettlementProgramId;

        private DateTime reportDate;

        private long[] municipalityIds;

        public InformationOnApartments()
            : base(new ReportTemplateBinary(Properties.Resources.InformationOnApartments))
        {
        }

        public override string Name
        {
            get
            {
                return "Сведения по квартирам";
            }
        }
        
        public override string Desciption
        {
            get
            {
                return "Сведения по квартирам";
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
                return "B4.controller.report.InformationOnApartments";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.InformationOnApartments";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.resettlementProgramId = baseParams.Params["resettlementProgramId"].ToInt();
        }

        public override void PrepareReport(ReportParams reportParams)
        {

            var serviceEmergencyObject = this.Container.Resolve<IDomainService<EmergencyObject>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serviceEmerObjResettlementProgram = this.Container.Resolve<IDomainService<EmerObjResettlementProgram>>();
            var serviceRealityObjectApartInfo = this.Container.Resolve<IDomainService<RealityObjectApartInfo>>();

            var municipalityDict = serviceMunicipality.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);


            var EmergencyObjectDict = serviceEmergencyObject.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ResettlementProgram.Id == this.resettlementProgramId)
                .Select(x => new
                        {
                            MoId = x.RealityObject.Municipality.Id,
                            RoId = x.RealityObject.Id,
                            x.RealityObject.Address,
                            x.DocumentNumber,
                            x.DocumentDate,
                            x.DemolitionDate,
                            x.RealityObject.NumberLiving,
                            x.RealityObject.AreaMkd,
                            x.ResettlementFlatArea,
                            x.ResettlementFlatAmount
                        })
                .ToList()
                .GroupBy(x => x.MoId)
                .ToDictionary(x => x.Key, x => x);

            var roIdsQuery = serviceEmergencyObject.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ResettlementProgram.Id == this.resettlementProgramId)
                .Select(x => x.RealityObject.Id);

            var RealityObjectApartInfo = serviceRealityObjectApartInfo.GetAll()
                .Where(x => roIdsQuery.Contains(x.RealityObject.Id))
                .Select(x => new
                        {
                            RoId = x.RealityObject.Id,
                            x.CountPeople,
                            x.Privatized,
                            x.NumApartment,
                            x.AreaTotal
                        })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => 
                    x.Select(y =>
                            {
                                decimal? PrivatizedAreaTotal = 0;
                                decimal? NoPrivatizedAreaTotal = 0;
                                decimal? ProvidedArea = 0;

                                var CountPeople = y.CountPeople;
                                var NumApartment = y.NumApartment;

                                if (y.Privatized == YesNoNotSet.Yes)
                                {
                                    PrivatizedAreaTotal = ProvidedArea = y.AreaTotal;
                                }
                                else
                                {
                                    NoPrivatizedAreaTotal = y.AreaTotal;

                                    switch (CountPeople)
                                    {
                                        case 1:
                                            ProvidedArea = 33;
                                            break;
                                        case 2:
                                            ProvidedArea = 42;
                                            break;
                                        default:
                                            if (CountPeople >= 3)
                                            {
                                                ProvidedArea = 18 * CountPeople;
                                            }

                                            break;
                                    }
                                }

                                return new
                                        {
                                            CountPeople,
                                            NumApartment,
                                            ProvidedArea,
                                            PrivatizedAreaTotal,
                                            NoPrivatizedAreaTotal
                                        };
                            }).ToList());

            var EmerObjResettlementProgramDict = serviceEmerObjResettlementProgram.GetAll()
                .Where(x => x.EmergencyObject.ResettlementProgram.Id == this.resettlementProgramId)
                .WhereIf(municipalityIds.Length > 0, x => roIdsQuery.Contains(x.EmergencyObject.RealityObject.Id))
                .GroupBy(x => x.EmergencyObject.RealityObject.Id)
                .Select(x => new
                        {
                            x.Key,
                            Cost = x.Sum(y => y.Cost),
                            CountResidents = x.Sum(y => y.CountResidents),
                        })
                .ToDictionary(x => x.Key);

            var fieldNames = new List<string>
                {
                    "NumberLiving",
                    "RelocationNumberLiving",
                    "AreaMKD",
                    "ResettlementArea",
                    "ResettlementRooms",
                    "NumberRoom",
                    "ARNumberLiving",
                    "ARPrivatization",
                    "ARNoProvatization",
                    "TotalCost",
                    "ProvidedArea"
                };

            var irDict = fieldNames.ToDictionary(x => "IR" + x, x => new decimal?());
            var totalDict = fieldNames.ToDictionary(x => "Total" + x, x => new decimal?());
            totalDict.Keys.ToList().ForEach(x => totalDict[x] = 0);
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var sectionRoom = sectionRo.ДобавитьСекцию("sectionRoom");

            var count = 0;

            foreach (var municipality in municipalityDict.OrderBy(x => x.Value))
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MuName"] = municipality.Value;

                irDict.Keys.ToList().ForEach(x => irDict[x] = 0);
                

                count = 0;

                if (!EmergencyObjectDict.ContainsKey(municipality.Key))
                {
                    continue;
                }

                foreach (var emergencyObject in EmergencyObjectDict[municipality.Key])
                {
                    sectionRo.ДобавитьСтроку();
                    sectionRo["Number"] = ++count;

                    if (EmerObjResettlementProgramDict.ContainsKey(emergencyObject.RoId))
                    {
                        var emerObjResettlementProgram = EmerObjResettlementProgramDict[emergencyObject.RoId];
                        sectionRo["RelocationNumberLiving"] = emerObjResettlementProgram.CountResidents;
                        irDict["IRRelocationNumberLiving"] += emerObjResettlementProgram.CountResidents;
                        irDict["IRTotalCost"] += emerObjResettlementProgram.Cost;
                        sectionRo["TotalCost"] = emerObjResettlementProgram.Cost;
                    }
                    sectionRo["RoAddres"] = emergencyObject.Address;
                    sectionRo["DocNumber"] = emergencyObject.DocumentNumber;
                    sectionRo["DocDate"] = emergencyObject.DocumentDate.HasValue ? emergencyObject.DocumentDate.Value.ToShortDateString() : string.Empty;
                    sectionRo["DemolitionPlanDate"] = emergencyObject.DemolitionDate.HasValue ? emergencyObject.DemolitionDate.Value.ToShortDateString() : string.Empty;
                    sectionRo["NumberLiving"] = emergencyObject.NumberLiving;
                    sectionRo["AreaMKD"] = emergencyObject.AreaMkd;
                    sectionRo["ResettlementArea"] = emergencyObject.ResettlementFlatArea;
                    sectionRo["ResettlementRooms"] = emergencyObject.ResettlementFlatAmount;
                    
                    irDict["IRNumberLiving"] += emergencyObject.NumberLiving;
                    irDict["IRAreaMKD"] += emergencyObject.AreaMkd;
                    irDict["IRResettlementArea"] += emergencyObject.ResettlementFlatArea;
                    irDict["IRResettlementRooms"] += emergencyObject.ResettlementFlatAmount;
                    
                    if (!RealityObjectApartInfo.ContainsKey(emergencyObject.RoId))
                    {
                        continue;
                    }

                    var rooms = RealityObjectApartInfo[emergencyObject.RoId];
                    
                    foreach (var room in rooms)
                    {
                        sectionRoom.ДобавитьСтроку();
                        
                        sectionRoom["TNumberRoom"] = room.NumApartment;
                        sectionRoom["TARNumberLiving"] = room.CountPeople;
                        sectionRoom["TARPrivatization"] = room.PrivatizedAreaTotal;
                        sectionRoom["TARNoProvatization"] = room.NoPrivatizedAreaTotal;
                        sectionRoom["TProvidedArea"] = room.ProvidedArea;
                    }

                    var roomsCount = rooms.Count;
                    var roomsSumPeople = rooms.Sum(x => x.CountPeople);
                    var roomsSumPrivatizedArea = rooms.Sum(x => x.PrivatizedAreaTotal);
                    var roomsSumNoPrivatizedArea = rooms.Sum(x => x.NoPrivatizedAreaTotal);
                    var roomsSumProvidedArea = rooms.Sum(x => x.ProvidedArea);

                    sectionRo["NumberRoom"] = roomsCount;
                    sectionRo["ARNumberLiving"] = roomsSumPeople;
                    sectionRo["ARPrivatization"] = roomsSumPrivatizedArea;
                    sectionRo["ARNoProvatization"] = roomsSumNoPrivatizedArea;
                    sectionRo["ProvidedArea"] = roomsSumProvidedArea;
                    
                    irDict["IRNumberRoom"] += rooms.Count;
                    irDict["IRARNumberLiving"] += roomsSumPeople;
                    irDict["IRARPrivatization"] += roomsSumPrivatizedArea;
                    irDict["IRARNoProvatization"] += roomsSumNoPrivatizedArea;
                    irDict["IRProvidedArea"] += roomsSumProvidedArea;
                }

                foreach (var fieldName in fieldNames)
                {
                    totalDict["Total" + fieldName] += irDict["IR" + fieldName];
                }
                
                foreach (var ir in irDict)
                {
                    sectionMu[ir.Key] = ir.Value;
                }
            }

            foreach (var total in totalDict)
            {
                reportParams.SimpleReportParams[total.Key] = total.Value;
            }
        }
    }
}