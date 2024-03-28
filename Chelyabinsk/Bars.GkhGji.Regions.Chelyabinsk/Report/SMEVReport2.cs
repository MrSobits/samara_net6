namespace Bars.GkhGji.Regions.Chelyabinsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    //using Bars.B4.Modules.ReportPanel;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;
    using Enums;
    using GkhGji.Enums;
    using Entities;

    public class SMEVReport2 : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd= DateTime.MaxValue;

        public SMEVReport2() : base(new ReportTemplateBinary(Properties.Resources.SMEV2))
        {

        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ActPresentation";
            }
        }

        public override string Desciption
        {
            get { return "Отчет СМЭВ2"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Smev1"; }
        }

        public override string Name
        {
            get { return "Отчет СМЭВ1"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            //this.ParseIds(municipalityListId, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var table = reportParams.ComplexReportParams.ДобавитьСекцию("table");

            var contragentLicenseList = Container.Resolve<IDomainService<ManOrgLicense>>().GetAll()
              .Select(x => x.Contragent.Id).Distinct().ToList();

            //Все проверки
            var actChecks = Container.Resolve<IDomainService<ActCheck>>().GetAll()
                .Where(x => x.DocumentDate >= dateStart && x.DocumentDate <= dateEnd)
                .Where(x => x.Inspection.Contragent != null 
                && contragentLicenseList.Contains(x.Inspection.Contragent.Id))
                .ToList();

            var disposals = Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Where(x => x.DocumentDate >= dateStart && x.DocumentDate <= dateEnd)
                .Where(x => x.Inspection.Contragent != null
                && contragentLicenseList.Contains(x.Inspection.Contragent.Id))
                .Select(x => new
                {
                    x.Inspection.Id,
                    x.DocumentNumber,
                    x.TypeAgreementResult
                }).ToList();

            foreach (var disposal in disposals)
            {
                var actCheck = Container.Resolve<IDomainService<ActCheck>>().GetAll()
                    .Where(x => x.Inspection.Id == disposal.Id && x.DocumentNumber == disposal.DocumentNumber)
                    .FirstOrDefault();

                if (actCheck != null)
                    actChecks.Add(actCheck);
            }

            actChecks = actChecks.Distinct().ToList();

            var actChecksId = actChecks.Select(x => x.Id).ToList();

            Int32 actChecksCount = actChecks.Count;

            //
            var actCheckViolations = Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
                .Where(x => actChecksId.Contains(x.ActObject.ActCheck.Id) /*&& x.ActObject.HaveViolation == Gkh.Enums.YesNoNotSet.Yes*/)
                .Select(x => new
                {
                    Id = x.Id,
                    ActCheckId = x.ActObject.ActCheck.Id,
                    HaveViolation = x.ActObject.HaveViolation == Gkh.Enums.YesNoNotSet.Yes ? true : false,
                    PersonInspection = x.ActObject.ActCheck.Inspection.PersonInspection,
                    Contragent = x.ActObject.ActCheck.Inspection.PersonInspection == PersonInspection.Organization && x.ActObject.ActCheck.Inspection.Contragent != null 
                        ? x.ActObject.ActCheck.Inspection.Contragent.Name + "|" + x.ActObject.ActCheck.Inspection.Contragent.Inn
                            : x.ActObject.ActCheck.Inspection.PersonInspection == PersonInspection.PhysPerson
                                ? x.ActObject.ActCheck.Inspection.PhysicalPerson
                                    : x.ActObject.ActCheck.Inspection.PersonInspection == PersonInspection.Official && x.ActObject.ActCheck.Inspection.Contragent != null
                                        ? x.ActObject.ActCheck.Inspection.Contragent.Name + "|" + x.ActObject.ActCheck.Inspection.Contragent.Inn + "|" + x.ActObject.ActCheck.Inspection.PhysicalPerson
                                            : x.ActObject.ActCheck.Inspection.PhysicalPerson,
                    ContragentId = x.ActObject.ActCheck.Inspection.PersonInspection == PersonInspection.Organization && x.ActObject.ActCheck.Inspection.Contragent != null
                        ? x.ActObject.ActCheck.Inspection.Contragent.Id
                            : x.ActObject.ActCheck.Inspection.PersonInspection == PersonInspection.PhysPerson
                                ? 0
                                    : x.ActObject.ActCheck.Inspection.PersonInspection == PersonInspection.Official && x.ActObject.ActCheck.Inspection.Contragent != null
                                        ? x.ActObject.ActCheck.Inspection.Contragent.Id
                                            : 0
                })
                .ToList();

            List<string> contragents = actCheckViolations
                .Where(x => contragentLicenseList.Contains(x.ContragentId))
                .Select(x => x.Contragent).Distinct().ToList();

            Int32 contragentsCount = contragents.Count;

            List<string> contragentsWithViol = actCheckViolations
                .Where(x => x.HaveViolation)
                .Where(x => contragentLicenseList.Contains(x.ContragentId))
                .Select(x => x.Contragent).Distinct().ToList();

            Int32 contragentsWithViolCount = contragentsWithViol.Count;

            Int32 contragentPctViol = contragentsCount != 0 ? contragentsWithViolCount / contragentsCount * 100 : 0;

            var t = actCheckViolations.Select(y => y.Id).ToList();

            var b_1 = 0;
            var contragentPctViolDo = 0;
            try
            {
                var actChecksWithViolDo = Container.Resolve<IDomainService<ActCheckViolation>>().GetAll()
               .Where(x => t.Contains(x.Id))
               .Where(x => x.InspectionViolation.Inspection.Contragent !=null
                                && contragentLicenseList.Contains(x.InspectionViolation.Inspection.Contragent.Id))                                   
               .Select(x => new
               {
                   ActCheckId = x.ActObject.ActCheck.Id,
                   DateFactRemoval = x.InspectionViolation.DateFactRemoval.HasValue && x.InspectionViolation.DateFactRemoval.Value != DateTime.MinValue
                    ? x.InspectionViolation.DateFactRemoval.Value : DateTime.MinValue
               })
               .ToList()
               .GroupBy(x => x.ActCheckId).Select(g => new
               {
                   Id = g.Key,
                   Date = g.Min(x => x.DateFactRemoval)
               })
               .ToList()
               .Where(x => x.Date != DateTime.MinValue)
               .Select(x => x.Id).ToList();

                List<string> contragentsWithViolDo = actCheckViolations
               .Where(x => actChecksWithViolDo.Contains(x.ActCheckId))
              .Select(x => x.Contragent).Distinct().ToList();

                Int32 contragentsWithViolDoCount = contragentsWithViolDo.Count;

                contragentPctViolDo = contragentsCount != 0 ? contragentsWithViolDoCount / contragentsCount * 100 : 0;

                b_1 = actChecksCount != 0 ? actChecksWithViolDo.Count / actChecksCount * 100 : 0;
            }
            catch
            {
                // ignored
            }


            var appealCits = Container.Resolve<IDomainService<AppealCits>>().GetAll()
                .Where(x => x.DateFrom >= dateStart && x.DateFrom <= dateEnd)
                .Select(x => x.Id).ToList();

            Int32 appealCitsCount = appealCits.Count;

            var disposalsAppCitsCount = Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Where(x => x.Inspection.TypeBase == TypeBase.CitizenStatement).Distinct().Count();

            Int32 appealCitsPct = appealCitsCount != 0 ? disposalsAppCitsCount / appealCitsCount * 100 : 0;

            //В.2.8
            var appealCitsAdmonitionCount = Container.Resolve<IDomainService<AppealCitsAdmonition>>().GetAll()
                .Where(x => x.AppealCits.DateFrom >= dateStart && x.AppealCits.DateFrom <= dateEnd).Count();

            //В.3.1.2
            Int32 planActCheck = 0;

            //В.3.1.3
            Int32 notPlanActCheck = 0;

            //В.3.1.22
            Int32 b_3_1_22 = 0;

           

            Int32 disposalProsecutotCount = 0;

            Int32 actCheckIsNotHaveViolCount= 0;

            foreach (var actCheck in actChecks)
            {
                var isPlan = actCheck.Inspection.TypeBase == TypeBase.PlanAction || actCheck.Inspection.TypeBase == TypeBase.PlanJuridicalPerson ? true : false;

                if (!isPlan)
                {
                    notPlanActCheck++;
                    if (actCheck.Inspection.Contragent != null
                        && contragentLicenseList.Contains(actCheck.Inspection.Contragent.Id))
                        b_3_1_22++;
                }                 
                else
                {
                    planActCheck++;
                    var isAgreement = Container.Resolve<IDomainService<Disposal>>().GetAll()
                        .Where(x => x.TypeAgreementResult == TypeAgreementResult.Agreed && x.Inspection.Id >= actCheck.Id && x.DocumentNumber == actCheck.DocumentNumber)
                        .FirstOrDefault();
                    if (isAgreement != null)
                    {
                        disposalProsecutotCount++;
                        var isNotHaveViol = actCheckViolations
                            .Where(x => !x.HaveViolation && x.ActCheckId == actCheck.Id).FirstOrDefault();
                        if(isNotHaveViol != null)
                        {
                            actCheckIsNotHaveViolCount++;
                        }
                    }
                        
                }               
            }

            var totalArea = Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                .GetAll()
                .Where(x => x.ManOrgContract.ManagingOrganization != null
                && x.ManOrgContract.ManagingOrganization.Contragent != null
                && contragentLicenseList.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
                .Where(x => x.ManOrgContract.StartDate >= dateStart
                && (x.ManOrgContract.EndDate == null || (x.ManOrgContract.EndDate <= dateEnd)))
                .Select(x => x.RealityObject.AreaMkd).Sum();

            //В.3.1.16
            Int32 disposalProsecutotCountPct = notPlanActCheck != 0 ? disposalProsecutotCount / notPlanActCheck * 100 : 0;

            //В.3.1.17
            Int32 actCheckIsNotHaveViolCountPct = notPlanActCheck != 0 ? actCheckIsNotHaveViolCount / notPlanActCheck * 100 : 0;

            //В.3.1.19
            Int32 b_3_1_19 = contragentsCount != 0 ? actChecksCount / contragentsCount : 0;

            var protocolArticleLaw_19_4_1 = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll()
                .Where(x => x.ArticleLaw.Name.Contains("19.4.1")).AsEnumerable();

            var b_3_1_29 = actChecksCount != 0 ? actChecks
                .Join(protocolArticleLaw_19_4_1, x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Protocol.Inspection.Id + "|" + y.Protocol.DocumentNumber, (x, y) => new { x, y })
                .Select(x => x.x.Id).Distinct().Count() / actChecksCount * 100 : 0;

            var protocolArticleLaw_19_5 = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll()
                .Where(x => x.ArticleLaw.Name.Contains("19.5.1") || x.ArticleLaw.Name.Contains("19.5.24")).AsEnumerable();
            
            var b_3_1_30 = actChecksCount != 0 ? actChecks
                .Join(protocolArticleLaw_19_5, x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Protocol.Inspection.Id + "|" + y.Protocol.DocumentNumber, (x, y) => new { x, y })
                .Select(x => x.x.Id).Distinct().Count() / actChecksCount * 100 : 0;

            var resolutions = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .Where(x => x.Sanction.Code == "1" && x.PenaltyAmount.HasValue).AsEnumerable();

            var penaltyAmount = actChecks
                .Join(resolutions, x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Inspection.Id + "|" + y.DocumentNumber, (x, y) => new { x, y })
                .Select(x => x.y.PenaltyAmount.Value).ToList();
                
            var b_3_1_35 = penaltyAmount.Sum();

            var resolutionPayFine = Container.Resolve<IDomainService<ResolutionPayFine>>().GetAll()
                .Where(x => x.Amount.HasValue).AsEnumerable();

            var b_3_1_36 = actChecks
                .Join(resolutionPayFine, x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Resolution.Inspection.Id + "|" + y.Resolution.DocumentNumber, (x, y) => new { x, y })
                .Sum(x => x.y.Amount.Value);

            var b_3_1_37 = b_3_1_35 != 0 ? b_3_1_36 / b_3_1_35 : 0;

            var b_3_1_38 = penaltyAmount.Count != 0 ? b_3_1_35 / penaltyAmount.Count : 0;

            var b_3_1_40 = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                .Where(x => actChecksId.Contains(x.DocumentGji.Id))
                .Select(x => x.Id).Count();

            var b_3_4_1 = contragentsCount;

            var protocols = Container.Resolve<IDomainService<Protocol>>().GetAll().AsEnumerable()
                .Join(actChecks, x => x.Inspection.Id + x.DocumentNumber, y => y.Inspection.Id + y.DocumentNumber,
                (x, y) => x.Id).ToList();


            var protocolsCount = protocols.Count();

            var b_3_4_6 = actChecksCount != 0 ? protocolsCount / actChecksCount * 100 : 0;

            var b_3_6_1 = protocolsCount;

            var b_3_6_2 = 0;

            var b_3_6_4 = b_3_1_35;

            var resolutions2 = Container.Resolve<IDomainService<Resolution>>().GetAll()
                .AsEnumerable();

            var penaltyAmount2 = actChecks
                .Join(resolutions2, x => x.Inspection.Id + "|" + x.DocumentNumber, y => y.Inspection.Id + "|" + y.DocumentNumber, (x, y) => new { x, y })
                .Select(x => x.y.Id).ToList();

            var b_3_6_5 = penaltyAmount2.Count() > 0 
                ? penaltyAmount.Count() / penaltyAmount2.Count() * 100 
                    : 0;

            var b_3_6_6 = b_3_1_36;

            var b_3_6_7 = b_3_6_4 != 0 ? b_3_6_6 / b_3_6_4 * 100 : 0;

            var b_3_6_8 = b_3_1_38;

            table.ДобавитьСтроку();
            table["a_3"] = totalArea != 0
                            ? actCheckViolations.Count() / (totalArea / 1000) * 100
                                : 0;
            table["b_1"] = b_1;
            table["b_2_1"] = actChecksCount;
            table["b_2_5"] = contragentPctViolDo;
            table["b_3_1_1"] = actChecksCount;
            table["b_3_1_2"] = planActCheck;
            table["b_3_1_3"] = notPlanActCheck;
            table["b_3_1_3_1"] = Decimal.Round(notPlanActCheck/3, 0);
            table["b_3_1_3_2"] = notPlanActCheck - disposalProsecutotCount - Decimal.Round(notPlanActCheck / 3, 0);
            table["b_3_1_3_4"] = disposalProsecutotCount;
            table["b_3_1_19"] = b_3_1_19;
            table["b_3_1_22"] = b_3_1_22;
            table["b_3_1_27"] = actChecksCount != 0 ? actCheckViolations.Count() / actChecksCount * 100 : 0 ;
            table["b_3_1_29"] = b_3_1_29;
            table["b_3_1_30"] = b_3_1_30;
            table["b_3_1_35"] = b_3_1_35;
            table["b_3_1_36"] = b_3_1_36;
            table["b_3_1_37"] = b_3_1_37;
            table["b_3_1_38"] = b_3_1_38;
            table["b_3_4_1"] = b_3_4_1;
            table["b_3_4_5"] = actCheckViolations.Count();
            table["b_3_4_6"] = b_3_4_6;
            table["b_3_6_1"] = b_3_6_1;
            table["b_3_6_2"] = b_3_6_2;
            table["b_3_6_4"] = b_3_6_4;
            table["b_3_6_5"] = b_3_6_5;
            table["b_3_6_6"] = b_3_6_6;
            table["b_3_6_7"] = b_3_6_7;
            table["b_3_6_8"] = b_3_6_8;
        }
    }
}