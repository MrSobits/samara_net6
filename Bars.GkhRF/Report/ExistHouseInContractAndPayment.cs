namespace Bars.GkhRf.Report
{
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Properties;

    using Castle.Windsor;

    public class ExistHouseInContractAndPayment : BasePrintForm
    {
        private long[] municipalities;

        public ExistHouseInContractAndPayment()
            : base(new ReportTemplateBinary(Resources.ExistHouseInContractAndPayment))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет по наличию домов в договоре и в оплатах";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по наличию домов в договоре и в оплатах";
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
                return "B4.controller.report.ExistHouseInContractAndPayment";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.ExistHouseInContractAndPayment";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalitiesParam = baseParams.Params["municipalityIds"].ToString();
            municipalities = string.IsNullOrEmpty(municipalitiesParam) ? new long[0] : municipalitiesParam.Split(',').Select(x => x.ToLong()).ToArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realtyObjects = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalities.Length > 0, x => municipalities.Contains(x.Municipality.Id))
                .Where(y => this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll().Any(x => x.RealityObject.Id == y.Id && x.TypeCondition == TypeCondition.Include) || this.Container.Resolve<IDomainService<Payment>>().GetAll().Any(x => x.RealityObject.Id == y.Id))
                .Select(x => new { x.Id, x.Address, Municipality = x.Municipality.Name })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Address)
                .ToArray();

            var contractRfObjectDict =
                this.Container.Resolve<IDomainService<ContractRfObject>>()
                    .GetAll()
                    .WhereIf(municipalities.Length > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.TypeCondition == TypeCondition.Include)
                    .Select(
                        x =>
                        new
                            {
                                x.RealityObject.Id,
                                ManagingOrganization = x.ContractRf.ManagingOrganization.Contragent.Name,
                                x.ContractRf.DocumentNum,
                                x.ContractRf.DocumentDate,
                                x.IncludeDate
                            })
                            .AsEnumerable() 
                            .GroupBy(x => x.Id)
                            .ToDictionary(
                                x => x.Key,
                                x => 
                                new
                                    {
                                        ManagingOrganization = x.Select(y => y.ManagingOrganization).FirstOrDefault(),
                                        DocumentNum = x.Select(y => y.DocumentNum).FirstOrDefault(),
                                        DocumentDate = x.Select(y => y.DocumentDate.HasValue ? y.DocumentDate.Value.ToShortDateString() : string.Empty).FirstOrDefault(),
                                        IncludeDate = x.Select(y => y.IncludeDate.HasValue ? y.IncludeDate.Value.ToShortDateString() : string.Empty).FirstOrDefault()
                                    });

            var paymentDict =
                this.Container.Resolve<IDomainService<Payment>>()
                    .GetAll()
                    .WhereIf(municipalities.Length > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                    .ToDictionary(x => x.RealityObject.Id);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var realtyObj in realtyObjects)
            {
                section.ДобавитьСтроку();
                section["municipality"] = realtyObj.Municipality;
                section["address"] = realtyObj.Address;
                section["isContract"] = contractRfObjectDict.ContainsKey(realtyObj.Id) ? "Да" : "Нет";
                section["managingOrganization"] = contractRfObjectDict.ContainsKey(realtyObj.Id)
                                                      ? contractRfObjectDict[realtyObj.Id].ManagingOrganization
                                                      : string.Empty;
                section["documentNum"] = contractRfObjectDict.ContainsKey(realtyObj.Id)
                                                      ? contractRfObjectDict[realtyObj.Id].DocumentNum
                                                      : string.Empty;
                section["documentDate"] = contractRfObjectDict.ContainsKey(realtyObj.Id)
                                                     ? contractRfObjectDict[realtyObj.Id].DocumentDate
                                                     : string.Empty;
                section["includeDate"] = contractRfObjectDict.ContainsKey(realtyObj.Id)
                                                     ? contractRfObjectDict[realtyObj.Id].IncludeDate
                                                     : string.Empty;
                section["isExist"] = paymentDict.ContainsKey(realtyObj.Id) ? "Да" : "Нет";
            }
        }
    }
}