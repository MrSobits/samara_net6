namespace Bars.GkhGji.Regions.Tatarstan.Report.GjiWorkReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    public class GjiWorkReport : BasePrintForm
    {
        private DateTime dateStart = DateTime.Now;
        
        private DateTime dateEnd = DateTime.Now;
        
        private long[] municipalityListId;

        public override string ReportGenerator { get; set; }

        public override string RequiredPermission => "Reports.GJI.GjiWork";

        public override string Name => "Работа ГЖИ за период";

        public override string Desciption => "Работа ГЖИ за период";

        public override string GroupName => "Инспекторская деятельность";

        public override string ParamsController => "B4.controller.report.GjiWork";
        
        public IWindsorContainer Container { get; set; }

        public IRepository<Disposal> DisposalRepository { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }

        public GjiWorkReport() : base(new ReportTemplateBinary(Bars.GkhGji.Regions.Tatarstan.Properties.Resources.GjiWorkReport))
        {
        }
        
        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateStart = baseParams.Params["dateStart"].ToDateTime();
            this.dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            this.municipalityListId = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            // коды видов обследования (столбцы 26-43). Такой порядок кодов необходим для заполнения секций.
            var codesList = new List<string> { "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "22", "20", "21", "19" };

            reportParams.SimpleReportParams["ДатаНачала"] = string.Format("{0:d MMMM yyyy}", this.dateStart);
            reportParams.SimpleReportParams["ДатаКонца"] = string.Format("{0:d MMMM yyyy}", this.dateEnd);

            var groupSection = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияГруппа");
            var groupNameSection = groupSection.ДобавитьСекцию("ИмяГруппы");
            var districtSection = groupSection.ДобавитьСекцию("СекцияРайон");
            var disposalSection = districtSection.ДобавитьСекцию("СекцияДом");

            var disposals = this.GetDisposals();
            var disposalDic = disposals.ToDictionary(x => x.disposalId);

            // Дома по распоряжениям/решениям по проверке предписания - дома берем из нарушений предписания, идентификаторы которых возвращаются out-параметром.
            var housesFromDisposalsByPrescription = this.GetHousesFromDisposalsByPrescription(out var disposalByPrescriptionRoIdsQuery);

            // Дома по распоряжениям/решениям НЕ по проверке предписания.
            // Идентификаторы домов для распоряжений/решений с актами и для распоряжений без актов возвращаются out-параметрами.
            var housesFromDisposalsNotByPrescription = this.GetHousesFromDisposalsNotByPrescription(out var disposalNotByPrescriptionRoIdsQuery1, out var disposalNotByPrescriptionRoIdsQuery2);

            var houses = new List<House>();
            houses.AddRange(housesFromDisposalsNotByPrescription);
            houses.AddRange(housesFromDisposalsByPrescription);

            var dataDict = houses.GroupBy(x => x.group)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => new
                    {
                        y.municipalityId,
                        y.municipalityName
                    }).ToDictionary(
                            y => y.Key,
                            y => y.Select(z => new
                            {
                                roAddress = z.address,
                                roId = z.homeId,
                                z.disposalId,
                                z.typeOwnerShip
                            })
                            .Distinct()
                            .ToList()));

            // упр. организации (столбцы 7-8, 13-16)
            var manOrgs = this.GetManagingOrganization();

            // словарь домов с непостредственным управлением (для поля 16) содержит период непоср. управления
            var directManagement = this.GetHousesWithDirectManag(disposalByPrescriptionRoIdsQuery, disposalNotByPrescriptionRoIdsQuery1, disposalNotByPrescriptionRoIdsQuery2);

            var disposalQuery = this.DisposalRepository.GetAll()
                .Where(x => x.Inspection != null)
                .Where(x => x.DocumentDate >= this.dateStart)
                .Where(x => x.DocumentDate <= this.dateEnd)
                .Where(x => x.KindCheck != null)
                .Select(x => x.Id);

            var typeSurvey = this.GetNotPlannedInspectionsByTypes(disposalQuery);
            var inspectorDic = this.GetInspectors(disposalQuery);
            var actData = this.GetActData(disposalQuery);
            var disposalsWithWarningDoc = this.GetDisposalsWithWarningDoc();
            var actsRemovalOfDisposalByPrescription = this.GetActRemovals();
            var fixedActsRemovalOfDisposalByPrescription = this.GetFixedActRemovals();

            var realtyObjIdsList = houses.Select(x => x.homeId).ToList();

            var presriptionProxyDict = this.GetPrescriptionData(realtyObjIdsList);
            var protocolProxyDict = this.GetProtocolData(realtyObjIdsList, out var allProtocols);
            var resolutionProxyDict = this.GetResolutionData(allProtocols);
            var presentationHashSet = this.GetDisposalWithPresentation();
            var inspectionTypeBaseDict = this.GetDisposalsInspectionTypeBase();

            var totalsList = new List<int>();
            totalsList.AddRange(Enumerable.Range(18, 104));
            totalsList.AddRange(Enumerable.Range(9, 8));
            totalsList.AddRange(Enumerable.Range(130, 19));
            totalsList.Add(6);
            var totalsListSum = new List<int>();
            totalsListSum.AddRange(Enumerable.Range(122, 8));

            var totalsDic = totalsList.ToDictionary(x => x, x => (long)0);
            var totalsDicSum = totalsListSum.ToDictionary(x => x, x => 0m);

            var totalRowsWithZero = new List<int> { 45, 88, 113 };
            for (int k = 18; k <= 22; k++)
            {
                totalRowsWithZero.Add(k);
            }

            var numpp = 0;
            var totalArea = 0M;

            Func<bool, string> oneOrEmpty = x => x ? "1" : string.Empty;
            Func<long, string> numberOrEmpty = x => x > 0 ? x.ToStr() : string.Empty;

            var roDocInfoDict = this.PrepareDocsInfo();

            foreach (var group in dataDict)
            {
                groupSection.ДобавитьСтроку();

                if (group.Key != string.Empty)
                {
                    groupNameSection.ДобавитьСтроку();
                    groupNameSection["Группа"] = group.Key;
                }

                foreach (var municip in group.Value.OrderBy(x => x.Key.municipalityName))
                {
                    var totalAreaMunic = 0M;
                    var totalsDicMunicip = totalsList.ToDictionary(x => x, x => (long)0);
                    var totalsDicSumMunicip = totalsListSum.ToDictionary(x => x, x => 0m);

                    districtSection.ДобавитьСтроку();
                    districtSection["Район"] = municip.Key.municipalityName;

                    foreach (var realtyObjectDisposalProxy in municip.Value.OrderBy(x => x.disposalId).ThenBy(x => x.roAddress))
                    {
                        disposalSection.ДобавитьСтроку();
                        disposalSection["param1"] = ++numpp;

                        var disposalId = realtyObjectDisposalProxy.disposalId;
                        var disposal = disposalDic[disposalId];
                        var contragentId = disposal.contragentId ?? -1;
                        var typeManagement = manOrgs.TryGetValue(contragentId, out var org) ? org.typeManagement : (TypeManagementManOrg)(-1);
                        var homeId = realtyObjectDisposalProxy.roId;

                        disposalSection["param2"] = disposal.docNumber;
                        disposalSection["param3"] = disposal.docDate;
                        disposalSection["param4"] = municip.Key.municipalityName;
                        disposalSection["param5"] = realtyObjectDisposalProxy.roAddress;

                        if (disposal.typeJurPerson == TypeJurPerson.SupplyResourceOrg)
                        {
                            disposalSection["param7"] = "РСО";
                        }
                        else
                        {
                            if (disposal.typeJurPerson == TypeJurPerson.ManagingOrganization)
                            {
                                if (typeManagement == TypeManagementManOrg.JSK) { disposalSection["param7"] = "ЖСК"; }
                                if (typeManagement == TypeManagementManOrg.TSJ) { disposalSection["param7"] = "ТСЖ"; }
                                if (typeManagement == TypeManagementManOrg.UK) { disposalSection["param7"] = "УК"; }
                            }

                            if (disposal.typeBase == TypeBase.CitizenStatement && disposal.personInspection == PersonInspection.PhysPerson)
                            {
                                disposalSection["param7"] = "Собственник";
                            }
                        }

                        disposalSection["param8"] = disposal.contragentName;

                        disposalSection["param13"] = oneOrEmpty(typeManagement == TypeManagementManOrg.UK);
                        disposalSection["param14"] = oneOrEmpty(typeManagement == TypeManagementManOrg.TSJ);
                        disposalSection["param15"] = oneOrEmpty(typeManagement == TypeManagementManOrg.JSK);

                        if (typeManagement == TypeManagementManOrg.Other && directManagement.Keys.Contains(homeId))
                        {
                            foreach (var contract in directManagement[homeId])
                            {
                                if ((contract.StartDate < disposalDic[disposalId].docDate)
                                    && (disposalDic[disposalId].docDate < contract.EndDate))
                                {
                                    disposalSection["param16"] = 1;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(realtyObjectDisposalProxy.typeOwnerShip))
                        {
                            var typeOwnerShipLower = realtyObjectDisposalProxy.typeOwnerShip.ToLower();
                            disposalSection["param9"] = oneOrEmpty(typeOwnerShipLower == "муниципальная");
                            disposalSection["param10"] = oneOrEmpty(typeOwnerShipLower == "государственная");
                            disposalSection["param11"] = oneOrEmpty(typeOwnerShipLower == "частная");
                            disposalSection["param12"] = oneOrEmpty(typeOwnerShipLower == "смешанная");
                        }

                        disposalSection["param17"] = inspectorDic.ContainsKey(disposalId)
                                                         ? inspectorDic[disposalId]
                                                         : string.Empty;
                        disposalSection["param18"] = 1; // т.к. всегда по распоряжениям

                        disposalSection["param19"] = oneOrEmpty(actData.ContainsKey(disposalId) && actData[disposalId].ContainsKey(homeId));

                        // Вид обследования
                        var planned =
                            new[]
                            {
                                TypeCheck.PlannedDocumentation,
                                TypeCheck.PlannedExit,
                                TypeCheck.PlannedDocumentationExit,
                                TypeCheck.PlannedInspectionVisit
                            }.Contains(disposal.kindCheckCode)
                            && disposal.typeBase == TypeBase.PlanJuridicalPerson;

                        if (planned)
                        {
                            disposalSection["param23"] = 1;

                            for (var k = 24; k < 44; k++)
                            {
                                disposalSection["param" + k.ToString()] = string.Empty;
                            }
                        }
                        else
                        {
                            disposalSection["param24"] = oneOrEmpty(new[]
                            {
                                TypeBase.ProsecutorsClaim,
                                TypeBase.DisposalHead,
                                TypeBase.CitizenStatement,
                                TypeBase.InspectionActionIsolated
                            }.Contains(disposal.typeBase)
                                && new[]
                            {
                                TypeCheck.NotPlannedDocumentation,
                                TypeCheck.NotPlannedExit,
                                TypeCheck.NotPlannedDocumentationExit,
                                TypeCheck.NotPlannedInspectionVisit
                            }.Contains(disposal.kindCheckCode));

                            if (disposal.typeDisposal == TypeDisposalGji.DocumentGji)
                            {
                                disposalSection["param25"] = "1";
                            }
                            else
                            {
                                // в DisposalService.cs сделано так (т.е. без учета того что тип документа должен быть предписание)
                                disposalSection["param26"] = oneOrEmpty(disposal.typeBase == TypeBase.CitizenStatement);

                                var paramCode = 27;
                                if (typeSurvey.ContainsKey(disposalId))
                                {
                                    foreach (var code in codesList)
                                    {
                                        disposalSection["param" + paramCode] = oneOrEmpty(typeSurvey[disposalId].Contains(code));
                                        paramCode++;
                                    }

                                    // если код обследования не попадает в codesList и не удовлетворяет условию поля 25 то пишем в 43 поле
                                    disposalSection["param43"] = oneOrEmpty(!typeSurvey[disposalId].Any(codesList.Contains) && disposal.typeBase != TypeBase.CitizenStatement);
                                }
                            }
                        }
                        if (disposal.typeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement)
                        {
                            if (disposal.typeAgreementResult == TypeAgreementResult.Agreed) { disposalSection["param44"] = 0; }
                            if (disposal.typeAgreementResult == TypeAgreementResult.NotAgreed) { disposalSection["param44"] = 1; }
                        }

                        if (actData.ContainsKey(disposalId) && actData[disposalId].ContainsKey(homeId))
                        {
                            var roActData = actData[disposalId][homeId];

                            var currentArea = roActData.area.HasValue ? roActData.area.Value : 0M;
                            disposalSection["param6"] = currentArea;
                            totalAreaMunic += currentArea;
                            totalArea += currentArea;

                            ActProxy removalData = null;
                            bool isAllViolationsFixed = false;

                            if (disposal.isByPrescription)
                            {
                                if (actsRemovalOfDisposalByPrescription.ContainsKey(disposalId)
                                    && actsRemovalOfDisposalByPrescription[disposalId].ContainsKey(homeId))
                                {
                                    removalData = actsRemovalOfDisposalByPrescription[disposalId][homeId];
                                }

                                if (fixedActsRemovalOfDisposalByPrescription.ContainsKey(disposalId)
                                    && fixedActsRemovalOfDisposalByPrescription[disposalId].ContainsKey(homeId))
                                {
                                    isAllViolationsFixed = fixedActsRemovalOfDisposalByPrescription[disposalId][homeId];
                                }
                            }

                            var index = planned ? 46 : 64;

                            if (removalData != null && removalData.violationsCount > 0)
                            {
                                // если распоряжение на проверку исполнения предписания и в акте проверки предписания нарушения устранены == нет, то тянуть Количество нарушений оттуда
                                disposalSection["param" + (index - 1).ToStr()] = numberOrEmpty(removalData.violationsCount);

                                for (int i = 1; i <= 17; ++i)
                                {
                                    disposalSection["param" + index.ToString()] = numberOrEmpty(removalData.violationsByFeatures[i]);

                                    if (++index == 74)
                                    {
                                        ++index;
                                    }
                                }
                            }
                            else
                            {
                                disposalSection["param" + (index - 1).ToStr()] = numberOrEmpty(roActData.violationsCount);

                                for (int i = 1; i <= 17; ++i)
                                {
                                    disposalSection["param" + index.ToStr()] = numberOrEmpty(roActData.violationsByFeatures[i]);

                                    if (++index == 74)
                                    {
                                        ++index;
                                    }
                                }
                            }

                            disposalSection["param82"] = (roActData.violationsCount == 0 || isAllViolationsFixed) ? "1" : "0";
                        }
                        else
                        {
                            disposalSection["param82"] = 0;
                            disposalSection["param6"] = 0;
                        }

                        var prescriptionsOfDisposal = 0;
                        var protocolOfDisposal = 0;
                        var resolutionOfDisposal = 0;

                        if (presriptionProxyDict.ContainsKey(disposalId) && presriptionProxyDict[disposalId].ContainsKey(homeId))
                        {
                            var prescriptionProxy = presriptionProxyDict[disposalId][homeId];

                            prescriptionsOfDisposal = prescriptionProxy.totalCount > 0 ? 1 : 0;

                            disposalSection["param83"] = numberOrEmpty(prescriptionProxy.totalCount);
                            disposalSection["param84"] = numberOrEmpty(prescriptionProxy.byJurPersonCount);
                            disposalSection["param85"] = numberOrEmpty(prescriptionProxy.byOffPersonCount);
                            disposalSection["param86"] = numberOrEmpty(prescriptionProxy.byPhysPersonCount);
                            disposalSection["param87"] = numberOrEmpty(prescriptionProxy.eliminatedVoilationsCount);
                        }

                        #region Столбцы 88-129

                        if (protocolProxyDict.ContainsKey(disposalId) && protocolProxyDict[disposalId].ContainsKey(homeId))
                        {
                            var roProtocolProxy = protocolProxyDict[disposalId][homeId];

                            protocolOfDisposal = roProtocolProxy.totalCount > 0 ? 1 : 0;

                            disposalSection["param88"] = numberOrEmpty(roProtocolProxy.totalCount);
                            disposalSection["param89"] = oneOrEmpty(roProtocolProxy.st_7_21_1);
                            disposalSection["param90"] = oneOrEmpty(roProtocolProxy.st_7_21_2);
                            disposalSection["param91"] = oneOrEmpty(roProtocolProxy.st_7_22);
                            disposalSection["param92"] = oneOrEmpty(roProtocolProxy.st_7_23);
                            disposalSection["param93"] = oneOrEmpty(roProtocolProxy.st_9_16_4);
                            disposalSection["param94"] = oneOrEmpty(roProtocolProxy.st_9_16_5);
                            disposalSection["param95"] = oneOrEmpty(roProtocolProxy.st_19_4);
                            disposalSection["param96"] = oneOrEmpty(roProtocolProxy.st_19_5_1);
                            disposalSection["param97"] = oneOrEmpty(roProtocolProxy.st_19_6);
                            disposalSection["param98"] = oneOrEmpty(roProtocolProxy.st_19_7);
                            disposalSection["param99"] = oneOrEmpty(roProtocolProxy.st_20_25_1);
                            disposalSection["param100"] = oneOrEmpty(roProtocolProxy.st_7_23_1);
                            disposalSection["param101"] = oneOrEmpty(roProtocolProxy.st_13_19_2_1);
                            disposalSection["param102"] = oneOrEmpty(roProtocolProxy.st_13_19_2_2);
                            disposalSection["param103"] = oneOrEmpty(roProtocolProxy.st_9_5_1_12);
                            disposalSection["param104"] = oneOrEmpty(roProtocolProxy.st_9_5_1_5);
                            disposalSection["param105"] = oneOrEmpty(roProtocolProxy.st_9_5_1_4);
                            disposalSection["param106"] = oneOrEmpty(roProtocolProxy.st_9_23_1);
                            disposalSection["param107"] = oneOrEmpty(roProtocolProxy.st_9_23_2);
                            disposalSection["param108"] = oneOrEmpty(roProtocolProxy.st_9_23_3);
                            disposalSection["param109"] = oneOrEmpty(roProtocolProxy.st_9_23_4);
                            disposalSection["param110"] = oneOrEmpty(roProtocolProxy.st_14_1_3_1);
                            disposalSection["param111"] = oneOrEmpty(roProtocolProxy.st_14_1_3_2);
                            disposalSection["param142"] = oneOrEmpty(roProtocolProxy.st_19_5_24);
                            disposalSection["param143"] = oneOrEmpty(roProtocolProxy.st_19_5_24_1);
                            disposalSection["param144"] = oneOrEmpty(roProtocolProxy.st_7_23_2_1);
                            disposalSection["param145"] = oneOrEmpty(roProtocolProxy.st_7_23_3);

                            var resolutions = new List<ResolutionProxy>();

                            foreach (var protocol in roProtocolProxy.protocols)
                            {
                                if (resolutionProxyDict.TryGetValue(protocol, out var value))
                                {
                                    resolutionOfDisposal = 1;
                                    resolutions.Add(value);
                                }
                            }

                            var resolutionsCount = resolutions.Sum(x => x.Count);

                            disposalSection["param113"] = resolutionsCount > 0 ? resolutionsCount.ToStr() : string.Empty;

                            var resDic = new Dictionary<string, int>
                            {
                                { "114", 0 },
                                { "115", 0 },
                                { "116", 0 },
                                { "117", 0 },
                                { "118", 0 },
                                { "119", 0 },
                                { "120", 0 },
                                { "121", 0 }
                            };
                            
                            var penaltyDic = new Dictionary<string, decimal>
                            {
                                { "122", 0 },
                                { "123", 0 },
                                { "124", 0 },
                                { "125", 0 },
                                { "126", 0 },
                                { "127", 0 },
                                { "128", 0 },
                                { "129", 0 }
                            };

                            foreach (var resolution in resolutions)
                            {
                                disposalSection["param146"] = numberOrEmpty(resolution.ProsecutedOfficialCount);
                                disposalSection["param147"] = numberOrEmpty(resolution.ProsecutedLegalCount);
                                
                                if (planned)
                                {
                                    resDic["114"] += resolution.CourtAdmPenalty;
                                    resDic["115"] += resolution.CourtFinished;
                                    resDic["116"] += resolution.GjiAdmPenalty;
                                    resDic["117"] += resolution.GjiFinished;

                                    penaltyDic["122"] += resolution.CourtSumPenalty;
                                    penaltyDic["123"] += resolution.GjiSumPenalty;
                                    penaltyDic["124"] += resolution.CourtSumPenaltyPaid;
                                    penaltyDic["125"] += resolution.GjiSumPenaltyPaid;
                                }
                                else
                                {
                                    resDic["118"] += resolution.CourtAdmPenalty;
                                    resDic["119"] += resolution.CourtFinished;
                                    resDic["120"] += resolution.GjiAdmPenalty;
                                    resDic["121"] += resolution.GjiFinished;

                                    penaltyDic["126"] += resolution.CourtSumPenalty;
                                    penaltyDic["127"] += resolution.GjiSumPenalty;
                                    penaltyDic["128"] += resolution.CourtSumPenaltyPaid;
                                    penaltyDic["129"] += resolution.GjiSumPenaltyPaid;
                                }
                            }

                            // с 114 по 121 поля
                            foreach (var row in resDic)
                            {
                                disposalSection["param" + row.Key] = numberOrEmpty(row.Value);
                            }

                            // с 122 по 129 поля
                            foreach (var sum in penaltyDic)
                            {
                                disposalSection["param" + sum.Key] = sum.Value > 0 ? sum.Value.ToStr() : string.Empty;
                            }
                        }
                        #endregion

                        disposalSection["param20"] = prescriptionsOfDisposal;
                        disposalSection["param21"] = protocolOfDisposal;
                        disposalSection["param22"] = resolutionOfDisposal;

                        disposalSection["param130"] = oneOrEmpty(presentationHashSet.Contains(disposal.disposalId));
                        disposalSection["param148"] = oneOrEmpty(disposal.InspectionForm.HasValue && disposal.InspectionForm == TypeFormInspection.Documentary);
                        disposalSection["param131"] = oneOrEmpty(disposal.InspectionForm.HasValue && disposal.InspectionForm == TypeFormInspection.Exit);
                        disposalSection["param132"] = oneOrEmpty(disposal.InspectionForm.HasValue && disposal.InspectionForm == TypeFormInspection.InspectionVisit);
                        disposalSection["param133"] = oneOrEmpty(inspectionTypeBaseDict.Get(disposal.disposalId)?.Equals("113") ?? false);
                        disposalSection["param134"] = oneOrEmpty(disposal.typeBase == TypeBase.InspectionActionIsolated);
                        disposalSection["param135"] = oneOrEmpty(disposal.typeBase == TypeBase.ProsecutorsClaim);
                        disposalSection["param138"] = oneOrEmpty(disposalsWithWarningDoc.Contains(disposal.disposalId));
                        disposalSection["param141"] = oneOrEmpty(disposal.typeAgreementResult == TypeAgreementResult.Agreed);

                        // собираем итоги по МО
                        foreach (var param in totalsDicMunicip.Keys.ToList())
                        {
                            totalsDicMunicip[param] += disposalSection["param" + param].ToLong();
                        }

                        foreach (var param in totalsDicSumMunicip.Keys.ToList())
                        {
                            totalsDicSumMunicip[param] += disposalSection["param" + param].ToDecimal();
                        }
                    }

                    var processedEntities = new List<RealityObjectProxy>();
                    roDocInfoDict
                        .Where(x => x.Key.MunicipalityId == municip.Key.municipalityId)
                        .ForEach(x =>
                        {
                            disposalSection.ДобавитьСтроку();
                            disposalSection["param1"] = ++numpp;
                            disposalSection["param4"] = municip.Key.municipalityName;
                            disposalSection["param5"] = x.Key.Address;

                            var area = x.Key.Area ?? 0M;
                            disposalSection["param6"] = area;
                            totalAreaMunic += area;
                            
                            FillDisposalIndependentFields(ref disposalSection, x.Value);
                            
                            foreach (var param in totalsDicMunicip.Keys.ToList())
                            {
                                totalsDicMunicip[param] += disposalSection["param" + param].ToLong();
                            }
                            
                            processedEntities.Add(x.Key);
                        });
                    processedEntities.ForEach(x => roDocInfoDict.Remove(x));

                    foreach (var param in totalsDicMunicip)
                    {
                        if (totalRowsWithZero.Contains(param.Key))
                        {
                            districtSection["districtParam" + param.Key] = param.Value;
                        }
                        else
                        {
                            districtSection["districtParam" + param.Key] = numberOrEmpty(param.Value);
                        }

                        totalsDic[param.Key] += param.Value;
                    }

                    foreach (var param in totalsDicSumMunicip)
                    {
                        if (totalRowsWithZero.Contains(param.Key))
                        {
                            districtSection["districtParam" + param.Key] = param.Value;
                        }
                        else
                        {
                            districtSection["districtParam" + param.Key] = param.Value > 0 ? param.Value.ToStr() : string.Empty;
                        }

                        totalsDicSum[param.Key] += param.Value;
                    }

                    districtSection["districtParam6"] = totalAreaMunic;
                }
                
                // Если остались необработанные МО по док-ам, не привязанным к распоряжению
                if(roDocInfoDict.Any())
                {
                    roDocInfoDict.GroupBy(x => new
                    {
                        x.Key.MunicipalityId,
                        x.Key.MunicipalityName
                    }, x => new
                    {
                        x.Key.Address,
                        x.Key.Area,
                        roDocInfo = x.Value
                    }).ForEach(x =>
                    {
                        districtSection.ДобавитьСтроку();
                        districtSection["Район"] = x.Key.MunicipalityName;
                        var totalsDicMunicip = totalsList.ToDictionary(y => y, y => (long)0);
                        var totalAreaMunic = 0M;

                        x.ForEach(info => 
                        {
                            disposalSection.ДобавитьСтроку();
                            disposalSection["param1"] = ++numpp;
                            disposalSection["param4"] = x.Key.MunicipalityName;
                            disposalSection["param5"] = info.Address;
                            
                            var area = info.Area ?? 0M;
                            disposalSection["param6"] = area;
                            totalAreaMunic += area;

                            FillDisposalIndependentFields(ref disposalSection, info.roDocInfo);
                            
                            foreach (var param in totalsDicMunicip.Keys.ToList())
                            {
                                totalsDicMunicip[param] += disposalSection["param" + param].ToLong();
                            }
                        });

                        districtSection["districtParam6"] = totalAreaMunic;
                    });
                }
            }

            foreach (var param in totalsDic)
            {
                if (totalRowsWithZero.Contains(param.Key))
                {
                    reportParams.SimpleReportParams["totalParam" + param.Key] = param.Value;
                }
                else
                {
                    reportParams.SimpleReportParams["totalParam" + param.Key] = numberOrEmpty(param.Value);
                }
            }

            foreach (var param in totalsDicSum)
            {
                if (totalRowsWithZero.Contains(param.Key))
                {
                    reportParams.SimpleReportParams["totalParam" + param.Key] = param.Value;
                }
                else
                {
                    reportParams.SimpleReportParams["totalParam" + param.Key] = param.Value > 0 ? param.Value.ToStr() : string.Empty;
                }
            }

            reportParams.SimpleReportParams["totalParam6"] = totalArea;

        }

        /// <summary>
        /// Заполнить поля, не связанные с распоряжением
        /// </summary>
        /// <param name="section"></param>
        /// <param name="roDocInfo"></param>
        private void FillDisposalIndependentFields(ref Section section, RealityObjectDocInfo roDocInfo)
        {
            Func<int, string> numberOrEmpty = x => x > 0 ? x.ToStr() : string.Empty;
            
            section["param130"] = numberOrEmpty(roDocInfo.PresentationCount);
            section["param136"] = numberOrEmpty(roDocInfo.ObservationTaskActionIsolatedCount);
            section["param137"] = numberOrEmpty(roDocInfo.SurveyTaskActionIsolatedCount);
            section["param138"] = numberOrEmpty(roDocInfo.WarningDocCount);
            section["param142"] = numberOrEmpty(roDocInfo.Article_19_5_p_24_Count);
            section["param143"] = numberOrEmpty(roDocInfo.Article_19_5_p_24_1_Count);
            section["param144"] = numberOrEmpty(roDocInfo.Article_7_23_2_p_1_Count);
            section["param145"] = numberOrEmpty(roDocInfo.Article_7_23_3_Count);
            section["param146"] = numberOrEmpty(roDocInfo.ProsecutedOfficialCount);
            section["param147"] = numberOrEmpty(roDocInfo.ProsecutedLegalCount);
        }

        private List<DisposalProxy> GetDisposals()
        {
            Func<InspectionGji, TypeFormInspection?> getFormInspection = inspection =>
                inspection.GetType().GetProperty("TypeForm")?.GetValue(inspection) as TypeFormInspection?;

            return this.DisposalRepository.GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => x.DocumentDate != null)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.KindCheck != null)
                    .Select(x => new
                    {
                        disposalId = x.Id,
                        docDate = x.DocumentDate,
                        docNumber = x.DocumentNumber,
                        typeBase = x.Inspection.TypeBase,
                        typeJurPerson = x.Inspection.TypeJurPerson,
                        contragentName = x.Inspection.Contragent.Name,
                        contragentId = (long?)x.Inspection.Contragent.Id,
                        kindCheckCode = x.KindCheck.Code,
                        typeDisposal = x.TypeDisposal,
                        typeAgreementProsecutor = x.TypeAgreementProsecutor,
                        typeAgreementResult = x.TypeAgreementResult,
                        isByPrescription = x.TypeDisposal == TypeDisposalGji.DocumentGji,
                        personInspection = x.Inspection.PersonInspection,
                        x.Inspection
                    })
                    .AsEnumerable()
                    .Select(x => new DisposalProxy
                    {
                        disposalId = x.disposalId,
                        docDate = x.docDate,
                        docNumber = x.docNumber,
                        typeBase = x.typeBase,
                        typeJurPerson = x.typeJurPerson,
                        contragentName = x.contragentName,
                        contragentId = x.contragentId,
                        kindCheckCode = x.kindCheckCode,
                        typeDisposal = x.typeDisposal,
                        typeAgreementProsecutor = x.typeAgreementProsecutor,
                        typeAgreementResult = x.typeAgreementResult,
                        isByPrescription = x.isByPrescription,
                        personInspection = x.personInspection,
                        InspectionForm = getFormInspection(x.Inspection)
                    })
                    .ToList();
        }

        private Dictionary<int, List<long>> GetViolationFeatureCodes()
        {
            var violCodes = new Dictionary<int, List<long>>();
            violCodes.Add(1, new List<long>() { 1 });
            violCodes.Add(2, new List<long>() { 2 });
            violCodes.Add(3, new List<long>() { 3 });
            violCodes.Add(4, new List<long>() { 23, 35 });
            violCodes.Add(5, new List<long>() { 21, 22, 26 });
            violCodes.Add(6, new List<long>() { 27, 28, 29, 30, 31, 32 });
            violCodes.Add(7, new List<long>() { 36 });
            violCodes.Add(8, new List<long>() { 19 });
            violCodes.Add(9, new List<long>() { 20 });
            violCodes.Add(10, new List<long>() { 25 });
            violCodes.Add(11, new List<long>() { 6, 7, 8, 9, 10, 11, 12 });
            violCodes.Add(12, new List<long>() { 15, 17 });
            violCodes.Add(13, new List<long>() { 16 });
            violCodes.Add(14, new List<long>() { 4 });
            violCodes.Add(15, new List<long>() { 5 });
            violCodes.Add(16, new List<long>() { 13 });
            violCodes.Add(17, new List<long>() { 14, 18, 24, 33, 34 });

            return violCodes;
        }

        private Dictionary<long, Dictionary<long, ActProxy>> GetActData(IQueryable<long> disposalIdQuery)
        {
            var serviceChildrenParentActs = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceActCheck = this.Container.Resolve<IDomainService<ActCheck>>();
            var serviceActCheckRealityObject = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceActCheckViolation = this.Container.Resolve<IDomainService<ActCheckViolation>>();
            var serviceViolationFeature = this.Container.Resolve<IDomainService<ViolationFeatureGji>>();

            try
            {
                var actIdsQuery = serviceChildrenParentActs.GetAll()
                .Where(x => disposalIdQuery.Contains(x.Parent.Id))
                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Select(x => x.Children.Id);

                // Акты распоряжений
                var disposalIdByActIdMap = serviceChildrenParentActs.GetAll()
                    .Where(x => disposalIdQuery.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x => new
                    {
                        parentId = x.Parent.Id,
                        childrenId = x.Children.Id
                    })
                    .AsEnumerable()
                    .Distinct()
                    .GroupBy(x => x.childrenId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.parentId).FirstOrDefault());

                var actList = serviceActCheck.GetAll()
                    .Where(x => actIdsQuery.Contains(x.Id))
                    .Select(x => new { x.Id, x.TypeActCheck })
                    .ToList();

                var actChecksIndividualList = serviceActCheckRealityObject.GetAll()
                    .Where(x => actIdsQuery.Contains(x.ActCheck.Id))
                    .Where(x => x.RealityObject != null)
                    .Select(x => new
                    {
                        realtyObjectId = x.RealityObject.Id,
                        actCheckId = x.ActCheck.Id,
                        area = x.ActCheck.Area
                    })
                    .ToList();

                var nonGeneralActsList = actList.Where(x => x.TypeActCheck != TypeActCheckGji.ActCheckGeneral).Select(x => x.Id).ToList();

                var actProxyDict = actChecksIndividualList
                    .Where(x => disposalIdByActIdMap.ContainsKey(x.actCheckId))
                    .GroupBy(x => disposalIdByActIdMap[x.actCheckId])
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.realtyObjectId)
                            .ToDictionary(
                                y => y.Key,
                                y =>
                                {
                                    // Если по распоряжению дом входит в несколько актов (возможно общий и индивидуальный), то площадь считаем по индивидуальному
                                    var area = y.First().area;
                                    var act = y.OrderBy(z => z.actCheckId).FirstOrDefault(r => nonGeneralActsList.Contains(r.actCheckId));
                                    if (act != null)
                                    {
                                        area = act.area;
                                    }

                                    return new ActProxy { area = area, violationsCount = 0 };
                                }));

                // Получение нарушений
                var violationsByRoDict = serviceActCheckViolation.GetAll()
                    .Where(x => actIdsQuery.Contains(x.ActObject.ActCheck.Id))
                    .Where(x => x.InspectionViolation.RealityObject != null)
                    .Select(x => new
                    {
                        actId = x.ActObject.ActCheck.Id,
                        homeId = x.InspectionViolation.RealityObject.Id,
                        violId = x.InspectionViolation.Violation.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.homeId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => new { y.actId, y.violId }).ToList());

                var violIdsQuery = serviceActCheckViolation.GetAll()
                    .Where(x => actIdsQuery.Contains(x.ActObject.ActCheck.Id))
                    .Select(x => x.InspectionViolation.Violation.Id);

                var violFeatureList = serviceViolationFeature.GetAll()
                    .Where(x => violIdsQuery.Contains(x.ViolationGji.Id))
                    .Select(x => new { x.FeatureViolGji.Code, x.ViolationGji.Id })
                    .ToList();

                var violFeature = violFeatureList.GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.Select(y => y.Code.ToLong()).ToList());

                var violCodes = this.GetViolationFeatureCodes();

                var violationFeaturesByRoIdDict = violationsByRoDict.ToDictionary(
                    x => x.Key,
                    x => x.Value.GroupBy(y => disposalIdByActIdMap[y.actId])
                        .ToDictionary(
                            y => y.Key,
                            y =>
                            {
                                var count = y.Count();

                                var violCodesMap = violCodes.ToDictionary(z => z.Key, z => 0);

                                foreach (var violation in y)
                                {
                                    if (violFeature.ContainsKey(violation.violId))
                                    {
                                        var features = violFeature[violation.violId];

                                        var violationsByFeatures = violCodes.ToDictionary(z => z.Key, z => features.Any(z.Value.Contains) ? 1 : 0);

                                        for (int i = 1; i <= 17; ++i)
                                        {
                                            violCodesMap[i] += violationsByFeatures[i];
                                        }
                                    }
                                }

                                return new { count, violCodesMap };
                            }));

                foreach (var actProxyByRoDict in actProxyDict)
                {
                    foreach (var actProxy in actProxyByRoDict.Value)
                    {
                        if (violationFeaturesByRoIdDict.ContainsKey(actProxy.Key) && violationFeaturesByRoIdDict[actProxy.Key].ContainsKey(actProxyByRoDict.Key))
                        {
                            var violationFeaturesByRo = violationFeaturesByRoIdDict[actProxy.Key][actProxyByRoDict.Key];

                            actProxy.Value.violationsCount = violationFeaturesByRo.count;

                            for (int i = 1; i <= 17; ++i)
                            {
                                actProxy.Value.violationsByFeatures[i] = violationFeaturesByRo.violCodesMap[i];
                            }
                        }
                    }
                }

                return actProxyDict;
            }
            finally
            {
                this.Container.Release(serviceChildrenParentActs);
                this.Container.Release(serviceActCheck);
                this.Container.Release(serviceActCheckRealityObject);
                this.Container.Release(serviceActCheckViolation);
                this.Container.Release(serviceViolationFeature);
            }
        }

        private Dictionary<long, Dictionary<long, PrescriptionProxy>> GetPrescriptionData(List<long> realtyObjIds)
        {
            var servicePrescription = this.Container.Resolve<IDomainService<Prescription>>();
            var servicePrescriptionViolation = this.Container.Resolve<IDomainService<PrescriptionViol>>();
            var inspectionViolStageDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();

            try
            {
                var codesList84 = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
                var codesList85 = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
                var codesList86 = new List<string> { "6", "7", "14" };

                var result = this.DisposalRepository.GetAll()
                    .Join(
                        servicePrescription.GetAll(),
                        x => x.Stage.Id,
                        y => y.Stage.Parent.Id,
                        (a, b) => new { Disposal = a, Prescription = b })
                    .Join(
                        inspectionViolStageDomain.GetAll(),
                        x => x.Prescription.Id,
                        y => y.Document.Id,
                        (c, d) => new { InspectionViolStageId = d.Id, c.Disposal, c.Prescription, PrescriptionViol = d })
                    .Join(
                        servicePrescriptionViolation.GetAll(),
                        x => x.InspectionViolStageId,
                        x => x.Id,
                        (a, b) => a)
                    .Where(x => x.Disposal.Inspection != null)
                    .Where(x => x.Disposal.DocumentDate >= this.dateStart)
                    .Where(x => x.Disposal.DocumentDate <= this.dateEnd)
                    .Where(x => x.Disposal.KindCheck != null)
                    .Where(x => x.PrescriptionViol.InspectionViolation.RealityObject != null)
                    .Select(x => new
                    {
                        disposalId = x.Disposal.Id,
                        executantCode = x.Prescription.Executant.Code,
                        roId = x.PrescriptionViol.InspectionViolation.RealityObject != null ? x.PrescriptionViol.InspectionViolation.RealityObject.Id : 0,
                        x.PrescriptionViol.InspectionViolation.DateFactRemoval,
                        prescriptionId = x.Prescription.Id
                    })
                    .AsEnumerable()
                    .Where(x => realtyObjIds.Contains(x.roId))
                    .GroupBy(x => x.disposalId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.roId)
                              .ToDictionary(
                                y => y.Key,
                                y =>
                                {
                                    var executantCodeTypes = y.GroupBy(z => new { z.prescriptionId, z.executantCode })
                                        .Select(z => z.Key.executantCode)
                                        .Select(z => new
                                        {
                                            byJurPerson = codesList84.Contains(z),
                                            byOffPerson = codesList85.Contains(z),
                                            byPhysPerson = codesList86.Contains(z),
                                        })
                                        .ToList();

                                    var prescriptionProxy = new PrescriptionProxy
                                    {
                                        totalCount = y.Select(z => z.prescriptionId).Distinct().Count(),
                                        eliminatedVoilationsCount = y.Count(z => z.DateFactRemoval != null),
                                        byJurPersonCount = executantCodeTypes.Count(z => z.byJurPerson),
                                        byOffPersonCount = executantCodeTypes.Count(z => z.byOffPerson),
                                        byPhysPersonCount = executantCodeTypes.Count(z => z.byPhysPerson),
                                    };

                                    return prescriptionProxy;
                                }));

                return result;
            }
            finally 
            {
                this.Container.Release(servicePrescription);
                this.Container.Release(servicePrescriptionViolation);
            }
        }

        // возвращает Период непосредственного управления по домам
        private Dictionary<long, ManagPeriodProxy[]> GetHousesWithDirectManag(IQueryable<long> realtyObjIdsQuery0, IQueryable<long> realtyObjIdsQuery1, IQueryable<long> realtyObjIdsQuery2)
        {
            var manOrgContragentRoService = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            try
            {
                Func<IQueryable<long>, List<RealtyObjectManagPeriodProxy>> getRealtyObjectManagPeriod = query => query
                .Join(
                    manOrgContragentRoService.GetAll(),
                    x => x,
                    y => y.RealityObject.Id,
                    (x, y) => new RealtyObjectManagPeriodProxy
                    {
                        homeId = y.RealityObject.Id,
                        StartDate = y.ManOrgContract.StartDate,
                        EndDate = y.ManOrgContract.EndDate,
                        TypeContractManOrgRealObj = y.ManOrgContract.TypeContractManOrgRealObj
                    })
                .Where(x => x.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
                .ToList();

            var directManagementList = getRealtyObjectManagPeriod(realtyObjIdsQuery1)
                .Union(getRealtyObjectManagPeriod(realtyObjIdsQuery2))
                .Union(getRealtyObjectManagPeriod(realtyObjIdsQuery0))
                .ToList();

            return directManagementList.GroupBy(x => x.homeId).ToDictionary(x => x.Key, x => x.Select(y => new ManagPeriodProxy { StartDate = y.StartDate, EndDate = y.EndDate }).ToArray());
            }
            finally
            {
                this.Container.Release(manOrgContragentRoService);
            }
        }

        private Dictionary<long, ManOrganizationProxy> GetManagingOrganization()
        {
            var manOrgService = this.Container.Resolve<IDomainService<ManagingOrganization>>();

            try
            {
                var disposalsContragentIdsQuery = this.DisposalRepository
                    .GetAll()
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.Inspection != null)
                    .Where(x => x.KindCheck != null)
                    .Where(x => x.Inspection.Contragent != null)
                    .Select(x => x.Inspection.Contragent.Id);

                var manOrgsInfo =
                        manOrgService
                        .GetAll()
                        .Where(x => disposalsContragentIdsQuery.Contains(x.Contragent.Id))
                        .Select(x => new ManOrganizationProxy { contragentId = x.Contragent.Id, typeManagement = x.TypeManagement })
                        .ToList();

                return manOrgsInfo
                    .GroupBy(x => x.contragentId)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());
            }
            finally 
            {
                this.Container.Release(manOrgService);
            }
        }

        private Dictionary<long, Dictionary<long, ProtocolProxy>> GetProtocolData(List<long> realtyObjIds, out IQueryable<long> protocolIds)
        {
            var serviceProtocol = this.Container.Resolve<IDomainService<Protocol>>();
            var serviceProtocolViolation = this.Container.Resolve<IDomainService<ProtocolViolation>>();
            var serviceProtocolArticleLaw = this.Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var inspectionViolStageDomain = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>();

            try
            {
                protocolIds = this.DisposalRepository.GetAll()
                .Join(
                    serviceProtocol.GetAll(),
                    x => x.Stage.Id,
                    y => y.Stage.Parent.Id,
                    (a, b) => new { Disposal = a, Protocol = b })
                .Join(
                    inspectionViolStageDomain.GetAll(),
                    x => x.Protocol.Id,
                    y => y.Document.Id,
                    (c, d) => new
                    {
                        InspectionViolStageId = d.Id,
                        c.Disposal,
                        c.Protocol,
                        ProtocolViol = d
                    })
                .Join(
                    serviceProtocolViolation.GetAll(),
                    x => x.InspectionViolStageId,
                    x => x.Id,
                    (a, b) => a)
                .Where(x => x.Disposal.Inspection != null)
                .Where(x => x.Disposal.DocumentDate >= this.dateStart)
                .Where(x => x.Disposal.DocumentDate <= this.dateEnd)
                .Where(x => x.Disposal.KindCheck != null)
                .Select(x => x.Protocol.Id);

                var protocolsQuery = this.DisposalRepository.GetAll()
                    .Join(
                        serviceProtocol.GetAll(),
                        x => x.Stage.Id,
                        y => y.Stage.Parent.Id,
                        (a, b) => new { Disposal = a, Protocol = b })
                    .Join(
                        inspectionViolStageDomain.GetAll(),
                        x => x.Protocol.Id,
                        y => y.Document.Id,
                        (a, b) => new
                        {
                            InspectionGjiViolStageId = b.Id,
                            a.Disposal,
                            a.Protocol,
                            ProtocolViol = b
                        })
                    .Join(serviceProtocolViolation.GetAll(),
                        x => x.InspectionGjiViolStageId,
                        x => x.Id,
                        (a, b) => a)
                    .Where(x => x.Disposal.Inspection != null)
                    .Where(x => x.Disposal.DocumentDate >= this.dateStart)
                    .Where(x => x.Disposal.DocumentDate <= this.dateEnd)
                    .Where(x => x.Disposal.KindCheck != null)
                    .Where(x => x.ProtocolViol.InspectionViolation.RealityObject != null);

                var protocols = protocolsQuery
                    .Select(x => new
                    {
                        disposalId = x.Disposal.Id,
                        roId = x.ProtocolViol.InspectionViolation.RealityObject.Id,
                        protocolId = x.Protocol.Id
                    })
                    .AsEnumerable()
                    .Distinct()
                    .ToList();

                var protococolsIdsQuery = protocolsQuery.Select(x => x.Protocol.Id);

                var protocolArticleLawDict =
                    serviceProtocolArticleLaw.GetAll()
                    .Where(x => protococolsIdsQuery.Contains(x.Protocol.Id))
                    .Select(x => new { x.Protocol.Id, x.ArticleLaw.Code })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Code).ToList());

                var result = protocols
                    .Where(x => realtyObjIds.Contains(x.roId))
                    .GroupBy(x => x.disposalId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.roId)
                              .ToDictionary(
                                y => y.Key,
                                y =>
                                {
                                    var distinctProtocolIdsOfDisposal = y.Select(z => z.protocolId).Distinct().ToList();

                                    var houseProtocolsStates = protocolArticleLawDict.Where(z => distinctProtocolIdsOfDisposal.Contains(z.Key))
                                                          .Select(z => z.Value)
                                                          .ToList();

                                    var protocolProxy = new ProtocolProxy
                                    {
                                        protocols = distinctProtocolIdsOfDisposal,
                                        totalCount = distinctProtocolIdsOfDisposal.Count(),
                                        st_7_21_1 = houseProtocolsStates.Any(z => z.Any(r => r == "2")),
                                        st_7_21_2 = houseProtocolsStates.Any(z => z.Any(r => r == "3")),
                                        st_7_22 = houseProtocolsStates.Any(z => z.Any(r => r == "4")),
                                        st_7_23 = houseProtocolsStates.Any(z => z.Any(r => r == "5")),
                                        st_9_16_4 = houseProtocolsStates.Any(z => z.Any(r => r == "11")),
                                        st_9_16_5 = houseProtocolsStates.Any(z => z.Any(r => r == "12")),
                                        st_19_4 = houseProtocolsStates.Any(z => z.Any(r => new[] { "6", "110", "111", "119" }.Contains(r))),
                                        st_19_5_1 = houseProtocolsStates.Any(z => z.Any(r => new[] { "1", "7" }.Contains(r))),
                                        st_19_6 = houseProtocolsStates.Any(z => z.Any(r => r == "8")),
                                        st_19_7 = houseProtocolsStates.Any(z => z.Any(r => r == "9")),
                                        st_20_25_1 = houseProtocolsStates.Any(z => z.Any(r => r == "10")),
                                        st_7_23_1 = houseProtocolsStates.Any(z => z.Any(r => new[] { "13", "118" }.Contains(r))),
                                        st_13_19_2_1 = houseProtocolsStates.Any(z => z.Any(r => r == "28")),
                                        st_13_19_2_2 = houseProtocolsStates.Any(z => z.Any(r => r == "26")),
                                        st_6_24_1 = houseProtocolsStates.Any(z => z.Any(r => r == "15")),
                                        st_9_5_1_12 = houseProtocolsStates.Any(z => z.Any(r => r == "30")),
                                        st_9_5_1_5 = houseProtocolsStates.Any(z => z.Any(r => r == "31")),
                                        st_9_5_1_4 = houseProtocolsStates.Any(z => z.Any(r => r == "32")),
                                        st_9_23_1 = houseProtocolsStates.Any(z => z.Any(r => r == "22")),
                                        st_9_23_2 = houseProtocolsStates.Any(z => z.Any(r => r == "23")),
                                        st_9_23_3 = houseProtocolsStates.Any(z => z.Any(r => r == "24")),
                                        st_9_23_4 = houseProtocolsStates.Any(z => z.Any(r => r == "25")),
                                        st_14_1_3_1 = houseProtocolsStates.Any(z => z.Any(r => r == "37")),
                                        st_14_1_3_2 = houseProtocolsStates.Any(z => z.Any(r => r == "36")),
                                        st_19_5_24 = houseProtocolsStates.Any(z => z.Any(r => r == "20")),
                                        st_19_5_24_1 = houseProtocolsStates.Any(z => z.Any(r => r == "35")),
                                        st_7_23_2_1 = houseProtocolsStates.Any(z => z.Any(r => r == "39")),
                                        st_7_23_3 = houseProtocolsStates.Any(z => z.Any(r => r == "18")),
                                    };

                                    return protocolProxy;
                                }));

                return result;
            }
            finally 
            {
                this.Container.Release(serviceProtocol);
                this.Container.Release(serviceProtocolViolation);
                this.Container.Release(inspectionViolStageDomain);
                this.Container.Release(serviceProtocolArticleLaw);
            }
        }

        private Dictionary<long, ResolutionProxy> GetResolutionData(IQueryable<long> protocolIds)
        {
            var serviceDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceResolution = this.Container.Resolve<IDomainService<Resolution>>();
            var serviceResolutionPayFine = this.Container.Resolve<IDomainService<ResolutionPayFine>>();

            try
            {
                var dataQuery = serviceDocumentGjiChildren.GetAll()
                .Join(
                    serviceResolution.GetAll(),
                    x => x.Children.Id,
                    y => y.Id,
                    (e, f) => new { DocumentGjiChildren = e, Resolution = f });

                var resolutions = dataQuery
                    .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                    .Where(x => protocolIds.Contains(x.DocumentGjiChildren.Parent.Id))
                    .Select(x => new
                    {
                        resolutionId = x.Resolution.Id,
                        parentStageId = x.Resolution.Stage.Parent.Id,
                        sanctionCode = x.Resolution.Sanction.Code,
                        initiativeOrg = x.Resolution.TypeInitiativeOrg,
                        penaltyAmount = x.Resolution.PenaltyAmount,
                        paided = x.Resolution.Paided,
                        protocolId = x.DocumentGjiChildren.Parent.Id,
                        executantCode = x.Resolution.Executant.Code
                    })
                    .ToList();

                var paidPenalty = dataQuery
                    .Join(
                        serviceResolutionPayFine.GetAll(),
                        x => x.Resolution.Id,
                        y => y.Resolution.Id,
                        (g, h) => new { g.Resolution, g.DocumentGjiChildren, ResolutionPayFine = h })
                    .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                    .Where(x => protocolIds.Contains(x.DocumentGjiChildren.Parent.Id))
                    .Select(x => new
                    {
                        resolutionId = x.Resolution.Id,
                        protocolId = x.DocumentGjiChildren.Parent.Id,
                        paidAmount = x.ResolutionPayFine.Amount ?? 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.resolutionId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.paidAmount).Sum());
                var officialsCodes = new[] { "16","10","12","13","1","3","5" };
                var legalsCodes = new[] { "15","9","0","2","4"};
                var result = resolutions
                    .GroupBy(x => x.protocolId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var res = new ResolutionProxy { ProsecutedLegalCount = 0, ProsecutedOfficialCount = 0 };

                            foreach (var currentData in x)
                            {
                                var resolution = currentData;
                                var court = resolution.initiativeOrg == TypeInitiativeOrgGji.Court;
                                var gji = resolution.initiativeOrg == TypeInitiativeOrgGji.HousingInspection;
                                var isAdmPenalty = resolution.sanctionCode == "1";
                                var isFinished = new[] { "2", "3", "4" }.Contains(resolution.sanctionCode);
                                var paidSum = paidPenalty.ContainsKey(resolution.resolutionId)
                                                  ? paidPenalty[resolution.resolutionId]
                                                  : 0;

                                if (officialsCodes.Contains(resolution.executantCode))
                                {
                                    res.ProsecutedOfficialCount++;
                                }

                                if (legalsCodes.Contains(resolution.executantCode))
                                {
                                    res.ProsecutedLegalCount++;
                                }
                                
                                res.Count++;
                                res.CourtAdmPenalty += court && isAdmPenalty ? 1 : 0;
                                res.CourtFinished += court && isFinished ? 1 : 0;
                                res.GjiAdmPenalty += gji && isAdmPenalty ? 1 : 0;
                                res.GjiFinished += gji && isFinished ? 1 : 0;
                                res.CourtSumPenalty += court && isAdmPenalty ? (resolution.penaltyAmount.HasValue ? resolution.penaltyAmount.Value : 0m) : 0m;
                                res.GjiSumPenalty += gji && isAdmPenalty ? (resolution.penaltyAmount.HasValue ? resolution.penaltyAmount.Value : 0m) : 0m;
                                res.CourtSumPenaltyPaid += court ? paidSum : 0;
                                res.GjiSumPenaltyPaid += gji ? paidSum : 0;
                            }

                            return res;
                        });

                return result;
            }
            finally 
            {
                this.Container.Release(serviceDocumentGjiChildren);
                this.Container.Release(serviceResolution);
                this.Container.Release(serviceResolutionPayFine);
            }
        }

        private Dictionary<long, List<string>> GetNotPlannedInspectionsByTypes(IQueryable<long> disposalIdsQuery)
        {
            var surveyService = this.Container.Resolve<IDomainService<DisposalTypeSurvey>>();

            try
            {
                var typeSurveyList = surveyService.GetAll()
                .Where(x => disposalIdsQuery.Contains(x.Disposal.Id))
                .Select(x => new
                {
                    disposalId = x.Disposal.Id,
                    typeSurveyCode = x.TypeSurvey.Code
                })
                .ToList();

                return typeSurveyList.GroupBy(x => x.disposalId).ToDictionary(x => x.Key, x => x.Select(y => y.typeSurveyCode).ToList());
            }
            finally 
            {
                this.Container.Release(surveyService);
            }
        }

        private Dictionary<long, string> GetInspectors(IQueryable<long> disposalIdsQuery)
        {
            var inspectorService = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();

            try
            {
                var inspectorList = inspectorService.GetAll()
                .Where(x => disposalIdsQuery.Contains(x.DocumentGji.Id))
                .Where(x => x.Inspector != null)
                .Select(x => new
                {
                    x.DocumentGji.Id,
                    x.Inspector.Fio
                })
                .ToList();

            return inspectorList
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key, 
                    x => x.Select(y => y.Fio)
                          .Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)));
            }
            finally 
            {
                this.Container.Release(inspectorService);
            }
        }

        // Возвращает распоряжения/решения с домами (распоряжения НА проверку предписаний) + запрос идентификаторов домов
        private List<House> GetHousesFromDisposalsByPrescription(out IQueryable<long> houseIdsQuery)
        {
            var repChildrenParentActs = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var prescViolService = this.Container.Resolve<IDomainService<PrescriptionViol>>();

            try
            {
                // запрос распоряжений/решений на проверку предписаний
                var disposaldGjiDocIdsQuery = this.DisposalRepository.GetAll()
                        .Where(x => x.Inspection != null)
                        .Where(x => x.DocumentDate != null)
                        .Where(x => x.DocumentDate >= this.dateStart)
                        .Where(x => x.DocumentDate <= this.dateEnd)
                        .Where(x => x.KindCheck != null)
                        .Where(x => x.TypeDisposal == TypeDisposalGji.DocumentGji)
                        .Select(x => x.Id);

                var parentPrescriptionsQuery = repChildrenParentActs.GetAll()
                    .Where(x => disposaldGjiDocIdsQuery.Contains(x.Children.Id))
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription);

                var parentPrescriptions = parentPrescriptionsQuery.Select(x => new { parentId = x.Parent.Id, childId = x.Children.Id }).ToList();

                var disposalByPrescriptionDict = parentPrescriptions
                    .Distinct()
                    .GroupBy(x => x.parentId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.childId).First());

                var disposalParentPrescriptionIdsQuery = parentPrescriptionsQuery.Select(x => x.Parent.Id);

                var servicePrescriptionViolations = prescViolService.GetAll()
                    .WhereIf(this.municipalityListId.Length > 0, x => this.municipalityListId.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
                    .Where(x => x.InspectionViolation.RealityObject != null)
                    .Where(x => disposalParentPrescriptionIdsQuery.Contains(x.Document.Id));

                var homesFromPrescriptionViolation =
                    servicePrescriptionViolations
                    .Select(x => new
                    {
                        DocumentId = x.Document.Id,
                        inspectionId = x.InspectionViolation.Inspection.Id,
                        homeId = x.InspectionViolation.RealityObject.Id,
                        address = x.InspectionViolation.RealityObject.Address,
                        municipalityId = x.InspectionViolation.RealityObject.Municipality != null ? x.InspectionViolation.RealityObject.Municipality.Id : 0,
                        municipalityName = x.InspectionViolation.RealityObject.Municipality != null ? x.InspectionViolation.RealityObject.Municipality.Name : string.Empty,
                        group = x.InspectionViolation.RealityObject.Municipality != null ? x.InspectionViolation.RealityObject.Municipality.Group ?? string.Empty : string.Empty,
                        typeOwnerShip = x.InspectionViolation.RealityObject.TypeOwnership.Name
                    })
                    .AsEnumerable()
                    .Select(x => new House
                    {
                        disposalId = disposalByPrescriptionDict.TryGetValue(x.DocumentId, out var value) ? value : -1,
                        homeId = x.homeId,
                        address = x.address,
                        municipalityId = x.municipalityId,
                        municipalityName = x.municipalityName,
                        @group = x.group,
                        typeOwnerShip = x.typeOwnerShip
                    })
                    .ToList();

                houseIdsQuery = servicePrescriptionViolations.Select(x => x.InspectionViolation.RealityObject.Id);

                return homesFromPrescriptionViolation;
            }
            finally 
            {
                this.Container.Release(repChildrenParentActs);
                this.Container.Release(prescViolService);
            }
        }
        
        // Возвращает распоряжения с домами, с показателем устранения всех нарушений
        private Dictionary<long, Dictionary<long, bool>> GetFixedActRemovals()
            => this.GetActRemovalViolationsProxy(YesNoNotSet.Yes)
                .GroupBy(x => x.DisposalId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.HomeId)
                        .ToDictionary(y => y.Key, y => y.All(z => z.DateFactRemoval.HasValue)));

        private Dictionary<long, Dictionary<long, ActProxy>> GetActRemovals()
        {
            var violationFeatureDomain = this.Container.Resolve<IDomainService<ViolationFeatureGji>>();
            using (this.Container.Using(violationFeatureDomain))
            {
                var actRemovalViolations = this.GetActRemovalViolationsProxy(YesNoNotSet.No);

                var violIds = actRemovalViolations.Select(x => x.ViolationId);

                var violFeatureList = violationFeatureDomain.GetAll()
                    .Where(x => violIds.Contains(x.ViolationGji.Id))
                    .Select(x => new { x.FeatureViolGji.Code, x.ViolationGji.Id })
                    .ToList();

                var violFeature = violFeatureList
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Code.ToLong()).ToList());

                var violCodes = this.GetViolationFeatureCodes();

                var violationsByRoByDisposalDict = actRemovalViolations
                    .GroupBy(x => x.DisposalId)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.HomeId)
                    .ToDictionary(y => y.Key, y =>
                    {
                        var actProxy = new ActProxy { violationsCount = y.Count() };

                        foreach (var violation in y)
                        {
                            if (violFeature.TryGetValue(violation.ViolationId, out var features))
                            {
                                var violationsByFeatures = violCodes.ToDictionary(z => z.Key, z => features.Any(z.Value.Contains) ? 1 : 0);

                                for (int i = 1; i <= 17; ++i)
                                {
                                    actProxy.violationsByFeatures[i] += violationsByFeatures[i];
                                }
                            }
                        }

                        return actProxy;
                    }));

                return violationsByRoByDisposalDict;
            }
        }

        // Возвращает распоряжения/решения с домами (распоряжения/решение НЕ на проверку предписаний) + запросы идентификаторов домов (распоряжений/решений с актами и без актов)
        private List<House> GetHousesFromDisposalsNotByPrescription(out IQueryable<long> housesIdsQuery1, out IQueryable<long> housesIdsQuery2)
        {
            var docChildDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var actCheckRealityObjectDomain = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();

            using (this.Container.Using(docChildDomain, actCheckRealityObjectDomain))
            {
                var disposalQuery = this.DisposalRepository.GetAll()
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.Inspection != null)
                    .Where(x => x.KindCheck != null)
                    .Where(x => x.TypeDisposal != TypeDisposalGji.DocumentGji);

                var disposalIdsQuery = disposalQuery.Select(x => x.Id);

                var disposalsWithActsQuery = docChildDomain.GetAll()
                    .Where(x => disposalIdsQuery.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x=> new { disposalId = x.Parent.Id, actId = x.Children.Id });

                var serviceDisposalsWithActsHouses = this.DisposalRepository.GetAll()
                    .Join(
                        docChildDomain.GetAll(),
                        a => a.Id,
                        b => b.Parent.Id,
                        (a, b) => new { Disposal = a, DocumentGjiChildren = b })
                    .Join(
                        actCheckRealityObjectDomain.GetAll(),
                        c => c.DocumentGjiChildren.Children.Id,
                        d => d.ActCheck.Id,
                        (c, d) => new { c.Disposal, ActCheckRealityObject = d })
                    .Where(x => x.Disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
                    .Where(x => x.Disposal.DocumentDate >= this.dateStart)
                    .Where(x => x.Disposal.DocumentDate <= this.dateEnd)
                    .Where(x => x.Disposal.Inspection != null)
                    .Where(x => x.Disposal.KindCheck != null)
                    .WhereIf(this.municipalityListId.Length > 0,
                        x => this.municipalityListId.Contains(x.ActCheckRealityObject.RealityObject.Municipality.Id));

                var serviceDisposalsWithoutActsHouses = this.DisposalRepository.GetAll()
                        .Join(this.InspectionRoDomain.GetAll(),
                            a => a.Inspection.Id,
                            b => b.Inspection.Id,
                            (a, b) => new
                            {
                                Disposal = a,
                                InspectionGjiRealityObject = b
                            })
                        .Where(x => x.Disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
                        .Where(x => x.Disposal.DocumentDate >= this.dateStart)
                        .Where(x => x.Disposal.DocumentDate <= this.dateEnd)
                        .Where(x => x.Disposal.Inspection != null)
                        .Where(x => x.Disposal.KindCheck != null)
                        .Where(x => !disposalsWithActsQuery.Select(y => y.disposalId).Contains(x.Disposal.Id))
                        .WhereIf(this.municipalityListId.Length > 0,
                            x => this.municipalityListId.Contains(x.InspectionGjiRealityObject.RealityObject.Municipality.Id));
                // дома распоряжений/решений с актами проверки
                var disposalsWithActsHouses = serviceDisposalsWithActsHouses
                    .Where(x => x.ActCheckRealityObject.RealityObject != null)
                    .Select(x => new House
                    {
                        disposalId = x.Disposal.Id,
                        homeId = x.ActCheckRealityObject.RealityObject.Id,
                        address = x.ActCheckRealityObject.RealityObject.Address,
                        municipalityId =
                            x.ActCheckRealityObject.RealityObject.Municipality != null ? x.ActCheckRealityObject.RealityObject.Municipality.Id : 0,
                        municipalityName = x.ActCheckRealityObject.RealityObject.Municipality != null
                            ? x.ActCheckRealityObject.RealityObject.Municipality.Name
                            : string.Empty,
                        @group = x.ActCheckRealityObject.RealityObject.Municipality != null
                            ? x.ActCheckRealityObject.RealityObject.Municipality.Group ?? string.Empty
                            : string.Empty,
                        typeOwnerShip = x.ActCheckRealityObject.RealityObject.TypeOwnership.Name
                    })
                    .ToList();

                // дома распоряжений/решений БЕЗ актов проверки
                var disposalsWithoutActsHouses = serviceDisposalsWithoutActsHouses
                    .Select(x => new House
                    {
                        disposalId = x.Disposal.Id,
                        homeId = x.InspectionGjiRealityObject.RealityObject.Id,
                        address = x.InspectionGjiRealityObject.RealityObject.Address,
                        municipalityId = x.InspectionGjiRealityObject.RealityObject.Municipality.Id,
                        municipalityName = x.InspectionGjiRealityObject.RealityObject.Municipality.Name,
                        @group = x.InspectionGjiRealityObject.RealityObject.Municipality.Group ?? string.Empty,
                        typeOwnerShip = x.InspectionGjiRealityObject.RealityObject.TypeOwnership.Name
                    })
                    .ToList();

                // запрос идентификаторов домов
                housesIdsQuery1 = serviceDisposalsWithActsHouses.Select(x => x.ActCheckRealityObject.RealityObject.Id);
                housesIdsQuery2 = serviceDisposalsWithoutActsHouses.Select(x => x.InspectionGjiRealityObject.RealityObject.Id);

                return disposalsWithoutActsHouses.Union(disposalsWithActsHouses).ToList();
            }
        }

        private HashSet<long> GetDisposalWithPresentation()
        {
            var presentationDomain = this.Container.ResolveDomain<Presentation>();

            using (this.Container.Using(presentationDomain))
            {
                return presentationDomain.GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Join(this.DisposalRepository.GetAll(),
                        x => x.Inspection.Id,
                        x => x.Inspection.Id,
                        (pres, disp) => disp
                    )
                    .Where(x => x.Inspection != null)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.KindCheck != null)
                    .Select(x => x.Id)
                    .ToHashSet();
            }
        }
        
        private Dictionary<long,string> GetDisposalsInspectionTypeBase()
        {
            var disposalRepository = this.Container.ResolveRepository<TatarstanDisposal>();

            using (Container.Using(disposalRepository))
            {
                return disposalRepository.GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => this.dateStart <= x.DocumentDate)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.InspectionBase != null)
                    .ToDictionary(x => x.Id, x => x.InspectionBase.Code);
            }
        }

        private  Dictionary<RealityObjectProxy, RealityObjectDocInfo> PrepareDocsInfo()
        {
           var roDocInfoDict = new Dictionary<RealityObjectProxy, RealityObjectDocInfo>();
            
            this.GetPresentationInfo(ref roDocInfoDict);
            this.GetTaskActionIsolatedInfo(ref roDocInfoDict);
            this.GetWarningDocInfo(ref roDocInfoDict);
            this.GetProtocolArticleInfo(ref roDocInfoDict);
            this.GetResolutionInfo(ref roDocInfoDict);

            return roDocInfoDict;
        }

        private void GetPresentationInfo(ref Dictionary<RealityObjectProxy, RealityObjectDocInfo> dict)
        {
            var actCheckRealityObjectDomain = this.Container.ResolveDomain<ActCheckRealityObject>();
            var resolProsRealityObjectDomain = this.Container.ResolveDomain<ResolProsRealityObject>();
            var protocolMvdRealityObjectDomain = this.Container.ResolveDomain<ProtocolMvdRealityObject>();
            var inspectionGjiViolDomain = this.Container.ResolveDomain<InspectionGjiViol>();
            var presentationDomain = this.Container.ResolveDomain<Presentation>();

            using (this.Container.Using
            (
                actCheckRealityObjectDomain,
                resolProsRealityObjectDomain,
                protocolMvdRealityObjectDomain,
                inspectionGjiViolDomain,
                presentationDomain)
            )
            {
                var disposalQuery = this.DisposalRepository
                    .GetAll()
                    .Where(x => x.Inspection != null)
                    .Select(x => x.Inspection.Id);
                
                var presentationQuery = presentationDomain
                    .GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => !disposalQuery.Any(y => y == x.Inspection.Id))
                    .Select(x => x.Inspection.Id);
                
                var actCheck = actCheckRealityObjectDomain
                    .GetAll()
                    .Where(x => x.ActCheck != null && x.RealityObject != null)
                    .Where(x => x.ActCheck.Inspection != null)
                    .Where(x => presentationQuery.Any(y => y == x.ActCheck.Inspection.Id))
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.ActCheck.Inspection.Id
                    })
                    .AsEnumerable();
                
                var resolPros = resolProsRealityObjectDomain
                    .GetAll()
                    .Where(x => x.ResolPros != null && x.RealityObject != null)
                    .Where(x => x.ResolPros.Inspection != null)
                    .Where(x => presentationQuery.Any(y => y == x.ResolPros.Inspection.Id))
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.ResolPros.Inspection.Id
                    })
                    .AsEnumerable();
                
                var protocolMvd = protocolMvdRealityObjectDomain
                    .GetAll()
                    .Where(x => x.ProtocolMvd != null && x.RealityObject != null)
                    .Where(x => x.ProtocolMvd.Inspection != null)
                    .Where(x => presentationQuery.Any(y => y == x.ProtocolMvd.Inspection.Id))
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.ProtocolMvd.Inspection.Id
                    })
                    .AsEnumerable();
                
                var inspectionViol = inspectionGjiViolDomain
                    .GetAll()
                    .Where(x => x.Inspection != null && x.RealityObject != null)
                    .Where(x => presentationQuery.Any(y => y == x.Inspection.Id))
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.Inspection.Id
                    })
                    .AsEnumerable();
                
                dict = actCheck
                    .Union(resolPros)
                    .Union(protocolMvd)
                    .Union(inspectionViol)
                    .GroupBy(x => new RealityObjectProxy
                    {
                        MunicipalityId = x.RealityObject.Municipality.Id,
                        RoId = x.RealityObject.Id,
                        Address = x.RealityObject.Address,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Area = x.RealityObject.AreaMkd
                    })
                    .ToDictionary(x => x.Key,
                        x => new RealityObjectDocInfo()
                        {
                            PresentationCount = x.Count()
                        });
            }
        }

        private void GetTaskActionIsolatedInfo(ref Dictionary<RealityObjectProxy, RealityObjectDocInfo> dict)
        {
            var taskActionIsolatedRealityObjectDomain = this.Container.ResolveDomain<TaskActionIsolatedRealityObject>();
            var dictRef = dict;

            using (this.Container.Using(taskActionIsolatedRealityObjectDomain))
            {
                var docInfoDict = taskActionIsolatedRealityObjectDomain
                    .GetAll()
                    .Where(x => x.Task != null && x.RealityObject != null)
                    .Where(x => x.Task.Inspection != null)
                    .Where(x => x.Task.DocumentDate >= this.dateStart)
                    .Where(x => x.Task.DocumentDate <= this.dateEnd)
                    .GroupBy(x => new RealityObjectProxy
                        {
                            MunicipalityId = x.RealityObject.Municipality.Id,
                            RoId = x.RealityObject.Id,
                            Address = x.RealityObject.Address,
                            MunicipalityName = x.RealityObject.Municipality.Name,
                            Area = x.RealityObject.AreaMkd
                        },
                        x => x.Task.KindAction)
                    .ToDictionary(x => x.Key,
                        x => new RealityObjectDocInfo
                        {
                            SurveyTaskActionIsolatedCount = x.Count(y => y == KindAction.Survey),
                            ObservationTaskActionIsolatedCount = x.Count(y => y == KindAction.Observation)
                        });
                
                docInfoDict.ForEach(x =>
                {
                    if (dictRef.ContainsKey(x.Key))
                    {
                        dictRef[x.Key].SurveyTaskActionIsolatedCount = x.Value.SurveyTaskActionIsolatedCount;
                        dictRef[x.Key].ObservationTaskActionIsolatedCount = x.Value.ObservationTaskActionIsolatedCount;
                    }
                    else
                    {
                        dictRef[x.Key] = x.Value;
                    }
                });
            }
        }

        private void GetWarningDocInfo(ref Dictionary<RealityObjectProxy, RealityObjectDocInfo> dict)
        {
            var warningDocViolationsDomain = this.Container.ResolveDomain<WarningDocViolations>();
            var dictRef = dict;

            using (this.Container.Using(warningDocViolationsDomain))
            {
                var docInfoDict = warningDocViolationsDomain
                    .GetAll()
                    .Where(x => x.WarningDoc != null && x.RealityObject != null)
                    .Where(x => x.WarningDoc.Inspection != null)
                    .Where(x => new[]{TypeBase.ActionIsolated, TypeBase.CitizenStatement}.Contains(x.WarningDoc.Inspection.TypeBase))
                    .Where(x => x.WarningDoc.DocumentDate >= this.dateStart)
                    .Where(x => x.WarningDoc.DocumentDate <= this.dateEnd)
                    .GroupBy(x => new RealityObjectProxy
                        {
                            MunicipalityId = x.RealityObject.Municipality.Id,
                            RoId = x.RealityObject.Id,
                            Address = x.RealityObject.Address,
                            MunicipalityName = x.RealityObject.Municipality.Name,
                            Area = x.RealityObject.AreaMkd
                        },
                        x => x.WarningDoc.Id)
                    .ToDictionary(x => x.Key,
                        x => new RealityObjectDocInfo
                        {
                            WarningDocCount = x.Count()
                        });

                docInfoDict.ForEach(x =>
                {
                    if (dictRef.ContainsKey(x.Key))
                    {
                        dictRef[x.Key].WarningDocCount = x.Value.WarningDocCount;
                    }
                    else
                    {
                        dictRef[x.Key] = x.Value;
                    }
                });
            }
        }

        private HashSet<long> GetDisposalsWithWarningDoc()
        {
            var warningDocDomain = this.Container.ResolveDomain<WarningDoc>();
            var documentGjiDomain = this.Container.ResolveDomain<DocumentGji>();

            using (this.Container.Using(warningDocDomain, documentGjiDomain))
            {
                return this.DisposalRepository.GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.KindCheck != null)
                    .Join(documentGjiDomain.GetAll(),
                        x => x.Id,
                        y => y.Id,
                        (x, y) => new
                        {
                            DisposalId = x.Id,
                            InspectionId = y.Inspection.Id
                        })
                    .Join(documentGjiDomain.GetAll(),
                        x => x.InspectionId,
                        y => y.Inspection.Id,
                        (x, y) => new
                        {
                            x.DisposalId,
                            DocId = y.Id
                        })
                    .Join(
                        warningDocDomain.GetAll(),
                        x => x.DocId,
                        x => x.Id,
                        (x, y) => new
                        {
                            x.DisposalId,
                            y.DocumentDate
                        }
                    )
                    .Select(x => x.DisposalId)
                    .ToHashSet();
            }
        }

        private void GetProtocolArticleInfo(ref Dictionary<RealityObjectProxy, RealityObjectDocInfo> dict)
        {
            var dictRef = dict;
            var protocolMvdRealityObjectDomain = this.Container.ResolveDomain<ProtocolMvdRealityObject>();
            var protocolArticleLawDomain = this.Container.ResolveDomain<ProtocolArticleLaw>();
            var tatarstanProtocolGjiRealityObjectDomain = this.Container.ResolveDomain<TatarstanProtocolGjiRealityObject>();
            var tatarstanProtocolGjiArticleLawDomain = this.Container.ResolveDomain<TatarstanProtocolGjiArticleLaw>();

            using (this.Container.Using(
                protocolMvdRealityObjectDomain,
                protocolArticleLawDomain,
                tatarstanProtocolGjiRealityObjectDomain,
                tatarstanProtocolGjiArticleLawDomain))
            {
                var codes = new[] {"18", "20", "35", "39"};

                var tatrstanProtocolQuery = tatarstanProtocolGjiRealityObjectDomain
                    .GetAll()
                    .Where(x => x.RealityObject != null && x.TatarstanProtocolGji != null)
                    .Where(x => x.TatarstanProtocolGji.Inspection != null)
                    .Where(x => x.TatarstanProtocolGji.DocumentDate >= this.dateStart)
                    .Where(x => x.TatarstanProtocolGji.DocumentDate <= this.dateEnd)
                    .Join(tatarstanProtocolGjiArticleLawDomain.GetAll(),
                        x => x.TatarstanProtocolGji.Id,
                        x => x.TatarstanProtocolGji.Id,
                        (ro, article) => new
                        {
                            ro.RealityObject,
                            article.ArticleLaw.Code
                        })
                    .Where(x => codes.Contains(x.Code))
                    .AsEnumerable();

                var docInfoDict = protocolMvdRealityObjectDomain
                    .GetAll()
                    .Where(x => x.RealityObject != null && x.ProtocolMvd != null)
                    .Where(x => x.ProtocolMvd.Inspection != null)
                    .Where(x => x.ProtocolMvd.DocumentDate >= this.dateStart)
                    .Where(x => x.ProtocolMvd.DocumentDate <= this.dateEnd)
                    .Join(protocolArticleLawDomain.GetAll(),
                        x => x.ProtocolMvd.Inspection.Id,
                        x => x.Protocol.Inspection.Id,
                        (ro, article) => new
                        {
                            ro.RealityObject,
                            article
                        })
                    .Where(x => x.article.Protocol != null && x.article.ArticleLaw != null)
                    .Where(x => x.article.Protocol.Inspection != null)
                    .Where(x => codes.Contains(x.article.ArticleLaw.Code))
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.article.ArticleLaw.Code
                    })
                    .AsEnumerable()
                    .Union(tatrstanProtocolQuery)
                    .GroupBy(x => new RealityObjectProxy()
                        {
                            MunicipalityId = x.RealityObject.Municipality.Id,
                            RoId = x.RealityObject.Id,
                            Address = x.RealityObject.Address,
                            MunicipalityName = x.RealityObject.Municipality.Name,
                            Area = x.RealityObject.AreaMkd
                        },
                        x => x.Code)
                    .ToDictionary(x => x.Key,
                        x => new RealityObjectDocInfo
                        {
                            Article_19_5_p_24_Count = x.Count(y => y.Equals("20")),
                            Article_19_5_p_24_1_Count = x.Count(y => y.Equals("35")),
                            Article_7_23_2_p_1_Count = x.Count(y => y.Equals("39")),
                            Article_7_23_3_Count = x.Count(y => y.Equals("18"))
                        });
                
                docInfoDict.ForEach(x =>
                {
                    if (dictRef.ContainsKey(x.Key))
                    {
                        dictRef[x.Key].Article_19_5_p_24_Count = x.Value.Article_19_5_p_24_Count;
                        dictRef[x.Key].Article_19_5_p_24_1_Count = x.Value.Article_19_5_p_24_1_Count;
                        dictRef[x.Key].Article_7_23_2_p_1_Count = x.Value.Article_7_23_2_p_1_Count;
                        dictRef[x.Key].Article_7_23_3_Count = x.Value.Article_7_23_3_Count;
                    }
                    else
                    {
                        dictRef[x.Key] = x.Value;
                    }
                });
            }
        }

        private void GetResolutionInfo(ref Dictionary<RealityObjectProxy, RealityObjectDocInfo> dict)
        {
            var dictRef = dict;
            var officialsCodes = new[] { "16","10","12","13","1","3","5" };
            var legalsCodes = new[] { "15","9","0","2","4"};
            var protocolMvdRealityObjectDomain = this.Container.ResolveDomain<ProtocolMvdRealityObject>();
            var resolutionDomain = this.Container.ResolveDomain<Resolution>();
            var tatarstanProtocolGjiRealityObjectDomain = this.Container.ResolveDomain<TatarstanProtocolGjiRealityObject>();
            var tatarstanResolutionDomain = this.Container.ResolveDomain<TatarstanResolutionGji>();

            using (this.Container.Using(
                protocolMvdRealityObjectDomain, 
                tatarstanProtocolGjiRealityObjectDomain, 
                resolutionDomain, 
                tatarstanResolutionDomain))
            {
                var protocolMvdResolData = protocolMvdRealityObjectDomain
                    .GetAll()
                    .Where(x => x.RealityObject != null && x.ProtocolMvd != null)
                    .Where(x => x.ProtocolMvd.Inspection != null)
                    .Join(resolutionDomain.GetAll(),
                        x => x.ProtocolMvd.Inspection.Id,
                        x => x.Inspection.Id,
                        (ro, resol) => new
                        {
                            ro.RealityObject,
                            Resolution = resol
                        })
                    .Where(x => x.Resolution.Inspection != null)
                    .Where(x => x.Resolution.DocumentDate >= this.dateStart)
                    .Where(x => x.Resolution.DocumentDate <= this.dateEnd)
                    .Where(x => officialsCodes.Concat(legalsCodes).Contains(x.Resolution.Executant.Code))
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.Resolution.Executant.Code
                    })
                    .AsEnumerable();

                var tatarstanProtocolResolData = tatarstanProtocolGjiRealityObjectDomain
                    .GetAll()
                    .Where(x => x.RealityObject != null && x.TatarstanProtocolGji != null)
                    .Where(x => x.TatarstanProtocolGji.Inspection != null)
                    .Join(resolutionDomain.GetAll(),
                        x => x.TatarstanProtocolGji.Inspection.Id,
                        x => x.Inspection.Id,
                        (a, b) => new
                        {
                            TatProtocolGjiRo = a,
                            Resolution = b
                        })
                    .Join(tatarstanResolutionDomain.GetAll(),
                        x => x.Resolution.Id,
                        x => x.Id,
                        (ro, resol) => new
                        {
                            ro.TatProtocolGjiRo.RealityObject,
                            Resolution = resol
                        })
                    .Where(x => x.Resolution.Inspection != null)
                    .Where(x => x.Resolution.DocumentDate >= this.dateStart)
                    .Where(x => x.Resolution.DocumentDate <= this.dateEnd)
                    .Where(x => officialsCodes.Concat(legalsCodes).Contains(x.Resolution.Executant.Code))
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.Resolution.Executant.Code
                    })
                    .AsEnumerable();

                var docInfoDict = protocolMvdResolData
                    .Union(tatarstanProtocolResolData)
                    .GroupBy(x => new RealityObjectProxy()
                        {
                            MunicipalityId = x.RealityObject.Municipality.Id,
                            RoId = x.RealityObject.Id,
                            Address = x.RealityObject.Address,
                            MunicipalityName = x.RealityObject.Municipality.Name,
                            Area = x.RealityObject.AreaMkd
                        },
                        x => x.Code)
                    .ToDictionary(x => x.Key,
                    x => new RealityObjectDocInfo 
                    {
                        ProsecutedLegalCount = x.Count(y => legalsCodes.Contains(y)),
                        ProsecutedOfficialCount = x.Count(y => officialsCodes.Contains(y))
                    });
            
            docInfoDict.ForEach(x =>
            {
                if (dictRef.ContainsKey(x.Key))
                {
                    dictRef[x.Key].ProsecutedLegalCount = x.Value.ProsecutedLegalCount;
                    dictRef[x.Key].ProsecutedOfficialCount = x.Value.ProsecutedOfficialCount;
                }
                else
                {
                    dictRef[x.Key] = x.Value;
                }
            });

            }
        }

        private List<ActRemovalViolationsProxy> GetActRemovalViolationsProxy(YesNoNotSet typeRemoval)
        {
            var actRemovalDomain = this.Container.Resolve<IDomainService<ActRemoval>>();
            var actRemovalViolationDomain = this.Container.Resolve<IDomainService<ActRemovalViolation>>();
            var documentGjiDomain = this.Container.ResolveDomain<DocumentGji>();
            var inspectionGjiStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();
            var inspectionViolStageDomain = this.Container.ResolveDomain<InspectionGjiViolStage>();

            using (this.Container.Using(
                actRemovalDomain,
                actRemovalViolationDomain,
                documentGjiDomain,
                inspectionGjiStageDomain,
                inspectionViolStageDomain))
            {
                return this.DisposalRepository.GetAll()
                    .Join(documentGjiDomain.GetAll(),
                        x => x.Id,
                        y => y.Id,
                        (x, y) => new
                        {
                            DisposalId = x.Id,
                            y.DocumentDate,
                            x.TypeDisposal,
                            InspectionId = (long?)y.Inspection.Id,
                            KindCheckId = (long?)x.KindCheck.Id,
                            StageId = y.Stage.Id
                        })
                    .Join(inspectionGjiStageDomain.GetAll(),
                        x => x.StageId,
                        y => y.Parent.Id,
                        (x, y) => new
                        {
                            x.DisposalId,
                            x.DocumentDate,
                            x.TypeDisposal,
                            x.InspectionId,
                            x.KindCheckId,
                            InspStageId = y.Id,
                            ParentStageId = y.Parent.Id
                        })
                    .Join(documentGjiDomain.GetAll(),
                        x => x.InspStageId,
                        y => y.Stage.Id,
                        (x, y) => new
                        {
                            x.DisposalId,
                            x.DocumentDate,
                            x.TypeDisposal,
                            x.InspectionId,
                            x.KindCheckId,
                            x.ParentStageId,
                            DocumentGjiId = y.Id
                        })
                    .Join(actRemovalDomain.GetAll(),
                        x => x.DocumentGjiId,
                        y => y.Id,
                        (x, y) => new
                        {
                            x.DisposalId,
                            x.DocumentDate,
                            x.TypeDisposal,
                            x.InspectionId,
                            x.KindCheckId,
                            y.TypeRemoval,
                            x.ParentStageId,
                            ActRemovalId = y.Id
                        })
                    .Join(inspectionViolStageDomain.GetAll(),
                        x => x.ActRemovalId,
                        y => y.Document.Id,
                        (x, y) => new
                        {
                            InspectionViolStageId = y.Id,
                            x.DisposalId,
                            x.TypeDisposal,
                            x.DocumentDate,
                            x.ActRemovalId,
                            x.InspectionId,
                            x.KindCheckId,
                            x.TypeRemoval,
                            x.ParentStageId,
                            HomeId = (long?)y.InspectionViolation.RealityObject.Id,
                            y.DateFactRemoval,
                            ViolId = y.InspectionViolation.Violation.Id,
                            ActId = y.Document.Id
                        })
                    .Join(actRemovalViolationDomain.GetAll(),
                        x => x.InspectionViolStageId,
                        x => x.Id,
                        (a, b) => a)
                    .Where(x => x.TypeDisposal == TypeDisposalGji.DocumentGji)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.TypeRemoval == typeRemoval)
                    .Where(x => x.InspectionId != null)
                    .Where(x => x.KindCheckId != null)
                    .Where(x => x.HomeId != null)
                    .Select(x => new ActRemovalViolationsProxy
                    {
                        DisposalId = x.DisposalId,
                        TypeDisposal = x.TypeDisposal,
                        DocumentDate = x.DocumentDate,
                        InspectionId = x.InspectionId,
                        KindCheckId = x.KindCheckId,
                        ActRemovalId = x.ActRemovalId,
                        ParentStageId = x.ParentStageId,
                        HomeId = x.HomeId.Value,
                        DateFactRemoval = x.DateFactRemoval,
                        ViolationId = x.ViolId,
                        ActId = x.ActId
                    })
                    .ToList();
            }
        }

        private class ActRemovalViolationsProxy
        {
            public long DisposalId { get; set; }

            public TypeDisposalGji TypeDisposal { get; set; }

            public DateTime? DocumentDate { get; set; }

            public long ParentStageId { get; set; }

            public long HomeId { get; set; }

            public DateTime? DateFactRemoval { get; set; }

            public long? InspectionId { get; set; }

            public long? KindCheckId { get; set; }

            public long ActRemovalId { get; set; }

            public long ViolationId { get; set; }

            public long ActId { get; set; }
        }
    }
    
    public class RealityObjectProxy
    {
        /// <summary>
        /// Идентификатор МО
        /// </summary>
        public long MunicipalityId;
        
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public long RoId;
        
        /// <summary>
        /// Площадь МКД
        /// </summary>
        public decimal? Area;
        
        /// <summary>
        /// Адрес дома
        /// </summary>
        public string Address;
        
        /// <summary>
        /// Название МО
        /// </summary>
        public string MunicipalityName;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var RoProxy = obj as RealityObjectProxy;

            if (RoProxy == null)
            {
                return false;
            }

            return this.RoId == RoProxy.RoId;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.RoId.GetHashCode();
        }
    }

    struct ResolutionProxy
    {
        public int Count;
        public int CourtAdmPenalty;
        public int CourtFinished;
        public int GjiAdmPenalty;
        public int GjiFinished;
        public int ProsecutedOfficialCount;
        public int ProsecutedLegalCount;
        public decimal CourtSumPenalty;
        public decimal GjiSumPenalty;
        public decimal CourtSumPenaltyPaid;
        public decimal GjiSumPenaltyPaid;
    }

    struct ProtocolProxy
    {
        public List<long> protocols;
        public int totalCount;
        public bool st_7_21_1;
        public bool st_7_21_2;
        public bool st_7_22;
        public bool st_7_23;
        public bool st_9_16_4;
        public bool st_9_16_5;
        public bool st_19_4;
        public bool st_19_5_1;
        public bool st_19_6;
        public bool st_19_7;
        public bool st_20_25_1;
        public bool st_7_23_1;
        public bool st_13_19_2_1;
        public bool st_13_19_2_2;
        public bool st_6_24_1;
        public bool st_9_5_1_12;
        public bool st_9_5_1_5;
        public bool st_9_5_1_4;
        public bool st_9_23_1;
        public bool st_9_23_2;
        public bool st_9_23_3;
        public bool st_9_23_4;
        public bool st_14_1_3_1;
        public bool st_14_1_3_2;
        public bool st_19_5_24;
        public bool st_19_5_24_1;
        public bool st_7_23_2_1;
        public bool st_7_23_3;
    }

    struct PrescriptionProxy
        {
            public int totalCount;
            public int byJurPersonCount;
            public int byOffPersonCount;
            public int byPhysPersonCount;
            public int eliminatedVoilationsCount;
        }

    public class House
    {
        public long disposalId;

        public long homeId;

        public string address;

        public long municipalityId;

        public string municipalityName;

        public string group;

        public string typeOwnerShip;
    }

    public class ActProxy
    {
        public decimal? area;
        public Dictionary<long, long> violationsByFeatures;

        public int violationsCount;

        public ActProxy()
        {
            this.violationsByFeatures = new Dictionary<long, long>();

            for (int i = 1; i <= 17; ++i)
            {
                this.violationsByFeatures[i] = 0;
            }
        }
    }

    public class ManOrganizationProxy
    {
        public long contragentId;

        public TypeManagementManOrg typeManagement;
    }

    public class ManagPeriodProxy
    {
        public DateTime? StartDate;

        public DateTime? EndDate;
    }

    public class RealtyObjectManagPeriodProxy
    {
        public long homeId;

        public DateTime? StartDate;

        public DateTime? EndDate;

        public TypeContractManOrg TypeContractManOrgRealObj;
    }

    /// <summary>
    /// Класс для хранения информации о док-ах, не связанных с разпоряжением
    /// в разрезе дома
    /// </summary>
    public class RealityObjectDocInfo
    {
        /// <summary>
        /// Кол-во представлений
        /// </summary>
        public int PresentationCount;
        
        /// <summary>
        /// Кол-во выездных проверок КНМ при взаимодействии с КЛ
        /// </summary>
        public int SurveyTaskActionIsolatedCount;
        
        /// <summary>
        /// Кол-во инспекционных визитов КНМ при взаимодействии с КЛ
        /// </summary>
        public int ObservationTaskActionIsolatedCount;
        
        /// <summary>
        /// Кол-во предостережений профилактического мероприятия
        /// </summary>
        public int WarningDocCount;

        /// <summary>
        /// Кол-во протоколов по ст.19.5 ч.24
        /// </summary>
        public int Article_19_5_p_24_Count;
        
        /// <summary>
        /// Кол-во протоколов по ст.19.5 ч.24.1
        /// </summary>
        public int Article_19_5_p_24_1_Count;
        
        /// <summary>
        /// Кол-во протоколов по ст.7.23.2 ч.1
        /// </summary>
        public int Article_7_23_2_p_1_Count;
        
        /// <summary>
        /// Кол-во протоколов по ст.7.23.3
        /// </summary>
        public int Article_7_23_3_Count;
        
        /// <summary>
        /// Кол-во должностных лиц, привлеченных к ответственности
        /// </summary>
        public int ProsecutedOfficialCount;
        
        /// <summary>
        /// Кол-во юридических лиц, привлеченных к ответственности
        /// </summary>
        public int ProsecutedLegalCount;
    }

    public class DisposalProxy
    {
        public long disposalId;

        public DateTime? docDate;

        public string docNumber;

        public TypeBase typeBase;

        public TypeJurPerson typeJurPerson;

        public string contragentName;

        public long? contragentId;

        public TypeCheck kindCheckCode;

        public TypeDisposalGji typeDisposal;

        public TypeAgreementProsecutor typeAgreementProsecutor;

        public TypeAgreementResult typeAgreementResult;

        public bool isByPrescription;

        public PersonInspection personInspection;

        public TypeFormInspection? InspectionForm;
    }
}
