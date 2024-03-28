namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class SpecialAccountReportInterceptor : EmptyDomainInterceptor<SpecialAccountReport>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ManOrgContractRealityObject> MorgRODomain { get; set; }

        public IDomainService<SpecialAccountRow> SpecialAccountReportRowDomain { get; set; }

        public IDomainService<SpecialAccountRow> SpecialAccountReportRowDomain2 { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<SpecialAccountReport> SpecialAccountReportDomain { get; set; }

        public IDomainService<SPAccOwnerRealityObject> SPAccOwnerRealityObjectDomain { get; set; }

        public IDomainService<SpecialAccountOwner> SpecialAccountOwnerDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SpecialAccountReport> service, SpecialAccountReport entity)
        {
            entity.AmmountMeasurement = AmmountMeasurement.NotLiv;
            Int64 id = entity.Id;
            Int64 cOid = 0;
            var contragentList = UserManager.GetContragentIds();
            Operator thisOperator = UserManager.GetActiveOperator();
            entity.Author = thisOperator.User.Name;
            if (entity.Contragent != null && entity.Contragent.Id != 0)
            {
                var owner = SpecialAccountOwnerDomain.GetAll()
                    .FirstOrDefault(x => x.Contragent == entity.Contragent);
                if (owner == null || owner.ActivityDateEnd.HasValue)
                {
                    return Failure("Организация не является владельцем специального счета");
                }

                cOid = entity.Contragent.Id;
                if (contragentList == null || contragentList.Count == 0)
                {
                    contragentList.Add(entity.Contragent.Id);
                    entity.Sertificate = "Отчет сдан в письменном виде";
                }
            }
            if (thisOperator.Inspector == null)
            {
                entity.DateAccept = DateTime.Now;
            }

            if (contragentList.Contains(84367374))
            {
                contragentList = new List<long>();
            }

            if (contragentList == null || contragentList.Count == 0)
            {
                return Failure("У оператора не проставлен контрагент");
            }
            else
            {
                entity.Contragent = this.ContragentDomain.GetAll().FirstOrDefault(x => x.Id == contragentList[0]);
                cOid = entity.Contragent.Id;
            }

            if (entity.Contragent != null)
            {
                var owner = SpecialAccountOwnerDomain.GetAll()
                   .FirstOrDefault(x => x.Contragent == entity.Contragent);
                if (owner == null || owner.ActivityDateEnd.HasValue)
                {
                    return Failure("Организация не является владельцем специального счета");
                }

                var reports = SpecialAccountReportDomain.GetAll()
              .Where(x => x.Contragent.Id == entity.Contragent.Id && x.MonthEnums == entity.MonthEnums && x.YearEnums == entity.YearEnums)
              .Select(x => new
              {
                  x.Id
              }).ToList();

                if (reports.Count > 0)
                {
                    return Failure("В отчетном периоде " + entity.MonthEnums.GetDisplayName() + " " + entity.YearEnums.GetDisplayName() + " уже создан отчет для " + entity.Contragent.Name);
                }
            }

            //Int32 year = Convert.ToInt32(entity.YearEnums.GetDisplayName());
            //Int32 month = 1;
            //Int32 factor = 1;
            //switch (entity.MonthEnums)
            //{

            //    case Enums.MonthEnums.Quarter1:
            //        month = 4;
            //        factor = 3;
            //        break;
            //    case Enums.MonthEnums.Quarter2:
            //        month = 7;
            //        factor = 3;
            //        break;
            //    case Enums.MonthEnums.Quarter3:
            //        month = 10;
            //        factor = 3;
            //        break;
            //    case Enums.MonthEnums.Quarter4:
            //        month = 1;
            //        factor = 3;
            //        break;
            //}

            //DateTime reportDate = new DateTime(year, month, 1);

            //  SPAccOwnerRealityObjectDomain

            var realityObjects = SPAccOwnerRealityObjectDomain.GetAll()
            .Where(x => x.DateStart <= DateTime.Now)
            .Where(x => !x.DateEnd.HasValue || x.DateEnd.Value > DateTime.Now)
            .WhereIf(contragentList.Any(), x => contragentList.Contains(x.SpecialAccountOwner.Contragent.Id))
           .ToList();

            //Переделано под справочник владельцев спецсчетов
            //var realityObjects = MorgRODomain.GetAll()
            // .Where(x => (x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= reportDate)
            // && x.ManOrgContract.ManagingOrganization.Contragent != null && x.RealityObject.CRAccountWay == Gkh.Enums.CRAccountWay.SpecialAccount)
            // .WhereIf(contragentList.Any(), x => contragentList.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
            // .Select(x => new
            // {
            //    x.RealityObject
            // }).ToList();
            Dictionary<Int64, string> saNumDict = new Dictionary<long, string>();
            Dictionary<Int64, decimal> areaDict = new Dictionary<long, decimal>();
            Dictionary<Int64, DateTime> dateDict = new Dictionary<long, DateTime>();
            try
            {
                foreach (var ro in realityObjects)
                {
                    var sprowDomain = SpecialAccountReportRowDomain2.GetAll()
                     .Where(x => x.RealityObject.Id == ro.RealityObject.Id)
                     .Where(x => x.SpecialAccountReport.Contragent.Id == cOid)
                     .Where(x => x.SpecialAccountReport.Id != id)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
                    string san = "";
                    decimal accArea = 0;
                    DateTime startDate;
                    if (sprowDomain != null && sprowDomain.SpecialAccountNum != null)
                    {
                        san = sprowDomain.SpecialAccountNum;
                        accArea = sprowDomain.AccuracyArea;
                        startDate = sprowDomain.StartDate ?? DateTime.MinValue;
                        if (!saNumDict.ContainsKey(ro.Id))
                        {
                            saNumDict.Add(ro.Id, san);
                        }
                        if (accArea > 0 && !areaDict.ContainsKey(ro.RealityObject.Id))
                        {
                            areaDict.Add(ro.Id, accArea);
                        }
                        if (startDate > DateTime.MinValue && !dateDict.ContainsKey(ro.Id))
                        {
                            dateDict.Add(ro.Id, startDate);
                        }

                    }
                }
                foreach (var ro in realityObjects)
                {
                    SpecialAccountRow sprow = new SpecialAccountRow
                    {
                        SpecialAccountReport = entity,
                        AmmountDebt = 0,
                        Ballance = 0,
                        Accured = 0,
                        AccuredTotal = 0,
                        IncomingTotal = 0,
                        TransferTotal = 0,
                        Tariff = entity.Tariff,
                        Contracts = "",
                        Incoming = 0,
                        AccuracyArea = areaDict.ContainsKey(ro.Id) ? areaDict[ro.Id] : ro.RealityObject.AreaLivingNotLivingMkd ?? 0,
                        StartDate = dateDict.ContainsKey(ro.Id) ? dateDict[ro.Id] : ro.DateStart,
                        Municipality = ro.RealityObject.Municipality,
                        RealityObject = ro.RealityObject,
                        SpecialAccountNum = saNumDict.ContainsKey(ro.RealityObject.Id) ? saNumDict[ro.RealityObject.Id] : ro.SpacAccNumber,
                        Transfer = 0
                    };

                    this.SpecialAccountReportRowDomain.Save(sprow);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

        }

        public override IDataResult AfterCreateAction(IDomainService<SpecialAccountReport> service, SpecialAccountReport entity)
        {
            try
            {
                var reportRows = SpecialAccountReportRowDomain.GetAll()
                    .Where(x => x.SpecialAccountReport == entity);

                foreach (var reportRow in reportRows)
                {
                    var previousReportRows = SpecialAccountReportRowDomain.GetAll()
                    .Where(x => x.RealityObject == reportRow.RealityObject)
                    .Where(x => x.SpecialAccountReport.Contragent == reportRow.SpecialAccountReport.Contragent);

                    if (previousReportRows != null && previousReportRows.Count() != 0)
                    {
                        YearEnums year;
                        MonthEnums quarter;

                        switch (entity.MonthEnums)
                        {
                            case MonthEnums.Quarter1:
                                quarter = MonthEnums.Quarter4;
                                break;
                            case MonthEnums.Quarter2:
                                quarter = MonthEnums.Quarter1;
                                break;
                            case MonthEnums.Quarter3:
                                quarter = MonthEnums.Quarter2;
                                break;
                            case MonthEnums.Quarter4:
                                quarter = MonthEnums.Quarter3;
                                break;
                            default:
                                quarter = 0;
                                break;
                        }

                        if (quarter != 0)
                        {
                            if (quarter == MonthEnums.Quarter4)
                            {
                                year = entity.YearEnums - 1;
                            }
                            else
                            {
                                year = entity.YearEnums;
                            }

                            var previousReportRow = previousReportRows
                                .Where(x => x.SpecialAccountReport.YearEnums == year)
                                .Where(x => x.SpecialAccountReport.MonthEnums == quarter)
                                .FirstOrDefault();

                            if (previousReportRow != null)
                            {
                                reportRow.AccuredTotal = previousReportRow.AccuredTotal + previousReportRow.Accured;
                                reportRow.IncomingTotal = previousReportRow.IncomingTotal + previousReportRow.Incoming;
                                reportRow.TransferTotal = previousReportRow.TransferTotal + previousReportRow.Transfer;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SpecialAccountReport> service, SpecialAccountReport entity)
        {
            Operator thisOperator = UserManager.GetActiveOperator();
            entity.Author = thisOperator.User.Name;
            if (entity.SignedXMLFile != null)
            {
                entity.DateAccept = DateTime.Now;
                entity.Sertificate = "Подписан";
            }
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SpecialAccountReport> service, SpecialAccountReport entity)
        {
            try
            {
                var reportRow = SpecialAccountReportRowDomain.GetAll()
               .Where(x => x.SpecialAccountReport.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    SpecialAccountReportRowDomain.Delete(id);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }

        }
    }
}
