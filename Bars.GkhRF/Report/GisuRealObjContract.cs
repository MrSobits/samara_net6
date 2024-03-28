namespace Bars.GkhRf.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class GisuRealObjContract : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        /// <summary> Идентификаторы муниципальных образований </summary>
        protected readonly List<long> MunicipalityIds = new List<long>();

        public GisuRealObjContract(ReportTemplateBinary tpl) : base(tpl) { }

        public GisuRealObjContract()
            : base(new ReportTemplateBinary(Properties.Resources.GisuRObjectContract))
        {
        }

        /// <inheritdoc />
        public override string RequiredPermission => "Reports.RF.GisuRealObjContract";

        /// <inheritdoc />
        public override string Name => "Отчет по домам, включенным в договор с ГИСУ";

        /// <inheritdoc />
        public override string Desciption => "Отчет по домам, включенным в договор с ГИСУ";

        /// <inheritdoc />
        public override string GroupName => "Отчеты Рег.Фонд";

        /// <inheritdoc />
        public override string ParamsController => "B4.controller.report.GisuRealObjContract";

        /// <inheritdoc />
        public override void SetUserParams(BaseParams baseParams)
        {
            this.MunicipalityIds.Clear();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            if (!string.IsNullOrEmpty(municipalityStr))
            {
                var mcp = municipalityStr.Split(',');
                foreach (var id in mcp)
                {
                    if (long.TryParse(id, out var mcpId))
                    {
                        if (!this.MunicipalityIds.Contains(mcpId))
                        {
                            this.MunicipalityIds.Add(mcpId);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override string ReportGenerator { get; set; }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            var houses = this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll()
                .WhereIf(this.MunicipalityIds.Count > 0, x => this.MunicipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    MunName = x.RealityObject.Municipality.Name,
                    ContrName = x.ContractRf.ManagingOrganization.Contragent.Name,
                    x.RealityObject.Address,
                    x.ContractRf.DocumentNum,
                    x.ContractRf.DocumentDate,
                    x.IncludeDate,
                    x.TypeCondition,
                    x.ExcludeDate,
                    x.Note
                })
                .ToList();

            foreach (var house in houses)
            {
                section.ДобавитьСтроку();

                section["Район"] = house.MunName;
                section["УправляющаяОрганизация"] = house.ContrName ?? string.Empty;
                section["Адрес"] = house.Address;
                section["НомерДоговора"] = house.DocumentNum;
                section["ДатаДоговора"] = house.DocumentDate;
                section["ДатаВключенияВДоговор"] = house.IncludeDate;
                section["ДатаИсключенияИзДоговора"] = house.TypeCondition == TypeCondition.Exclude
                    ? house.ExcludeDate.HasValue ? house.ExcludeDate.Value.ToShortDateString() : string.Empty
                    : string.Empty;
                section["ПримечаниеДомаДоговора"] = house.TypeCondition == TypeCondition.Include ? house.Note : string.Empty;
                section["ПримечаниеДомаВнеДоговора"] = house.TypeCondition == TypeCondition.Exclude ? house.Note : string.Empty;
            }

        }
    }
}