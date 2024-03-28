namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using B4.DataAccess;

    public class SpecialAccountReportInterceptor : EmptyDomainInterceptor<SpecialAccountReport>
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
            entity.AmmountMeasurement = Enums.AmmountMeasurement.NotLiv;
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
                contragentList = new List<long>();

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
           
            Int32 year = Convert.ToInt32(entity.YearEnums.GetDisplayName());
            Int32 month = 1;
            Int32 factor = 1;
            switch (entity.MonthEnums)
            {
               
                case Enums.MonthEnums.Quarter1:
                    month = 4;
                    factor = 3;
                    break;
                case Enums.MonthEnums.Quarter2:
                    month = 7;
                    factor = 3;
                    break;
                case Enums.MonthEnums.Quarter3:
                    month = 10;
                    factor = 3;
                    break;
                case Enums.MonthEnums.Quarter4:
                    month = 1;
                    factor = 3;
                    break;
            }

            DateTime reportDate = new DateTime(year, month, 1);

          //  SPAccOwnerRealityObjectDomain

             var realityObjects = SPAccOwnerRealityObjectDomain.GetAll()
             .Where(x => x.DateStart<=DateTime.Now)
             .Where(x=> !x.DateEnd.HasValue || x.DateEnd.Value >DateTime.Now)             
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
                        startDate = sprowDomain.StartDate.HasValue? sprowDomain.StartDate.Value: DateTime.MinValue;
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

                    SpecialAccountRow sprow = new SpecialAccountRow();
                    sprow.SpecialAccountReport = entity;
                    sprow.AmmountDebt = 0;
                    sprow.Ballance = 0;
                    sprow.Accured = 0;
                    sprow.AccuredTotal = 0;
                    sprow.IncomingTotal = 0;
                    sprow.TransferTotal = 0;
                    sprow.Tariff = entity.Tariff;
                    sprow.Contracts = "";
                    sprow.Incoming = 0;
                    sprow.AccuracyArea = areaDict.ContainsKey(ro.Id) ? areaDict[ro.Id] : ro.RealityObject.AreaLivingNotLivingMkd.HasValue? ro.RealityObject.AreaLivingNotLivingMkd.Value:0;
                    sprow.StartDate = dateDict.ContainsKey(ro.Id) ? dateDict[ro.Id] : ro.DateStart;
                    sprow.Municipality = ro.RealityObject.Municipality;
                    sprow.RealityObject = ro.RealityObject;
                    sprow.SpecialAccountNum = saNumDict.ContainsKey(ro.RealityObject.Id)? saNumDict[ro.RealityObject.Id]: ro.SpacAccNumber;
                    sprow.Transfer = 0;
                    this.SpecialAccountReportRowDomain.Save(sprow);
                    
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }
            
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
