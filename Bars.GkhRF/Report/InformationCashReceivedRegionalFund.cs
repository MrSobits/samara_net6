using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.GkhRf.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    class InformationCashReceivedRegionalFund : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private List<long> municipalityIdsList = new List<long>();

        public InformationCashReceivedRegionalFund()
            : base(new ReportTemplateBinary(Properties.Resources.InformationCashReceivedRegionalFund))
        {
        }
        
        public override string Name
        {
            get
            {
                return "Информация о поступивших денежных средствах в Региональный фонд";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация о поступивших денежных средствах в Региональный фонд";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.InformationCashReceivedRegionalFund";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.InformationCashReceivedRegionalFund";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();

            municipalityIdsList.Clear();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            if (!string.IsNullOrEmpty(municipalityStr))
            {
                var mcp = municipalityStr.Split(',');
                foreach (var id in mcp)
                {
                    long mcpId = 0;
                    if (long.TryParse(id, out mcpId))
                    {
                        if (!municipalityIdsList.Contains(mcpId))
                        {
                            municipalityIdsList.Add(mcpId);
                        }
                    }
                }
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceTransferRfRecObj = Container.Resolve<IDomainService<TransferRfRecObj>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            var dictMunName = serviceMunicipality.GetAll()
                         .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.Id))
                         .Select(x => new { x.Id, x.Name })
                         .OrderBy(x => x.Name)
                         .ToDictionary(x => x.Id, v => v.Name);

            var data = serviceTransferRfRecObj.GetAll()
                .WhereIf(this.municipalityIdsList.Count > 0, x => this.municipalityIdsList.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.TransferRfRecord.DateFrom >= dateStart && x.TransferRfRecord.DateFrom <= dateEnd)
                .GroupBy(x => new
                {
                    StatusName = x.TransferRfRecord.State.Name,
                    ContragentName = x.TransferRfRecord.TransferRf.ContractRf.ManagingOrganization.Contragent.Name,
                    ContractNum = x.TransferRfRecord.TransferRf.ContractRf.DocumentNum,
                    DocName = x.TransferRfRecord.DocumentNum,
                    DocDate = x.TransferRfRecord.DateFrom
                })
                .Select(x => new
                {
                    x.Key,
                    Sum = x.Sum(y => y.Sum),
                    roCount = x.Select(y => y.RealityObject.Id).Distinct().Count(),
                    muId = x.Min(y => y.RealityObject.Municipality.Id)
                })
                .ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            int num = 1;
            decimal totalItogoSum = 0m;
            foreach (var mun in data.OrderBy(x => x.Key.ContragentName))
            {
                section.ДобавитьСтроку();
                
                var sum = mun.Sum;

                section["Num"] = num++;
                section["Status"] = mun.Key.StatusName == null ? string.Empty : mun.Key.StatusName.ToStr();
                section["MO"] = dictMunName[mun.muId] == null ? string.Empty : dictMunName[mun.muId].ToStr();
                section["UKName"] = mun.Key.ContragentName == null ? string.Empty : mun.Key.ContragentName.ToStr();
                section["NumContract"] = mun.Key.ContractNum == null ? string.Empty : mun.Key.ContractNum.ToStr();
                section["PaymentOrder"] = mun.Key.DocName == null ? string.Empty : mun.Key.DocName.ToStr();
                section["Date"] = mun.Key.DocDate == null ? string.Empty : mun.Key.DocDate.ToStr();
                section["MKDCount"] = mun.roCount > 0 ? mun.roCount.ToStr() : string.Empty;
                section["ItogoSum"] = sum > 0 ? sum.ToStr() : string.Empty;
                totalItogoSum += sum.ToDecimal();
            }

            reportParams.SimpleReportParams["TotalItogoSum"] = totalItogoSum > 0 ? totalItogoSum.ToStr() : string.Empty;
            reportParams.SimpleReportParams["DateStart"] = dateStart.ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToShortDateString();
        }

    }
}
