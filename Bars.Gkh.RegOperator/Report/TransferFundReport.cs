using System;
using Bars.Gkh.Domain;
using Bars.GkhRf.Entities;

namespace Bars.Gkh.RegOperator.Report
{
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    public class TransferFundReport : BasePrintForm
    {
        private long[] RoIds;

        private long ManOrgId;

        private DateTime StartDate;

        private DateTime EndDate;

        public IDomainService<PersonalAccountPayment> PersonalAccountPaymentDomain { get; set; }

        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        public IDomainService<ManagingOrganization> ManOrgDomain { get; set; }

        public IDomainService<TransferRfRecObj> TransferRfRecObjDomain { get; set; }

        public TransferFundReport()
            : base(new ReportTemplateBinary(Properties.Resources.TransferFundReport))
        {
        }

        public override string Name { get { return "Отчет по перечислениям в Фонд"; } }

        public override string Desciption { get { return "Отчет по перечислениям в Фонд"; } }

        public override string GroupName { get { return "Региональный фонд"; } }

        public override string ParamsController { get { return "B4.controller.report.TransferFundReport"; } }

        public override string RequiredPermission { get { return "Reports.GkhRegOp.TransferFundReport"; } }

        public override void SetUserParams(BaseParams baseParams)
        {
            RoIds = baseParams.Params.GetAs("roIds", string.Empty).ToLongArray();

            ManOrgId = baseParams.Params.GetAs<long>("manOrgId");

            StartDate = baseParams.Params.GetAs("startDate", DateTime.MinValue);

            EndDate = baseParams.Params.GetAs("endDate", DateTime.MaxValue);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var manOrgInfo = ManOrgDomain.GetAll()
                .Where(x => x.Id == ManOrgId)
                .Select(x =>
                    new
                    {
                        x.Contragent.Name,
                        MuName = x.Contragent.Municipality.Name
                    })
                .FirstOrDefault();

            var persAccountPayments = PersonalAccountPaymentDomain.GetAll()
                .Where(x => StartDate <= x.PaymentDate && x.PaymentDate <= EndDate)
                .Select(x => new
                {
                    x.BasePersonalAccount.Id,
                    x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Sum));

            var personalAccounts = PersonalAccountDomain.GetAll()
                .Where(x => RoIds.Contains(x.Room.RealityObject.Id))
                .Select(x => new
                {
                    PersAccId = x.Id,
                    RoId = x.Room.RealityObject.Id,
                    x.Room.RealityObject.Address,
                    x.Room.RoomNum,
                    x.PersonalAccountNum
                })
                .OrderBy(x => x.Address)
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.ToList());

            var transferRecSums = TransferRfRecObjDomain.GetAll()
                        .Where(x => RoIds.Contains(x.RealityObject.Id)  && 
                            StartDate <= x.TransferRfRecord.TransferDate && x.TransferRfRecord.TransferDate <= EndDate)
                        .Select(x => new
                        {
                            x.RealityObject.Id,
                            x.Sum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.SafeSum(x => x.Sum.ToDecimal()));

            var sectionRealObj = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRo");

            var number = 1;
            var totalPaymentSum = 0M;
            var totalTransferSum = 0M;
            foreach (var roData in personalAccounts)
            {
                sectionRealObj.ДобавитьСтроку();

                if (manOrgInfo != null)
                {
                    sectionRealObj["Municipality"] = manOrgInfo.MuName;
                    sectionRealObj["ManOrg"] = manOrgInfo.Name;
                }

                sectionRealObj["Address"] = roData.Value.First().Address;

                var sectionAccount = sectionRealObj.ДобавитьСекцию("sectionAccount");

                var roPaymentSum = 0M;
                var roTransferSum = 0M;
                foreach (var account in roData.Value)
                {
                    sectionAccount.ДобавитьСтроку();

                    var paymentSum = persAccountPayments.Get(account.PersAccId);
                    var transferSum = transferRecSums.Get(account.RoId);

                    roPaymentSum += paymentSum;
                    roTransferSum += transferSum;

                    sectionAccount["Number"] = number;
                    sectionAccount["RoomNum"] = account.RoomNum;
                    sectionAccount["PersonalAccountNum"] = account.PersonalAccountNum;
                    sectionAccount["PaymentSum"] = paymentSum;
                    sectionAccount["TransferSum"] = transferSum;
                }

                sectionRealObj["RoPaymentSum"] = roPaymentSum;
                sectionRealObj["RoTransferSum"] = roTransferSum;

                totalPaymentSum += roPaymentSum;
                totalTransferSum += roTransferSum;
            }


            reportParams.SimpleReportParams["TotalPaymentSum"] = totalPaymentSum;
            reportParams.SimpleReportParams["TotalTransferSum"] = totalTransferSum;
        }
    }
}