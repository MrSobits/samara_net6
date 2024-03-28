namespace Bars.GkhDi.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    using NHibernate;

    /// <summary>
    /// Отчет "Расчет процентов раскрытия информации до 2013"
    /// </summary>
    public class B3PercentCalculationReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateReport = DateTime.MinValue;

        private bool? transsferredManag;

        private List<long> municipalityIdsList = new List<long>();

        private long periodDiId;

        public B3PercentCalculationReport()
            : base(new ReportTemplateBinary(Properties.Resources.B3PercentCalculation))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.B3PercentCalculation";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по раскрытию информации по ПП РФ №731 до 2013";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Раскрытие информации";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.B3PercentCalculation";
            }
        }

        public override string Name
        {
            get
            {
                return "Отчет по раскрытию информации по ПП РФ №731 до 2013";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            periodDiId = baseParams.Params.GetAs<long>("periodDi");

            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIdsList = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            transsferredManag = baseParams.Params.GetAs<bool?>("transsferredManag");

            dateReport = baseParams.Params.GetAs<DateTime>("dateReport");
        }

        public override string ReportGenerator { get; set; }

        protected ISession OpenSession()
        {
            return Container.Resolve<ISessionProvider>().GetCurrentSession();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var contragentList = userManager.GetContragentIds();
            var municipalityIds = userManager.GetMunicipalityIds();

            if (municipalityIdsList.Count == 0)
            {
                municipalityIdsList = municipalityIds;
            }
            else
            {
                if (municipalityIds.Count > 0)
                {
                    municipalityIdsList = municipalityIdsList.Intersect(municipalityIds).ToList();
                }
            }

            if (periodDiId == 0)
            {
                return;
            }

            var period = Container.Resolve<IDomainService<PeriodDi>>().GetAll().FirstOrDefault(x => x.Id == periodDiId);

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
                         .WhereIf(dateReport != DateTime.MinValue, x => (x.ActivityDateEnd.HasValue && x.ActivityDateEnd > dateReport)
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

            if (roIds.Count == 0)
            {
                return;
            }

            var manOrgsPercent = new List<MoPercProxy>();
            var realObjsPercentDict = new Dictionary<long, RoPercProxy>();

            var query = OpenSession().CreateSQLQuery(SqlQueryManOrg());
            query.SetParameter("period", periodDiId);
            var sqlQueryResult = query.List();

            var moIdsDict = moIds.ToDictionary(x => x);

            foreach (object[] record in sqlQueryResult)
            {
                if (!moIdsDict.ContainsKey(record[0].ToLong()))
                {
                    continue;
                }

                var manOrgPercent = new MoPercProxy
                                        {
                                            ManOrgId = record[0].ToLong(),
                                            Municipality = record[1].ToStr(),
                                            ManOrgName = record[2].ToStr(),
                                            Director = record[3].To<int?>(),
                                            DirectionMember = record[4].To<int?>(),
                                            RevComMember = record[5].To<int?>(),
                                            MailingAddress = record[6].To<int?>(),
                                            FactAddress = record[7].To<int?>(),
                                            Phone = record[8].To<int?>(),
                                            Email = record[9].To<int?>(),
                                            Site = record[10].To<int?>(),
                                            WorkMode = record[11].To<int?>(),
                                            RecepCitizen = record[12].To<int?>(),
                                            DispatchWork = record[13].To<int?>(),
                                            Ogrn = record[14].To<int?>(),
                                            FileProjContr = record[15].To<int?>(),
                                            AdminResp = record[16].To<int?>(),
                                            MemUnion = record[17].To<int?>(),
                                            TermContr = record[18].To<int?>(),
                                            TaxSystem = record[19].To<int?>(),
                                            Balance = record[20].To<int?>(),
                                            BalanceAnnex = record[21].To<int?>(),
                                            EstimateCurr = record[22].To<int?>(),
                                            EstimatePrev = record[23].To<int?>(),
                                            ReportEstimatePrev = record[24].To<int?>(),
                                            ConRevCurr = record[25].To<int?>(),
                                            ConRevPrev = record[26].To<int?>(),
                                            ConRevPrevPrev = record[27].To<int?>(),
                                            AuditCurr = record[28].To<int?>(),
                                            AuditPrev = record[29].To<int?>(),
                                            AuditPrevPrev = record[30].To<int?>(),
                                            FundsInfo = record[31].To<int?>(),
                                            ManagRealObjs = record[32].To<int?>()
                                        };

                manOrgPercent = CalcMoPercent(manOrgPercent);

                manOrgsPercent.Add(manOrgPercent);
            }

            var query2 = OpenSession().CreateSQLQuery(SqlQueryRealObj());
            query2.SetParameter("period", periodDiId);
            var sqlQueryResult2 = query2.List();

            foreach (object[] record in sqlQueryResult2)
            {
                if (!roIds.ContainsKey(record[0].ToLong()))
                {
                    continue;
                }

                var realObjPercent = new RoPercProxy
                {
                    RealObjId = record[0].ToLong(),
                    Address = record[1].ToStr(),
                    PlanRedExp = record[2].To<int?>(),
                    PlanRedPay = record[3].To<int?>(),
                    Docs = record[4].To<int?>(),
                    PlanWorkServ = record[5].To<int?>(),
                    NonResPlace = record[6].To<int?>(),
                    PlaceGenUse = record[7].To<int?>(),
                    ManagServ = record[8].To<int?>(),
                    Serv2 = record[9].To<int?>(),
                    Serv6 = record[10].To<int?>(),
                    Serv7 = record[11].To<int?>(),
                    Serv13 = record[12].To<int?>(),
                    Serv14 = record[13].To<int?>(),
                    Serv27_28 = record[14].To<int?>(),
                    Serv8 = record[15].To<int?>(),
                    Serv9 = record[16].To<int?>(),
                    Serv11 = record[17].To<int?>(),
                    Serv12 = record[18].To<int?>(),
                    Serv18 = record[19].To<int?>(),
                    Serv17 = record[20].To<int?>(),
                    Serv20 = record[21].To<int?>(),
                    Serv22 = record[22].To<int?>(),
                    AddServ = record[23].To<int?>()
                };

                realObjPercent = CalcRoPercent(realObjPercent);

                realObjsPercentDict.Add(record[0].ToLong(), realObjPercent);
            }

            var manOrgsPercByMunicipality = manOrgsPercent.GroupBy(x => x.Municipality)
                                                          .ToDictionary(x => x.Key,
                                                                         y => y.GroupBy(z => z.ManOrgId).ToDictionary(x => x.Key, z => z.FirstOrDefault()));

            var sectionMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияМО");

            decimal manOrgInfoPercents = 0;
            decimal realObjsPercents = 0;
            int number = 0;
            foreach (var manOrgsPerc in manOrgsPercByMunicipality)
            {
                sectionMunicipality.ДобавитьСтроку();
                sectionMunicipality["НаименованиеМО"] = manOrgsPerc.Key;

                var sectionManOrg = sectionMunicipality.ДобавитьСекцию("СекцияУО");
                decimal averMoDisInfosPerc = 0;
                decimal averMoRealObjPerc = 0;

                foreach (var manOrgPerc in manOrgsPerc.Value)
                {
                    var moPerc = manOrgPerc.Value;
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
                    if (realObjsByMo.ContainsKey(moPerc.ManOrgId))
                    {
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
                                sectionRealObj["Документы"] = roPerc.Docs != null
                                                                   ? roPerc.Docs.ToStr()
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
                                sectionRealObj["Лифты"] = roPerc.Serv27_28 != null ? roPerc.Serv27_28.ToStr() : " - ";
                                sectionRealObj["ТекРемонтЖД"] = roPerc.Serv8 != null ? roPerc.Serv8.ToStr() : " - ";
                                sectionRealObj["ТекРемонтИСиЦО"] = roPerc.Serv9 != null ? roPerc.Serv9.ToStr() : " - ";
                                sectionRealObj["ТекРемонтИСВиВ"] = roPerc.Serv11 != null ? roPerc.Serv11.ToStr() : " - ";
                                sectionRealObj["ТекРемонтИСЭ"] = roPerc.Serv12 != null ? roPerc.Serv12.ToStr() : " - ";
                                sectionRealObj["ГВС"] = roPerc.Serv18 != null ? roPerc.Serv18.ToStr() : " - ";
                                sectionRealObj["ХВС"] = roPerc.Serv17 != null ? roPerc.Serv17.ToStr() : " - ";
                                sectionRealObj["Электроснабжение"] = roPerc.Serv20 != null
                                                                         ? roPerc.Serv20.ToStr()
                                                                         : " - ";
                                sectionRealObj["Теплоснабжение"] = roPerc.Serv22 != null ? roPerc.Serv22.ToStr() : " - ";
                                sectionRealObj["СведенияЖД"] = roPerc.Percent;

                                averRealObjPerc += roPerc.Percent;

                                sectionRealObj["ОбщийПроцентДом"] = roPerc.Percent + moPerc.Percent;
                            }
                        }

                        averRealObjPerc = averRealObjPerc / realObjsByMo[moPerc.ManOrgId].Count();
                    }
                    else
                    {
                        sectionRealObj.ДобавитьСтроку();
                        averRealObjPerc = 50;
                    }

                    sectionManOrg["СведенияЖДпоУО"] = averRealObjPerc.RoundDecimal(2);
                    sectionManOrg["ОбщийПроцентУО"] = (averRealObjPerc + moPerc.Percent).RoundDecimal(2);
                    averMoRealObjPerc += averRealObjPerc;
                }

                averMoDisInfosPerc = averMoDisInfosPerc / manOrgsPerc.Value.Count;
                averMoRealObjPerc = averMoRealObjPerc / manOrgsPerc.Value.Count;

                // среднее по сведениям УО по мо
                sectionMunicipality["СрСведПоMО"] = averMoDisInfosPerc;
                sectionMunicipality["СведенияЖДпоМО"] = averMoRealObjPerc.RoundDecimal(2);
                sectionMunicipality["ОбщийПроцентМО"] = (averMoDisInfosPerc + averMoRealObjPerc).RoundDecimal(2);

                manOrgInfoPercents += averMoDisInfosPerc;
                realObjsPercents += averMoRealObjPerc;
            }
            manOrgInfoPercents = manOrgInfoPercents / (manOrgsPercByMunicipality.Keys.Count == 0 ? 1 : manOrgsPercByMunicipality.Keys.Count);
            realObjsPercents = realObjsPercents / (manOrgsPercByMunicipality.Keys.Count == 0 ? 1 : manOrgsPercByMunicipality.Keys.Count);
            reportParams.SimpleReportParams["СредСвед"] = manOrgInfoPercents.RoundDecimal(2);
            reportParams.SimpleReportParams["СведенияЖДОбщ"] = realObjsPercents.RoundDecimal(2);
            reportParams.SimpleReportParams["ОбщийПроцентОбщ"] = (manOrgInfoPercents + realObjsPercents).RoundDecimal(2);

            manOrgsPercByMunicipality.Clear();
            realObjsByMo.Clear();

        }

        private Dictionary<long, long[]> GetManagRealObj(IEnumerable<long> manOrgIds, PeriodDi period)
        {
            var realObjManOrg = Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                .GetAll()
                .Where(x => manOrgIds.Contains(x.ManOrgContract.ManagingOrganization.Id))
                .Where(x => x.ManOrgContract.StartDate <= period.DateEnd && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= period.DateStart))
                 .WhereIf(period.DateAccounting.HasValue, x => !x.RealityObject.DateCommissioning.HasValue || x.RealityObject.DateCommissioning.Value <= period.DateAccounting.Value)
                 .Select(x => new { x.RealityObject.Id, ManOrgId = x.ManOrgContract.ManagingOrganization.Id })
                 .AsEnumerable()
                 .GroupBy(x => x.ManOrgId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).Distinct().ToArray());

            return realObjManOrg;
        }

        public string SqlQueryManOrg()
        {
            return @"
                             select 
                                mo.id mo_id,
                                mu.name mu_name, -- муниципальное образование
                                c.name,   -- управляющая компания
                                (
                                                   select 
                                                         case when count(1) > 0 then 1 else 0 end
                                                   from gkh_contragent_contact con 
                                                    left join gkh_dict_position pos ON pos.id = con.position_id
                                                   where con.contragent_id = c.id and (pos.code = '1' or pos.code = '4') and
                                                   ((con.date_start_work is not null and (con.date_start_work >= per.date_start and per.date_end >= con.date_start_work) or con.date_start_work is null)
                                                   or (per.date_start >= con.date_start_work and ((con.date_end_work is not null and con.date_end_work >= per.date_start) or con.date_end_work is null)))
                                ) director,  -- заполненность поля 'ФИО руководителя'
                                (
                                                   select 
                                                        case when mo.type_management <> 10
                                                        then
                                                         case when count(1) > 0 then 1 else 0
                                                         end
                                                        end
                                                   from gkh_contragent_contact con 
                                                    left join gkh_dict_position pos ON pos.id = con.position_id
                                                   where con.contragent_id = c.id and pos.code = '6' and
                                                   ((con.date_start_work is not null and (con.date_start_work >= per.date_start and per.date_end >= con.date_start_work) or con.date_start_work is null)
                                                   or (per.date_start >= con.date_start_work and ((con.date_end_work is not null and con.date_end_work >= per.date_start) or con.date_end_work is null)))
                                ) direction_member,  -- 'Члены правления'
                                (
                                                   select 
                                                        case when mo.type_management <> 10
                                                        then
                                                         case when count(1) > 0 then 1 else 0
                                                         end
                                                        end
                                                   from gkh_contragent_contact con 
                                                    left join gkh_dict_position pos ON pos.id = con.position_id
                                                   where con.contragent_id = c.id and pos.code = '5' and mo.type_management <> 10 and
                                                   ((con.date_start_work is not null and (con.date_start_work >= per.date_start and per.date_end >= con.date_start_work) or con.date_start_work is null)
                                                   or (per.date_start >= con.date_start_work and ((con.date_end_work is not null and con.date_end_work >= per.date_start) or con.date_end_work is null)))
                                ) rev_com_member,  -- 'Члены рев комиссии'
                                 (case when c.fias_mail_address_id is null then 0 else 1  end) mail_address, -- почтовый адрес
                                (case when c.fias_fact_address_id is null then 0 else 1  end) fact_address, -- фактический адрес
                                (case when c.phone is null then 0 else 1  end) phone, -- телефон
                                (case when c.email is null or c.email = ''  then 0 else 1  end) email, -- e-mail
                                (case when cast(c.is_site as int)  = 1 and c.official_website is null then 0
                                      when cast(c.is_site as int) = 1 and c.official_website is not null then 1
                                    end
                                 ) site, -- e-mail
                                 (
                                               select 
                                                         case when count(1) > 0 then 1 else 0 end
                                                   from gkh_man_org_work mw 
                                                    where mw.man_org_id = mo.id and mw.type_mode = 10
                                 ) work_mode, -- Режим работы УО
                                 (
                                               select 
                                                         case when count(1) > 0 then 1 else 0 end
                                                   from gkh_man_org_work mw 
                                                    where mw.man_org_id = mo.id and mw.type_mode = 20
                                 ) recep_citizen, -- Режим приема граждан
                                 (
                                                   select 
                                                        case when mo.type_management = 10
                                                        then
                                                         case when count(1) > 0 then 1 else 0 end
                                                        end
                                                  from gkh_man_org_work mw 
                                                    where mw.man_org_id = mo.id and mw.type_mode = 30
                                 ) dispatch_work,  --Режим работы диспетчерских служб
                                 (case when c.ogrn is null or c.ogrn_reg is null then 0 else 1  end) ogrn, -- ОГРН
                                 (
                                   case when (mo.type_management = 20 and exists
                                                (select 1 from di_disinfo_documents docs
                                                       where docs.disinfo_id = di.id and cast (docs.not_available as int) = 1)) or mo.type_management = 40
                                   then null
                                   else
                                            case when exists
                                                       (select 1 from di_disinfo_documents docs
                                                        where docs.disinfo_id = di.id and docs.file_proj_contr_id is not null)
                                            then 1 else 0  end
                                   end
                                 ) file_proj_contr,  --Проект договора управления МКД с собственником жилья
                                 (
                                                   select 
                                                         case when di.admin_response = 20
                                                           then null
                                                           else 
                                                             case when di.admin_response = 10 and count(1) > 0 then 1 else 0 end
                                                         end
                                                   from di_admin_resp adm
                                                   where adm.disinfo_id = di.id
                                ) admin_resp,  --Информация по привлечению УО к админстративной ответственности
                                (
                                                   select 
                                                         case when di.membership_unions = 20
                                                           then null
                                                           else 
                                                             case when di.membership_unions = 10 and count(1) > 0 then 1 else 0 end
                                                         end
                                                   from gkh_man_org_membership mem
                                                   where mem.man_org_id = mo.id and(
                                            (mem.date_start >= per.date_start and per.date_end >= mem.date_start) 
                                            or( per.date_start >= mem.date_start and (mem.date_end is not null and mem.date_end >= per.date_start))
                                            or mem.date_end is null)
                                 ) mem_union,  --Сведения о членстве в СРО
                                (
                                                   select 
                                                         case when di.terminate_contract = 20
                                                           then null
                                                           else 
                                                             case when di.terminate_contract = 10 and count(1) > 0 then 1 else 0 end
                                                         end
                                                   from gkh_morg_contract_realobj mro
                                                      left join gkh_morg_contract con on con.id = mro.man_org_contract_id
                                                      left join gkh_managing_organization morg on  morg.id = con.manag_org_id
                                                   where con.manag_org_id = mo.id and con.end_date >= per.date_start and con.end_date < per.date_end
                                 ) term_contr,  --Перечень домов в отношении которых договора расторгнуты
                                (            
                                                   select 
                                                        case when count(1) > 0 then 1 else 0 end
                                                   from di_disinfo_fin_activity fin
                                                   where fin.disinfo_id = di.id and fin.tax_system_id is not null
                                 ) tax_system, --Система налогообложения
                                 (            
                                                   select 
                                                        case when count(1) > 0 then 1 else 0 end
                                                   from di_disinfo_finact_docs doc
                                                   where doc.disinfo_id = di.id and doc.bookkeep_balance is not null
                                 ) balance, --Бухгалтерский баланс/ налоговая декларация
                                 (            
                                     case when exists ( select 1  from di_disinfo_finact_docs doc
                                                        where doc.disinfo_id = di.id and doc.bookkeep_balance_annex is not null) then 1 else 
                                             case when exists (select 1 from di_disinfo_finact_docs doc
                                                       where doc.disinfo_id = di.id and doc.bookkeep_balance is not null) then null else 0 end
                                             end              
                                 ) balance_annex, -- Приложение к бухгалтерскому балансу
                                 (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_docyear dy
                                                   where dy.managing_org_id = mo.id and dy.year = extract(year from per.date_start) and type_doc_by_year = 10
                                 ) estimate_curr, -- Сметы доходов и расходов на текущий год (ТСЖ/ЖСК) 
                                (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_docyear dy
                                                   where dy.managing_org_id = mo.id and dy.year = (extract(year from per.date_start) - 1) and type_doc_by_year = 10
                                 ) estimate_prev, -- Сметы доходов и расходов за предшествующий год (ТСЖ/ЖСК)
                                 (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_docyear dy
                                                   where dy.managing_org_id = mo.id and dy.year = (extract(year from per.date_start) - 1) and type_doc_by_year = 30
                                 ) rep_est_prev, -- Отчет о выполнении сметы доходов и расходов за предшествующий год (ТСЖ/ЖСК)
                                 (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_docyear dy
                                                   where dy.managing_org_id = mo.id and dy.year = extract(year from per.date_start) and type_doc_by_year = 20
                                  ) con_rev_curr, -- Заключение ревизионной комиссии по результатам проверки годовой бухгалтерской (финансовой) отчетности за текущий год (ТСЖ/ЖСК) 
                                  (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_docyear dy
                                                   where dy.managing_org_id = mo.id and dy.year = (extract(year from per.date_start) - 1) and type_doc_by_year = 20
                                  ) con_rev_prev, -- Заключение ревизионной комиссии по результатам проверки годовой бухгалтерской (финансовой) отчетности за год предшествующий текущему году (ТСЖ/ЖСК)
                                  (
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_docyear dy
                                                   where dy.managing_org_id = mo.id and dy.year = (extract(year from per.date_start) - 2) and type_doc_by_year = 20
                                  ) con_rev_prev_prev, -- Заключение ревизионной комиссии по результатам проверки годовой бухгалтерской (финансовой) отчетности за 2 года предшествующих текущему году (ТСЖ/ЖСК)
                                  (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_audit aud
                                                   where aud.managing_org_id = mo.id and aud.year = extract(year from per.date_start) and (aud.file_id is not null or aud.type_audit_state =30)
                                  ) audit_curr, -- Аудиторское заключение за текущий год (ТСЖ/ЖСК) 
                                  (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_audit aud
                                                   where aud.managing_org_id = mo.id and aud.year = (extract(year from per.date_start) - 1) and (aud.file_id is not null or aud.type_audit_state =30)
                                  ) audit_prev, -- Аудиторское заключение за год предшествующий текущему году (ТСЖ/ЖСК)
                                  (            
                                                   select 
                                                   case when mo.type_management = 10
                                                   then null
                                                   else
                                                        case when count(1) > 0 then 1 else 0 end
                                                   end
                                                   from di_disinfo_finact_audit aud
                                                   where aud.managing_org_id = mo.id and aud.year = (extract(year from per.date_start) - 2) and (aud.file_id is not null or aud.type_audit_state =30)
                                  ) audit_prev_prev, -- Аудиторское заключение за 2 года предшествующих текущему году (ТСЖ/ЖСК)
                                  (
                                                   select 
                                                         case when mo.type_management = 10  or  (di.funds_info = 20 and file_fund_without is not null)
                                                           then null 
                                                           else 
                                                             case when (di.funds_info = 10 and di.size_payments is not null and count(1) > 0) then 1 else 0 end
                                                         end
                                                   from di_disinfo_funds fund
                                                   where fund.disinfo_id = di.id
                                  ) funds_info,  --Сведения о фондах
                                  (
                                    select  case when count(1) > 0 then 0 else 1 end  
                                    from  gkh_morg_contract_realobj cro
                                                left join gkh_morg_contract con on con.id = cro.man_org_contract_id
                                                left join gkh_reality_object robj on robj.id = cro.reality_obj_id 
                                    where con.manag_org_id = mo.id and (robj.date_commissioning is null or per.date_accounting is  null or (per.date_accounting >= robj.date_commissioning)) and
                                      (((con.start_date >= per.date_start or per.date_start is null) and (per.date_end  >= con.start_date or per.date_end is null))
                                      or(per.date_start  >= con.start_date and( con.end_date >= per.date_start or  con.end_date is null))) and not exists 
                                      (select 1 from di_disinfo_finact_realobj diro
                                        where  diro.disinfo_id = di.id and DIRO.REALITY_OBJ_ID = robj.id  and diro.presented_to_repay is not null and diro.received_provided_serv is not null)
                                 ) manag_ro --Предъявлено/получено по услуге 'Управление МКД'
                            from   gkh_managing_organization mo
	                            left join di_disinfo di on mo.id = di.manag_org_id and  di.period_di_id = :period 
	                            left join gkh_contragent c on c.id = mo.contragent_id
	                            left join gkh_dict_municipality mu on mu.id = c.municipality_id
	                            left join di_dict_period per on per.id = di.period_di_id
                            order by mu.name
             ";
        }

        public string SqlQueryRealObj()
        {
            return @" 
                     select 
                        ro.id,
                        (mu.name || ', '  || ro.address) address, -- адрес дома
                        (
                             select 
                                    case when count(1) > 0 then 1 else 0 end
                             from di_disinfo_ro_reduct_exp re
                             where re.disinfo_ro_id = diro.id   
                        ) plan_red_exp, -- План мер по снижению расходов 
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
                                case when exists (select *
                                                    from di_disinfo_doc_ro dro
                                                    where dro.disinfo_ro_id = diro.id  and (file_act_state_id is not null or  file_catrepair_id is not null or  file_plan_repair_id is not null)) then 1
                              else case when exists (select *
                                                    from di_disinfo_doc_prot dp
                                                    where dp.reality_obj_id = ro.id and (dp.year = extract(year from per.date_start) or dp.year = extract(year from per.date_start) - 1)) then 1 else 0 end end
                            ) docs,
                            (
                              select 
                                   case when count(1) > 0 then 1 else 0 end
                              from di_disinfo_ro_serv_repair rsr
                              where rsr.disinfo_ro_id = diro.id 
                            ) plan_work_serv, -- План работ по содержанию и ремонту
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
                                                     or exists (select *
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
                             (                               case when exists (
                                          select 1
                                          from  di_base_service s 
                                            left join di_housing_service hs on hs.id = s.id
                                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                                            inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                          where ts.code in ('27', '28') and s.disinfo_ro_id = diro.id
                                          and not ( ro.number_lifts = 0 or (hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                    else 
                                         case when  ro.number_lifts = 0 or exists (select 1
                                                              from  di_base_service s 
                                                    left join di_housing_service hs on hs.id = s.id
                                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                                  where ts.code in ('27', '28') and s.disinfo_ro_id = diro.id  and ((hs.type_of_provision_service = 40 and hs.protocol is not null) or  (hs.type_of_provision_service = 20 and s.provider_id is not null)))
                                         then null else 0 end
                               end     
                            ) serv_27_28, -- Лифты
                            (
                                 case when  exists (  select 1
                                  from  di_base_service s 
                                    left join di_repair_service rs on rs.id = s.id
                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                    inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                  where ts.code = '8' and s.disinfo_ro_id = diro.id
                                  and not (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null))) then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_repair_service rs on rs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '8' and s.disinfo_ro_id = diro.id and (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                            end
                            ) serv_8, -- Текущий ремонт жилого здания и благоустройство территории
                            (
                                 case when exists (
                                              select 1
                                  from  di_base_service s 
                                    left join di_repair_service rs on rs.id = s.id
                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                    inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                  where ts.code = '9' and s.disinfo_ro_id = diro.id
                                  and not (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null)))
                                then  1                
                                else 
                                   case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_repair_service rs on rs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '9' and s.disinfo_ro_id = diro.id and (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null)))
                                       then null else 0 end
                            end
                            ) serv_9, -- Текущий ремонт и содержание внутридомовых инженерных сетей центрального отопления
                            (
                                case when exists 
                                  (select 1
                                  from  di_base_service s 
                                    left join di_repair_service rs on rs.id = s.id
                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                    inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                  where ts.code = '11' and s.disinfo_ro_id = diro.id
                                  and not (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null)))
                                then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_repair_service rs on rs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '11' and s.disinfo_ro_id = diro.id and (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null))) 
                                        then null else 0 end
                                        end
                            ) serv_11, -- Текущий ремонт и содержание внутридомовых инженерных сетей водоснабжения и водоотведения
                            (
                                case when exists (
                                  select 1
                                  from  di_base_service s 
                                    left join di_repair_service rs on rs.id = s.id
                                    left join di_dict_templ_service ts on ts.id = s.template_service_id
                                    inner join  di_tariff_fconsumers  tf on tf.base_service_id = s.id 
                                    inner join  di_repair_work_list  w on w.base_service_id = s.id 
                                  where ts.code = '12' and s.disinfo_ro_id = diro.id
                                  and not (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null)))
                                then  1                
                                else 
                                    case when  exists (select 1
                                      from  di_base_service s 
                                        left join di_repair_service rs on rs.id = s.id
                                        left join di_dict_templ_service ts on ts.id = s.template_service_id
                                      where ts.code = '12' and s.disinfo_ro_id = diro.id and (rs.type_of_provision_service = 30 or  (rs.type_of_provision_service = 20 and s.provider_id is not null)))
                                        then null else 0 end
                                end
                            ) serv_12, -- Текущий ремонт и содержание внутридомовых инженерных сетей электроснабжения
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
                          case when exists (select 1
                          from di_base_service s
                            left join di_dict_templ_service ts on ts.id = s.template_service_id
                          where ts.kind_service = 60 and s.disinfo_ro_id = diro.id) then 1 else 0 end
                            ) add_serv -- Услуги с типом 'Дополнительная'
                    from gkh_reality_object ro
                        left join di_disinfo_realobj diro  on ro.id = diro.reality_obj_id and  diro.period_di_id = :period 
                        left join di_dict_period per on per.id = diro.period_di_id
                        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
         ";
        }

        private class MoPercProxy
        {
            public long ManOrgId;
            public string Municipality;
            public string ManOrgName;
            public int? Director;
            public int? DirectionMember;
            public int? RevComMember;
            public int? MailingAddress;
            public int? FactAddress;
            public int? Phone;
            public int? Email;
            public int? Site;
            public int? WorkMode;
            public int? RecepCitizen;
            public int? DispatchWork;
            public int? Ogrn;
            public int? FileProjContr;
            public int? AdminResp;
            public int? MemUnion;
            public int? TermContr;
            public int? TaxSystem;
            public int? Balance;
            public int? BalanceAnnex;
            public int? EstimateCurr;
            public int? EstimatePrev;
            public int? ReportEstimatePrev;
            public int? ConRevCurr;
            public int? ConRevPrev;
            public int? ConRevPrevPrev;
            public int? AuditCurr;
            public int? AuditPrev;
            public int? AuditPrevPrev;
            public int? FundsInfo;
            public int? ManagRealObjs;
            public decimal Percent;
        }

        private class RoPercProxy
        {
            public long RealObjId;
            public string Address;
            public int? PlanRedExp;
            public int? PlanRedPay;
            public int? Docs;
            public int? PlanWorkServ;
            public int? NonResPlace;
            public int? PlaceGenUse;
            public int? ManagServ;
            public int? Serv2;
            public int? Serv6;
            public int? Serv7;
            public int? Serv13;
            public int? Serv14;
            public int? Serv27_28;
            public int? Serv8;
            public int? Serv9;
            public int? Serv11;
            public int? Serv12;
            public int? Serv18;
            public int? Serv17;
            public int? Serv20;
            public int? Serv22;
            public int? AddServ;
            public decimal Percent;
        }

        private MoPercProxy CalcMoPercent(MoPercProxy moPerc)
        {
            var completePositionCount = 0;
            var positionCount = 30;

            completePositionCount += moPerc.Director ?? 0;
            completePositionCount += moPerc.DirectionMember ?? 0;
            completePositionCount += moPerc.RevComMember ?? 0;
            completePositionCount += moPerc.MailingAddress ?? 0;
            completePositionCount += moPerc.FactAddress ?? 0;
            completePositionCount += moPerc.Phone ?? 0;
            completePositionCount += moPerc.Email ?? 0;
            completePositionCount += moPerc.Site ?? 0;
            completePositionCount += moPerc.WorkMode ?? 0;
            completePositionCount += moPerc.RecepCitizen ?? 0;
            completePositionCount += moPerc.DispatchWork ?? 0;
            completePositionCount += moPerc.Ogrn ?? 0;
            completePositionCount += moPerc.FileProjContr ?? 0;
            completePositionCount += moPerc.AdminResp ?? 0;
            completePositionCount += moPerc.MemUnion ?? 0;
            completePositionCount += moPerc.TermContr ?? 0;
            completePositionCount += moPerc.TaxSystem ?? 0;
            completePositionCount += moPerc.Balance ?? 0;
            completePositionCount += moPerc.BalanceAnnex ?? 0;
            completePositionCount += moPerc.EstimateCurr ?? 0;
            completePositionCount += moPerc.EstimatePrev ?? 0;
            completePositionCount += moPerc.ReportEstimatePrev ?? 0;
            completePositionCount += moPerc.ConRevCurr ?? 0;
            completePositionCount += moPerc.ConRevPrev ?? 0;
            completePositionCount += moPerc.ConRevPrevPrev ?? 0;
            completePositionCount += moPerc.AuditCurr ?? 0;
            completePositionCount += moPerc.AuditPrev ?? 0;
            completePositionCount += moPerc.AuditPrevPrev ?? 0;
            completePositionCount += moPerc.FundsInfo ?? 0;
            completePositionCount += moPerc.ManagRealObjs ?? 0;

            positionCount -= moPerc.Director == null ? 1 : 0;
            positionCount -= moPerc.DirectionMember == null ? 1 : 0;
            positionCount -= moPerc.RevComMember == null ? 1 : 0;
            positionCount -= moPerc.MailingAddress == null ? 1 : 0;
            positionCount -= moPerc.FactAddress == null ? 1 : 0;
            positionCount -= moPerc.Phone == null ? 1 : 0;
            positionCount -= moPerc.Email == null ? 1 : 0;
            positionCount -= moPerc.Site == null ? 1 : 0;
            positionCount -= moPerc.WorkMode == null ? 1 : 0;
            positionCount -= moPerc.RecepCitizen == null ? 1 : 0;
            positionCount -= moPerc.DispatchWork == null ? 1 : 0;
            positionCount -= moPerc.Ogrn == null ? 1 : 0;
            positionCount -= moPerc.FileProjContr == null ? 1 : 0;
            positionCount -= moPerc.AdminResp == null ? 1 : 0;
            positionCount -= moPerc.MemUnion == null ? 1 : 0;
            positionCount -= moPerc.TermContr == null ? 1 : 0;
            positionCount -= moPerc.TaxSystem == null ? 1 : 0;
            positionCount -= moPerc.Balance == null ? 1 : 0;
            positionCount -= moPerc.BalanceAnnex == null ? 1 : 0;
            positionCount -= moPerc.EstimateCurr == null ? 1 : 0;
            positionCount -= moPerc.EstimatePrev == null ? 1 : 0;
            positionCount -= moPerc.ReportEstimatePrev == null ? 1 : 0;
            positionCount -= moPerc.ConRevCurr == null ? 1 : 0;
            positionCount -= moPerc.ConRevPrev == null ? 1 : 0;
            positionCount -= moPerc.ConRevPrevPrev == null ? 1 : 0;
            positionCount -= moPerc.AuditCurr == null ? 1 : 0;
            positionCount -= moPerc.AuditPrev == null ? 1 : 0;
            positionCount -= moPerc.AuditPrevPrev == null ? 1 : 0;
            positionCount -= moPerc.FundsInfo == null ? 1 : 0;
            positionCount -= moPerc.ManagRealObjs == null ? 1 : 0;

            moPerc.Percent = (decimal.Divide(completePositionCount, positionCount) * 50).RoundDecimal(2);

            return moPerc;
        }

        private RoPercProxy CalcRoPercent(RoPercProxy roPerc)
        {
            var completePositionCount = 0;
            var positionCount = 21;

            completePositionCount += roPerc.PlanRedExp ?? 0;
            completePositionCount += roPerc.PlanRedPay ?? 0;
            completePositionCount += roPerc.Docs ?? 0;
            completePositionCount += roPerc.PlanWorkServ ?? 0;
            completePositionCount += roPerc.NonResPlace ?? 0;
            completePositionCount += roPerc.PlaceGenUse ?? 0;
            completePositionCount += roPerc.ManagServ ?? 0;
            completePositionCount += roPerc.Serv2 ?? 0;
            completePositionCount += roPerc.Serv6 ?? 0;
            completePositionCount += roPerc.Serv7 ?? 0;
            completePositionCount += roPerc.Serv13 ?? 0;
            completePositionCount += roPerc.Serv14 ?? 0;
            completePositionCount += roPerc.Serv27_28 ?? 0;
            completePositionCount += roPerc.Serv8 ?? 0;
            completePositionCount += roPerc.Serv9 ?? 0;
            completePositionCount += roPerc.Serv11 ?? 0;
            completePositionCount += roPerc.Serv12 ?? 0;
            completePositionCount += roPerc.Serv18 ?? 0;
            completePositionCount += roPerc.Serv17 ?? 0;
            completePositionCount += roPerc.Serv20 ?? 0;
            completePositionCount += roPerc.Serv22 ?? 0;

            positionCount -= roPerc.PlanRedExp == null ? 1 : 0;
            positionCount -= roPerc.PlanRedPay == null ? 1 : 0;
            positionCount -= roPerc.Docs == null ? 1 : 0;
            positionCount -= roPerc.PlanWorkServ == null ? 1 : 0;
            positionCount -= roPerc.NonResPlace == null ? 1 : 0;
            positionCount -= roPerc.PlaceGenUse == null ? 1 : 0;
            positionCount -= roPerc.ManagServ == null ? 1 : 0;
            positionCount -= roPerc.Serv2 == null ? 1 : 0;
            positionCount -= roPerc.Serv6 == null ? 1 : 0;
            positionCount -= roPerc.Serv7 == null ? 1 : 0;
            positionCount -= roPerc.Serv13 == null ? 1 : 0;
            positionCount -= roPerc.Serv14 == null ? 1 : 0;
            positionCount -= roPerc.Serv27_28 == null ? 1 : 0;
            positionCount -= roPerc.Serv8 == null ? 1 : 0;
            positionCount -= roPerc.Serv9 == null ? 1 : 0;
            positionCount -= roPerc.Serv11 == null ? 1 : 0;
            positionCount -= roPerc.Serv12 == null ? 1 : 0;
            positionCount -= roPerc.Serv18 == null ? 1 : 0;
            positionCount -= roPerc.Serv17 == null ? 1 : 0;
            positionCount -= roPerc.Serv20 == null ? 1 : 0;
            positionCount -= roPerc.Serv22 == null ? 1 : 0;

            roPerc.Percent = (decimal.Divide(completePositionCount, positionCount) * 50).RoundDecimal(2);

            return roPerc;
        }

    }
}