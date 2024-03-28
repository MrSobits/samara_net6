namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Report;

    public class GisuRealObjContractWithFundDecision : GisuRealObjContract
    {
        private DateTime _reportDate = DateTime.Now;

        public GisuRealObjContractWithFundDecision() : base(new ReportTemplateBinary(Properties.Resources.GisuRObjectContract)) { }

        public override string Name
        {
            get { return "Отчет по домам, включенным в договор с ГИСУ (сведения о способе формирования КР)"; }
        }

        public override string Desciption
        {
            get { return "Отчет по домам, включенным в договор с ГИСУ (сведения о способе формирования КР)"; }
        }
        public override void SetUserParams(BaseParams baseParams)
        {
            base.SetUserParams(baseParams);
            _reportDate = baseParams.Params.GetAs("reportDate", DateTime.Now);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var contractRfDomain = Container.Resolve<IDomainService<ContractRfObject>>();
            var decisionDomain = Container.Resolve<IDomainService<BasePropertyOwnerDecision>>();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            using (Container.Using(contractRfDomain, decisionDomain))
            {

                var houseQuery = contractRfDomain.GetAll()
                    .WhereIf(this.MunicipalityIds.Count > 0,
                        x => this.MunicipalityIds.Contains(x.RealityObject.Municipality.Id));

                var houses = houseQuery
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
                        RoId = x.RealityObject.Id
                    })
                    .ToArray();

                var housesIds = houseQuery.Select(x => x.RealityObject.Id);

                var decisions = decisionDomain.GetAll()
                    .Where(x => housesIds.Contains(x.RealityObject.Id))
                    .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                    .Where(
                        x =>
                            x.PropertyOwnerProtocol.DocumentDate.HasValue &&
                            (x.PropertyOwnerProtocol.DocumentDate.GetValueOrDefault() <= _reportDate))
                    .Select(x => new
                    {
                        Decision = x,
                        x.PropertyOwnerProtocol.DocumentDate,
                        x.RealityObject.Id
                    })
                    .ToArray()
                    .GroupBy(x => x.Id).Select(g => new
                    {
                        RoId = g.Key,
                        Decision = g.OrderByDescending(d => d.DocumentDate).Select(x => x.Decision).FirstOrDefault()
                    })
                    .ToDictionary(x => x.RoId);


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
                    section["СпоспобФормирования"] = decisions.ContainsKey(house.RoId)
                        ? GetMethodFormFundCrDisplay(decisions[house.RoId].Decision)
                        : MethodFormFundCr.NotSet.GetEnumMeta().Display;
                }
            }
        }

        private string GetMethodFormFundCrDisplay(BasePropertyOwnerDecision decision)
        {
            if (decision == null)
            {
                return MethodFormFundCr.NotSet.GetEnumMeta().Display;
            }

            return decision.MethodFormFund.HasValue ? decision.MethodFormFund.GetValueOrDefault().GetEnumMeta().Display : MethodFormFundCr.NotSet.GetEnumMeta().Display;
        }
    }
}
