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

    public class GjiWorkReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.Now;
        private DateTime dateEnd = DateTime.Now;
        private List<long> municipalityListId = new List<long>();

        public GjiWorkReport() : base(new ReportTemplateBinary(Bars.GkhGji.Properties.Resources.GjiWorkReport))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.GjiWork";
            }
        }

        public override string Name
        {
            get { return "Работа ГЖИ за период"; }
        }

        public override string Desciption
        {
            get { return "Работа ГЖИ за период"; }
        }

        public override string GroupName
        {
            get { return "Инспекторская деятельность"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.GjiWork"; }
        }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id = 0;
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
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            this.ParseIds(municipalityListId, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalService = this.Container.Resolve<IDomainService<Disposal>>();

            try
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

                IQueryable<long> disposalByPrescriptionRoIdsQuery;

                // Дома по распоряжениям по проверке предписания - дома берем из нарушений предписания, идентификаторы которых возвращаются out-параметром.
                var housesFromDisposalsByPrescription = this.GetHousesFromDisposalsByPrescription(out disposalByPrescriptionRoIdsQuery);

                IQueryable<long> disposalNotByPrescriptionRoIdsQuery1;
                IQueryable<long> disposalNotByPrescriptionRoIdsQuery2;

                // Дома по распоряжениям НЕ по проверке предписания. Идентификаторы домов для распоряжений с актами и для распоряжений без актов возвращаются out-параметрами.
                var housesFromDisposalsNotByPrescription = this.GetHousesFromDisposalsNotByPrescription(out disposalNotByPrescriptionRoIdsQuery1, out disposalNotByPrescriptionRoIdsQuery2);

                var houses = new List<House>();
                houses.AddRange(housesFromDisposalsNotByPrescription);
                houses.AddRange(housesFromDisposalsByPrescription);

                var dataDic = houses.GroupBy(x => x.group)
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => new
                        {
                            y.municipalityId,
                            y.municipalityName
                        })
                                .ToDictionary(
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

                var disposalQuery = disposalService.GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.KindCheck != null)
                    .Select(x => x.Id);

                var typeSurvey = this.GetNotPlannedInspectionsByTypes(disposalQuery);
                var inspectorDic = this.GetInspectors(disposalQuery);
                var actData = this.GetActData(disposalQuery);
                var actsRemovalOfDisposalByPrescription = this.GetActRemovals();
                var fixedActsRemovalOfDisposalByPrescription = this.GetFixedActRemovals();

                var realtyObjIdsList = houses.Select(x => x.homeId).ToList();

                var presriptionProxyDict = this.GetPrescriptionData(realtyObjIdsList);
                IQueryable<long> allProtocols;
                var protocolProxyDict = this.GetProtocolData(realtyObjIdsList, out allProtocols);
                var resolutionProxyDict = this.GetResolutionData(allProtocols);

                var totalsList = new List<int>();
                totalsList.AddRange(Enumerable.Range(18, 104));
                totalsList.AddRange(Enumerable.Range(9, 8));
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

                foreach (var group in dataDic)
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
                            var typeManagement = manOrgs.ContainsKey(contragentId) ? manOrgs[contragentId].typeManagement : (TypeManagementManOrg)(-1);
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
                            var planned = (disposal.kindCheckCode == TypeCheck.PlannedDocumentation
                                          || disposal.kindCheckCode == TypeCheck.PlannedExit)
                                          || disposal.typeBase == TypeBase.Inspection;

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
                                disposalSection["param24"] = 1;

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

                                var resolutions = new List<ResolutionProxy>();

                                foreach (var protocol in roProtocolProxy.protocols)
                                {
                                    if (resolutionProxyDict.ContainsKey(protocol))
                                    {
                                        resolutionOfDisposal = 1;
                                        resolutions.Add(resolutionProxyDict[protocol]);
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
            finally
            {
                Container.Release(disposalService);
            }
        }

        private List<DisposalProxy> GetDisposals()
        {
            var disposalService = this.Container.Resolve<IDomainService<Disposal>>();

            try
            {
                return disposalService.GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => x.DocumentDate != null)
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.KindCheck != null)
                    .Select(x => new DisposalProxy
                    {
                        disposalId = x.Id,
                        inspectionId = x.Inspection.Id,
                        docDate = x.DocumentDate,
                        docNumber = x.DocumentNumber,
                        typeBase = x.Inspection.TypeBase,
                        typeJurPerson = x.Inspection.TypeJurPerson,
                        contragentName = x.Inspection.Contragent.Name,
                        contragentId = (long?)x.Inspection.Contragent.Id,
                        kindCheckCode = x.KindCheck.Code,
                        inspectorFio = x.ResponsibleExecution.Fio,
                        typeDisposal = x.TypeDisposal,
                        typeAgreementProsecutor = x.TypeAgreementProsecutor,
                        typeAgreementResult = x.TypeAgreementResult,
                        stageId = x.Stage.Id,
                        isByPrescription = x.TypeDisposal == TypeDisposalGji.DocumentGji,
                        personInspection = x.Inspection.PersonInspection
                    })
                    .ToList();
            }
            finally 
            {
                Container.Release(disposalService);
            }
            
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

                var violCodes = GetViolationFeatureCodes();

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
                Container.Release(serviceChildrenParentActs);
                Container.Release(serviceActCheck);
                Container.Release(serviceActCheckRealityObject);
                Container.Release(serviceActCheckViolation);
                Container.Release(serviceViolationFeature);
            }

        }

        private Dictionary<long, Dictionary<long, PrescriptionProxy>> GetPrescriptionData(List<long> realtyObjIds)
        {
            var servicePrescription = this.Container.Resolve<IDomainService<Prescription>>();
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();
            var servicePrescriptionViolation = this.Container.Resolve<IDomainService<PrescriptionViol>>();

            try
            {
                var codesList84 = new List<string> { "0", "9", "11", "8", "15", "18", "4" };
                var codesList85 = new List<string> { "1", "10", "12", "13", "16", "19", "5" };
                var codesList86 = new List<string> { "6", "7", "14" };

                var result = serviceDisposal.GetAll()
                    .Join(
                        servicePrescription.GetAll(),
                        x => x.Stage.Id,
                        y => y.Stage.Parent.Id,
                        (a, b) => new { Disposal = a, Prescription = b })
                    .Join(
                        servicePrescriptionViolation.GetAll(),
                        x => x.Prescription.Id,
                        y => y.Document.Id,
                        (c, d) => new { c.Disposal, c.Prescription, PrescriptionViol = d })
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
                Container.Release(servicePrescription);
                Container.Release(serviceDisposal);
                Container.Release(servicePrescriptionViolation);
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
                        contractId = y.ManOrgContract.Id,
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
                Container.Release(manOrgContragentRoService);
            }
        }

        private Dictionary<long, ManOrganizationProxy> GetManagingOrganization()
        {
            var disposalService = this.Container.Resolve<IDomainService<Disposal>>();
            var manOrgService = this.Container.Resolve<IDomainService<ManagingOrganization>>();

            try
            {
                var disposalsContragentIdsQuery =
                    disposalService
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
                Container.Release(disposalService);
                Container.Release(manOrgService);
            }

            
        }

        private Dictionary<long, Dictionary<long, ProtocolProxy>> GetProtocolData(List<long> realtyObjIds, out IQueryable<long> protocolIds)
        {
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();
            var serviceProtocol = this.Container.Resolve<IDomainService<Protocol>>();
            var serviceProtocolViolation = this.Container.Resolve<IDomainService<ProtocolViolation>>();
            var serviceProtocolArticleLaw = this.Container.Resolve<IDomainService<ProtocolArticleLaw>>();

            try
            {
                protocolIds =
                serviceDisposal.GetAll()
                .Join(
                    serviceProtocol.GetAll(),
                    x => x.Stage.Id,
                    y => y.Stage.Parent.Id,
                    (a, b) => new { Disposal = a, Protocol = b })
                .Join(
                    serviceProtocolViolation.GetAll(),
                    x => x.Protocol.Id,
                    y => y.Document.Id,
                    (c, d) => new { c.Disposal, c.Protocol, ProtocolViol = d })
                .Where(x => x.Disposal.Inspection != null)
                .Where(x => x.Disposal.DocumentDate >= this.dateStart)
                .Where(x => x.Disposal.DocumentDate <= this.dateEnd)
                .Where(x => x.Disposal.KindCheck != null)
                .Select(x => x.Protocol.Id);

                var protocolsQuery =
                    serviceDisposal.GetAll()
                    .Join(
                        serviceProtocol.GetAll(),
                        x => x.Stage.Id,
                        y => y.Stage.Parent.Id,
                        (a, b) => new { Disposal = a, Protocol = b })
                    .Join(
                        serviceProtocolViolation.GetAll(),
                        x => x.Protocol.Id,
                        y => y.Document.Id,
                        (c, d) => new { c.Disposal, c.Protocol, ProtocolViol = d })
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
                                        st_14_1_3_2 = houseProtocolsStates.Any(z => z.Any(r => r == "36"))
                                    };

                                    return protocolProxy;
                                }));

                return result;
            }
            finally 
            {
                Container.Release(serviceDisposal);
                Container.Release(serviceProtocol);
                Container.Release(serviceProtocolViolation);
                Container.Release(serviceProtocolArticleLaw);
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
                        protocolId = x.DocumentGjiChildren.Parent.Id
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

                var result = resolutions
                    .GroupBy(x => x.protocolId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var res = new ResolutionProxy();

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
                Container.Release(serviceDocumentGjiChildren);
                Container.Release(serviceResolution);
                Container.Release(serviceResolutionPayFine);
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
                Container.Release(surveyService);
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
                Container.Release(inspectorService);
            }
        }

        // Возвращает распоряжения с домами (распоряжения НА проверку предписаний) + запрос идентификаторов домов
        private List<House> GetHousesFromDisposalsByPrescription(out IQueryable<long> houseIdsQuery)
        {
            var disposalService = this.Container.Resolve<IDomainService<Disposal>>();
            var repChildrenParentActs = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var prescViolService = this.Container.Resolve<IDomainService<PrescriptionViol>>();

            try
            {
                // запрос распоряжений на проверку предписаний
                var disposaldGjiDocIdsQuery =
                    disposalService.GetAll()
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
                    .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.InspectionViolation.RealityObject.Municipality.Id))
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
                        disposalId = disposalByPrescriptionDict.ContainsKey(x.DocumentId) ? disposalByPrescriptionDict[x.DocumentId] : -1,
                        inspectionId = x.inspectionId,
                        homeId = x.homeId,
                        address = x.address,
                        municipalityId = x.municipalityId,
                        municipalityName = x.municipalityName,
                        group = x.group,
                        typeOwnerShip = x.typeOwnerShip
                    })
                    .ToList();

                houseIdsQuery = servicePrescriptionViolations.Select(x => x.InspectionViolation.RealityObject.Id);

                return homesFromPrescriptionViolation;
            }
            finally 
            {
                Container.Release(disposalService);
                Container.Release(repChildrenParentActs);
                Container.Release(prescViolService);
            }
        }
        
        // Возвращает распоряжения с домами, с показателем устранения всех нарушений
        private Dictionary<long, Dictionary<long, bool>> GetFixedActRemovals()
        {
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();
            var serviceActRemoval = this.Container.Resolve<IDomainService<ActRemoval>>();
            var serviceActRemovalViolation = this.Container.Resolve<IDomainService<ActRemovalViolation>>();

            try
            {
                var fixedActRemovalViolations = serviceDisposal.GetAll()
                .Join(
                    serviceActRemoval.GetAll(),
                    a => a.Stage.Id,
                    b => b.Stage.Parent.Id,
                    (a, b) => new
                    {
                        DisposalId = a.Id,
                        a.DocumentDate,
                        a.TypeDisposal,
                        InspectionId = (long?)a.Inspection.Id,
                        KindCheckId = (long?)a.KindCheck.Id,
                        b.TypeRemoval,
                        b.Id,
                        parentStageId = b.Stage.Parent.Id
                    })
                .Join(
                    serviceActRemovalViolation.GetAll(),
                    c => c.Id,
                    d => d.Document.Id,
                    (c, d) => new { c, d })
                .Where(x => x.c.TypeDisposal == TypeDisposalGji.DocumentGji)
                .Where(x => x.c.DocumentDate >= this.dateStart)
                .Where(x => x.c.DocumentDate <= this.dateEnd)
                .Where(x => x.c.InspectionId != null)
                .Where(x => x.c.KindCheckId != null)
                .Where(x => x.c.TypeRemoval == YesNoNotSet.Yes)
                .Where(x => x.d.InspectionViolation.RealityObject != null)
                .Select(x => new
                {
                    x.c,
                    homeId = x.d.InspectionViolation.RealityObject.Id,
                    x.d.DateFactRemoval
                })
                .ToList();

                var isAllViolsFixedByRoByDisopalDict = fixedActRemovalViolations
                    .GroupBy(x => x.c.DisposalId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.homeId)
                            .ToDictionary(y => y.Key, y => y.All(z => z.DateFactRemoval.HasValue)));

                return isAllViolsFixedByRoByDisopalDict;
            }
            finally 
            {
                Container.Release(serviceActRemoval);
                Container.Release(serviceDisposal);
                Container.Release(serviceActRemovalViolation);
            }
        }

        private Dictionary<long, Dictionary<long, ActProxy>> GetActRemovals()
        {
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();
            var serviceActRemoval = this.Container.Resolve<IDomainService<ActRemoval>>();
            var serviceActRemovalViolation = this.Container.Resolve<IDomainService<ActRemovalViolation>>();
            var serviceViolFeature = this.Container.Resolve<IDomainService<ViolationFeatureGji>>();

            try
            {
                var actRemovalViolationsQuery = serviceDisposal.GetAll()
               .Join(
                   serviceActRemoval.GetAll(),
                   a => a.Stage.Id,
                   b => b.Stage.Parent.Id,
                   (a, b) => new
                   {
                       DisposalId = a.Id,
                       a.DocumentDate,
                       a.TypeDisposal,
                       InspectionId = (long?)a.Inspection.Id,
                       KindCheckId = (long?)a.KindCheck.Id,
                       b.TypeRemoval,
                       b.Id,
                       parentStageId = b.Stage.Parent.Id
                   })
               .Join(
                   serviceActRemovalViolation.GetAll(),
                   c => c.Id,
                   d => d.Document.Id,
                   (c, d) => new { c, d })
               .Where(x => x.c.TypeDisposal == TypeDisposalGji.DocumentGji)
               .Where(x => x.c.DocumentDate >= this.dateStart)
               .Where(x => x.c.DocumentDate <= this.dateEnd)
               .Where(x => x.c.InspectionId != null)
               .Where(x => x.c.KindCheckId != null)
               .Where(x => x.c.TypeRemoval == YesNoNotSet.No)
               .Where(x => x.d.InspectionViolation.RealityObject != null)
               .Select(x => new
               {
                   x.c,
                   homeId = x.d.InspectionViolation.RealityObject.Id,
                   x.d.DateFactRemoval,
                   violId = x.d.InspectionViolation.Violation.Id,
                   actId = x.d.Document.Id
               });

                var violIdsQuery = actRemovalViolationsQuery.Select(x => x.violId);

                var actRemovalViolations = actRemovalViolationsQuery.ToList();

                var violFeatureList = serviceViolFeature.GetAll()
                                 .Where(x => violIdsQuery.Contains(x.ViolationGji.Id))
                                 .Select(x => new { x.FeatureViolGji.Code, x.ViolationGji.Id })
                                 .ToList();

                var violFeature = violFeatureList.GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.Select(y => y.Code.ToLong()).ToList());

                var violCodes = this.GetViolationFeatureCodes();

                var violationsByRoByDisopalDict = actRemovalViolations
                    .GroupBy(x => x.c.DisposalId)
                    .ToDictionary(x => x.Key,
                          x => x.GroupBy(y => y.homeId)
                                .ToDictionary(y => y.Key, y =>
                                {
                                    var actProxy = new ActProxy { violationsCount = y.Count() };

                                    foreach (var violation in y)
                                    {
                                        if (violFeature.ContainsKey(violation.violId))
                                        {
                                            var features = violFeature[violation.violId];

                                            var violationsByFeatures = violCodes.ToDictionary(z => z.Key, z => features.Any(z.Value.Contains) ? 1 : 0);

                                            for (int i = 1; i <= 17; ++i)
                                            {
                                                actProxy.violationsByFeatures[i] += violationsByFeatures[i];
                                            }
                                        }
                                    }

                                    return actProxy;
                                }));

                return violationsByRoByDisopalDict;
            }
            finally 
            {
                Container.Release(serviceDisposal);
                Container.Release(serviceActRemoval);
                Container.Release(serviceActRemovalViolation);
                Container.Release(serviceViolFeature);
            }
        }

        // Возвращает распоряжения с домами (распоряжения НЕ на проверку предписаний) + запросы идентификаторов домов (распоряжений с актами и без актов)
        private List<House> GetHousesFromDisposalsNotByPrescription(out IQueryable<long> housesIdsQuery1, out IQueryable<long> housesIdsQuery2)
        {
            var disposalQuery = 
                this.Container.Resolve<IDomainService<Disposal>>()
                    .GetAll()
                    .Where(x => x.DocumentDate >= this.dateStart)
                    .Where(x => x.DocumentDate <= this.dateEnd)
                    .Where(x => x.Inspection != null)
                    .Where(x => x.KindCheck != null)
                    .Where(x => x.TypeDisposal != TypeDisposalGji.DocumentGji);

            var disposalIdsQuery = disposalQuery.Select(x => x.Id);

            var disposalsWithActsQuery =
                this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => disposalIdsQuery.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Select(x => new { disposalId = x.Parent.Id, actId = x.Children.Id });

            var serviceDisposalsWithActsHouses =
                this.Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Join(
                this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll(),
                a => a.Id,
                b => b.Parent.Id,
                (a, b) => new { Disposal = a, DocumentGjiChildren = b })
                .Join(
                this.Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll(),
                c => c.DocumentGjiChildren.Children.Id,
                d => d.ActCheck.Id,
                (c, d) => new { c.Disposal, ActCheckRealityObject = d })
                .Where(x => x.Disposal.TypeDisposal != TypeDisposalGji.DocumentGji)
                .Where(x => x.Disposal.DocumentDate >= this.dateStart)
                .Where(x => x.Disposal.DocumentDate <= this.dateEnd)
                .Where(x => x.Disposal.Inspection != null)
                .Where(x => x.Disposal.KindCheck != null)
                .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ActCheckRealityObject.RealityObject.Municipality.Id));

            var serviceDisposalsWithoutActsHouses = 
                this.Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll(),
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
                .WhereIf(municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.InspectionGjiRealityObject.RealityObject.Municipality.Id));

            // дома распоряжений с актами проверки
            var disposalsWithActsHouses = serviceDisposalsWithActsHouses
                .Where(x => x.ActCheckRealityObject.RealityObject != null)
                .Select(x => new House
                {
                    disposalId = x.Disposal.Id,
                    actId = x.ActCheckRealityObject.ActCheck.Id,
                    inspectionId = x.ActCheckRealityObject.ActCheck.Inspection.Id,
                    homeId = x.ActCheckRealityObject.RealityObject.Id,
                    address = x.ActCheckRealityObject.RealityObject.Address,
                    municipalityId = x.ActCheckRealityObject.RealityObject.Municipality != null ? x.ActCheckRealityObject.RealityObject.Municipality.Id : 0,
                    municipalityName = x.ActCheckRealityObject.RealityObject.Municipality != null ? x.ActCheckRealityObject.RealityObject.Municipality.Name : string.Empty,
                    group = x.ActCheckRealityObject.RealityObject.Municipality != null ? x.ActCheckRealityObject.RealityObject.Municipality.Group ?? string.Empty : string.Empty,
                    typeOwnerShip = x.ActCheckRealityObject.RealityObject.TypeOwnership.Name
                })
                .ToList();

            // дома распоряжений БЕЗ актов проверки
            var disposalsWithoutActsHouses = serviceDisposalsWithoutActsHouses
                .Select(x => new House
                {
                    disposalId = x.Disposal.Id,
                    inspectionId = x.InspectionGjiRealityObject.Inspection.Id,
                    homeId = x.InspectionGjiRealityObject.RealityObject.Id,
                    address = x.InspectionGjiRealityObject.RealityObject.Address,
                    municipalityId = x.InspectionGjiRealityObject.RealityObject.Municipality.Id,
                    municipalityName = x.InspectionGjiRealityObject.RealityObject.Municipality.Name,
                    group = x.InspectionGjiRealityObject.RealityObject.Municipality.Group ?? string.Empty,
                    typeOwnerShip = x.InspectionGjiRealityObject.RealityObject.TypeOwnership.Name
                })
                .ToList();

            // запрос идентификаторов домов
            housesIdsQuery1 = serviceDisposalsWithActsHouses.Select(x => x.ActCheckRealityObject.RealityObject.Id);
            housesIdsQuery2 = serviceDisposalsWithoutActsHouses.Select(x => x.InspectionGjiRealityObject.RealityObject.Id);

            return disposalsWithoutActsHouses.Union(disposalsWithActsHouses).ToList();
        }
    }

    public class MunicipalityProxy
    {
        public long municipalityId;

        public string municipalityName;
    }

    struct ResolutionProxy
    {
        public int Count;
        public int CourtAdmPenalty;
        public int CourtFinished;
        public int GjiAdmPenalty;
        public int GjiFinished;
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
               
        public long actId;
               
        public long inspectionId;
               
        public long homeId;

        public string address;

        public long municipalityId;

        public string municipalityName;

        public string group;

        public string typeOwnerShip;

        public ManagPeriodProxy managPeriodProxy;
    }

    public class ActProxy
    {
        public decimal? area;
        public Dictionary<long, long> violationsByFeatures;

        public int violationsCount;

        public ActProxy()
        {
            violationsByFeatures = new Dictionary<long, long>();

            for (int i = 1; i <= 17; ++i)
            {
                violationsByFeatures[i] = 0;
            }
        }
    }

    public class ManOrganizationProxy
    {
        public long contragentId;

        public string contragentName;

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
               
        public long contractId;
        
        public DateTime? StartDate;

        public DateTime? EndDate;

        public TypeContractManOrg TypeContractManOrgRealObj;
    }

    public class DisposalProxy
    {
        public long disposalId;
               
        public long inspectionId;

        public DateTime? docDate;

        public string docNumber;

        public TypeBase typeBase;

        public TypeJurPerson typeJurPerson;

        public string contragentName;

        public long? contragentId;

        public TypeCheck kindCheckCode;

        public string inspectorFio;

        public TypeDisposalGji typeDisposal;

        public TypeAgreementProsecutor typeAgreementProsecutor;

        public TypeAgreementResult typeAgreementResult;

        public long stageId;

        public bool isByPrescription;

        public PersonInspection personInspection;
    }
}
