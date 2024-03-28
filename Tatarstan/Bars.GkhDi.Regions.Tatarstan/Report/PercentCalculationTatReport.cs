namespace Bars.GkhDi.Regions.Tatarstan.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class PercentCalculationTatReport : GkhDi.Report.PercentCalculationReport
    {
        public override string Desciption
        {
            get
            {
                return "Отчет по раскрытию информации по ПП РФ №731 (Татарстан)";
            }
        }

        public override string Name
        {
            get
            {
                return "Отчет по раскрытию информации по ПП РФ №731 (Татарстан)";
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var contragentList = userManager.GetContragentIds();
            var municipalityList = userManager.GetMunicipalityIds();

            if (municipalityIdsList.Count == 0)
            {
                municipalityIdsList = municipalityList;
            }
            else
            {
                if (municipalityList.Count > 0)
                {
                    municipalityIdsList = municipalityIdsList.Intersect(municipalityList).ToList();
                }
            }

            var period = Container.Resolve<IDomainService<PeriodDi>>().GetAll().FirstOrDefault(x => x.Id == periodDiId);
            if (period == null)
            {
                throw new Exception("Не определен период отчета");
            }

            var transferredManOrgIds = new List<long>();

            if (transsferredManag == false)
            {
                transferredManOrgIds = Container.Resolve<IDomainService<ManOrgContractRelation>>()
                .GetAll()
                .Where(x => x.TypeRelation == TypeContractRelation.TransferTsjUk)
                .WhereIf(municipalityIdsList.Count > 0, x => municipalityIdsList.Contains(x.Parent.ManagingOrganization.Contragent.Municipality.Id))
                .Where(x => x.Parent.StartDate <= period.DateEnd && (!x.Parent.EndDate.HasValue || x.Parent.EndDate >= period.DateStart))
                .Select(x => x.Parent.ManagingOrganization.Id)
                .Distinct()
                .ToList();
            }

            var moIds = Container.Resolve<IDomainService<ManagingOrganization>>()
                         .GetAll()
                         .WhereIf(municipalityIdsList.Count > 0, x => municipalityIdsList.Contains(x.Contragent.Municipality.Id))
                         .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.Contragent.Id))
                         .Where(x => (x.ActivityDateEnd.HasValue && x.ActivityDateEnd > period.DateStart)
                             || (!x.ActivityDateEnd.HasValue && x.ActivityGroundsTermination == GroundsTermination.NotSet))
                         .Select(x => x.Id)
                         .ToArray()
                         .AsQueryable()
                         .WhereIf(transsferredManag == false, x => !transferredManOrgIds.Contains(x))
                         .ToArray();

            if (moIds.Length == 0)
            {
                return;
            }

            var start = 1000;
            var tmpMoIds = moIds.Length > start ? moIds.Take(1000).ToArray() : moIds.ToArray();

            var manOrgContractsList = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                        .Where(x => x.ManOrgContract != null && tmpMoIds.Contains(x.ManOrgContract.ManagingOrganization.Id))
                        .Where(x => x.ManOrgContract.StartDate <= period.DateEnd && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= period.DateStart))
                        .Select(x => new { x.ManOrgContract.TypeContractManOrgRealObj, x.ManOrgContract.Id, ManOrgId = x.ManOrgContract.ManagingOrganization.Id, RealObjId = x.RealityObject.Id })
                        .ToList();


            var manOrgJskTsjContractsDict = Container.Resolve<IDomainService<ManOrgContractTransfer>>().GetAll()
                .Where(x => tmpMoIds.Contains(x.ManagingOrganization.Id))
                .Where(x => x.StartDate <= period.DateEnd && (!x.EndDate.HasValue || x.EndDate >= period.DateStart))
                .ToDictionary(x => x.Id, y => y.ManOrgJskTsj.Contragent.Name);

            var noTransferredManagContracts = Container.Resolve<IDomainService<ManOrgJskTsjContract>>()
                .GetAll()
                .Where(x => tmpMoIds.Contains(x.ManagingOrganization.Id) && x.IsTransferredManagement == YesNoNotSet.No)
                .Where(x => x.StartDate <= period.DateEnd && (!x.EndDate.HasValue || x.EndDate >= period.DateStart))
                .Select(x => x.Id)
                .ToList();

            var realObjsByMo = GetManagRealObj(tmpMoIds, period);

            while (start < moIds.Length)
            {
                tmpMoIds = moIds.Skip(start).Take(1000).ToArray();
                manOrgContractsList.AddRange(Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                        .Where(x => x.ManOrgContract != null && tmpMoIds.Contains(x.ManOrgContract.ManagingOrganization.Id))
                        .Where(x => x.ManOrgContract.StartDate <= period.DateEnd && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= period.DateStart))
                        .Select(x => new { x.ManOrgContract.TypeContractManOrgRealObj, x.ManOrgContract.Id, ManOrgId = x.ManOrgContract.ManagingOrganization.Id, RealObjId = x.RealityObject.Id })
                        .ToList());

                manOrgJskTsjContractsDict = manOrgJskTsjContractsDict.Union(Container.Resolve<IDomainService<ManOrgContractTransfer>>().GetAll()
                .Where(x => tmpMoIds.Contains(x.ManagingOrganization.Id))
                .Where(x => x.StartDate <= period.DateEnd && (!x.EndDate.HasValue || x.EndDate >= period.DateStart))
               .ToDictionary(x => x.Id, y => y.ManOrgJskTsj.Contragent.Name)).ToDictionary(x => x.Key, y => y.Value);

                noTransferredManagContracts.AddRange(Container.Resolve<IDomainService<ManOrgJskTsjContract>>()
                .GetAll()
                .Where(x => tmpMoIds.Contains(x.ManagingOrganization.Id) && x.IsTransferredManagement == YesNoNotSet.No)
                .Where(x => x.StartDate <= period.DateEnd && (!x.EndDate.HasValue || x.EndDate >= period.DateStart))
                .Select(x => x.Id)
                .ToList());

                realObjsByMo = realObjsByMo.Union(GetManagRealObj(tmpMoIds, period)).ToDictionary(x => x.Key, y => y.Value);

                start += 1000;
            }

            var manOrgContracts = manOrgContractsList.GroupBy(x => x.RealObjId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.ManOrgId).ToDictionary(x => x.Key, z => z.Select(x => new { x.TypeContractManOrgRealObj, x.Id }).FirstOrDefault()));

            var roIds = realObjsByMo.SelectMany(x => x.Value).Distinct().ToDictionary(x => x);

            var manOrgsPercent = GetManOrgData(moIds.ToList(), period);
            var realObjsPercentDict = new Dictionary<long, RoPercProxy>();
            
            var query2 = OpenSession().CreateSQLQuery(SqlQueryRealObj());
            query2.SetParameter("period", periodDiId);
            var sqlQueryResult2 = query2.List();

            //Получение доп. данных, которые сильно замедляют основной запрос
            var extraData = this.GetExtraData(period);

            foreach (object[] record in sqlQueryResult2)
            {
                var roId = record[0].ToLong();

                if (!roIds.ContainsKey(roId))
                {
                    continue;
                }

                var realObjPercent = new RoPercProxy
                {
                    RealObjId = record[0].ToLong(),
                    Address = record[1].ToStr(),
                    PlanRedPay = record[3].To<int?>(),
                    FileActState = record[4].To<int?>(),
                    FileCatRepair = record[5].To<int?>(),
                    FilePlanRepair = record[6].To<int?>(),
                    NonResPlace = record[8].To<int?>(),
                    PlaceGenUse = record[9].To<int?>(),
                    ManagServ = record[10].To<int?>(),
                    Serv2 = record[11].To<int?>(),
                    Serv6 = record[12].To<int?>(),
                    Serv7 = record[13].To<int?>(),
                    Serv13 = record[14].To<int?>(),
                    Serv14 = record[15].To<int?>(),
                    Serv27 = record[16].To<int?>(),
                    Serv28 = record[17].To<int?>(),
                    Serv8 = 0,
                    Serv9 = 0,
                    Serv10 = 0,
                    Serv11 = 0,
                    Serv12 = 0,
                    Serv18 = record[23].To<int?>(),
                    Serv17 = record[24].To<int?>(),
                    Serv19 = record[25].To<int?>(),
                    Serv20 = record[26].To<int?>(),
                    Serv22 = record[27].To<int?>(),
                    Serv21 = record[28].To<int?>(),
                    AddServ = record[29].To<int?>(),
                    Serv16 = record[32].To<int?>(),
                };

                if (extraData.RepairService.ContainsKey(roId))
                {
                    var repairServicesPercent = extraData.RepairService[roId];

                    /*
                        serv_8, -- Текущий ремонт жилого здания и благоустройство территории
                        serv_9, -- Текущий ремонт и содержание внутридомовых инженерных сетей центрального отопления
                        serv_10, -- Текущий ремонт и содержание внутридомовых инженерных сетей газоснабжения
                        serv_11, -- Текущий ремонт и содержание внутридомовых инженерных сетей водоснабжения и водоотведения
                        serv_12, -- Текущий ремонт и содержание внутридомовых инженерных сетей электроснабжения
                     */

                    realObjPercent.Serv8 = repairServicesPercent.ContainsKey("8") ? repairServicesPercent["8"] : 0;
                    realObjPercent.Serv9 = repairServicesPercent.ContainsKey("9") ? repairServicesPercent["9"] : 0;
                    realObjPercent.Serv10 = repairServicesPercent.ContainsKey("10") ? repairServicesPercent["10"] : 0;
                    realObjPercent.Serv11 = repairServicesPercent.ContainsKey("11") ? repairServicesPercent["11"] : 0;
                    realObjPercent.Serv12 = repairServicesPercent.ContainsKey("12") ? repairServicesPercent["12"] : 0;
                }

                realObjPercent.PlanRedExp = extraData.PlanRedExp.ContainsKey(roId) ? 1 : 0;
                realObjPercent.PlanWorkServ = extraData.PlanWorkServ.ContainsKey(roId) ? extraData.PlanWorkServ[roId] : 0;

                realObjPercent = CalcRoPercent(realObjPercent);
                if (!realObjsPercentDict.ContainsKey(record[0].ToLong()))
                {
                    realObjsPercentDict.Add(record[0].ToLong(), realObjPercent);
                }
            }


            var manOrgsPercByMunicipality = manOrgsPercent.GroupBy(x => x.Municipality)
                                                          .ToDictionary(x => x.Key,
                                                                         y => y.GroupBy(z => z.ManOrgId).ToDictionary(x => x.Key, z => z.FirstOrDefault()));

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияМО");

            decimal manOrgInfoPercents = 0;
            decimal realObjsPercents = 0;
            decimal planWorkPercents = 0;
            int number = 0;
            foreach (var manOrgsPerc in manOrgsPercByMunicipality)
            {
                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["НаименованиеМО"] = manOrgsPerc.Key;

                var sectionManOrg = sectionMunicipality.ДобавитьСекцию("СекцияУО");
                decimal averMoDisInfosPerc = 0;
                decimal averMoRealObjPerc = 0;
                decimal averMoPlanWorkPerc = 0;
                var skippedCount = 0;

                foreach (var manOrgPerc in manOrgsPerc.Value)
                {
                    var moPerc = manOrgPerc.Value;

                    if (!realObjsByMo.ContainsKey(moPerc.ManOrgId))
                    {
                        skippedCount++;
                        continue;
                    }

                    sectionManOrg.ДобавитьСтроку();
                    sectionManOrg["НаименованиеУО"] = moPerc.ManOrgName;
                    sectionManOrg["ФИОруководителя"] = moPerc.Director != null ? moPerc.Director.ToStr() : " - ";
                    sectionManOrg["ПочтовыйАдрес"] = moPerc.MailingAddress != null ? moPerc.MailingAddress.ToStr() : " - ";
                    sectionManOrg["ФактическийАдрес"] = moPerc.FactAddress != null ? moPerc.FactAddress.ToStr() : " - ";
                    sectionManOrg["Телефон"] = moPerc.Phone != null ? moPerc.Phone.ToStr() : " - ";
                    sectionManOrg["ОфициальныйСайт"] = moPerc.Site != null ? moPerc.Site.ToStr() : " - ";
                    sectionManOrg["ЭлектронныйАдрес"] = moPerc.Email != null ? moPerc.Email.ToStr() : " - ";
                    sectionManOrg["РежимРаботы"] = moPerc.WorkMode != null ? moPerc.WorkMode.ToStr() : " - ";
                    sectionManOrg["ОГРН"] = moPerc.Ogrn != null ? moPerc.Ogrn.ToStr() : " - ";
                    sectionManOrg["РежимПриемаГраждан"] = moPerc.RecepCitizen != null ? moPerc.RecepCitizen.ToStr() : " - ";
                    sectionManOrg["ЧленыПравления"] = moPerc.DirectionMember != null ? moPerc.DirectionMember.ToStr() : " - ";
                    sectionManOrg["ЧленыРевКомиссии"] = moPerc.RevComMember != null ? moPerc.RevComMember.ToStr() : " - ";
                    sectionManOrg["РежимДиспСлужб"] = moPerc.DispatchWork != null ? moPerc.DispatchWork.ToStr() : " - ";
                    sectionManOrg["ФайлПроекта"] = moPerc.FileProjContr != null ? moPerc.FileProjContr.ToStr() : " - ";
                    sectionManOrg["АдминистративнаяОтветстенность"] = moPerc.AdminResp != null ? moPerc.AdminResp.ToStr() : " - ";
                    sectionManOrg["ЧленствоОбъед"] = moPerc.MemUnion != null ? moPerc.MemUnion.ToStr() : " - ";
                    sectionManOrg["РасторгнутыеДоговоры"] = moPerc.TermContr != null ? moPerc.TermContr.ToStr() : " - ";
                    sectionManOrg["СведенияОФондах"] = moPerc.FundsInfo != null ? moPerc.FundsInfo.ToStr() : " - ";
                    sectionManOrg["СведенияОДоговорах"] = moPerc.InfoContr != null ? moPerc.InfoContr.ToStr() : " - ";
                    sectionManOrg["СистемаНалогообложения"] = moPerc.TaxSystem != null ? moPerc.TaxSystem.ToStr() : " - ";
                    sectionManOrg["БухгалтерскийБаланс"] = moPerc.Balance != null ? moPerc.Balance.ToStr() : " - ";
                    sectionManOrg["ПриложБухБаланс"] = moPerc.BalanceAnnex != null ? moPerc.BalanceAnnex.ToStr() : " - ";
                    sectionManOrg["СметаДоходов"] = moPerc.EstimateCurr != null ? moPerc.EstimateCurr.ToStr() : " - ";
                    sectionManOrg["СметаДоходовПрошлыйГод"] = moPerc.EstimatePrev != null ? moPerc.EstimatePrev.ToStr() : " - ";
                    sectionManOrg["ОтчетПрошлыйГод"] = moPerc.ReportEstimatePrev != null ? moPerc.ReportEstimatePrev.ToStr() : " - ";
                    sectionManOrg["ЗаклРевКом"] = moPerc.ConRevCurr != null ? moPerc.ConRevCurr.ToStr() : " - ";
                    sectionManOrg["ЗаклРевКомПрошлыйГод"] = moPerc.ConRevPrev != null ? moPerc.ConRevPrev.ToStr() : " - ";
                    sectionManOrg["ЗаклРевКомЗаДваГода"] = moPerc.ConRevPrevPrev != null ? moPerc.ConRevPrevPrev.ToStr() : " - ";
                    sectionManOrg["АудитЗаключение"] = moPerc.AuditCurr != null ? moPerc.AuditCurr.ToStr() : " - ";
                    sectionManOrg["АудитЗаключениеПрошлыйГод"] = moPerc.AuditPrev != null ? moPerc.AuditPrev.ToStr() : " - ";
                    sectionManOrg["АудитЗаключениеЗаДваГода"] = moPerc.AuditPrevPrev != null ? moPerc.AuditPrevPrev.ToStr() : " - ";
                    sectionManOrg["ПредъявленоПолучено"] = moPerc.ManagRealObjs != null ? moPerc.ManagRealObjs.ToStr() : " - ";
                    sectionManOrg["СведенияОбУО"] = moPerc.Percent;
                    averMoDisInfosPerc += moPerc.Percent;
                    var sectionRealObj = sectionManOrg.ДобавитьСекцию("СекцияЖД");

                    decimal averRealObjPerc = 0;
                    decimal averPlanWorkPerc = 0;

                    foreach (var realObjId in realObjsByMo[moPerc.ManOrgId])
                    {
                        if (realObjsPercentDict.ContainsKey(realObjId))
                        {
                            var roPerc = realObjsPercentDict[realObjId];

                            sectionRealObj.ДобавитьСтроку();
                            number++;

                            sectionRealObj["Номер"] = number;
                            sectionRealObj["Адрес"] = roPerc.Address;

                            if (manOrgContracts.ContainsKey(realObjId) && manOrgContracts[realObjId].ContainsKey(moPerc.ManOrgId))
                            {
                                var jskTsjId = manOrgContracts[realObjId][moPerc.ManOrgId].TypeContractManOrgRealObj
                                                == TypeContractManOrg.ManagingOrgJskTsj
                                                    ? manOrgContracts[realObjId][moPerc.ManOrgId].Id
                                                    : 0;

                                sectionRealObj["ТипДоговора"] = manOrgContracts[realObjId][moPerc.ManOrgId].TypeContractManOrgRealObj
                                                                == TypeContractManOrg.ManagingOrgOwners
                                                                || (manOrgContracts[realObjId][moPerc.ManOrgId]
                                                                        .TypeContractManOrgRealObj
                                                                    == TypeContractManOrg.JskTsj
                                                                    && noTransferredManagContracts.Contains(
                                                                        manOrgContracts[realObjId][moPerc.ManOrgId].Id))
                                                                    ? " 1 "
                                                                    : (jskTsjId != 0 ? "0" : " - ");

                                sectionRealObj["НаимТСЖ"] = jskTsjId != 0
                                                            && manOrgJskTsjContractsDict.ContainsKey(jskTsjId)
                                                                ? manOrgJskTsjContractsDict[jskTsjId]
                                                                : " - ";
                            }

                            sectionRealObj["СнижениеРасходов"] = roPerc.PlanRedExp != null
                                                                        ? roPerc.PlanRedExp.ToStr()
                                                                        : " - ";
                            sectionRealObj["СнижениеПлаты"] = roPerc.PlanRedPay != null
                                                                    ? roPerc.PlanRedPay.ToStr()
                                                                    : " - ";
                            sectionRealObj["ДокАктСост"] = roPerc.FileActState != null
                                                                ? roPerc.FileActState.ToStr()
                                                                : " - ";
                            sectionRealObj["ДокПереченьРабот"] = roPerc.FileCatRepair != null
                                                                        ? roPerc.FileCatRepair.ToStr()
                                                                        : " - ";
                            sectionRealObj["ДокОтчет"] = roPerc.FilePlanRepair != null
                                                                ? roPerc.FilePlanRepair.ToStr()
                                                                : " - ";
                            sectionRealObj["СодержаниеРемонт"] = roPerc.PlanWorkServ != null
                                                                        ? roPerc.PlanWorkServ.ToStr()
                                                                        : " - ";
                            sectionRealObj["НежилыхПомещений"] = roPerc.NonResPlace != null
                                                                        ? roPerc.NonResPlace.ToStr()
                                                                        : " - ";
                            sectionRealObj["МестОбщегоПользования"] = roPerc.PlaceGenUse != null
                                                                            ? roPerc.PlaceGenUse.ToStr()
                                                                            : " - ";
                            sectionRealObj["Дополнительная"] = roPerc.AddServ != null
                                                                    ? roPerc.AddServ.ToStr()
                                                                    : " - ";
                            sectionRealObj["УправлениеДомом"] = roPerc.ManagServ != null
                                                                    ? roPerc.ManagServ.ToStr()
                                                                    : " - ";
                            sectionRealObj["УборкаВнутМест"] = roPerc.Serv2 != null ? roPerc.Serv2.ToStr() : " - ";
                            sectionRealObj["УборкаПридомТерр"] = roPerc.Serv6 != null ? roPerc.Serv6.ToStr() : " - ";
                            sectionRealObj["ОбслужМусоропров"] = roPerc.Serv7 != null ? roPerc.Serv7.ToStr() : " - ";
                            sectionRealObj["Дератизация"] = roPerc.Serv13 != null ? roPerc.Serv13.ToStr() : " - ";
                            sectionRealObj["ВывозТБО"] = roPerc.Serv14 != null ? roPerc.Serv14.ToStr() : " - ";
                            sectionRealObj["СодерЛифтов"] = roPerc.Serv27 != null ? roPerc.Serv27.ToStr() : " - ";
                            sectionRealObj["ПользовЛифтом"] = roPerc.Serv28 != null ? roPerc.Serv28.ToStr() : " - ";
                            sectionRealObj["ТекРемонтЖД"] = roPerc.Serv8 != null ? roPerc.Serv8.ToStr() : " - ";
                            sectionRealObj["ТекРемонтИСиЦО"] = roPerc.Serv9 != null ? roPerc.Serv9.ToStr() : " - ";
                            sectionRealObj["ТекРемонтИСиГ"] = roPerc.Serv10 != null ? roPerc.Serv10.ToStr() : " - ";
                            sectionRealObj["ТекРемонтИСВиВ"] = roPerc.Serv11 != null ? roPerc.Serv11.ToStr() : " - ";
                            sectionRealObj["ТекРемонтИСЭ"] = roPerc.Serv12 != null ? roPerc.Serv12.ToStr() : " - ";
                            sectionRealObj["ГВС"] = roPerc.Serv18 != null ? roPerc.Serv18.ToStr() : " - ";
                            sectionRealObj["ХВС"] = roPerc.Serv17 != null ? roPerc.Serv17.ToStr() : " - ";
                            sectionRealObj["Водоотведение"] = roPerc.Serv19 != null ? roPerc.Serv19.ToStr() : " - ";
                            sectionRealObj["Электроснабжение"] = roPerc.Serv20 != null
                                                                        ? roPerc.Serv20.ToStr()
                                                                        : " - ";
                            sectionRealObj["Теплоснабжение"] = roPerc.Serv22 != null ? roPerc.Serv22.ToStr() : " - ";
                            sectionRealObj["Газоснабжение"] = roPerc.Serv21 != null ? roPerc.Serv21.ToStr() : " - ";
                            sectionRealObj["СведенияЖД"] = roPerc.Percent;
                            sectionRealObj["СведПоРабДом"] = (roPerc.PlanWorkServ.ToLong() + roPerc.PlanRedExp.ToLong()) * 25;
                            sectionRealObj["КапРемонт"] = roPerc.Serv16 != null ? roPerc.Serv16.ToStr() : " - ";

                            averRealObjPerc += roPerc.Percent;
                            averPlanWorkPerc += (roPerc.PlanWorkServ.ToLong() + roPerc.PlanRedExp.ToLong()) * 25;

                            sectionRealObj["ОбщийПроцентДом"] = roPerc.Percent + moPerc.Percent;
                        }
                    }

	                var realObjsByMoCount = realObjsByMo[moPerc.ManOrgId].Count();
                    averRealObjPerc = averRealObjPerc / (realObjsByMoCount == 0 ? 1 : realObjsByMoCount);
                    averPlanWorkPerc = averPlanWorkPerc / (realObjsByMoCount == 0 ? 1 : realObjsByMoCount);

                    sectionManOrg["СведенияЖДпоУО"] = averRealObjPerc.RoundDecimal(2);
                    sectionManOrg["СведПоРабЖДпоУО"] = averPlanWorkPerc.RoundDecimal(2);
                    sectionManOrg["ОбщийПроцентУО"] = (averRealObjPerc + moPerc.Percent).RoundDecimal(2);
                    averMoRealObjPerc += averRealObjPerc;
                    averMoPlanWorkPerc += averPlanWorkPerc;
                }

	            var manOrgPercSkippedCount = (manOrgsPerc.Value.Count - skippedCount);
				averMoDisInfosPerc = averMoDisInfosPerc / (manOrgPercSkippedCount == 0 ? 1 : manOrgPercSkippedCount);
				averMoRealObjPerc = averMoRealObjPerc / (manOrgPercSkippedCount == 0 ? 1 : manOrgPercSkippedCount);
				averMoPlanWorkPerc = averMoPlanWorkPerc / (manOrgPercSkippedCount == 0 ? 1 : manOrgPercSkippedCount);

                // среднее по сведениям УО по мо
                sectionMunicipality["СрСведПоMО"] = averMoDisInfosPerc;
                sectionMunicipality["СведенияЖДпоМО"] = averMoRealObjPerc.RoundDecimal(2);
                sectionMunicipality["СведПоРабЖДпоМО"] = averMoPlanWorkPerc.RoundDecimal(2);
                sectionMunicipality["ОбщийПроцентМО"] = (averMoDisInfosPerc + averMoRealObjPerc).RoundDecimal(2);

                manOrgInfoPercents += averMoDisInfosPerc;
                realObjsPercents += averMoRealObjPerc;
                planWorkPercents += averMoPlanWorkPerc;
            }
            manOrgInfoPercents = manOrgInfoPercents / (manOrgsPercByMunicipality.Keys.Count == 0 ? 1 : manOrgsPercByMunicipality.Keys.Count);
            realObjsPercents = realObjsPercents / (manOrgsPercByMunicipality.Keys.Count == 0 ? 1 : manOrgsPercByMunicipality.Keys.Count);
            planWorkPercents = planWorkPercents / (manOrgsPercByMunicipality.Keys.Count == 0 ? 1 : manOrgsPercByMunicipality.Keys.Count);
            reportParams.SimpleReportParams["СредСвед"] = manOrgInfoPercents.RoundDecimal(2);
            reportParams.SimpleReportParams["СведенияЖДОбщ"] = realObjsPercents.RoundDecimal(2);
            reportParams.SimpleReportParams["СведПоРабяЖДОбщ"] = planWorkPercents.RoundDecimal(2);
            reportParams.SimpleReportParams["ОбщийПроцентОбщ"] = (manOrgInfoPercents + realObjsPercents).RoundDecimal(2);

            manOrgsPercByMunicipality.Clear();
            realObjsByMo.Clear();

        }

         protected new Dictionary<long, long[]> GetManagRealObj(IEnumerable<long> manOrgIds, PeriodDi period)
        {
            var realObjManOrg = Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                            .GetAll()
                            .Where(x => manOrgIds.Contains(x.ManOrgContract.ManagingOrganization.Id))
                             .Where(x => x.ManOrgContract.StartDate <= period.DateEnd && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= period.DateStart) && x.RealityObject.TypeHouse == TypeHouse.ManyApartments)
                             .WhereIf(period.DateAccounting.HasValue, x => !x.RealityObject.DateCommissioning.HasValue || x.RealityObject.DateCommissioning.Value <= period.DateAccounting.Value)
                             .Select(x => new { x.RealityObject.Id, ManOrgId = x.ManOrgContract.ManagingOrganization.Id })
                             .AsEnumerable()
                             .GroupBy(x => x.ManOrgId)
                            .ToDictionary(x => x.Key, y => y.Select(x => x.Id).Distinct().ToArray());

            return realObjManOrg;
        }

        protected class ExtraDataProxy
        {
            public Dictionary<long, Dictionary<string, int?>> RepairService { get; set; }

            public Dictionary<long, int> PlanRedExp { get; set; }

            public Dictionary<long, int?> PlanWorkServ { get; set; }
        }

        protected ExtraDataProxy GetExtraData(PeriodDi period)
        {
            var disclosureInfoService = Container.Resolve<IDomainService<DisclosureInfo>>();
            
            var moRoQuery = Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                .GetAll()
                .Where(x => disclosureInfoService.GetAll().Where(y => y.PeriodDi.Id == period.Id).Any(y => y.ManagingOrganization.Id == x.ManOrgContract.ManagingOrganization.Id))
                .Where(x => x.ManOrgContract.StartDate <= period.DateEnd && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= period.DateStart))
                .WhereIf(period.DateAccounting.HasValue, x => !x.RealityObject.DateCommissioning.HasValue || x.RealityObject.DateCommissioning.Value <= period.DateAccounting.Value);

            var diRoQuery = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().GetAll()
                .Where(x => moRoQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id))
                .Where(x => x.PeriodDi.Id == period.Id)
                .WhereIf(municipalityIdsList.Count > 0, x => municipalityIdsList.Contains(x.RealityObject.Municipality.Id));

            /* Получение процентов для услуг с видом = "ремонт"
             * Не расчитывать процент, если: 
             * 1. Типом предоставления услуги "услуга не предоставляется"
             * или
             * 2. Тип предоставления услуги "Услуга предоставляется без участия УО" и задан(ы) поставщик(и).
             * 
             100% = если выполняется:
             * 1.в таблице поставщик есть хотя бы 1 запись
               2. в разделе "тариф" есть хотя бы 1 запись
               3. в разделе "Работы по ТО" есть хотя бы 1 запись И заполнена плановая сумма И дата начала  И дата окончания
               4. раздел "ППР":
               4.1 в разделе "ППР" в комбобоксе "наличие ППР"="нет"
                   или
               4.2   в разделе "ППР" в комбобоксе "наличие ППР"="да" 
                   4.2.1  И {есть хотя бы 1 запись работы И (по ней заполнена дата начала, плановая стоимость
                            4.2.2.1 И (есть хотя бы 1 запись детализации с указанием ед.измерения и планового объема) ).}

               5. если в разделе "ППР" несколько записей, то по по всем должны быть заполнены дата начала, детализация с указанием ед.измерения и план. объема.*/

            // Получаем детализацию ППР с обработкой условия 4.2.2.1
            var workRepairDetailDict = Container.Resolve<IDomainService<WorkRepairDetailTat>>().GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => new
                    {
                        GroupWorkPprId = x.WorkPpr.GroupWorkPpr.Id,
                        BaseServiceId = x.BaseService.Id,
                        x.BaseService.DisclosureInfoRealityObj.Id,
                        UnitMeasure = (long?)x.UnitMeasure.Id,
                        x.PlannedVolume
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.BaseServiceId)
                          .ToDictionary(
                              y => y.Key,
                              y => y.GroupBy(z => z.GroupWorkPprId)
                                    .ToDictionary(z => z.Key, z => z.Any(v => v.UnitMeasure != null && v.PlannedVolume.HasValue))));

            // Получаем ППР (тут же обрабатываем наличие детализации по ППР) + условие 5
            var workRepairListDict = Container.Resolve<IDomainService<WorkRepairList>>().GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.BaseService.DisclosureInfoRealityObj.Id))
                .Select(x => new
                    {
                        x.DateStart,
                        x.PlannedCost,
                        GroupWorkPprId = x.GroupWorkPpr.Id,
                        BaseServiceId = x.BaseService.Id,
                        x.BaseService.DisclosureInfoRealityObj.Id,
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.BaseServiceId)
                          .ToDictionary(
                              y => y.Key,
                              y => y.All(v =>
                                    {
                                        if (workRepairDetailDict.ContainsKey(v.Id))
                                        {
                                            var wkDetailForRo = workRepairDetailDict[v.Id];

                                            if (wkDetailForRo.ContainsKey(v.BaseServiceId))
                                            {
                                                var wkDetailForService = wkDetailForRo[v.BaseServiceId];

                                                if (wkDetailForService.ContainsKey(v.GroupWorkPprId))
                                                {
                                                    return wkDetailForService[v.GroupWorkPprId] && v.DateStart.HasValue && v.PlannedCost.HasValue;
                                                }

                                                return false;
                                            }

                                            return false;
                                        }

                                        return false;
                                    })));

            // Проценты по услугам с типом "ремонт" с учетом всех пунктов
            var repairServiceDataDict = Container.Resolve<IDomainService<RepairService>>()
                 .GetAll()
                 .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                 .Select(x => new
                    {
                        anyProvider = Container.Resolve<IDomainService<ProviderService>>().GetAll().Any(y => y.BaseService.Id == x.Id),
                        anyTarif = Container.Resolve<IDomainService<TariffForConsumers>>().GetAll().Any(y => y.BaseService.Id == x.Id),
                        anyWorkTechServ = Container.Resolve<IDomainService<WorkRepairTechServ>>().GetAll().Any(y => y.BaseService.Id == x.Id),
                        x.SumWorkTo,
                        x.DateStart,
                        x.DateEnd,
                        x.ScheduledPreventiveMaintanance,
                        roId = x.DisclosureInfoRealityObj.RealityObject.Id,
                        diId = x.DisclosureInfoRealityObj.Id,
                        x.TemplateService.Code,
                        BaseServiceId = x.Id,
                        x.TypeOfProvisionService
                    })
                 .AsEnumerable()
                 .Select(x => new
                    {
                        x.SumWorkTo,
                        x.DateStart,
                        x.DateEnd,
                        x.ScheduledPreventiveMaintanance,
                        x.roId,
                        x.diId,
                        x.BaseServiceId,
                        x.anyProvider,
                        x.anyTarif,
                        x.anyWorkTechServ,
                        x.Code,
                        x.TypeOfProvisionService,
                        hasValidPpr = workRepairListDict.ContainsKey(x.diId) && workRepairListDict[x.diId].ContainsKey(x.BaseServiceId) && workRepairListDict[x.diId][x.BaseServiceId]
                    })
                 .GroupBy(x => x.roId)
                 .ToDictionary(
                    x => x.Key,
                    x => x.Select(
                        y =>
                        {
                            if (y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceNotAvailable
                                || (y.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo
                                    && y.anyProvider))
                            {
                                return new { y.Code, res = (int?)null };
                            }
                            else if (y.anyProvider)
                            {
                                var res = y.SumWorkTo.HasValue && y.DateStart.HasValue && y.DateEnd.HasValue
                                          && y.anyTarif && y.anyWorkTechServ
                                          && ((y.hasValidPpr && y.ScheduledPreventiveMaintanance == YesNoNotSet.Yes)
                                              || y.ScheduledPreventiveMaintanance == YesNoNotSet.No);

                                return new { y.Code, res = (int?)(res ? 1 : 0) };
                            }
                            else
                            {
                                return new { y.Code, res = (int?)0 };
                            }
                        })
                        .GroupBy(y => y.Code)
                        .ToDictionary(y => y.Key, y => y.Select(z => z.res).Contains(1) ? 1 : (y.Select(z => z.res).Contains(null) ? (int?)null : 0)));


            /* План работ по содержанию и ремонту
                100% Если выполняются все условия:
                1. По каждой услуге с типом="ремонт" и типом предоставления="услуга предоставляется через УО" есть запись плана работ 
                2. Есть записи по ТО и указана плановая сумма, дата начала и окончания
                3. раздел "ППР":
                3.1 в разделе "ППР" в комбобоксе "наличие ППР"="нет"
                    или
                3.2 в разделе "ППР" в комбобоксе "наличие ППР"="да":
                     3.2.1 по каждой записи указана плановая сумма, дата начала и окончания    
             
             * 
             * Процент не расчитывается, если нет услуг с типом="ремонт" и типом предоставления="услуга предоставляется через УО",
             * но есть услуги с типом="ремонт" и типом предоставления != "услуга предоставляется через УО".
             * То есть, если у УО нет услуг по ремонту, то и нечего раскрывать
             */

            var planWorkServiceRepairWorksQuery = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>().GetAll();

            var planWorkServiceRepairQuery = Container.Resolve<IDomainService<PlanWorkServiceRepair>>().GetAll();

            var repairServiceQuery = Container.Resolve<IDomainService<RepairService>>().GetAll()
                .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo);

            var repairServiceProvidedWithoutMoQuery = Container.Resolve<IDomainService<RepairService>>().GetAll()
                .Where(x => x.TypeOfProvisionService != TypeOfProvisionServiceDi.ServiceProvidedMo);

            var planWorkServ = diRoQuery
                .Select(x => new
                {
                    x.RealityObject.Id,
                    anyServiceProvidedWithoutMo = repairServiceProvidedWithoutMoQuery.Any(y => y.DisclosureInfoRealityObj.Id == x.Id),
                    anyServiceProvidedMo = repairServiceQuery.Any(y => y.DisclosureInfoRealityObj.Id == x.Id),
                    allDiscoveredServiceProvidedMo = repairServiceQuery.Where(y => y.DisclosureInfoRealityObj.Id == x.Id)
                        .All(v => v.SumWorkTo.HasValue
                            && v.DateStart.HasValue
                            && v.DateEnd.HasValue
                            && planWorkServiceRepairQuery.Any(y => y.BaseService.Id == v.Id)
                            && Container.Resolve<IDomainService<WorkRepairTechServ>>().GetAll().Any(y => y.BaseService.Id == v.Id)
                            && (v.ScheduledPreventiveMaintanance == YesNoNotSet.No
                                || (v.ScheduledPreventiveMaintanance == YesNoNotSet.Yes
                                    && planWorkServiceRepairWorksQuery.Any(y => y.PlanWorkServiceRepair.BaseService.Id == v.Id)
                                    && planWorkServiceRepairWorksQuery
                                        .Where(y => y.PlanWorkServiceRepair.BaseService.Id == v.Id)
                                        .All(y => y.DateStart.HasValue && y.DateEnd.HasValue && y.Cost.HasValue))))
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    percent = x.anyServiceProvidedMo
                        ? (int?)(x.allDiscoveredServiceProvidedMo ? 1 : 0)
                        : (x.anyServiceProvidedWithoutMo ? null : (int?)0)
                })
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Max(y => y.percent));

            /*План мер по снижению расходов
             * 100% Если:
                1. по каждому дому должна быть хотя бы 1 запись
                2. по каждому плану мер должна быть хотя бы 1 запись меры с заполненными полями:
                    - наименование
                    - срок выполнения
                    - предполагаемое снижение*/
            var planReductionPlanReduceMeasureNameQuery = Container.Resolve<IDomainService<PlanReduceMeasureName>>().GetAll()
               .Where(x => x.MeasuresReduceCosts != null)
               .Select(x => x.PlanReductionExpenseWorks.Id);

            var planReductionExpenseWorksQuery = Container.Resolve<IDomainService<PlanReductionExpenseWorks>>().GetAll()
                .Where(x => (x.Name != null && x.Name != "") || planReductionPlanReduceMeasureNameQuery.Contains(x.Id))
                .Where(x => x.DateComplete.HasValue)
                .Where(x => x.PlannedReductionExpense.HasValue);

            var planReductionExpenseDict = Container.Resolve<IDomainService<PlanReductionExpense>>().GetAll()
                .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                .Where(x => planReductionExpenseWorksQuery.Any(y => y.PlanReductionExpense.Id == x.Id))
                .Select(x => x.DisclosureInfoRealityObj.RealityObject.Id)
                .Distinct()
                .ToDictionary(x => x, x => 1);

            var result = new ExtraDataProxy
                {
                    RepairService = repairServiceDataDict,
                    PlanRedExp = planReductionExpenseDict,
                    PlanWorkServ = planWorkServ
                };

            return result;
        }

        protected override RoPercProxy CalcRoPercent(RoPercProxy roPerc)
        {
            var completePositionCount = 0;
            var positionCount = 27;

            completePositionCount += roPerc.PlanRedExp ?? 0;
            completePositionCount += roPerc.PlanRedPay ?? 0;
            completePositionCount += roPerc.FileActState ?? 0;
            completePositionCount += roPerc.FileCatRepair ?? 0;
            completePositionCount += roPerc.PlanWorkServ ?? 0;
            completePositionCount += roPerc.NonResPlace ?? 0;
            completePositionCount += roPerc.PlaceGenUse ?? 0;
            completePositionCount += roPerc.ManagServ ?? 0;
            completePositionCount += roPerc.Serv2 ?? 0;
            completePositionCount += roPerc.Serv6 ?? 0;
            completePositionCount += roPerc.Serv7 ?? 0;
            completePositionCount += roPerc.Serv13 ?? 0;
            completePositionCount += roPerc.Serv14 ?? 0;
            completePositionCount += roPerc.Serv27 ?? 0;
            completePositionCount += roPerc.Serv28 ?? 0;
            completePositionCount += roPerc.Serv8 ?? 0;
            completePositionCount += roPerc.Serv9 ?? 0;
            completePositionCount += roPerc.Serv10 ?? 0;
            completePositionCount += roPerc.Serv11 ?? 0;
            completePositionCount += roPerc.Serv12 ?? 0;
            completePositionCount += roPerc.Serv18 ?? 0;
            completePositionCount += roPerc.Serv17 ?? 0;
            completePositionCount += roPerc.Serv19 ?? 0;
            completePositionCount += roPerc.Serv20 ?? 0;
            completePositionCount += roPerc.Serv22 ?? 0;
            completePositionCount += roPerc.Serv21 ?? 0;
            completePositionCount += roPerc.Serv16 ?? 0;

            positionCount -= roPerc.PlanRedExp == null ? 1 : 0;
            positionCount -= roPerc.PlanRedPay == null ? 1 : 0;
            positionCount -= roPerc.FileActState == null ? 1 : 0;
            positionCount -= roPerc.FileCatRepair == null ? 1 : 0;
            positionCount -= roPerc.PlanWorkServ == null ? 1 : 0;
            positionCount -= roPerc.NonResPlace == null ? 1 : 0;
            positionCount -= roPerc.PlaceGenUse == null ? 1 : 0;
            positionCount -= roPerc.ManagServ == null ? 1 : 0;
            positionCount -= roPerc.Serv2 == null ? 1 : 0;
            positionCount -= roPerc.Serv6 == null ? 1 : 0;
            positionCount -= roPerc.Serv7 == null ? 1 : 0;
            positionCount -= roPerc.Serv13 == null ? 1 : 0;
            positionCount -= roPerc.Serv14 == null ? 1 : 0;
            positionCount -= roPerc.Serv27 == null ? 1 : 0;
            positionCount -= roPerc.Serv28 == null ? 1 : 0;
            positionCount -= roPerc.Serv8 == null ? 1 : 0;
            positionCount -= roPerc.Serv9 == null ? 1 : 0;
            positionCount -= roPerc.Serv10 == null ? 1 : 0;
            positionCount -= roPerc.Serv11 == null ? 1 : 0;
            positionCount -= roPerc.Serv12 == null ? 1 : 0;
            positionCount -= roPerc.Serv18 == null ? 1 : 0;
            positionCount -= roPerc.Serv17 == null ? 1 : 0;
            positionCount -= roPerc.Serv19 == null ? 1 : 0;
            positionCount -= roPerc.Serv20 == null ? 1 : 0;
            positionCount -= roPerc.Serv22 == null ? 1 : 0;
            positionCount -= roPerc.Serv21 == null ? 1 : 0;
            positionCount -= roPerc.Serv16 == null ? 1 : 0;

            roPerc.Percent = (decimal.Divide(completePositionCount, (positionCount == 0 ? 1 : positionCount)) * 50).RoundDecimal(2);

            return roPerc;
        }
        
        public override string SqlQueryRealObj()
        {
            return @" 
            select  ro.id,
                        (mu.name || ', '  || ro.address) address, -- адрес дома
                        '' plan_red_exp, -- План мер по снижению расходов 
                            (
                          select 
                                case when diro.reduction_payment = 20
                                   then null
                                   else
                              case when diro.reduction_payment = 10 and  count(1) > 0 then 1 else 0 end
                                 end
                              from di_disinfo_ro_reduct_pay rp
                              where rp.disinfo_ro_id = diro.id 
                            ) plan_red_pay, -- Сведения о случаях снижения платы
                            (
                          select 
                            case when count(1) > 0 then 1 else 0 end
                              from di_disinfo_doc_ro dro
                              where dro.disinfo_ro_id = diro.id  and file_act_state_id is not null
                            ) file_act_state,
                            (
                          select 
                            case when count(1) > 0 then 1 else 0 end
                              from di_disinfo_doc_ro dro
                              where dro.disinfo_ro_id = diro.id  and file_catrepair_id is not null
                            ) file_catrepair, -- Перечень работ по содержанию и ремонту общего имущества
                            (
                          select 
                            case when count(1) > 0 then 1 else 0 end
                              from di_disinfo_doc_ro dro
                              where dro.disinfo_ro_id = diro.id  and file_plan_repair_id is not null
                            ) file_plan_repair, -- Отчет о выполнении годового плана мероприятий
                            '' plan_work_serv, -- План работ по содержанию и ремонту
                            (
                              case when diro.non_resident_place = 20
                                   then null
                                   else
                              case when diro.non_resident_place = 10 and  exists(
                              select 1 from di_disinfo_ro_nonresplace rn
                              left join di_disinfo_realobj diro1 on rn.disinfo_ro_id = diro1.id
                              where diro1.reality_obj_id = ro.id and
                              ((((rn.date_start >= per.date_start) or per.date_start is null)
                                        and (per.date_end >= rn.date_start) or per.date_end is null)
                                        or (((per.date_start >= rn.date_start) or rn.date_start is null)
                                            and ((rn.date_end >= per.date_start) or rn.date_end is null)))) then 1 else 0 end
                                 end
                              
                            ) non_res_place, -- Сведения об использовании нежилых помещений
                                                        (
                                case when diro.place_general_use = 20
                                   then null
                                   else
                              case when diro.place_general_use = 10 and  exists(
                               select 1 from di_disinfo_com_facils cf
                                    left join di_disinfo_realobj diro1 on diro1.id = cf.disinfo_ro_id
                              where diro1.reality_obj_id = ro.id and
                              ((((cf.date_start >= per.date_start) or per.date_start is null)
                                        and (per.date_end >= cf.date_start) or per.date_end is null)
                                        or (((per.date_start >= cf.date_start) or cf.date_start is null)
                                            and ((cf.date_end >= per.date_start) or cf.date_end is null))) ) then 1 else 0 end
                                 end
                              
                            ) place_gen_use, -- Сведения об использовании мест общего пользования
                            (
                          select 
                                   case when count(1) > 0 then 1 else 0 end
                              from di_tariff_fconsumers tf
                            left join di_base_service s on tf.base_service_id = s.id 
                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                          where ts.code = '1' and s.disinfo_ro_id = diro.id
                            ) manag_serv, -- Услуга 'Управление жилым домом' (по дому)
                            (
                              case when exists (
                                          select 1
                                          from  di_base_service s 
                                            left join di_housing_service hs on hs.id = s.id
                                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                                            inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                          where ts.code = '2' and s.disinfo_ro_id = diro.id
                                          and not ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))
                                      )  then  1                
                                    else 
                                        case when  exists (select 1
                                              from  di_base_service s 
                                                left join di_housing_service hs on hs.id = s.id
                                                left join di_dict_templ_service ts on ts.id = s.template_service_id
                                              where ts.code = '2' and s.disinfo_ro_id = diro.id and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) 
                                                                          
                                             then null else 0 end
                                  end )                     
                              serv_2, -- Уборка внутридомовых мест общего пользования 
                            (
                                    case when exists (
                                          select 1
                                          from  di_base_service s 
                                            left join di_housing_service hs on hs.id = s.id
                                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                                            inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                          where ts.code = '6' and s.disinfo_ro_id = diro.id
                                          and not ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                    else 
                                         case when  exists (select 1
                                                              from  di_base_service s 
                                                    left join di_housing_service hs on hs.id = s.id
                                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                  where ts.code = '6' and s.disinfo_ro_id = diro.id and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) 
                                                        then null else 0 end
                                    end

                            ) serv_6, -- Уборка придомовой территории 
                            (
                                    case when exists (
                                          select 1
                                          from  di_base_service s 
                                            left join di_housing_service hs on hs.id = s.id
                                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                                            inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                            left join di_disinfo_realobj diro1 on diro1.id = s.disinfo_ro_id
                                            left join tp_teh_passport tp on tp.reality_obj_id = diro1.reality_obj_id
                                                left join tp_teh_passport_value tpv on  tpv.teh_passport_id = tp.id
                                          where ts.code = '7' and s.disinfo_ro_id = diro.id and (tpv.form_code = 'Form_3_7' and tpv.cell_code = '1:3' or tp.id is null)
                                          and not (tpv.value = '0' or (hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                    else 
                                         case when  exists (select 1 from tp_teh_passport_value tpv
                                                           left join tp_teh_passport tp on tpv.teh_passport_id = tp.id
                                                           where tp.reality_obj_id = ro.id and (tpv.form_code = 'Form_3_7' and tpv.cell_code = '1:3'and tpv.value = '0')
                                                           ) 
                                                     or exists (select 1
                                                              from  di_base_service s 
                                                    left join di_housing_service hs on hs.id = s.id
                                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                  where ts.code = '7' and s.disinfo_ro_id = diro.id
                                                   and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null)))
                                                        then null else 0 end
                                    end
                            ) serv_7, -- Обслуживание мусоропроводов 
                            (
                                case when exists (
                                              select 1
                                              from  di_base_service s 
                                                left join di_housing_service hs on hs.id = s.id
                                                left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                              where ts.code = '13' and s.disinfo_ro_id = diro.id
                                              and not ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                else 
                                         case when  exists (select 1
                                                              from  di_base_service s 
                                                    left join di_housing_service hs on hs.id = s.id
                                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                  where ts.code = '13' and s.disinfo_ro_id = diro.id and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null)))
                                                                           
                                         then null else 0 end
                                end      
                            ) serv_13, -- Дератизация
                            (
                                case when exists (
                                          select 1
                                          from  di_base_service s 
                                            left join di_housing_service hs on hs.id = s.id
                                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                                            inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                          where ts.code = '14' and s.disinfo_ro_id = diro.id
                                          and not ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                else 
                                         case when  exists (select 1 from  di_base_service s 
                                                    left join di_housing_service hs on hs.id = s.id
                                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                  where ts.code = '14' and s.disinfo_ro_id = diro.id and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) 
                                                                          
                                         then null else 0 end
                                end    
                            ) serv_14, -- Вывоз ТБО
                             (
                               case when exists (
                                          select 1
                                          from  di_base_service s 
                                            left join di_housing_service hs on hs.id = s.id
                                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                                            inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                          where ts.code = '27' and s.disinfo_ro_id = diro.id
                                          and not ( ro.number_lifts = 0 or (hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                    else 
                                         case when  ro.number_lifts = 0 or exists (select 1
                                                              from  di_base_service s 
                                                    left join di_housing_service hs on hs.id = s.id
                                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                  where ts.code = '27' and s.disinfo_ro_id = diro.id  and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null)))
                                         then null else 0 end
                               end     

                            ) serv_27, -- Обслуживание лифтов
                             (
                               case when exists (
                                          select 1
                                          from  di_base_service s 
                                            left join di_housing_service hs on hs.id = s.id
                                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                                            inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                          where ts.code = '28' and s.disinfo_ro_id = diro.id
                                          and not (ro.number_lifts = 0 or (hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                    else 
                                         case when  ro.number_lifts = 0 or exists (select 1
                                                              from  di_base_service s 
                                                    left join di_housing_service hs on hs.id = s.id
                                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                  where ts.code = '28' and s.disinfo_ro_id = diro.id  and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null)))
                                         then null else 0 end
                               end      
                            ) serv_28, -- Пользование лифтом
                            '' serv_8, -- Текущий ремонт жилого здания и благоустройство территории
                            '' serv_9, -- Текущий ремонт и содержание внутридомовых инженерных сетей центрального отопления
                            '' serv_10, -- Текущий ремонт и содержание внутридомовых инженерных сетей газоснабжения
                            '' serv_11, -- Текущий ремонт и содержание внутридомовых инженерных сетей водоснабжения и водоотведения
                            '' serv_12, -- Текущий ремонт и содержание внутридомовых инженерных сетей электроснабжения
                            (
                              case when exists (
                                      select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                        inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                        inner join  di_tariff_frso  rso on rso.base_service_id = s.id 
                                      where ts.code = '18' and s.disinfo_ro_id = diro.id
                                      and not (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '18' and s.disinfo_ro_id = diro.id and (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                              end                        
                            ) serv_18, -- ГВС
                            (
                              case when exists (
                                      select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                        inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                        inner join  di_tariff_frso  rso on rso.base_service_id = s.id 
                                      where ts.code = '17' and s.disinfo_ro_id = diro.id  
                                      and not (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                              else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '17' and s.disinfo_ro_id = diro.id and (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                              end                        
                            ) serv_17, -- XВС
                            (
                             case when exists (
                                      select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                        inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                        inner join  di_tariff_frso  rso on rso.base_service_id = s.id 
                                      where ts.code = '19' and s.disinfo_ro_id = diro.id
                                      and not (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '19' and s.disinfo_ro_id = diro.id and (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                             end
                            ) serv_19, -- Водоотведение
                            (
                                 case when exists (
                                      select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                        inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                        inner join  di_tariff_frso  rso on rso.base_service_id = s.id 
                                      where ts.code = '20' and s.disinfo_ro_id = diro.id
                                      and not (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                 else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '20' and s.disinfo_ro_id = diro.id and (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                                    end                        
                            ) serv_20, -- Электроснабжение
                            (
                               case when exists (
                                  select 1
                                  from  di_base_service s 
                                    left join di_communal_service cs on cs.id = s.id
                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                    inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                    inner join  di_tariff_frso  rso on rso.base_service_id = s.id 
                                  where ts.code = '22' and s.disinfo_ro_id = diro.id
                                  and not (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null)))
                                then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '22' and s.disinfo_ro_id = diro.id and (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                               end                        
                            ) serv_22, -- Теплоснабжение
                            (
                                case when exists (  select 1
                                  from  di_base_service s 
                                    left join di_communal_service cs on cs.id = s.id
                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                    inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                    inner join  di_tariff_frso  rso on rso.base_service_id = s.id 
                                  where ts.code = '21' and s.disinfo_ro_id = diro.id
                                  and not (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_communal_service cs on cs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '21' and s.disinfo_ro_id = diro.id and (cs.type_of_provision_service = 30 or  (cs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                                end                        
                            ) serv_21, -- Газоснабжение
                            (
                          case when exists (select 1
                          from di_base_service s
                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                          where ts.kind_service = 60 and s.disinfo_ro_id = diro.id) then 1 else 0 end
                            ) add_serv, -- Услуги с типом 'Дополнительная'
                        '',
                        '',
                          (
                             case when exists (
                                  select 1
                                  from  di_base_service s 
                                    left join di_cap_rep_service crs on crs.id = s.id
                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                    inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                  where ts.code = '16' and s.disinfo_ro_id = diro.id
                                  and not (crs.type_of_provision_service = 30 or  (crs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_cap_rep_service crs on crs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '16' and s.disinfo_ro_id = diro.id and (crs.type_of_provision_service = 30 or  (crs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                             end                        
                            ) serv_16 -- Кап.ремонт
                    from gkh_reality_object ro
                        left join di_disinfo_realobj diro  on ro.id = diro.reality_obj_id and  diro.period_di_id = :period 
                        left join di_dict_period per on per.id = diro.period_di_id
                        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
         ";
        }
    }
}